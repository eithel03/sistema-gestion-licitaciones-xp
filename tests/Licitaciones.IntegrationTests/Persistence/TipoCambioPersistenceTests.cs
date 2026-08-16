using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.TiposCambio;
using Licitaciones.Domain.TiposCambio;
using Licitaciones.Infrastructure.Persistence;
using Licitaciones.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Licitaciones.IntegrationTests.Persistence;

[Collection(PostgreSqlContainerGroup.Name)]
public sealed class TipoCambioPersistenceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 10, 0, 0, TimeSpan.Zero);
    private static readonly DateOnly Fecha = new(2026, 8, 13);
    private readonly PostgreSqlContainerFixture _fixture;

    public TipoCambioPersistenceTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SavesRetrievesAndUpdatesTipoCambio()
    {
        await using var context = await CreateCleanContextAsync();
        var tipoCambio = TipoCambio.Create(Fecha, 520.25m, Now);
        context.Set<TipoCambio>().Add(tipoCambio);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();
        var stored = await context.Set<TipoCambio>().SingleAsync();
        stored.Update(Fecha.AddDays(1), 525.50m, Now.AddHours(1));
        await context.SaveChangesAsync();

        Assert.Equal(525.50m, (await context.Set<TipoCambio>().SingleAsync()).CrcPorUsd);
    }

    [Fact]
    public async Task DatabaseAllowsMultipleExchangeRatesForSameDate()
    {
        await using var context = await CreateCleanContextAsync();
        context.Set<TipoCambio>().AddRange(
            TipoCambio.Create(Fecha, 500m, Now),
            TipoCambio.Create(Fecha, 510m, Now.AddMinutes(1)),
            TipoCambio.Create(Fecha, 520m, Now.AddMinutes(2)));

        await context.SaveChangesAsync();

        var values = await context.Set<TipoCambio>().OrderBy(tipoCambio => tipoCambio.CrcPorUsd).Select(tipoCambio => tipoCambio.CrcPorUsd).ToListAsync();
        Assert.Equal([500m, 510m, 520m], values);
        Assert.All(await context.Set<TipoCambio>().ToListAsync(), tipoCambio => Assert.False(tipoCambio.Activo));
    }

    [Fact]
    public async Task DatabaseRejectsNonPositiveExchangeRate()
    {
        await using var context = await CreateCleanContextAsync();

        FormattableString sql = $@"INSERT INTO ""TiposCambio"" (""Id"", ""Fecha"", ""CrcPorUsd"", ""Activo"", ""CreatedAt"", ""UpdatedAt"")
              VALUES ({Guid.NewGuid()}, {Fecha}, {0m}, {false}, {Now}, {Now})";
        var exception = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlInterpolatedAsync(sql));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task DatabaseRejectsMultipleActiveExchangeRates()
    {
        await using var context = await CreateCleanContextAsync();
        var first = TipoCambio.Create(Fecha, 520m, Now);
        var second = TipoCambio.Create(Fecha.AddDays(1), 525m, Now);
        first.Activate(Now);
        second.Activate(Now);
        context.Set<TipoCambio>().AddRange(first, second);

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        var postgres = Assert.IsType<PostgresException>(exception.InnerException);
        Assert.Equal(PostgresErrorCodes.UniqueViolation, postgres.SqlState);
        Assert.Equal("IX_TiposCambio_UnicoActivo", postgres.ConstraintName);
    }

    [Fact]
    public async Task AllMigrationsIncludeTipoCambio()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        Assert.Empty(await context.Database.GetPendingMigrationsAsync());
        Assert.Contains((await context.Database.GetAppliedMigrationsAsync()), migration => migration.Contains("TiposCambio"));
    }

    [Fact]
    public async Task ConcurrentUpdatesReturnRepositoryConcurrencyConflict()
    {
        await using (var setup = await CreateCleanContextAsync())
        {
            setup.Set<TipoCambio>().Add(TipoCambio.Create(Fecha, 520m, Now));
            await setup.SaveChangesAsync();
        }

        await using var firstContext = CreateContext();
        await using var secondContext = CreateContext();
        var firstRepository = new TipoCambioRepository(firstContext);
        var secondRepository = new TipoCambioRepository(secondContext);
        var first = await firstRepository.GetByIdAsync(await firstContext.Set<TipoCambio>().Select(item => item.Id).SingleAsync());
        var second = await secondRepository.GetByIdAsync(first!.Id);

        first.Update(Fecha.AddDays(1), 525m, Now.AddMinutes(1));
        await firstRepository.SaveChangesAsync();
        second!.Update(Fecha.AddDays(2), 530m, Now.AddMinutes(2));

        await Assert.ThrowsAsync<TipoCambioConcurrencyException>(() => secondRepository.SaveChangesAsync());
    }

    [Fact]
    public async Task ConcurrentDeletesReturnRepositoryConcurrencyConflict()
    {
        await using (var setup = await CreateCleanContextAsync())
        {
            setup.Set<TipoCambio>().Add(TipoCambio.Create(Fecha, 520m, Now));
            await setup.SaveChangesAsync();
        }

        await using var firstContext = CreateContext();
        await using var secondContext = CreateContext();
        var firstRepository = new TipoCambioRepository(firstContext);
        var secondRepository = new TipoCambioRepository(secondContext);
        var first = await firstContext.Set<TipoCambio>().SingleAsync();
        var second = await secondContext.Set<TipoCambio>().SingleAsync();

        firstRepository.Remove(first);
        await firstRepository.SaveChangesAsync();
        secondRepository.Remove(second);

        await Assert.ThrowsAsync<TipoCambioConcurrencyException>(() => secondRepository.SaveChangesAsync());
    }

    [Fact]
    public async Task ConcurrentActivationsFromDifferentContextsLeaveExactlyOneActiveRate()
    {
        Guid firstId;
        Guid secondId;
        await using (var setup = await CreateCleanContextAsync())
        {
            var first = TipoCambio.Create(Fecha, 520m, Now);
            var second = TipoCambio.Create(Fecha.AddDays(1), 525m, Now.AddMinutes(1));
            firstId = first.Id;
            secondId = second.Id;
            setup.Set<TipoCambio>().AddRange(first, second);
            await setup.SaveChangesAsync();
        }

        await using var firstContext = CreateContext();
        await using var secondContext = CreateContext();
        var gate = new AsyncGate(2);
        var firstService = new TipoCambioService(
            new CoordinatedDeactivateRepository(new TipoCambioRepository(firstContext), gate),
            new FixedClock(Now.AddHours(1)));
        var secondService = new TipoCambioService(
            new CoordinatedDeactivateRepository(new TipoCambioRepository(secondContext), gate),
            new FixedClock(Now.AddHours(1)));

        var results = await Task.WhenAll(
            firstService.ActivateAsync(firstId),
            secondService.ActivateAsync(secondId));

        Assert.Single(results, result => result.Succeeded);
        Assert.Single(results, result => result.Status == TipoCambioResultStatus.Conflict);
        await using var verification = CreateContext();
        var active = await verification.Set<TipoCambio>().Where(item => item.Activo).ToListAsync();
        Assert.Single(active);
        Assert.Equal(results.Single(result => result.Succeeded).Value!.Id, active[0].Id);
    }

    [Fact]
    public async Task ActivationAtomicallyReplacesPreviousActiveRate()
    {
        Guid previousId;
        Guid replacementId;
        await using (var setup = await CreateCleanContextAsync())
        {
            var previous = TipoCambio.Create(Fecha, 520m, Now);
            var replacement = TipoCambio.Create(Fecha.AddDays(1), 525m, Now.AddMinutes(1));
            previous.Activate(Now.AddMinutes(2));
            previousId = previous.Id;
            replacementId = replacement.Id;
            setup.Set<TipoCambio>().AddRange(previous, replacement);
            await setup.SaveChangesAsync();
        }

        await using var context = CreateContext();
        var service = new TipoCambioService(
            new TipoCambioRepository(context),
            new FixedClock(Now.AddMinutes(3)));

        var result = await service.ActivateAsync(replacementId);

        Assert.True(result.Succeeded);
        await using var verification = CreateContext();
        var stored = await verification.Set<TipoCambio>().OrderBy(item => item.Id).ToListAsync();
        Assert.False(stored.Single(item => item.Id == previousId).Activo);
        Assert.True(stored.Single(item => item.Id == replacementId).Activo);
        Assert.Single(stored, item => item.Activo);
    }

    [Fact]
    public async Task FailedActivationRollsBackDeactivationOfPreviousActiveRate()
    {
        Guid activeId;
        Guid candidateId;
        await using (var setup = await CreateCleanContextAsync())
        {
            var seededActive = TipoCambio.Create(Fecha, 520m, Now);
            var candidate = TipoCambio.Create(Fecha.AddDays(1), 525m, Now.AddMinutes(1));
            seededActive.Activate(Now.AddMinutes(2));
            activeId = seededActive.Id;
            candidateId = candidate.Id;
            setup.Set<TipoCambio>().AddRange(seededActive, candidate);
            await setup.SaveChangesAsync();
        }

        await using var activationContext = CreateContext();
        var repository = new AfterGetByIdRepository(
            new TipoCambioRepository(activationContext),
            candidateId,
            async () =>
            {
                await using var competingContext = CreateContext();
                var competingCandidate = await competingContext.Set<TipoCambio>().SingleAsync(item => item.Id == candidateId);
                competingCandidate.Update(Fecha.AddDays(2), 530m, Now.AddMinutes(3));
                await competingContext.SaveChangesAsync();
            });
        var service = new TipoCambioService(repository, new FixedClock(Now.AddMinutes(4)));

        var result = await service.ActivateAsync(candidateId);

        Assert.Equal(TipoCambioResultStatus.ConcurrencyConflict, result.Status);
        await using var verification = CreateContext();
        var active = await verification.Set<TipoCambio>().SingleAsync(item => item.Activo);
        Assert.Equal(activeId, active.Id);
    }

    [Fact]
    public async Task ConversionUsesActiveExchangeRateRecoveredFromPostgreSql()
    {
        Guid activeId;
        await using (var setup = await CreateCleanContextAsync())
        {
            var active = TipoCambio.Create(Fecha, 500m, Now);
            active.Activate(Now.AddMinutes(1));
            activeId = active.Id;
            setup.Set<TipoCambio>().Add(active);
            await setup.SaveChangesAsync();
        }

        await using var context = CreateContext();
        var conversion = new MonedaConversionService(new TipoCambioRepository(context));

        var result = await conversion.ConvertFromCrcAsync(1000m, MonedaVisualizacion.USD);

        Assert.True(result.Succeeded);
        Assert.Equal(2m, result.Value!.MontoVisualizado);
        Assert.Equal(activeId, result.Value.TipoCambioId);
        Assert.Equal(Fecha, result.Value.FechaTipoCambio);
        Assert.Equal(500m, result.Value.CrcPorUsd);
    }

    [Fact]
    public async Task PaginationUsesCreationDateAsDeterministicTieBreaker()
    {
        Guid oldestId;
        Guid middleId;
        Guid newestId;
        await using (var setup = await CreateCleanContextAsync())
        {
            var oldest = TipoCambio.Create(Fecha, 500m, Now);
            var middle = TipoCambio.Create(Fecha, 510m, Now.AddMinutes(1));
            var newest = TipoCambio.Create(Fecha, 520m, Now.AddMinutes(2));
            oldestId = oldest.Id;
            middleId = middle.Id;
            newestId = newest.Id;
            setup.Set<TipoCambio>().AddRange(newest, middle, oldest);
            await setup.SaveChangesAsync();
        }

        await using var context = CreateContext();
        var repository = new TipoCambioRepository(context);

        var firstPage = await repository.ListAsync(new TipoCambioQuery(1, 1));
        var secondPage = await repository.ListAsync(new TipoCambioQuery(2, 1));
        var thirdPage = await repository.ListAsync(new TipoCambioQuery(3, 1));

        Assert.Equal(newestId, Assert.Single(firstPage.Items).Id);
        Assert.Equal(middleId, Assert.Single(secondPage.Items).Id);
        Assert.Equal(oldestId, Assert.Single(thirdPage.Items).Id);
    }

    private async Task<LicitacionesDbContext> CreateCleanContextAsync()
    {
        var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        return context;
    }

    private LicitacionesDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>().UseNpgsql(_fixture.ConnectionString).Options;
        return new LicitacionesDbContext(options);
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class AsyncGate(int participantCount)
    {
        private readonly TaskCompletionSource _allArrived = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _arrived;

        public async Task SignalAndWaitAsync()
        {
            if (Interlocked.Increment(ref _arrived) == participantCount)
            {
                _allArrived.TrySetResult();
            }

            await _allArrived.Task.WaitAsync(TimeSpan.FromSeconds(10));
        }
    }

    private sealed class CoordinatedDeactivateRepository(
        ITipoCambioRepository inner,
        AsyncGate gate) : DelegatingRepository(inner)
    {
        public override async Task DeactivateAllExceptAsync(
            Guid activeId,
            DateTimeOffset updatedAt,
            CancellationToken cancellationToken = default)
        {
            await Inner.DeactivateAllExceptAsync(activeId, updatedAt, cancellationToken);
            await gate.SignalAndWaitAsync();
        }
    }

    private sealed class AfterGetByIdRepository(
        ITipoCambioRepository inner,
        Guid targetId,
        Func<Task> afterGet) : DelegatingRepository(inner)
    {
        private bool _invoked;

        public override async Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await Inner.GetByIdAsync(id, cancellationToken);
            if (!_invoked && id == targetId)
            {
                _invoked = true;
                await afterGet();
            }

            return result;
        }
    }

    private abstract class DelegatingRepository(ITipoCambioRepository inner) : ITipoCambioRepository
    {
        protected ITipoCambioRepository Inner { get; } = inner;

        public Task AddAsync(TipoCambio tipoCambio, CancellationToken cancellationToken = default) =>
            Inner.AddAsync(tipoCambio, cancellationToken);

        public virtual Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Inner.GetByIdAsync(id, cancellationToken);

        public Task<TipoCambio?> GetActiveAsync(CancellationToken cancellationToken = default) =>
            Inner.GetActiveAsync(cancellationToken);

        public Task<TipoCambioPage> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default) =>
            Inner.ListAsync(query, cancellationToken);

        public virtual Task DeactivateAllExceptAsync(
            Guid activeId,
            DateTimeOffset updatedAt,
            CancellationToken cancellationToken = default) =>
            Inner.DeactivateAllExceptAsync(activeId, updatedAt, cancellationToken);

        public void Remove(TipoCambio tipoCambio) => Inner.Remove(tipoCambio);

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            Inner.SaveChangesAsync(cancellationToken);
    }
}

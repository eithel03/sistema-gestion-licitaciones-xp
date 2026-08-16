using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Domain.Licitaciones;

namespace Licitaciones.UnitTests.Application.Licitaciones;

public sealed class LicitacionServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 15, 14, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset FutureClose = Now.AddDays(5);

    [Fact]
    public async Task CreateStoresValidTenderWithNormalizedCode()
    {
        var repository = new InMemoryRepository();
        var service = CreateService(repository);

        var result = await service.CreateAsync(new CrearLicitacionRequest(
            " lic   2026-001 ", "Compra de equipo", 1500000m, FutureClose));

        Assert.True(result.Succeeded);
        Assert.Equal("LIC 2026-001", result.Value!.Codigo);
        Assert.Equal("LIC 2026-001", result.Value.CodigoNormalizado);
        Assert.Equal(LicitacionEstado.Borrador, result.Value.Estado);
        Assert.Equal(Now, result.Value.CreatedAt);
        Assert.Single(repository.Items, item => item.Id == result.Value.Id);
    }

    [Fact]
    public async Task CreateRejectsEquivalentNormalizedCode()
    {
        var existing = CreateTender("LIC 2026-001");
        var service = CreateService(new InMemoryRepository(existing));

        var result = await service.CreateAsync(new CrearLicitacionRequest(
            " lic   2026-001 ", "Compra duplicada", 1000m, FutureClose));

        Assert.Equal(LicitacionResultStatus.Conflict, result.Status);
        Assert.Equal(LicitacionErrors.CodigoDuplicado, result.ErrorCode);
    }

    [Fact]
    public async Task UpdateRejectsCodeEquivalentToAnotherTender()
    {
        var existing = CreateTender("LIC-2026-001");
        var editable = CreateTender("LIC-2026-002");
        var service = CreateService(new InMemoryRepository(existing, editable));

        var result = await service.UpdateAsync(editable.Id, new ActualizarLicitacionRequest(
            " lic-2026-001 ", "Compra actualizada", 2000m, FutureClose.AddDays(1), editable.Version));

        Assert.Equal(LicitacionResultStatus.Conflict, result.Status);
        Assert.Equal(LicitacionErrors.CodigoDuplicado, result.ErrorCode);
        Assert.Equal("LIC-2026-002", editable.Codigo);
    }

    [Fact]
    public async Task MissingTenderReturnsNotFoundForReadAndCommands()
    {
        var service = CreateService(new InMemoryRepository());
        var missingId = Guid.NewGuid();

        var get = await service.GetByIdAsync(missingId);
        var update = await service.UpdateAsync(missingId, new ActualizarLicitacionRequest(
            "LIC-404", "Inexistente", 1000m, FutureClose));
        var delete = await service.DeleteAsync(missingId);
        var publish = await service.PublishAsync(missingId);
        var close = await service.CloseAsync(missingId);
        var change = await service.ChangeEstadoAsync(missingId, new CambiarEstadoLicitacionRequest("Publicada"));

        Assert.All(
            new[] { get.Status, update.Status, delete.Status, publish.Status, close.Status, change.Status },
            status => Assert.Equal(LicitacionResultStatus.NotFound, status));
        Assert.All(
            new[] { get.ErrorCode, update.ErrorCode, delete.ErrorCode, publish.ErrorCode, close.ErrorCode, change.ErrorCode },
            code => Assert.Equal(LicitacionErrors.NoEncontrada, code));
    }

    [Fact]
    public async Task UpdateRejectsStaleVersionBeforeChangingTender()
    {
        var tender = CreateTender("LIC-2026-001");
        var service = CreateService(new InMemoryRepository(tender));

        var result = await service.UpdateAsync(tender.Id, new ActualizarLicitacionRequest(
            "LIC-2026-002", "Compra actualizada", 2000m, FutureClose.AddDays(1), tender.Version + 1));

        Assert.Equal(LicitacionResultStatus.ConcurrencyConflict, result.Status);
        Assert.Equal(LicitacionErrors.Concurrencia, result.ErrorCode);
        Assert.Equal("LIC-2026-001", tender.Codigo);
    }

    [Fact]
    public async Task RepositoryConcurrencyExceptionReturnsControlledConflict()
    {
        var tender = CreateTender("LIC-2026-001");
        var repository = new InMemoryRepository(tender)
        {
            SaveException = new LicitacionConcurrencyException()
        };
        var service = CreateService(repository);

        var result = await service.UpdateAsync(tender.Id, new ActualizarLicitacionRequest(
            tender.Codigo, "Compra actualizada", 2000m, FutureClose.AddDays(1), tender.Version));

        Assert.Equal(LicitacionResultStatus.ConcurrencyConflict, result.Status);
        Assert.Equal(LicitacionErrors.Concurrencia, result.ErrorCode);
    }

    [Fact]
    public async Task RepeatedPublishReturnsControlledValidationError()
    {
        var tender = CreateTender("LIC-2026-001");
        var service = CreateService(new InMemoryRepository(tender));

        var firstPublish = await service.PublishAsync(tender.Id);
        var repeatedPublish = await service.PublishAsync(tender.Id);

        Assert.True(firstPublish.Succeeded);
        Assert.Equal(LicitacionResultStatus.ValidationError, repeatedPublish.Status);
        Assert.Equal(LicitacionErrors.TransicionInvalida, repeatedPublish.ErrorCode);
    }

    [Fact]
    public async Task ChangeEstadoRejectsUnknownState()
    {
        var tender = CreateTender("LIC-2026-001");
        var service = CreateService(new InMemoryRepository(tender));

        var unknownState = await service.ChangeEstadoAsync(tender.Id, new CambiarEstadoLicitacionRequest("Archivada"));

        Assert.Equal(LicitacionResultStatus.ValidationError, unknownState.Status);
        Assert.Equal(LicitacionErrors.TransicionInvalida, unknownState.ErrorCode);
    }

    private static LicitacionService CreateService(InMemoryRepository repository) =>
        new(repository, new FixedClock(Now));

    private static Licitacion CreateTender(string code) =>
        Licitacion.Create(code, "Compra", 1000m, FutureClose, Now.AddHours(-1));

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class InMemoryRepository(params Licitacion[] items) : ILicitacionRepository
    {
        public List<Licitacion> Items { get; } = [.. items];
        public Exception? SaveException { get; init; }

        public Task AddAsync(Licitacion licitacion, CancellationToken cancellationToken = default)
        {
            Items.Add(licitacion);
            return Task.CompletedTask;
        }

        public Task<Licitacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(item => item.Id == id));

        public Task<bool> ExistsByNormalizedCodeAsync(
            string normalizedCode,
            Guid? excludedId = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item =>
                item.DeletedAt is null &&
                item.Id != excludedId &&
                item.CodigoNormalizado == normalizedCode));

        public Task<LicitacionPage> ListAsync(
            LicitacionQuery query,
            DateTimeOffset utcNow,
            CancellationToken cancellationToken = default)
        {
            var values = Items
                .Where(item => item.DeletedAt is null)
                .Select(item => LicitacionResponse.FromDomain(item, utcNow))
                .ToList();
            return Task.FromResult(new LicitacionPage(values, values.Count, query.ValidPage, query.ValidPageSize));
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            SaveException is null ? Task.CompletedTask : Task.FromException(SaveException);
    }
}

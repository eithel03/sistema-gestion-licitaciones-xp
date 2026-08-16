using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.TiposCambio;
using Licitaciones.Domain.TiposCambio;

namespace Licitaciones.UnitTests.Application.TiposCambio;

public sealed class TipoCambioServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 10, 0, 0, TimeSpan.Zero);
    private static readonly DateOnly Fecha = new(2026, 8, 13);

    [Fact]
    public async Task CrudCreatesUpdatesListsGetsAndDeletesExchangeRates()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CrearTipoCambioRequest(Fecha, 520.25m));

        var updated = await service.UpdateAsync(created.Value!.Id, new ActualizarTipoCambioRequest(Fecha.AddDays(1), 525.50m, created.Value.Version));
        var listed = await service.ListAsync(new TipoCambioQuery());
        var found = await service.GetByIdAsync(created.Value.Id);
        var deleted = await service.DeleteAsync(created.Value.Id);

        Assert.Equal(525.50m, updated.Value!.CrcPorUsd);
        Assert.Single(listed.Value!.Items);
        Assert.Equal(525.50m, found.Value!.CrcPorUsd);
        Assert.True(deleted.Succeeded);
        Assert.Empty((await service.ListAsync(new TipoCambioQuery())).Value!.Items);
    }

    [Fact]
    public async Task ActivateKeepsOnlyOneActiveExchangeRate()
    {
        var service = CreateService();
        var first = await service.CreateAsync(new CrearTipoCambioRequest(Fecha, 520m));
        var second = await service.CreateAsync(new CrearTipoCambioRequest(Fecha.AddDays(1), 525m));

        var firstActivation = await service.ActivateAsync(first.Value!.Id);

        Assert.True(firstActivation.Succeeded);
        Assert.True(firstActivation.Value!.Activo);

        var activated = await service.ActivateAsync(second.Value!.Id);
        var active = await service.GetActiveAsync();
        var firstAfterActivation = await service.GetByIdAsync(first.Value.Id);

        Assert.True(activated.Value!.Activo);
        Assert.Equal(second.Value.Id, active.Value!.Id);
        Assert.False(firstAfterActivation.Value!.Activo);
    }

    [Fact]
    public async Task ConvertCrcToUsdUsesActiveRateAndDoesNotChangeOriginalAmount()
    {
        var repository = new InMemoryRepository();
        var service = CreateService(repository);
        var conversion = new MonedaConversionService(repository);
        var created = await service.CreateAsync(new CrearTipoCambioRequest(Fecha, 500m));
        await service.ActivateAsync(created.Value!.Id);

        var result = await conversion.ConvertFromCrcAsync(1000m, MonedaVisualizacion.USD);

        Assert.True(result.Succeeded);
        Assert.Equal(1000m, result.Value!.MontoOriginalCrc);
        Assert.Equal(2m, result.Value.MontoVisualizado);
        Assert.Equal(MonedaVisualizacion.USD, result.Value.Moneda);
    }

    [Fact]
    public async Task ConvertToUsdWithoutActiveRateReturnsControlledNotFound()
    {
        var conversion = new MonedaConversionService(new InMemoryRepository());

        var result = await conversion.ConvertFromCrcAsync(1000m, MonedaVisualizacion.USD);

        Assert.Equal(TipoCambioResultStatus.NotFound, result.Status);
        Assert.Equal(TipoCambioErrors.ActivoNoEncontrado, result.ErrorCode);
    }

    [Fact]
    public async Task ConvertToCrcReturnsOriginalAmountWithoutActiveExchangeRate()
    {
        var repository = new InMemoryRepository();
        var conversion = new MonedaConversionService(repository);

        var result = await conversion.ConvertFromCrcAsync(1234.56m, MonedaVisualizacion.CRC);

        Assert.True(result.Succeeded);
        Assert.Equal(1234.56m, result.Value!.MontoOriginalCrc);
        Assert.Equal(1234.56m, result.Value.MontoVisualizado);
        Assert.Equal(MonedaVisualizacion.CRC, result.Value.Moneda);
        Assert.Null(result.Value.TipoCambioId);
        Assert.Equal(0, repository.ActiveLookupCount);
    }

    [Fact]
    public async Task MissingExchangeRateReturnsNotFoundForReadAndCommands()
    {
        var service = CreateService();
        var missingId = Guid.NewGuid();

        var get = await service.GetByIdAsync(missingId);
        var update = await service.UpdateAsync(missingId, new ActualizarTipoCambioRequest(Fecha, 520m));
        var activate = await service.ActivateAsync(missingId);
        var delete = await service.DeleteAsync(missingId);

        Assert.All(
            new[] { get.Status, update.Status, activate.Status, delete.Status },
            status => Assert.Equal(TipoCambioResultStatus.NotFound, status));
        Assert.All(
            new[] { get.ErrorCode, update.ErrorCode, activate.ErrorCode, delete.ErrorCode },
            code => Assert.Equal(TipoCambioErrors.NoEncontrado, code));
    }

    [Fact]
    public async Task UpdateRejectsStaleVersionBeforeChangingExchangeRate()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CrearTipoCambioRequest(Fecha, 520m));

        var result = await service.UpdateAsync(created.Value!.Id, new ActualizarTipoCambioRequest(
            Fecha.AddDays(1), 525m, created.Value.Version + 1));

        Assert.Equal(TipoCambioResultStatus.ConcurrencyConflict, result.Status);
        Assert.Equal(TipoCambioErrors.Concurrencia, result.ErrorCode);
        Assert.Equal(520m, (await service.GetByIdAsync(created.Value.Id)).Value!.CrcPorUsd);
    }

    [Fact]
    public async Task RepositoryConcurrencyExceptionReturnsControlledConflict()
    {
        var repository = new InMemoryRepository
        {
            SaveException = new TipoCambioConcurrencyException()
        };
        var service = CreateService(repository);

        var result = await service.CreateAsync(new CrearTipoCambioRequest(Fecha, 520m));

        Assert.Equal(TipoCambioResultStatus.ConcurrencyConflict, result.Status);
        Assert.Equal(TipoCambioErrors.Concurrencia, result.ErrorCode);
    }

    [Fact]
    public async Task RepositoryActiveConflictReturnsControlledConflict()
    {
        var repository = new InMemoryRepository();
        var service = CreateService(repository);
        var created = await service.CreateAsync(new CrearTipoCambioRequest(Fecha, 520m));
        repository.SaveException = new TipoCambioActiveConflictException();

        var result = await service.ActivateAsync(created.Value!.Id);

        Assert.Equal(TipoCambioResultStatus.Conflict, result.Status);
        Assert.Equal(TipoCambioErrors.ActivoDuplicado, result.ErrorCode);
    }

    private static TipoCambioService CreateService(InMemoryRepository? repository = null) =>
        new(repository ?? new InMemoryRepository(), new FixedClock(Now));

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class InMemoryRepository : ITipoCambioRepository
    {
        private readonly List<TipoCambio> _items = [];
        public Exception? SaveException { get; set; }
        public int ActiveLookupCount { get; private set; }

        public Task AddAsync(TipoCambio tipoCambio, CancellationToken cancellationToken = default)
        {
            _items.Add(tipoCambio);
            return Task.CompletedTask;
        }

        public Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.SingleOrDefault(item => item.Id == id));

        public Task<TipoCambio?> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            ActiveLookupCount++;
            return Task.FromResult(_items.SingleOrDefault(item => item.Activo));
        }

        public Task<TipoCambioPage> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default)
        {
            var values = _items
                .OrderByDescending(item => item.Activo)
                .ThenByDescending(item => item.Fecha)
                .Select(TipoCambioResponse.FromDomain)
                .ToList();
            return Task.FromResult(new TipoCambioPage(values, values.Count, query.ValidPage, query.ValidPageSize));
        }

        public Task DeactivateAllExceptAsync(Guid activeId, DateTimeOffset updatedAt, CancellationToken cancellationToken = default)
        {
            foreach (var item in _items.Where(item => item.Id != activeId && item.Activo))
            {
                item.Deactivate(updatedAt);
            }

            return Task.CompletedTask;
        }

        public void Remove(TipoCambio tipoCambio) => _items.Remove(tipoCambio);
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            SaveException is null ? Task.CompletedTask : Task.FromException(SaveException);
    }
}

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

        await service.ActivateAsync(first.Value!.Id);
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

    private static TipoCambioService CreateService(InMemoryRepository? repository = null) =>
        new(repository ?? new InMemoryRepository(), new FixedClock(Now));

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class InMemoryRepository : ITipoCambioRepository
    {
        private readonly List<TipoCambio> _items = [];

        public Task AddAsync(TipoCambio tipoCambio, CancellationToken cancellationToken = default)
        {
            _items.Add(tipoCambio);
            return Task.CompletedTask;
        }

        public Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.SingleOrDefault(item => item.Id == id));

        public Task<TipoCambio?> GetActiveAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.SingleOrDefault(item => item.Activo));

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
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}

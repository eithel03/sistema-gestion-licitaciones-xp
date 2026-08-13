using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Aprobaciones;
using Licitaciones.Domain.Aprobaciones;

namespace Licitaciones.UnitTests.Application.Aprobaciones;

public sealed class NivelAprobacionServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task CreateAcceptsSeparateRangesAndFindsApproverAtLimits()
    {
        var service = CreateService();
        await service.CreateAsync(new CrearNivelAprobacionRequest(0.01m, 999999.99m, "Encargado de area"));
        await service.CreateAsync(new CrearNivelAprobacionRequest(1000000m, 9999999.99m, "Gerencia"));
        await service.CreateAsync(new CrearNivelAprobacionRequest(10000000m, null, "Junta Directiva"));

        var lower = await service.FindApproverAsync(1000000m);
        var upper = await service.FindApproverAsync(9999999.99m);
        var open = await service.FindApproverAsync(10000000m);

        Assert.Equal("Gerencia", lower.Value!.Aprobador);
        Assert.Equal("Gerencia", upper.Value!.Aprobador);
        Assert.Equal("Junta Directiva", open.Value!.Aprobador);
    }

    [Fact]
    public async Task CreateRejectsOverlappingRange()
    {
        var service = CreateService();
        await service.CreateAsync(new CrearNivelAprobacionRequest(0.01m, 100m, "A"));

        var result = await service.CreateAsync(new CrearNivelAprobacionRequest(100m, 200m, "B"));

        Assert.Equal(NivelAprobacionResultStatus.Conflict, result.Status);
        Assert.Equal(NivelAprobacionErrors.RangoTraslapado, result.ErrorCode);
    }

    [Fact]
    public async Task CreateRejectsSecondOpenRangeWithSpecificError()
    {
        var service = CreateService();
        await service.CreateAsync(new CrearNivelAprobacionRequest(100m, null, "A"));

        var result = await service.CreateAsync(new CrearNivelAprobacionRequest(200m, null, "B"));

        Assert.Equal(NivelAprobacionResultStatus.Conflict, result.Status);
        Assert.Equal(NivelAprobacionErrors.SegundoRangoAbierto, result.ErrorCode);
    }

    [Fact]
    public async Task CrudCreatesUpdatesListsGetsAndDeletes()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CrearNivelAprobacionRequest(0.01m, 100m, "A"));

        var updated = await service.UpdateAsync(created.Value!.Id, new ActualizarNivelAprobacionRequest(0.01m, 99.99m, "B", created.Value.Version));
        var listed = await service.ListAsync(new NivelAprobacionQuery());
        var found = await service.GetByIdAsync(created.Value.Id);
        var deleted = await service.DeleteAsync(created.Value.Id);

        Assert.Equal("B", updated.Value!.Aprobador);
        Assert.Single(listed.Value!.Items);
        Assert.Equal("B", found.Value!.Aprobador);
        Assert.True(deleted.Succeeded);
        Assert.Empty((await service.ListAsync(new NivelAprobacionQuery())).Value!.Items);
    }

    [Fact]
    public async Task FindApproverWithoutMatchingRangeReturnsControlledNotFound()
    {
        var service = CreateService();

        var result = await service.FindApproverAsync(500m);

        Assert.Equal(NivelAprobacionResultStatus.NotFound, result.Status);
        Assert.Equal(NivelAprobacionErrors.AprobadorNoEncontrado, result.ErrorCode);
    }

    private static NivelAprobacionService CreateService() =>
        new(new InMemoryRepository(), new FixedClock(Now));

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class InMemoryRepository : INivelAprobacionRepository
    {
        private readonly List<NivelAprobacion> _items = [];

        public Task AddAsync(NivelAprobacion nivel, CancellationToken cancellationToken = default)
        {
            _items.Add(nivel);
            return Task.CompletedTask;
        }

        public Task<NivelAprobacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.SingleOrDefault(item => item.Id == id));

        public Task<NivelAprobacionPage> ListAsync(NivelAprobacionQuery query, CancellationToken cancellationToken = default)
        {
            var values = _items.OrderBy(item => item.MontoMinimoCrc).Select(NivelAprobacionResponse.FromDomain).ToList();
            return Task.FromResult(new NivelAprobacionPage(values, values.Count, query.ValidPage, query.ValidPageSize));
        }

        public Task<bool> HasOverlapAsync(decimal minimum, decimal? maximum, Guid? excludedId = null, CancellationToken cancellationToken = default)
        {
            var candidate = NivelAprobacion.Create(minimum, maximum, "Temporal", Now);
            return Task.FromResult(_items.Any(item => item.Id != excludedId && item.Overlaps(candidate)));
        }

        public Task<bool> HasOpenRangeAsync(Guid? excludedId = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.Any(item => item.Id != excludedId && item.IsOpen));

        public Task<NivelAprobacion?> FindByAmountAsync(decimal amount, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.SingleOrDefault(item => item.Contains(amount)));

        public void Remove(NivelAprobacion nivel) => _items.Remove(nivel);
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}

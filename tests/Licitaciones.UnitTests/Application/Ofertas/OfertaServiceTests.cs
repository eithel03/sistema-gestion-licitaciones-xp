using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Application.Ofertas;
using Licitaciones.Application.Proveedores;
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Ofertas;
using Licitaciones.Domain.Proveedores;

namespace Licitaciones.UnitTests.Application.Ofertas;

public sealed class OfertaServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task CreateRejectsDuplicateProviderForLicitacion()
    {
        var context = CreateContext();
        var first = await context.Service.CreateAsync(new CrearOfertaRequest(context.Licitacion.Id, context.Proveedor.Id, 900m));

        var duplicate = await context.Service.CreateAsync(new CrearOfertaRequest(context.Licitacion.Id, context.Proveedor.Id, 800m));

        Assert.True(first.Succeeded);
        Assert.Equal(OfertaResultStatus.Conflict, duplicate.Status);
        Assert.Equal(OfertaErrors.Duplicada, duplicate.ErrorCode);
    }

    [Fact]
    public async Task CreateRejectsUnknownLicitacionAndProveedor()
    {
        var context = CreateContext();

        var missingLicitacion = await context.Service.CreateAsync(new CrearOfertaRequest(Guid.NewGuid(), context.Proveedor.Id, 900m));
        var missingProveedor = await context.Service.CreateAsync(new CrearOfertaRequest(context.Licitacion.Id, Guid.NewGuid(), 900m));

        Assert.Equal(OfertaErrors.LicitacionNoEncontrada, missingLicitacion.ErrorCode);
        Assert.Equal(OfertaErrors.ProveedorNoEncontrado, missingProveedor.ErrorCode);
    }

    [Fact]
    public async Task CreateUsesInjectedClockToRejectExpiredLicitacion()
    {
        var context = CreateContext(Now.AddDays(2));

        var result = await context.Service.CreateAsync(new CrearOfertaRequest(context.Licitacion.Id, context.Proveedor.Id, 900m));

        Assert.Equal(OfertaResultStatus.ValidationError, result.Status);
        Assert.Equal(OfertaErrors.LicitacionNoRecibeOfertas, result.ErrorCode);
    }

    [Fact]
    public async Task UpdateAndDeleteRejectClosedLicitacion()
    {
        var context = CreateContext();
        var created = await context.Service.CreateAsync(new CrearOfertaRequest(context.Licitacion.Id, context.Proveedor.Id, 900m));
        context.Licitacion.Close(Now.AddMinutes(1));
        context.Clock.UtcNow = Now.AddMinutes(2);

        var update = await context.Service.UpdateAsync(created.Value!.Id, new ActualizarOfertaRequest(800m, created.Value.Version));
        var delete = await context.Service.DeleteAsync(created.Value.Id);

        Assert.Equal(OfertaResultStatus.ValidationError, update.Status);
        Assert.Equal(OfertaResultStatus.ValidationError, delete.Status);
        Assert.Single(context.Ofertas.Items);
    }

    [Fact]
    public async Task ListFiltersByLicitacionAndProveedor()
    {
        var context = CreateContext();
        await context.Service.CreateAsync(new CrearOfertaRequest(context.Licitacion.Id, context.Proveedor.Id, 900m));

        var result = await context.Service.ListAsync(new OfertaQuery(LicitacionId: context.Licitacion.Id, ProveedorId: context.Proveedor.Id));

        Assert.Single(result.Value!.Items);
        Assert.Equal(context.Licitacion.Id, result.Value.Items[0].LicitacionId);
        Assert.Equal(context.Proveedor.Id, result.Value.Items[0].ProveedorId);
    }

    [Fact]
    public async Task BestOfferWithoutOffersReturnsControlledResult()
    {
        var context = CreateContext();

        var result = await context.Service.GetBestAsync(context.Licitacion.Id);

        Assert.True(result.Succeeded);
        Assert.False(result.Value!.TieneOferta);
        Assert.Equal("Sin ofertas validas", result.Value.DescripcionClasificacion);
    }

    private static TestContext CreateContext(DateTimeOffset? clockNow = null)
    {
        var licitacion = Licitacion.Create("LIC-APP-OF", "Compra", 1000m, Now.AddDays(1), Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));
        var proveedor = Proveedor.Create("Proveedor Uno", Now);
        var ofertas = new InMemoryOfertaRepository();
        var licitaciones = new InMemoryLicitacionRepository(licitacion);
        var proveedores = new InMemoryProveedorRepository(proveedor);
        var clock = new MutableClock(clockNow ?? Now);
        return new TestContext(new OfertaService(ofertas, licitaciones, proveedores, clock), ofertas, licitacion, proveedor, clock);
    }

    private sealed record TestContext(
        OfertaService Service,
        InMemoryOfertaRepository Ofertas,
        Licitacion Licitacion,
        Proveedor Proveedor,
        MutableClock Clock);

    private sealed class MutableClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; set; } = utcNow;
    }

    private sealed class InMemoryOfertaRepository : IOfertaRepository
    {
        public List<Oferta> Items { get; } = [];

        public Task AddAsync(Oferta oferta, CancellationToken cancellationToken = default)
        {
            Items.Add(oferta);
            return Task.CompletedTask;
        }

        public Task<Oferta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(item => item.Id == id));

        public Task<bool> ExistsAsync(Guid licitacionId, Guid proveedorId, Guid? excludedId = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item => item.LicitacionId == licitacionId && item.ProveedorId == proveedorId && item.Id != excludedId));

        public Task<OfertaPage> ListAsync(OfertaQuery query, CancellationToken cancellationToken = default)
        {
            var filtered = Items
                .Where(item => !query.LicitacionId.HasValue || item.LicitacionId == query.LicitacionId)
                .Where(item => !query.ProveedorId.HasValue || item.ProveedorId == query.ProveedorId)
                .OrderBy(item => item.FechaRegistro)
                .Select(OfertaResponse.FromDomain)
                .ToList();
            return Task.FromResult(new OfertaPage(filtered, filtered.Count, query.ValidPage, query.ValidPageSize));
        }

        public Task<IReadOnlyList<Oferta>> ListByLicitacionAsync(Guid licitacionId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Oferta>>(Items.Where(item => item.LicitacionId == licitacionId).ToList());

        public void Remove(Oferta oferta) => Items.Remove(oferta);
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class InMemoryLicitacionRepository(Licitacion licitacion) : ILicitacionRepository
    {
        public Task AddAsync(Licitacion item, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<Licitacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(id == licitacion.Id ? licitacion : null);
        public Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, Guid? excludedId = null, CancellationToken cancellationToken = default) => Task.FromResult(false);
        public Task<LicitacionPage> ListAsync(LicitacionQuery query, DateTimeOffset utcNow, CancellationToken cancellationToken = default) => throw new NotSupportedException();
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class InMemoryProveedorRepository(Proveedor proveedor) : IProveedorRepository
    {
        public Task AddAsync(Proveedor item, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<Proveedor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(id == proveedor.Id ? proveedor : null);
        public Task<bool> ExistsByNormalizedNameAsync(string normalizedName, Guid? excludedId = null, CancellationToken cancellationToken = default) => Task.FromResult(false);
        public Task<ProveedorPage> ListAsync(ProveedorQuery query, CancellationToken cancellationToken = default) => throw new NotSupportedException();
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}

using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Proveedores;
using Licitaciones.Domain.Proveedores;

namespace Licitaciones.UnitTests.Application.Proveedores;

public sealed class ProveedorServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 10, 10, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task CreateRejectsDuplicateNormalizedName()
    {
        var repository = new InMemoryProveedorRepository();
        var service = new ProveedorService(repository, new FixedClock(Now));
        await service.CreateAsync(new CrearProveedorRequest("Empresa Central"));

        var result = await service.CreateAsync(new CrearProveedorRequest(" empresa   central "));

        Assert.False(result.Succeeded);
        Assert.Equal(ProveedorResultStatus.Conflict, result.Status);
        Assert.Equal(ProveedorErrors.NombreDuplicado, result.ErrorCode);
    }

    [Fact]
    public async Task CreateReturnsCreatedProvider()
    {
        var service = new ProveedorService(new InMemoryProveedorRepository(), new FixedClock(Now));

        var result = await service.CreateAsync(new CrearProveedorRequest("Empresa Central"));

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Value);
        Assert.Equal("Empresa Central", result.Value.Nombre);
        Assert.Equal(Now, result.Value.CreatedAt);
    }

    [Fact]
    public async Task UpdateRejectsNameEquivalentToAnotherProvider()
    {
        var repository = new InMemoryProveedorRepository();
        var service = new ProveedorService(repository, new FixedClock(Now));
        var first = await service.CreateAsync(new CrearProveedorRequest("Empresa Central"));
        var second = await service.CreateAsync(new CrearProveedorRequest("Proveedor Norte"));

        var result = await service.UpdateAsync(second.Value!.Id, new ActualizarProveedorRequest(first.Value!.Nombre));

        Assert.False(result.Succeeded);
        Assert.Equal(ProveedorResultStatus.Conflict, result.Status);
    }

    [Fact]
    public async Task DeleteRetiresProvider()
    {
        var repository = new InMemoryProveedorRepository();
        var service = new ProveedorService(repository, new FixedClock(Now));
        var created = await service.CreateAsync(new CrearProveedorRequest("Empresa Central"));

        var result = await service.DeleteAsync(created.Value!.Id);

        Assert.True(result.Succeeded);
        var stored = await repository.GetByIdAsync(created.Value.Id);
        Assert.NotNull(stored!.DeletedAt);
    }

    [Fact]
    public async Task ListFiltersSortsAndPaginatesProviders()
    {
        var service = new ProveedorService(new InMemoryProveedorRepository(), new FixedClock(Now));
        await service.CreateAsync(new CrearProveedorRequest("Delta Central"));
        await service.CreateAsync(new CrearProveedorRequest("Alfa Central"));
        await service.CreateAsync(new CrearProveedorRequest("Beta Norte"));

        var result = await service.ListAsync(new ProveedorQuery(Page: 1, PageSize: 1, Search: "central", Sort: "name"));

        Assert.True(result.Succeeded);
        Assert.Equal(2, result.Value!.TotalItems);
        Assert.Single(result.Value.Items);
        Assert.Equal("Alfa Central", result.Value.Items[0].Nombre);
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow => utcNow;
    }

    private sealed class InMemoryProveedorRepository : IProveedorRepository
    {
        private readonly List<Proveedor> _proveedores = [];

        public Task AddAsync(Proveedor proveedor, CancellationToken cancellationToken = default)
        {
            _proveedores.Add(proveedor);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByNormalizedNameAsync(
            string normalizedName,
            Guid? excludedId = null,
            CancellationToken cancellationToken = default)
        {
            var exists = _proveedores.Any(proveedor =>
                proveedor.DeletedAt is null
                && proveedor.NombreNormalizado == normalizedName
                && proveedor.Id != excludedId);

            return Task.FromResult(exists);
        }

        public Task<Proveedor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_proveedores.SingleOrDefault(proveedor => proveedor.Id == id));
        }

        public Task<ProveedorPage> ListAsync(ProveedorQuery query, CancellationToken cancellationToken = default)
        {
            var filtered = _proveedores
                .Where(proveedor => proveedor.DeletedAt is null)
                .Where(proveedor => string.IsNullOrWhiteSpace(query.Search)
                    || proveedor.Nombre.Contains(query.Search, StringComparison.OrdinalIgnoreCase))
                .OrderBy(proveedor => proveedor.Nombre)
                .ToList();

            var items = filtered
                .Skip((query.ValidPage - 1) * query.ValidPageSize)
                .Take(query.ValidPageSize)
                .Select(ProveedorResponse.FromDomain)
                .ToList();

            return Task.FromResult(new ProveedorPage(items, filtered.Count, query.ValidPage, query.ValidPageSize));
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}

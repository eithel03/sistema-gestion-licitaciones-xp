using Licitaciones.Application.Proveedores;
using Licitaciones.Domain.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Licitaciones.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Licitaciones.IntegrationTests.Persistence;

[Collection(PostgreSqlContainerGroup.Name)]
public sealed class ProveedorPersistenceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 10, 11, 0, 0, TimeSpan.Zero);
    private readonly PostgreSqlContainerFixture _fixture;

    public ProveedorPersistenceTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task MigrationCreatesProveedorTableAndAllowsSaving()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(_fixture.ConnectionString)
            .Options;

        await using var context = new LicitacionesDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        context.Proveedores.Add(Proveedor.Create("Empresa Central", Now));
        await context.SaveChangesAsync();

        Assert.Equal(1, await context.Proveedores.CountAsync());
    }

    [Fact]
    public async Task SavesAndRetrievesProveedor()
    {
        await using var context = await CreateContextAsync();
        var repository = new ProveedorRepository(context);
        var proveedor = Proveedor.Create("Empresa Central", Now);

        await repository.AddAsync(proveedor);
        await repository.SaveChangesAsync();

        var stored = await repository.GetByIdAsync(proveedor.Id);

        Assert.NotNull(stored);
        Assert.Equal("Empresa Central", stored.Nombre);
    }

    [Fact]
    public async Task PersistsNormalizedName()
    {
        await using var context = await CreateContextAsync();
        var proveedor = Proveedor.Create(" empresa   central ", Now);

        context.Proveedores.Add(proveedor);
        await context.SaveChangesAsync();

        var stored = await context.Proveedores.SingleAsync();

        Assert.Equal("empresa central", stored.Nombre);
        Assert.Equal("EMPRESA CENTRAL", stored.NombreNormalizado);
    }

    [Fact]
    public async Task UpdatesProveedorName()
    {
        await using var context = await CreateContextAsync();
        var repository = new ProveedorRepository(context);
        var proveedor = Proveedor.Create("Empresa Central", Now);
        await repository.AddAsync(proveedor);
        await repository.SaveChangesAsync();

        proveedor.Rename("Empresa Nacional", Now.AddHours(1));
        await repository.SaveChangesAsync();

        var stored = await repository.GetByIdAsync(proveedor.Id);

        Assert.Equal("Empresa Nacional", stored!.Nombre);
    }

    [Fact]
    public async Task UniqueIndexRejectsEquivalentNormalizedName()
    {
        await using var context = await CreateContextAsync();
        context.Proveedores.Add(Proveedor.Create("Empresa Central", Now));
        context.Proveedores.Add(Proveedor.Create(" empresa   central ", Now));

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.IsType<PostgresException>(exception.InnerException);
    }

    [Fact]
    public async Task UniqueIndexAllowsReusingNormalizedNameAfterLogicalDelete()
    {
        await using var context = await CreateContextAsync();
        var repository = new ProveedorRepository(context);
        var retired = Proveedor.Create("Empresa Central", Now);
        await repository.AddAsync(retired);
        await repository.SaveChangesAsync();
        retired.Retire(Now.AddHours(1));
        await repository.SaveChangesAsync();

        await repository.AddAsync(Proveedor.Create(" empresa   central ", Now.AddHours(2)));
        await repository.SaveChangesAsync();

        var activeProviders = await repository.ListAsync(new ProveedorQuery());
        var allProviders = await context.Proveedores.IgnoreQueryFilters().CountAsync();

        Assert.Single(activeProviders.Items);
        Assert.Equal(2, allProviders);
    }

    [Fact]
    public async Task RepositoryListFiltersSortsAndPaginatesWithPostgreSql()
    {
        await using var context = await CreateContextAsync();
        var repository = new ProveedorRepository(context);
        await repository.AddAsync(Proveedor.Create("Delta Central", Now));
        await repository.AddAsync(Proveedor.Create("Alfa Central", Now));
        await repository.AddAsync(Proveedor.Create("Beta Norte", Now));
        await repository.SaveChangesAsync();

        var ascending = await repository.ListAsync(new ProveedorQuery(Page: 1, PageSize: 1, Search: "central", Sort: "name"));
        var descending = await repository.ListAsync(new ProveedorQuery(Page: 1, PageSize: 1, Search: "central", Sort: "name_desc"));

        Assert.Equal(2, ascending.TotalItems);
        Assert.Single(ascending.Items);
        Assert.Equal("Alfa Central", ascending.Items[0].Nombre);
        Assert.Equal("Delta Central", descending.Items[0].Nombre);
    }
    [Fact]
    public async Task RetiresProveedorWithLogicalDelete()
    {
        await using var context = await CreateContextAsync();
        var repository = new ProveedorRepository(context);
        var proveedor = Proveedor.Create("Empresa Central", Now);
        await repository.AddAsync(proveedor);
        await repository.SaveChangesAsync();

        proveedor.Retire(Now.AddHours(1));
        await repository.SaveChangesAsync();

        var page = await repository.ListAsync(new ProveedorQuery());
        var stored = await context.Proveedores.IgnoreQueryFilters().SingleAsync();

        Assert.Empty(page.Items);
        Assert.NotNull(stored.DeletedAt);
    }

    private async Task<LicitacionesDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(_fixture.ConnectionString)
            .Options;

        var context = new LicitacionesDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        return context;
    }
}

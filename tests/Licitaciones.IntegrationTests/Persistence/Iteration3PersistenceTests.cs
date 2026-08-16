using Licitaciones.Application.Aprobaciones;
using Licitaciones.Application.Ofertas;
using Licitaciones.Domain.Aprobaciones;
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Ofertas;
using Licitaciones.Domain.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Licitaciones.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Licitaciones.IntegrationTests.Persistence;

[Collection(PostgreSqlContainerGroup.Name)]
public sealed class Iteration3PersistenceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);
    private readonly PostgreSqlContainerFixture _fixture;

    public Iteration3PersistenceTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SavesRetrievesAndUpdatesOfertaWithRequiredRelationships()
    {
        await using var context = await CreateCleanContextAsync();
        var (licitacion, proveedor) = await SeedRelationsAsync(context);
        var oferta = Oferta.Create(licitacion, proveedor.Id, 900m, Now);
        context.Ofertas.Add(oferta);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();
        var stored = await context.Ofertas.SingleAsync();
        var storedLicitacion = await context.Licitaciones.SingleAsync();
        stored.UpdateAmount(storedLicitacion, 800m, Now.AddMinutes(1));
        await context.SaveChangesAsync();

        Assert.Equal(800m, (await context.Ofertas.SingleAsync()).MontoOfertadoCrc);
    }

    [Fact]
    public async Task UniqueIndexRejectsDuplicateProviderForLicitacion()
    {
        await using var context = await CreateCleanContextAsync();
        var (licitacion, proveedor) = await SeedRelationsAsync(context);
        context.Ofertas.AddRange(
            Oferta.Create(licitacion, proveedor.Id, 900m, Now),
            Oferta.Create(licitacion, proveedor.Id, 800m, Now.AddMinutes(1)));

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.Equal(PostgresErrorCodes.UniqueViolation, ((PostgresException)exception.InnerException!).SqlState);
    }

    [Fact]
    public async Task ForeignKeysRejectMissingProveedor()
    {
        await using var context = await CreateCleanContextAsync();
        var licitacion = Licitacion.Create("LIC-FK", "Compra", 1000m, Now.AddDays(1), Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));
        context.Licitaciones.Add(licitacion);
        context.Ofertas.Add(Oferta.Create(licitacion, Guid.NewGuid(), 900m, Now));

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.Equal(PostgresErrorCodes.ForeignKeyViolation, ((PostgresException)exception.InnerException!).SqlState);
    }

    [Fact]
    public async Task ForeignKeyRejectsMissingLicitacion()
    {
        await using var context = await CreateCleanContextAsync();
        var proveedor = Proveedor.Create("Proveedor FK", Now);
        context.Proveedores.Add(proveedor);
        await context.SaveChangesAsync();
        var unknownLicitacion = Licitacion.Create("LIC-UNKNOWN", "Compra", 1000m, Now.AddDays(1), Now.AddHours(-1));
        unknownLicitacion.Publish(Now.AddMinutes(-30));
        context.Ofertas.Add(Oferta.Create(unknownLicitacion, proveedor.Id, 900m, Now));

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.Equal(PostgresErrorCodes.ForeignKeyViolation, ((PostgresException)exception.InnerException!).SqlState);
    }

    [Fact]
    public async Task DatabaseCheckRejectsNonPositiveOfertaAmount()
    {
        await using var context = await CreateCleanContextAsync();
        var (licitacion, proveedor) = await SeedRelationsAsync(context);

        FormattableString sql = $@"INSERT INTO ""Ofertas"" (""Id"", ""LicitacionId"", ""ProveedorId"", ""MontoOfertadoCrc"", ""FechaRegistro"", ""UpdatedAt"")
              VALUES ({Guid.NewGuid()}, {licitacion.Id}, {proveedor.Id}, {0m}, {Now}, {Now})";
        var exception = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlInterpolatedAsync(sql));

        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task SavesNivelAprobacionAndDatabaseChecksRange()
    {
        await using var context = await CreateCleanContextAsync();
        context.NivelesAprobacion.Add(NivelAprobacion.Create(0.01m, 999999.99m, "Encargado", Now));
        await context.SaveChangesAsync();

        var stored = await context.NivelesAprobacion.SingleAsync();
        Assert.Equal("Encargado", stored.Aprobador);

        FormattableString sql = $@"INSERT INTO ""NivelesAprobacion"" (""Id"", ""MontoMinimoCrc"", ""MontoMaximoCrc"", ""Aprobador"", ""CreatedAt"", ""UpdatedAt"")
              VALUES ({Guid.NewGuid()}, {100m}, {99m}, {"Invalido"}, {Now}, {Now})";
        var exception = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlInterpolatedAsync(sql));
        Assert.Equal(PostgresErrorCodes.CheckViolation, exception.SqlState);
    }

    [Fact]
    public async Task DatabaseRejectsSecondOpenApprovalRange()
    {
        await using var context = await CreateCleanContextAsync();
        context.NivelesAprobacion.AddRange(
            NivelAprobacion.Create(100m, null, "Gerencia", Now),
            NivelAprobacion.Create(1000m, null, "Junta", Now));

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.Contains(
            ((PostgresException)exception.InnerException!).SqlState,
            new[] { PostgresErrorCodes.UniqueViolation, PostgresErrorCodes.ExclusionViolation });
    }

    [Fact]
    public async Task ExclusionConstraintRejectsOverlappingApprovalRanges()
    {
        await using var context = await CreateCleanContextAsync();
        context.NivelesAprobacion.AddRange(
            NivelAprobacion.Create(0.01m, 100m, "Encargado", Now),
            NivelAprobacion.Create(100m, 200m, "Gerencia", Now));

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.Equal(PostgresErrorCodes.ExclusionViolation, ((PostgresException)exception.InnerException!).SqlState);
    }

    [Fact]
    public async Task AllMigrationsCanBeApplied()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        Assert.Empty(await context.Database.GetPendingMigrationsAsync());
        Assert.Contains((await context.Database.GetAppliedMigrationsAsync()), migration => migration.Contains("Iteration03"));
    }

    [Fact]
    public async Task ConcurrentOfertaUpdatesReturnRepositoryConcurrencyConflict()
    {
        await using (var setup = await CreateCleanContextAsync())
        {
            var (licitacion, proveedor) = await SeedRelationsAsync(setup);
            setup.Ofertas.Add(Oferta.Create(licitacion, proveedor.Id, 900m, Now));
            await setup.SaveChangesAsync();
        }

        await using var firstContext = CreateContext();
        await using var secondContext = CreateContext();
        var firstRepository = new OfertaRepository(firstContext);
        var secondRepository = new OfertaRepository(secondContext);
        var first = await firstContext.Ofertas.SingleAsync();
        var second = await secondContext.Ofertas.SingleAsync();
        var firstTender = await firstContext.Licitaciones.SingleAsync();
        var secondTender = await secondContext.Licitaciones.SingleAsync();

        first.UpdateAmount(firstTender, 850m, Now.AddMinutes(1));
        await firstRepository.SaveChangesAsync();
        second.UpdateAmount(secondTender, 800m, Now.AddMinutes(2));

        await Assert.ThrowsAsync<OfertaConcurrencyException>(() => secondRepository.SaveChangesAsync());
    }

    [Fact]
    public async Task ConcurrentApprovalLevelUpdatesReturnRepositoryConcurrencyConflict()
    {
        await using (var setup = await CreateCleanContextAsync())
        {
            setup.NivelesAprobacion.Add(NivelAprobacion.Create(0.01m, 1000m, "Encargado", Now));
            await setup.SaveChangesAsync();
        }

        await using var firstContext = CreateContext();
        await using var secondContext = CreateContext();
        var firstRepository = new NivelAprobacionRepository(firstContext);
        var secondRepository = new NivelAprobacionRepository(secondContext);
        var first = await firstContext.NivelesAprobacion.SingleAsync();
        var second = await secondContext.NivelesAprobacion.SingleAsync();

        first.Update(0.01m, 1100m, "Jefatura", Now.AddMinutes(1));
        await firstRepository.SaveChangesAsync();
        second.Update(0.01m, 1200m, "Gerencia", Now.AddMinutes(2));

        await Assert.ThrowsAsync<NivelAprobacionConcurrencyException>(() => secondRepository.SaveChangesAsync());
    }

    private static async Task<(Licitacion Licitacion, Proveedor Proveedor)> SeedRelationsAsync(LicitacionesDbContext context)
    {
        var licitacion = Licitacion.Create("LIC-INT-OF", "Compra", 1000m, Now.AddDays(1), Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));
        var proveedor = Proveedor.Create("Proveedor Integracion", Now);
        context.AddRange(licitacion, proveedor);
        await context.SaveChangesAsync();
        return (licitacion, proveedor);
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
}

using Licitaciones.Domain.Licitaciones;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Licitaciones.IntegrationTests.Persistence;

[Collection(PostgreSqlContainerGroup.Name)]
public sealed class LicitacionPersistenceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 10, 0, 0, TimeSpan.Zero);
    private readonly PostgreSqlContainerFixture _fixture;

    public LicitacionPersistenceTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SavesListsAndLogicallyDeletesLicitacion()
    {
        await using var context = await CreateContextAsync();
        var licitacion = Licitacion.Create("LIC-2026-INT", "Compra", 1000m, Now.AddDays(3), Now);
        context.Licitaciones.Add(licitacion);
        await context.SaveChangesAsync();
        licitacion.Retire(Now.AddHours(1));
        await context.SaveChangesAsync();

        Assert.Empty(await context.Licitaciones.ToListAsync());
        Assert.Equal(1, await context.Licitaciones.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task ConcurrentUpdatesDetectStaleVersion()
    {
        await using (var setup = await CreateContextAsync())
        {
            setup.Licitaciones.Add(Licitacion.Create("LIC-2026-CON", "Compra", 1000m, Now.AddDays(3), Now));
            await setup.SaveChangesAsync();
        }

        await using var first = CreateContext();
        await using var second = CreateContext();
        var firstTender = await first.Licitaciones.SingleAsync();
        var secondTender = await second.Licitaciones.SingleAsync();

        firstTender.Update("LIC-2026-CON", "Compra 1", 1200m, Now.AddDays(4), Now.AddHours(1));
        await first.SaveChangesAsync();
        secondTender.Update("LIC-2026-CON", "Compra 2", 1300m, Now.AddDays(4), Now.AddHours(2));

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => second.SaveChangesAsync());
    }

    private async Task<LicitacionesDbContext> CreateContextAsync()
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

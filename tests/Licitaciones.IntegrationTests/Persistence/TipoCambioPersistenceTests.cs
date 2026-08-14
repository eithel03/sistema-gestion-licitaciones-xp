using Licitaciones.Domain.TiposCambio;
using Licitaciones.Infrastructure.Persistence;
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

        Assert.Equal(PostgresErrorCodes.UniqueViolation, ((PostgresException)exception.InnerException!).SqlState);
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

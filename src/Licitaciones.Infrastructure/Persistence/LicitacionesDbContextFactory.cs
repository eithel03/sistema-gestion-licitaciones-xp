using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Licitaciones.Infrastructure.Persistence;

public sealed class LicitacionesDbContextFactory : IDesignTimeDbContextFactory<LicitacionesDbContext>
{
    private const string LocalDevelopmentConnectionString =
        "Host=localhost;Port=55432;Database=licitaciones_dev;Username=licitaciones_app;Password=change_this_password";

    public LicitacionesDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? LocalDevelopmentConnectionString;

        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(LicitacionesDbContext).Assembly.FullName))
            .Options;

        return new LicitacionesDbContext(options);
    }
}

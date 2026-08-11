using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Licitaciones.Infrastructure.Persistence.Repositories;
using Licitaciones.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Licitaciones.Infrastructure;

public static class DependencyInjection
{
    private const string ConnectionStringName = "DefaultConnection";
    private const string PostgreSqlHealthCheckEnabledKey = "HealthChecks:PostgreSQL:Enabled";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = GetRequiredConnectionString(configuration);

        services.AddDbContext<LicitacionesDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(LicitacionesDbContext).Assembly.FullName)));

        services.TryAddSingleton<IClock, SystemClock>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();

        return services;
    }

    public static IHealthChecksBuilder AddInfrastructureHealthChecks(
        this IHealthChecksBuilder builder,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configuration);

        if (!IsPostgreSqlHealthCheckEnabled(configuration))
        {
            return builder;
        }

        return builder.AddDbContextCheck<LicitacionesDbContext>(
            "postgresql",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["database", "postgresql"]);
    }

    private static string GetRequiredConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' is required. Configure ConnectionStrings__DefaultConnection.");
        }

        return connectionString;
    }

    private static bool IsPostgreSqlHealthCheckEnabled(IConfiguration configuration)
    {
        var value = configuration[PostgreSqlHealthCheckEnabledKey];

        return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }
}

using Microsoft.Extensions.DependencyInjection;
using Licitaciones.Application.Proveedores;
using Licitaciones.Application.Licitaciones;

namespace Licitaciones.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<ILicitacionService, LicitacionService>();

        return services;
    }
}

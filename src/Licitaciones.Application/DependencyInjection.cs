using Microsoft.Extensions.DependencyInjection;
using Licitaciones.Application.Proveedores;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Application.Ofertas;
using Licitaciones.Application.Aprobaciones;

namespace Licitaciones.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<ILicitacionService, LicitacionService>();
        services.AddScoped<IOfertaService, OfertaService>();
        services.AddScoped<INivelAprobacionService, NivelAprobacionService>();

        return services;
    }
}

using Licitaciones.Domain.Licitaciones;

namespace Licitaciones.Application.Licitaciones;

public sealed record CrearLicitacionRequest(string? Codigo, string? Titulo, decimal PresupuestoCrc, DateTimeOffset FechaCierreUtc);
public sealed record ActualizarLicitacionRequest(string? Codigo, string? Titulo, decimal PresupuestoCrc, DateTimeOffset FechaCierreUtc, uint? Version = null);
public sealed record CambiarEstadoLicitacionRequest(string? Estado);

public sealed record LicitacionResponse(
    Guid Id,
    string Codigo,
    string CodigoNormalizado,
    string Titulo,
    decimal PresupuestoCrc,
    DateTimeOffset FechaCierreUtc,
    LicitacionEstado Estado,
    LicitacionEstado EstadoEfectivo,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? PublishedAt,
    DateTimeOffset? ClosedAt,
    DateTimeOffset? DeletedAt,
    uint Version)
{
    public static LicitacionResponse FromDomain(Licitacion licitacion, DateTimeOffset utcNow)
    {
        return new LicitacionResponse(
            licitacion.Id,
            licitacion.Codigo,
            licitacion.CodigoNormalizado,
            licitacion.Titulo,
            licitacion.PresupuestoCrc,
            licitacion.FechaCierreUtc,
            licitacion.Estado,
            licitacion.GetEstadoEfectivo(utcNow),
            licitacion.CreatedAt,
            licitacion.UpdatedAt,
            licitacion.PublishedAt,
            licitacion.ClosedAt,
            licitacion.DeletedAt,
            licitacion.Version);
    }
}

using Licitaciones.Domain.Aprobaciones;

namespace Licitaciones.Application.Aprobaciones;

public sealed record CrearNivelAprobacionRequest(decimal MontoMinimoCrc, decimal? MontoMaximoCrc, string? Aprobador);
public sealed record ActualizarNivelAprobacionRequest(decimal MontoMinimoCrc, decimal? MontoMaximoCrc, string? Aprobador, uint? Version = null);

public sealed record NivelAprobacionResponse(
    Guid Id,
    decimal MontoMinimoCrc,
    decimal? MontoMaximoCrc,
    string Aprobador,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    uint Version)
{
    public static NivelAprobacionResponse FromDomain(NivelAprobacion nivel) => new(
        nivel.Id,
        nivel.MontoMinimoCrc,
        nivel.MontoMaximoCrc,
        nivel.Aprobador,
        nivel.CreatedAt,
        nivel.UpdatedAt,
        nivel.Version);
}

public sealed record NivelAprobacionQuery(int Page = 1, int PageSize = 20)
{
    public int ValidPage => Page < 1 ? 1 : Page;
    public int ValidPageSize => PageSize switch { < 1 => 20, > 100 => 100, _ => PageSize };
}

public sealed record NivelAprobacionPage(IReadOnlyList<NivelAprobacionResponse> Items, int TotalItems, int Page, int PageSize)
{
    public int TotalPages => TotalItems == 0 ? 0 : (TotalItems + PageSize - 1) / PageSize;
}

public sealed record AprobadorResponse(Guid NivelAprobacionId, string Aprobador, decimal MontoEvaluadoCrc);

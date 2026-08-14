using Licitaciones.Domain.TiposCambio;

namespace Licitaciones.Application.TiposCambio;

public enum MonedaVisualizacion
{
    CRC,
    USD
}

public sealed record CrearTipoCambioRequest(DateOnly Fecha, decimal CrcPorUsd);
public sealed record ActualizarTipoCambioRequest(DateOnly Fecha, decimal CrcPorUsd, uint? Version = null);

public sealed record TipoCambioResponse(
    Guid Id,
    DateOnly Fecha,
    decimal CrcPorUsd,
    bool Activo,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    uint Version)
{
    public static TipoCambioResponse FromDomain(TipoCambio tipoCambio) => new(
        tipoCambio.Id,
        tipoCambio.Fecha,
        tipoCambio.CrcPorUsd,
        tipoCambio.Activo,
        tipoCambio.CreatedAt,
        tipoCambio.UpdatedAt,
        tipoCambio.Version);
}

public sealed record TipoCambioQuery(int Page = 1, int PageSize = 20)
{
    public int ValidPage => Page < 1 ? 1 : Page;
    public int ValidPageSize => PageSize switch { < 1 => 20, > 100 => 100, _ => PageSize };
}

public sealed record TipoCambioPage(IReadOnlyList<TipoCambioResponse> Items, int TotalItems, int Page, int PageSize)
{
    public int TotalPages => TotalItems == 0 ? 0 : (TotalItems + PageSize - 1) / PageSize;
}

public sealed record MontoVisualizadoResponse(
    decimal MontoOriginalCrc,
    decimal MontoVisualizado,
    MonedaVisualizacion Moneda,
    Guid? TipoCambioId,
    DateOnly? FechaTipoCambio,
    decimal? CrcPorUsd);

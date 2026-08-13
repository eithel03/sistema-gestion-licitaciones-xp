using Licitaciones.Domain.Ofertas;

namespace Licitaciones.Application.Ofertas;

public sealed record CrearOfertaRequest(Guid LicitacionId, Guid ProveedorId, decimal MontoOfertadoCrc);
public sealed record ActualizarOfertaRequest(decimal MontoOfertadoCrc, uint? Version = null);

public sealed record OfertaResponse(
    Guid Id,
    Guid LicitacionId,
    Guid ProveedorId,
    decimal MontoOfertadoCrc,
    DateTimeOffset FechaRegistro,
    DateTimeOffset UpdatedAt,
    uint Version)
{
    public static OfertaResponse FromDomain(Oferta oferta) => new(
        oferta.Id,
        oferta.LicitacionId,
        oferta.ProveedorId,
        oferta.MontoOfertadoCrc,
        oferta.FechaRegistro,
        oferta.UpdatedAt,
        oferta.Version);
}

public sealed record OfertaQuery(
    int Page = 1,
    int PageSize = 10,
    Guid? LicitacionId = null,
    Guid? ProveedorId = null,
    string? Sort = "registered")
{
    public int ValidPage => Page < 1 ? 1 : Page;
    public int ValidPageSize => PageSize switch { < 1 => 10, > 100 => 100, _ => PageSize };
}

public sealed record OfertaPage(IReadOnlyList<OfertaResponse> Items, int TotalItems, int Page, int PageSize)
{
    public int TotalPages => TotalItems == 0 ? 0 : (TotalItems + PageSize - 1) / PageSize;
}

public sealed record MejorOfertaResponse(
    bool TieneOferta,
    OfertaResponse? MejorOferta,
    decimal? AhorroCrc,
    decimal? PorcentajeAhorro,
    ClasificacionOferta Clasificacion,
    string DescripcionClasificacion,
    string? Aprobador = null);

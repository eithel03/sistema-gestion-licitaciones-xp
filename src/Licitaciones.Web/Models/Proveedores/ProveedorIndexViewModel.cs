using Licitaciones.Application.Proveedores;

namespace Licitaciones.Web.Models.Proveedores;

public sealed record ProveedorIndexViewModel(
    IReadOnlyList<ProveedorResponse> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    string? Search,
    string Sort)
{
    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => TotalPages > Page;
}

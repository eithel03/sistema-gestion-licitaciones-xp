using Licitaciones.Application.Licitaciones;

namespace Licitaciones.Web.Models.Licitaciones;

public sealed record LicitacionIndexViewModel(
    IReadOnlyList<LicitacionResponse> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    string? Search,
    string? Sort)
{
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

namespace Licitaciones.Application.Licitaciones;

public sealed record LicitacionQuery(int Page = 1, int PageSize = 10, string? Search = null, string? Sort = "code")
{
    public int ValidPage => Page < 1 ? 1 : Page;
    public int ValidPageSize => PageSize switch { < 1 => 10, > 100 => 100, _ => PageSize };
}

public sealed record LicitacionPage(IReadOnlyList<LicitacionResponse> Items, int TotalItems, int Page, int PageSize)
{
    public int TotalPages => TotalItems == 0 ? 0 : (int)Math.Ceiling(TotalItems / (double)PageSize);
}

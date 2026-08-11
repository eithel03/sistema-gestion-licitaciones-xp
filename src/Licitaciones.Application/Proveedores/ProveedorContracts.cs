using Licitaciones.Domain.Proveedores;

namespace Licitaciones.Application.Proveedores;

public sealed record CrearProveedorRequest(string? Nombre);

public sealed record ActualizarProveedorRequest(string? Nombre);

public sealed record ProveedorResponse(
    Guid Id,
    string Nombre,
    string NombreNormalizado,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? DeletedAt)
{
    public static ProveedorResponse FromDomain(Proveedor proveedor)
    {
        ArgumentNullException.ThrowIfNull(proveedor);

        return new ProveedorResponse(
            proveedor.Id,
            proveedor.Nombre,
            proveedor.NombreNormalizado,
            proveedor.CreatedAt,
            proveedor.UpdatedAt,
            proveedor.DeletedAt);
    }
}

public sealed record ProveedorQuery(int Page = 1, int PageSize = 10, string? Search = null, string? Sort = "name")
{
    public int ValidPage => Page < 1 ? 1 : Page;

    public int ValidPageSize => PageSize switch
    {
        < 1 => 10,
        > 100 => 100,
        _ => PageSize
    };
}

public sealed record ProveedorPage(
    IReadOnlyList<ProveedorResponse> Items,
    int TotalItems,
    int Page,
    int PageSize)
{
    public int TotalPages => TotalItems == 0 ? 0 : (int)Math.Ceiling(TotalItems / (double)PageSize);
}

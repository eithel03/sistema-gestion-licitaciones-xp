using Licitaciones.Domain.Proveedores;

namespace Licitaciones.Application.Proveedores;

public interface IProveedorRepository
{
    Task AddAsync(Proveedor proveedor, CancellationToken cancellationToken = default);

    Task<Proveedor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNormalizedNameAsync(
        string normalizedName,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default);

    Task<ProveedorPage> ListAsync(ProveedorQuery query, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

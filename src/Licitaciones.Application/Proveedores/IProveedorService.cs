namespace Licitaciones.Application.Proveedores;

public interface IProveedorService
{
    Task<ProveedorResult<ProveedorResponse>> CreateAsync(
        CrearProveedorRequest request,
        CancellationToken cancellationToken = default);

    Task<ProveedorResult<ProveedorResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ProveedorResult<ProveedorPage>> ListAsync(
        ProveedorQuery query,
        CancellationToken cancellationToken = default);

    Task<ProveedorResult<ProveedorResponse>> UpdateAsync(
        Guid id,
        ActualizarProveedorRequest request,
        CancellationToken cancellationToken = default);

    Task<ProveedorResult<bool>> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}

using Licitaciones.Domain.Licitaciones;

namespace Licitaciones.Application.Licitaciones;

public interface ILicitacionRepository
{
    Task AddAsync(Licitacion licitacion, CancellationToken cancellationToken = default);
    Task<Licitacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, Guid? excludedId = null, CancellationToken cancellationToken = default);
    Task<LicitacionPage> ListAsync(LicitacionQuery query, DateTimeOffset utcNow, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface ILicitacionService
{
    Task<LicitacionResult<LicitacionResponse>> CreateAsync(CrearLicitacionRequest request, CancellationToken cancellationToken = default);
    Task<LicitacionResult<LicitacionResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LicitacionResult<LicitacionPage>> ListAsync(LicitacionQuery query, CancellationToken cancellationToken = default);
    Task<LicitacionResult<LicitacionResponse>> UpdateAsync(Guid id, ActualizarLicitacionRequest request, CancellationToken cancellationToken = default);
    Task<LicitacionResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LicitacionResult<LicitacionResponse>> PublishAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LicitacionResult<LicitacionResponse>> CloseAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LicitacionResult<LicitacionResponse>> ChangeEstadoAsync(Guid id, CambiarEstadoLicitacionRequest request, CancellationToken cancellationToken = default);
}

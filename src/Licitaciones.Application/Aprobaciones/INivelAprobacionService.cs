using Licitaciones.Domain.Aprobaciones;

namespace Licitaciones.Application.Aprobaciones;

public interface INivelAprobacionRepository
{
    Task AddAsync(NivelAprobacion nivel, CancellationToken cancellationToken = default);
    Task<NivelAprobacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NivelAprobacionPage> ListAsync(NivelAprobacionQuery query, CancellationToken cancellationToken = default);
    Task<bool> HasOverlapAsync(decimal minimum, decimal? maximum, Guid? excludedId = null, CancellationToken cancellationToken = default);
    Task<bool> HasOpenRangeAsync(Guid? excludedId = null, CancellationToken cancellationToken = default);
    Task<NivelAprobacion?> FindByAmountAsync(decimal amount, CancellationToken cancellationToken = default);
    void Remove(NivelAprobacion nivel);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface INivelAprobacionService
{
    Task<NivelAprobacionResult<NivelAprobacionResponse>> CreateAsync(CrearNivelAprobacionRequest request, CancellationToken cancellationToken = default);
    Task<NivelAprobacionResult<NivelAprobacionResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NivelAprobacionResult<NivelAprobacionPage>> ListAsync(NivelAprobacionQuery query, CancellationToken cancellationToken = default);
    Task<NivelAprobacionResult<NivelAprobacionResponse>> UpdateAsync(Guid id, ActualizarNivelAprobacionRequest request, CancellationToken cancellationToken = default);
    Task<NivelAprobacionResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NivelAprobacionResult<AprobadorResponse>> FindApproverAsync(decimal amount, CancellationToken cancellationToken = default);
}

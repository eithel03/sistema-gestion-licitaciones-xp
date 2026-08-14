using Licitaciones.Domain.TiposCambio;

namespace Licitaciones.Application.TiposCambio;

public interface ITipoCambioRepository
{
    Task AddAsync(TipoCambio tipoCambio, CancellationToken cancellationToken = default);
    Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TipoCambio?> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<TipoCambioPage> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default);
    Task DeactivateAllExceptAsync(Guid activeId, DateTimeOffset updatedAt, CancellationToken cancellationToken = default);
    void Remove(TipoCambio tipoCambio);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface ITipoCambioService
{
    Task<TipoCambioResult<TipoCambioResponse>> CreateAsync(CrearTipoCambioRequest request, CancellationToken cancellationToken = default);
    Task<TipoCambioResult<TipoCambioResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TipoCambioResult<TipoCambioResponse>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<TipoCambioResult<TipoCambioPage>> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default);
    Task<TipoCambioResult<TipoCambioResponse>> UpdateAsync(Guid id, ActualizarTipoCambioRequest request, CancellationToken cancellationToken = default);
    Task<TipoCambioResult<TipoCambioResponse>> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TipoCambioResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IMonedaConversionService
{
    Task<TipoCambioResult<MontoVisualizadoResponse>> ConvertFromCrcAsync(decimal amountCrc, MonedaVisualizacion targetCurrency, CancellationToken cancellationToken = default);
}

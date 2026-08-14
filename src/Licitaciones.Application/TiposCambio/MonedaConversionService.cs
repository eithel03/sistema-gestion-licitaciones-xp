using Licitaciones.Domain.TiposCambio;

namespace Licitaciones.Application.TiposCambio;

public sealed class MonedaConversionService : IMonedaConversionService
{
    private readonly ITipoCambioRepository _repository;

    public MonedaConversionService(ITipoCambioRepository repository)
    {
        _repository = repository;
    }

    public async Task<TipoCambioResult<MontoVisualizadoResponse>> ConvertFromCrcAsync(
        decimal amountCrc,
        MonedaVisualizacion targetCurrency,
        CancellationToken cancellationToken = default)
    {
        if (targetCurrency == MonedaVisualizacion.CRC)
        {
            return TipoCambioResult.Success(new MontoVisualizadoResponse(amountCrc, amountCrc, MonedaVisualizacion.CRC, null, null, null));
        }

        var active = await _repository.GetActiveAsync(cancellationToken);
        if (active is null)
        {
            return TipoCambioResult.Failure<MontoVisualizadoResponse>(
                TipoCambioResultStatus.NotFound,
                TipoCambioErrors.ActivoNoEncontrado,
                "No existe un tipo de cambio activo.");
        }

        return TipoCambioResult.Success(new MontoVisualizadoResponse(
            amountCrc,
            active.ConvertCrcToUsd(amountCrc),
            MonedaVisualizacion.USD,
            active.Id,
            active.Fecha,
            active.CrcPorUsd));
    }
}

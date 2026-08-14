using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Domain.TiposCambio;

namespace Licitaciones.Application.TiposCambio;

public sealed class TipoCambioService : ITipoCambioService
{
    private readonly ITipoCambioRepository _repository;
    private readonly IClock _clock;

    public TipoCambioService(ITipoCambioRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<TipoCambioResult<TipoCambioResponse>> CreateAsync(CrearTipoCambioRequest request, CancellationToken cancellationToken = default)
    {
        TipoCambio tipoCambio;
        try
        {
            tipoCambio = TipoCambio.Create(request.Fecha, request.CrcPorUsd, _clock.UtcNow);
        }
        catch (TipoCambioValidationException exception)
        {
            return Validation<TipoCambioResponse>(exception);
        }

        await _repository.AddAsync(tipoCambio, cancellationToken);
        return await SaveAsync(tipoCambio, cancellationToken);
    }

    public async Task<TipoCambioResult<TipoCambioResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tipoCambio = await _repository.GetByIdAsync(id, cancellationToken);
        return tipoCambio is null ? NotFound<TipoCambioResponse>() : TipoCambioResult.Success(TipoCambioResponse.FromDomain(tipoCambio));
    }

    public async Task<TipoCambioResult<TipoCambioResponse>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var tipoCambio = await _repository.GetActiveAsync(cancellationToken);
        return tipoCambio is null ? ActiveNotFound<TipoCambioResponse>() : TipoCambioResult.Success(TipoCambioResponse.FromDomain(tipoCambio));
    }

    public async Task<TipoCambioResult<TipoCambioPage>> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default) =>
        TipoCambioResult.Success(await _repository.ListAsync(query, cancellationToken));

    public async Task<TipoCambioResult<TipoCambioResponse>> UpdateAsync(Guid id, ActualizarTipoCambioRequest request, CancellationToken cancellationToken = default)
    {
        var tipoCambio = await _repository.GetByIdAsync(id, cancellationToken);
        if (tipoCambio is null) return NotFound<TipoCambioResponse>();
        if (request.Version.HasValue && request.Version != tipoCambio.Version) return Concurrency<TipoCambioResponse>();

        try
        {
            tipoCambio.Update(request.Fecha, request.CrcPorUsd, _clock.UtcNow);
        }
        catch (TipoCambioValidationException exception)
        {
            return Validation<TipoCambioResponse>(exception);
        }

        return await SaveAsync(tipoCambio, cancellationToken);
    }

    public async Task<TipoCambioResult<TipoCambioResponse>> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tipoCambio = await _repository.GetByIdAsync(id, cancellationToken);
        if (tipoCambio is null) return NotFound<TipoCambioResponse>();

        var now = _clock.UtcNow;
        await _repository.DeactivateAllExceptAsync(id, now, cancellationToken);
        tipoCambio.Activate(now);
        return await SaveAsync(tipoCambio, cancellationToken);
    }

    public async Task<TipoCambioResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tipoCambio = await _repository.GetByIdAsync(id, cancellationToken);
        if (tipoCambio is null) return NotFound<bool>();
        _repository.Remove(tipoCambio);
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            return TipoCambioResult.Success(true);
        }
        catch (TipoCambioConcurrencyException)
        {
            return Concurrency<bool>();
        }
        catch (TipoCambioActiveConflictException)
        {
            return ActiveConflict<bool>();
        }
    }

    private async Task<TipoCambioResult<TipoCambioResponse>> SaveAsync(TipoCambio tipoCambio, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            return TipoCambioResult.Success(TipoCambioResponse.FromDomain(tipoCambio));
        }
        catch (TipoCambioConcurrencyException)
        {
            return Concurrency<TipoCambioResponse>();
        }
        catch (TipoCambioActiveConflictException)
        {
            return ActiveConflict<TipoCambioResponse>();
        }
    }

    private static TipoCambioResult<T> Validation<T>(TipoCambioValidationException exception)
    {
        var error = exception.Errors[0];
        return TipoCambioResult.Failure<T>(TipoCambioResultStatus.ValidationError, error.Code, error.Message);
    }

    private static TipoCambioResult<T> NotFound<T>() => TipoCambioResult.Failure<T>(TipoCambioResultStatus.NotFound, TipoCambioErrors.NoEncontrado, "El tipo de cambio solicitado no existe.");
    private static TipoCambioResult<T> ActiveNotFound<T>() => TipoCambioResult.Failure<T>(TipoCambioResultStatus.NotFound, TipoCambioErrors.ActivoNoEncontrado, "No existe un tipo de cambio activo.");
    private static TipoCambioResult<T> Concurrency<T>() => TipoCambioResult.Failure<T>(TipoCambioResultStatus.ConcurrencyConflict, TipoCambioErrors.Concurrencia, "El tipo de cambio fue modificado por otro proceso.");
    private static TipoCambioResult<T> ActiveConflict<T>() => TipoCambioResult.Failure<T>(TipoCambioResultStatus.Conflict, TipoCambioErrors.ActivoDuplicado, "Solo puede existir un tipo de cambio activo.");
}

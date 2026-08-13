using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Domain.Aprobaciones;

namespace Licitaciones.Application.Aprobaciones;

public sealed class NivelAprobacionService : INivelAprobacionService
{
    private readonly INivelAprobacionRepository _repository;
    private readonly IClock _clock;

    public NivelAprobacionService(INivelAprobacionRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<NivelAprobacionResult<NivelAprobacionResponse>> CreateAsync(CrearNivelAprobacionRequest request, CancellationToken cancellationToken = default)
    {
        NivelAprobacion nivel;
        try
        {
            nivel = NivelAprobacion.Create(request.MontoMinimoCrc, request.MontoMaximoCrc, request.Aprobador, _clock.UtcNow);
        }
        catch (NivelAprobacionValidationException exception)
        {
            return Validation<NivelAprobacionResponse>(exception);
        }

        var conflict = await ValidateRangeAsync(nivel.MontoMinimoCrc, nivel.MontoMaximoCrc, null, cancellationToken);
        if (conflict is not null) return conflict;
        await _repository.AddAsync(nivel, cancellationToken);
        return await SaveAsync(nivel, cancellationToken);
    }

    public async Task<NivelAprobacionResult<NivelAprobacionResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var nivel = await _repository.GetByIdAsync(id, cancellationToken);
        return nivel is null ? NotFound<NivelAprobacionResponse>() : NivelAprobacionResult.Success(NivelAprobacionResponse.FromDomain(nivel));
    }

    public async Task<NivelAprobacionResult<NivelAprobacionPage>> ListAsync(NivelAprobacionQuery query, CancellationToken cancellationToken = default) =>
        NivelAprobacionResult.Success(await _repository.ListAsync(query, cancellationToken));

    public async Task<NivelAprobacionResult<NivelAprobacionResponse>> UpdateAsync(Guid id, ActualizarNivelAprobacionRequest request, CancellationToken cancellationToken = default)
    {
        var nivel = await _repository.GetByIdAsync(id, cancellationToken);
        if (nivel is null) return NotFound<NivelAprobacionResponse>();
        if (request.Version.HasValue && request.Version != nivel.Version) return Concurrency<NivelAprobacionResponse>();

        try
        {
            _ = NivelAprobacion.Create(request.MontoMinimoCrc, request.MontoMaximoCrc, request.Aprobador, _clock.UtcNow);
        }
        catch (NivelAprobacionValidationException exception)
        {
            return Validation<NivelAprobacionResponse>(exception);
        }

        var conflict = await ValidateRangeAsync(request.MontoMinimoCrc, request.MontoMaximoCrc, id, cancellationToken);
        if (conflict is not null) return conflict;
        nivel.Update(request.MontoMinimoCrc, request.MontoMaximoCrc, request.Aprobador, _clock.UtcNow);
        return await SaveAsync(nivel, cancellationToken);
    }

    public async Task<NivelAprobacionResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var nivel = await _repository.GetByIdAsync(id, cancellationToken);
        if (nivel is null) return NotFound<bool>();
        _repository.Remove(nivel);
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            return NivelAprobacionResult.Success(true);
        }
        catch (NivelAprobacionConcurrencyException)
        {
            return Concurrency<bool>();
        }
        catch (NivelAprobacionRangeConflictException)
        {
            return RangeConflict<bool>();
        }
    }

    public async Task<NivelAprobacionResult<AprobadorResponse>> FindApproverAsync(decimal amount, CancellationToken cancellationToken = default)
    {
        var nivel = await _repository.FindByAmountAsync(amount, cancellationToken);
        return nivel is null
            ? NivelAprobacionResult.Failure<AprobadorResponse>(NivelAprobacionResultStatus.NotFound, NivelAprobacionErrors.AprobadorNoEncontrado, "No existe un aprobador configurado para el monto indicado.")
            : NivelAprobacionResult.Success(new AprobadorResponse(nivel.Id, nivel.Aprobador, amount));
    }

    private async Task<NivelAprobacionResult<NivelAprobacionResponse>?> ValidateRangeAsync(
        decimal minimum,
        decimal? maximum,
        Guid? excludedId,
        CancellationToken cancellationToken)
    {
        if (!maximum.HasValue && await _repository.HasOpenRangeAsync(excludedId, cancellationToken))
        {
            return NivelAprobacionResult.Failure<NivelAprobacionResponse>(NivelAprobacionResultStatus.Conflict, NivelAprobacionErrors.SegundoRangoAbierto, "Solo puede existir un rango de aprobacion abierto.");
        }

        if (await _repository.HasOverlapAsync(minimum, maximum, excludedId, cancellationToken))
        {
            return NivelAprobacionResult.Failure<NivelAprobacionResponse>(NivelAprobacionResultStatus.Conflict, NivelAprobacionErrors.RangoTraslapado, "El rango de aprobacion se traslapa con otro existente.");
        }

        return null;
    }

    private async Task<NivelAprobacionResult<NivelAprobacionResponse>> SaveAsync(NivelAprobacion nivel, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            return NivelAprobacionResult.Success(NivelAprobacionResponse.FromDomain(nivel));
        }
        catch (NivelAprobacionConcurrencyException)
        {
            return Concurrency<NivelAprobacionResponse>();
        }
        catch (NivelAprobacionRangeConflictException)
        {
            return RangeConflict<NivelAprobacionResponse>();
        }
    }

    private static NivelAprobacionResult<T> Validation<T>(NivelAprobacionValidationException exception)
    {
        var error = exception.Errors[0];
        return NivelAprobacionResult.Failure<T>(NivelAprobacionResultStatus.ValidationError, error.Code, error.Message);
    }

    private static NivelAprobacionResult<T> NotFound<T>() => NivelAprobacionResult.Failure<T>(NivelAprobacionResultStatus.NotFound, NivelAprobacionErrors.NoEncontrado, "El nivel de aprobacion solicitado no existe.");
    private static NivelAprobacionResult<T> Concurrency<T>() => NivelAprobacionResult.Failure<T>(NivelAprobacionResultStatus.ConcurrencyConflict, NivelAprobacionErrors.Concurrencia, "El nivel de aprobacion fue modificado por otro proceso.");
    private static NivelAprobacionResult<T> RangeConflict<T>() => NivelAprobacionResult.Failure<T>(NivelAprobacionResultStatus.Conflict, NivelAprobacionErrors.RangoTraslapado, "El rango de aprobacion se traslapa con otro existente.");
}

using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Domain.Licitaciones;

namespace Licitaciones.Application.Licitaciones;

public sealed class LicitacionService : ILicitacionService
{
    private readonly ILicitacionRepository _repository;
    private readonly IClock _clock;

    public LicitacionService(ILicitacionRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<LicitacionResult<LicitacionResponse>> CreateAsync(CrearLicitacionRequest request, CancellationToken cancellationToken = default)
    {
        var created = CreateLicitacion(request);
        if (!created.Succeeded) return Fail<LicitacionResponse, Licitacion>(created);
        var licitacion = created.Value!;
        if (await _repository.ExistsByNormalizedCodeAsync(licitacion.CodigoNormalizado, cancellationToken: cancellationToken)) return Duplicate<LicitacionResponse>();
        await _repository.AddAsync(licitacion, cancellationToken);
        return await SaveAndReturnAsync(licitacion, cancellationToken);
    }

    public async Task<LicitacionResult<LicitacionResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var licitacion = await _repository.GetByIdAsync(id, cancellationToken);
        return licitacion is null || licitacion.DeletedAt is not null ? NotFound<LicitacionResponse>() : Success(licitacion);
    }
    public async Task<LicitacionResult<LicitacionPage>> ListAsync(LicitacionQuery query, CancellationToken cancellationToken = default)
    {
        return LicitacionResult.Success(await _repository.ListAsync(query, _clock.UtcNow, cancellationToken));
    }

    public async Task<LicitacionResult<LicitacionResponse>> UpdateAsync(Guid id, ActualizarLicitacionRequest request, CancellationToken cancellationToken = default)
    {
        var licitacion = await _repository.GetByIdAsync(id, cancellationToken);
        if (licitacion is null || licitacion.DeletedAt is not null) return NotFound<LicitacionResponse>();
        if (request.Version.HasValue && request.Version.Value != licitacion.Version) return Concurrency<LicitacionResponse>();
        var normalized = NormalizeCode(request.Codigo);
        if (!normalized.Succeeded) return Fail<LicitacionResponse, string>(normalized);
        if (normalized.Value != licitacion.CodigoNormalizado && await _repository.ExistsByNormalizedCodeAsync(normalized.Value!, id, cancellationToken)) return Duplicate<LicitacionResponse>();
        try
        {
            licitacion.Update(request.Codigo, request.Titulo, request.PresupuestoCrc, request.FechaCierreUtc, _clock.UtcNow);
        }
        catch (LicitacionValidationException ex)
        {
            return ValidationFailure<LicitacionResponse>(ex);
        }
        return await SaveAndReturnAsync(licitacion, cancellationToken);
    }

    public async Task<LicitacionResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var licitacion = await _repository.GetByIdAsync(id, cancellationToken);
        if (licitacion is null || licitacion.DeletedAt is not null) return NotFound<bool>();
        try { licitacion.Retire(_clock.UtcNow); }
        catch (LicitacionValidationException ex) { return ValidationFailure<bool>(ex); }
        return await SaveAndReturnAsync(licitacion, cancellationToken) is { Succeeded: true } ? LicitacionResult.Success(true) : Concurrency<bool>();
    }
    public async Task<LicitacionResult<LicitacionResponse>> PublishAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var licitacion = await _repository.GetByIdAsync(id, cancellationToken);
        if (licitacion is null || licitacion.DeletedAt is not null) return NotFound<LicitacionResponse>();
        try { licitacion.Publish(_clock.UtcNow); }
        catch (LicitacionValidationException ex) { return ValidationFailure<LicitacionResponse>(ex); }
        return await SaveAndReturnAsync(licitacion, cancellationToken);
    }

    public async Task<LicitacionResult<LicitacionResponse>> CloseAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var licitacion = await _repository.GetByIdAsync(id, cancellationToken);
        if (licitacion is null || licitacion.DeletedAt is not null) return NotFound<LicitacionResponse>();
        try { licitacion.Close(_clock.UtcNow); }
        catch (LicitacionValidationException ex) { return ValidationFailure<LicitacionResponse>(ex); }
        return await SaveAndReturnAsync(licitacion, cancellationToken);
    }

    public async Task<LicitacionResult<LicitacionResponse>> ChangeEstadoAsync(Guid id, CambiarEstadoLicitacionRequest request, CancellationToken cancellationToken = default)
    {
        var licitacion = await _repository.GetByIdAsync(id, cancellationToken);
        if (licitacion is null || licitacion.DeletedAt is not null) return NotFound<LicitacionResponse>();
        if (!Enum.TryParse<LicitacionEstado>(request.Estado, ignoreCase: true, out var estado))
        {
            return LicitacionResult.Failure<LicitacionResponse>(
                LicitacionResultStatus.ValidationError,
                LicitacionErrors.TransicionInvalida,
                "El estado solicitado no es valido.");
        }
        try { licitacion.ChangeEstado(estado, _clock.UtcNow); }
        catch (LicitacionValidationException ex) { return ValidationFailure<LicitacionResponse>(ex); }
        return await SaveAndReturnAsync(licitacion, cancellationToken);
    }

    private LicitacionResult<Licitacion> CreateLicitacion(CrearLicitacionRequest request)
    {
        try { return LicitacionResult.Success(Licitacion.Create(request.Codigo, request.Titulo, request.PresupuestoCrc, request.FechaCierreUtc, _clock.UtcNow)); }
        catch (LicitacionValidationException ex) { return ValidationFailure<Licitacion>(ex); }
    }

    private static LicitacionResult<string> NormalizeCode(string? codigo)
    {
        try { return LicitacionResult.Success(Licitacion.Create(codigo, "Temporal", 1m, DateTimeOffset.UnixEpoch.AddDays(1), DateTimeOffset.UnixEpoch).CodigoNormalizado); }
        catch (LicitacionValidationException ex) { return ValidationFailure<string>(ex); }
    }
    private async Task<LicitacionResult<LicitacionResponse>> SaveAndReturnAsync(Licitacion licitacion, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            return Success(licitacion);
        }
        catch (LicitacionConcurrencyException)
        {
            return Concurrency<LicitacionResponse>();
        }
    }

    private LicitacionResult<LicitacionResponse> Success(Licitacion licitacion) =>
        LicitacionResult.Success(LicitacionResponse.FromDomain(licitacion, _clock.UtcNow));

    private static LicitacionResult<T> ValidationFailure<T>(LicitacionValidationException exception)
    {
        var error = exception.Errors.First();
        return LicitacionResult.Failure<T>(LicitacionResultStatus.ValidationError, error.Code, error.Message);
    }

    private static LicitacionResult<T> Duplicate<T>() =>
        LicitacionResult.Failure<T>(LicitacionResultStatus.Conflict, LicitacionErrors.CodigoDuplicado, "Ya existe una licitacion con un codigo equivalente.");

    private static LicitacionResult<T> NotFound<T>() =>
        LicitacionResult.Failure<T>(LicitacionResultStatus.NotFound, LicitacionErrors.NoEncontrada, "La licitacion solicitada no existe.");

    private static LicitacionResult<T> Concurrency<T>() =>
        LicitacionResult.Failure<T>(LicitacionResultStatus.ConcurrencyConflict, LicitacionErrors.Concurrencia, "La licitacion fue modificada por otro proceso.");

    private static LicitacionResult<T> Fail<T, TOther>(LicitacionResult<TOther> result) =>
        LicitacionResult.Failure<T>(result.Status, result.ErrorCode!, result.ErrorMessage!);
}

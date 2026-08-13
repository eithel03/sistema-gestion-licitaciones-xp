namespace Licitaciones.Application.Aprobaciones;

public enum NivelAprobacionResultStatus
{
    Success,
    ValidationError,
    NotFound,
    Conflict,
    ConcurrencyConflict
}

public static class NivelAprobacionResult
{
    public static NivelAprobacionResult<T> Success<T>(T value) => new(NivelAprobacionResultStatus.Success, value, null, null);
    public static NivelAprobacionResult<T> Failure<T>(NivelAprobacionResultStatus status, string code, string message) => new(status, default, code, message);
}

public sealed record NivelAprobacionResult<T>(NivelAprobacionResultStatus Status, T? Value, string? ErrorCode, string? ErrorMessage)
{
    public bool Succeeded => Status == NivelAprobacionResultStatus.Success;
}

namespace Licitaciones.Application.TiposCambio;

public enum TipoCambioResultStatus
{
    Success,
    ValidationError,
    NotFound,
    Conflict,
    ConcurrencyConflict
}

public static class TipoCambioResult
{
    public static TipoCambioResult<T> Success<T>(T value) => new(TipoCambioResultStatus.Success, value, null, null);
    public static TipoCambioResult<T> Failure<T>(TipoCambioResultStatus status, string code, string message) => new(status, default, code, message);
}

public sealed record TipoCambioResult<T>(TipoCambioResultStatus Status, T? Value, string? ErrorCode, string? ErrorMessage)
{
    public bool Succeeded => Status == TipoCambioResultStatus.Success;
}

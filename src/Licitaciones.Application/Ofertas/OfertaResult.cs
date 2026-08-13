namespace Licitaciones.Application.Ofertas;

public enum OfertaResultStatus
{
    Success,
    ValidationError,
    NotFound,
    Conflict,
    ConcurrencyConflict
}

public static class OfertaResult
{
    public static OfertaResult<T> Success<T>(T value) => new(OfertaResultStatus.Success, value, null, null);
    public static OfertaResult<T> Failure<T>(OfertaResultStatus status, string code, string message) => new(status, default, code, message);
}

public sealed record OfertaResult<T>(OfertaResultStatus Status, T? Value, string? ErrorCode, string? ErrorMessage)
{
    public bool Succeeded => Status == OfertaResultStatus.Success;
}

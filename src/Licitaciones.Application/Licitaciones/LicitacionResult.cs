namespace Licitaciones.Application.Licitaciones;

public enum LicitacionResultStatus
{
    Success,
    ValidationError,
    NotFound,
    Conflict,
    ConcurrencyConflict
}

public static class LicitacionResult
{
    public static LicitacionResult<T> Success<T>(T value) => new(LicitacionResultStatus.Success, value, null, null);
    public static LicitacionResult<T> Failure<T>(LicitacionResultStatus status, string errorCode, string errorMessage)
    {
        if (status == LicitacionResultStatus.Success) throw new ArgumentException("Failure status cannot be Success.", nameof(status));
        return new LicitacionResult<T>(status, default, errorCode, errorMessage);
    }
}

public sealed class LicitacionResult<T>
{
    internal LicitacionResult(LicitacionResultStatus status, T? value, string? errorCode, string? errorMessage)
    {
        Status = status;
        Value = value;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public LicitacionResultStatus Status { get; }
    public bool Succeeded => Status == LicitacionResultStatus.Success;
    public T? Value { get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
}

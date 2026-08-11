namespace Licitaciones.Application.Proveedores;

public enum ProveedorResultStatus
{
    Success,
    ValidationError,
    NotFound,
    Conflict
}

public static class ProveedorResult
{
    public static ProveedorResult<T> Success<T>(T value)
    {
        return new ProveedorResult<T>(ProveedorResultStatus.Success, value, null, null);
    }

    public static ProveedorResult<T> Failure<T>(
        ProveedorResultStatus status,
        string errorCode,
        string errorMessage)
    {
        if (status == ProveedorResultStatus.Success)
        {
            throw new ArgumentException("Failure status cannot be Success.", nameof(status));
        }

        return new ProveedorResult<T>(status, default, errorCode, errorMessage);
    }
}

public sealed class ProveedorResult<T>
{
    internal ProveedorResult(ProveedorResultStatus status, T? value, string? errorCode, string? errorMessage)
    {
        Status = status;
        Value = value;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public ProveedorResultStatus Status { get; }

    public bool Succeeded => Status == ProveedorResultStatus.Success;

    public T? Value { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }
}

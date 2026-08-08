namespace Licitaciones.Domain.Common;

public sealed class ValidationResult
{
    private static readonly ValidationResult ValidResult = new([]);

    private ValidationResult(IReadOnlyCollection<ValidationError> errors)
    {
        Errors = errors;
    }

    public bool IsValid => Errors.Count == 0;

    public IReadOnlyCollection<ValidationError> Errors { get; }

    public static ValidationResult Success()
    {
        return ValidResult;
    }

    public static ValidationResult Failure(params ValidationError[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (errors.Length == 0)
        {
            throw new ArgumentException("At least one validation error is required.", nameof(errors));
        }

        return new ValidationResult(Array.AsReadOnly(errors));
    }
}

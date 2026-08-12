namespace Licitaciones.Domain.Licitaciones;

public sealed class LicitacionValidationException : Exception
{
    public LicitacionValidationException(params LicitacionValidationError[] errors)
        : base(errors.Length == 0 ? "Licitacion invalida." : errors[0].Message)
    {
        if (errors.Length == 0)
        {
            throw new ArgumentException("At least one validation error is required.", nameof(errors));
        }

        Errors = Array.AsReadOnly(errors);
    }

    public IReadOnlyCollection<LicitacionValidationError> Errors { get; }
}

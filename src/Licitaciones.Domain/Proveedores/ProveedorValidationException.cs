namespace Licitaciones.Domain.Proveedores;

public sealed class ProveedorValidationException : Exception
{
    public ProveedorValidationException(params ProveedorValidationError[] errors)
        : base(errors.Length == 0 ? "Proveedor invalido." : errors[0].Message)
    {
        if (errors.Length == 0)
        {
            throw new ArgumentException("At least one validation error is required.", nameof(errors));
        }

        Errors = Array.AsReadOnly(errors);
    }

    public IReadOnlyCollection<ProveedorValidationError> Errors { get; }
}

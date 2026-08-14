namespace Licitaciones.Domain.TiposCambio;

public sealed class TipoCambioValidationException : Exception
{
    public TipoCambioValidationException(params TipoCambioValidationError[] errors)
        : base(errors.Length == 0 ? "Tipo de cambio invalido." : errors[0].Message)
    {
        Errors = errors;
    }

    public IReadOnlyList<TipoCambioValidationError> Errors { get; }
}

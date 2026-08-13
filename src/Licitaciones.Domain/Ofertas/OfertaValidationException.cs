namespace Licitaciones.Domain.Ofertas;

public sealed record OfertaValidationError(string Code, string Message);

public sealed class OfertaValidationException : Exception
{
    public OfertaValidationException(params OfertaValidationError[] errors)
        : base(errors.FirstOrDefault()?.Message ?? "La oferta no es valida.")
    {
        Errors = errors;
    }

    public IReadOnlyList<OfertaValidationError> Errors { get; }
}

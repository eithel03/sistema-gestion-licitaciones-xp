namespace Licitaciones.Domain.Aprobaciones;

public sealed record NivelAprobacionValidationError(string Code, string Message);

public sealed class NivelAprobacionValidationException : Exception
{
    public NivelAprobacionValidationException(params NivelAprobacionValidationError[] errors)
        : base(errors.FirstOrDefault()?.Message ?? "El nivel de aprobacion no es valido.")
    {
        Errors = errors;
    }

    public IReadOnlyList<NivelAprobacionValidationError> Errors { get; }
}

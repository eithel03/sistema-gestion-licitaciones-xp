namespace Licitaciones.Application.TiposCambio;

public sealed class TipoCambioActiveConflictException : Exception
{
    public TipoCambioActiveConflictException()
        : base("Solo puede existir un tipo de cambio activo.")
    {
    }
}

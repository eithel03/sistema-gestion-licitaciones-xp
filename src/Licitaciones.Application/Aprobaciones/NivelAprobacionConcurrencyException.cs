namespace Licitaciones.Application.Aprobaciones;

public sealed class NivelAprobacionConcurrencyException : Exception
{
    public NivelAprobacionConcurrencyException()
        : base("El nivel de aprobacion fue modificado por otro proceso.")
    {
    }
}

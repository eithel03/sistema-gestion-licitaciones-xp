namespace Licitaciones.Application.Aprobaciones;

public sealed class NivelAprobacionRangeConflictException : Exception
{
    public NivelAprobacionRangeConflictException()
        : base("El rango de aprobacion entra en conflicto con otro existente.")
    {
    }
}

namespace Licitaciones.Application.Licitaciones;

public sealed class LicitacionConcurrencyException : Exception
{
    public LicitacionConcurrencyException()
        : base("La licitacion fue modificada por otro proceso.")
    {
    }
}

namespace Licitaciones.Application.Ofertas;

public sealed class OfertaConcurrencyException : Exception
{
    public OfertaConcurrencyException()
        : base("La oferta fue modificada por otro proceso.")
    {
    }
}

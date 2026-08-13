namespace Licitaciones.Application.Ofertas;

public sealed class OfertaDuplicateException : Exception
{
    public OfertaDuplicateException()
        : base("El proveedor ya presento una oferta para esta licitacion.")
    {
    }
}

namespace Licitaciones.Application.TiposCambio;

public sealed class TipoCambioConcurrencyException : Exception
{
    public TipoCambioConcurrencyException()
        : base("El tipo de cambio fue modificado por otro proceso.")
    {
    }
}

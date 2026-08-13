namespace Licitaciones.Domain.Ofertas;

public enum ClasificacionOferta
{
    SinOfertasValidas,
    Conveniente,
    Aceptable,
    ValidaSinAhorro
}

public sealed record EvaluacionOfertas(
    Oferta? MejorOferta,
    decimal? AhorroCrc,
    decimal? PorcentajeAhorro,
    ClasificacionOferta Clasificacion,
    string DescripcionClasificacion)
{
    public bool TieneOferta => MejorOferta is not null;
}

public static class EvaluadorOfertas
{
    public static EvaluacionOfertas Evaluar(decimal presupuestoCrc, IEnumerable<Oferta> ofertas)
    {
        ArgumentNullException.ThrowIfNull(ofertas);
        if (presupuestoCrc <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(presupuestoCrc), "El presupuesto debe ser mayor que cero.");
        }

        var mejorOferta = ofertas
            .OrderBy(oferta => oferta.MontoOfertadoCrc)
            .ThenBy(oferta => oferta.FechaRegistro)
            .ThenBy(oferta => oferta.Id)
            .FirstOrDefault();

        if (mejorOferta is null)
        {
            return new EvaluacionOfertas(null, null, null, ClasificacionOferta.SinOfertasValidas, "Sin ofertas validas");
        }

        var ahorro = presupuestoCrc - mejorOferta.MontoOfertadoCrc;
        var porcentaje = ahorro / presupuestoCrc * 100m;
        var clasificacion = porcentaje switch
        {
            >= 10m => ClasificacionOferta.Conveniente,
            > 0m => ClasificacionOferta.Aceptable,
            _ => ClasificacionOferta.ValidaSinAhorro
        };

        return new EvaluacionOfertas(
            mejorOferta,
            ahorro,
            porcentaje,
            clasificacion,
            Describe(clasificacion));
    }

    private static string Describe(ClasificacionOferta classification) => classification switch
    {
        ClasificacionOferta.Conveniente => "Oferta conveniente",
        ClasificacionOferta.Aceptable => "Oferta aceptable",
        ClasificacionOferta.ValidaSinAhorro => "Oferta valida sin ahorro",
        _ => "Sin ofertas validas"
    };
}

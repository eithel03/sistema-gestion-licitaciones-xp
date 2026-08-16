using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Ofertas;

namespace Licitaciones.UnitTests.Domain.Ofertas;

public sealed class EvaluadorOfertasTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);

    [Fact]
    public void EvaluateWithoutOffersReturnsFunctionalEmptyResult()
    {
        var result = EvaluadorOfertas.Evaluar(1000m, []);

        Assert.False(result.TieneOferta);
        Assert.Equal(ClasificacionOferta.SinOfertasValidas, result.Clasificacion);
        Assert.Equal("Sin ofertas validas", result.DescripcionClasificacion);
    }

    [Fact]
    public void EvaluateSelectsOnlyOffer()
    {
        var oferta = CreateOferta(900m, Now);

        var result = EvaluadorOfertas.Evaluar(1000m, [oferta]);

        Assert.Equal(oferta.Id, result.MejorOferta!.Id);
    }

    [Fact]
    public void EvaluateSelectsLowestAmount()
    {
        var higher = CreateOferta(900m, Now);
        var lower = CreateOferta(800m, Now.AddMinutes(1));

        var result = EvaluadorOfertas.Evaluar(1000m, [higher, lower]);

        Assert.Equal(lower.Id, result.MejorOferta!.Id);
    }

    [Fact]
    public void EqualAmountsUseRegistrationDateAndThenId()
    {
        var later = CreateOferta(800m, Now.AddMinutes(1));
        var earlier = CreateOferta(800m, Now);
        var sameInstant = CreateOferta(800m, Now);
        var expected = new[] { earlier, sameInstant }.OrderBy(item => item.Id).First();

        var result = EvaluadorOfertas.Evaluar(1000m, [later, sameInstant, earlier]);

        Assert.Equal(expected.Id, result.MejorOferta!.Id);
    }

    public static TheoryData<decimal, ClasificacionOferta, decimal> SavingsBoundaryCases => new()
    {
        { 900m, ClasificacionOferta.Conveniente, 10m },
        { 900.01m, ClasificacionOferta.Aceptable, 9.999m },
        { 899.99m, ClasificacionOferta.Conveniente, 10.001m },
        { 950m, ClasificacionOferta.Aceptable, 5m },
        { 1000m, ClasificacionOferta.ValidaSinAhorro, 0m }
    };

    [Theory]
    [MemberData(nameof(SavingsBoundaryCases))]
    public void EvaluateClassifiesSavings(decimal amount, ClasificacionOferta expected, decimal percentage)
    {
        var result = EvaluadorOfertas.Evaluar(1000m, [CreateOferta(amount, Now)]);

        Assert.Equal(expected, result.Clasificacion);
        Assert.Equal(percentage, result.PorcentajeAhorro);
    }

    private static Oferta CreateOferta(decimal amount, DateTimeOffset registeredAt)
    {
        var licitacion = Licitacion.Create("LIC-" + Guid.NewGuid().ToString("N"), "Compra", 1000m, Now.AddDays(1), Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));
        return Oferta.Create(licitacion, Guid.NewGuid(), amount, registeredAt);
    }
}

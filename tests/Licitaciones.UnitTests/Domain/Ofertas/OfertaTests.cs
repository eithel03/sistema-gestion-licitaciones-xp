using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Ofertas;

namespace Licitaciones.UnitTests.Domain.Ofertas;

public sealed class OfertaTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);
    private static readonly Guid ProveedorId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateRejectsNonPositiveAmount(decimal amount)
    {
        var licitacion = PublishedLicitacion(1000m);

        var exception = Assert.Throws<OfertaValidationException>(() =>
            Oferta.Create(licitacion, ProveedorId, amount, Now));

        Assert.Contains(exception.Errors, error => error.Code == OfertaErrors.MontoInvalido);
    }

    [Fact]
    public void CreateRejectsAmountAboveBudget()
    {
        var licitacion = PublishedLicitacion(1000m);

        var exception = Assert.Throws<OfertaValidationException>(() =>
            Oferta.Create(licitacion, ProveedorId, 1000.01m, Now));

        Assert.Contains(exception.Errors, error => error.Code == OfertaErrors.SuperaPresupuesto);
    }

    [Fact]
    public void CreateAcceptsAmountEqualToBudget()
    {
        var licitacion = PublishedLicitacion(1000m);

        var oferta = Oferta.Create(licitacion, ProveedorId, 1000m, Now);

        Assert.Equal(1000m, oferta.MontoOfertadoCrc);
    }

    [Fact]
    public void CreateRejectsDraftLicitacion()
    {
        var licitacion = Licitacion.Create("LIC-OF-1", "Compra", 1000m, Now.AddDays(1), Now.AddHours(-1));

        var exception = Assert.Throws<OfertaValidationException>(() =>
            Oferta.Create(licitacion, ProveedorId, 900m, Now));

        Assert.Contains(exception.Errors, error => error.Code == OfertaErrors.LicitacionNoRecibeOfertas);
    }

    [Fact]
    public void CreateRejectsLicitacionAtClosingInstant()
    {
        var licitacion = Licitacion.Create("LIC-OF-1", "Compra", 1000m, Now, Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));

        var exception = Assert.Throws<OfertaValidationException>(() =>
            Oferta.Create(licitacion, ProveedorId, 900m, Now));

        Assert.Contains(exception.Errors, error => error.Code == OfertaErrors.LicitacionNoRecibeOfertas);
    }

    [Fact]
    public void UpdateAndDeleteAreRejectedWhenLicitacionIsClosed()
    {
        var licitacion = PublishedLicitacion(1000m);
        var oferta = Oferta.Create(licitacion, ProveedorId, 900m, Now);
        licitacion.Close(Now.AddMinutes(1));

        var update = Assert.Throws<OfertaValidationException>(() =>
            oferta.UpdateAmount(licitacion, 800m, Now.AddMinutes(2)));
        var delete = Assert.Throws<OfertaValidationException>(() =>
            oferta.EnsureCanDelete(licitacion, Now.AddMinutes(2)));

        Assert.All([update, delete], error =>
            Assert.Contains(error.Errors, item => item.Code == OfertaErrors.LicitacionNoRecibeOfertas));
    }

    private static Licitacion PublishedLicitacion(decimal presupuesto)
    {
        var licitacion = Licitacion.Create("LIC-OF-1", "Compra", presupuesto, Now.AddDays(1), Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));
        return licitacion;
    }
}

using Licitaciones.Domain.TiposCambio;

namespace Licitaciones.UnitTests.Domain.TiposCambio;

public sealed class TipoCambioTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 10, 0, 0, TimeSpan.Zero);
    private static readonly DateOnly Fecha = new(2026, 8, 13);

    [Fact]
    public void CreateAcceptsValidExchangeRate()
    {
        var tipoCambio = TipoCambio.Create(Fecha, 520.25m, Now);

        Assert.Equal(Fecha, tipoCambio.Fecha);
        Assert.Equal(520.25m, tipoCambio.CrcPorUsd);
        Assert.False(tipoCambio.Activo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    public void CreateRejectsNonPositiveExchangeRate(decimal crcPorUsd)
    {
        var exception = Assert.Throws<TipoCambioValidationException>(() =>
            TipoCambio.Create(Fecha, crcPorUsd, Now));

        Assert.Contains(exception.Errors, error => error.Code == TipoCambioErrors.ValorInvalido);
    }

    [Fact]
    public void ActivateAndDeactivateUpdateActiveState()
    {
        var tipoCambio = TipoCambio.Create(Fecha, 520.25m, Now);

        tipoCambio.Activate(Now.AddMinutes(1));
        tipoCambio.Deactivate(Now.AddMinutes(2));

        Assert.False(tipoCambio.Activo);
        Assert.Equal(Now.AddMinutes(2), tipoCambio.UpdatedAt);
    }

    [Fact]
    public void UpdateKeepsValidationAndAudit()
    {
        var tipoCambio = TipoCambio.Create(Fecha, 520.25m, Now);
        var nuevaFecha = Fecha.AddDays(1);

        tipoCambio.Update(nuevaFecha, 525.50m, Now.AddHours(1));

        Assert.Equal(nuevaFecha, tipoCambio.Fecha);
        Assert.Equal(525.50m, tipoCambio.CrcPorUsd);
        Assert.Equal(Now.AddHours(1), tipoCambio.UpdatedAt);
    }
}

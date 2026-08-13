using Licitaciones.Domain.Aprobaciones;

namespace Licitaciones.UnitTests.Domain.Aprobaciones;

public sealed class NivelAprobacionTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);

    [Fact]
    public void CreateAcceptsValidClosedRange()
    {
        var nivel = NivelAprobacion.Create(0.01m, 999999.99m, "Encargado de area", Now);

        Assert.Equal(0.01m, nivel.MontoMinimoCrc);
        Assert.Equal(999999.99m, nivel.MontoMaximoCrc);
        Assert.Equal("Encargado de area", nivel.Aprobador);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    public void CreateRejectsInvalidMinimum(decimal minimum)
    {
        var exception = Assert.Throws<NivelAprobacionValidationException>(() =>
            NivelAprobacion.Create(minimum, 100m, "Gerencia", Now));

        Assert.Contains(exception.Errors, error => error.Code == NivelAprobacionErrors.MinimoInvalido);
    }

    [Fact]
    public void CreateRejectsMaximumBelowMinimum()
    {
        var exception = Assert.Throws<NivelAprobacionValidationException>(() =>
            NivelAprobacion.Create(100m, 99.99m, "Gerencia", Now));

        Assert.Contains(exception.Errors, error => error.Code == NivelAprobacionErrors.MaximoInvalido);
    }

    [Fact]
    public void DetectsOverlappingRangesIncludingSharedBoundary()
    {
        var first = NivelAprobacion.Create(100m, 200m, "A", Now);
        var overlapping = NivelAprobacion.Create(200m, 300m, "B", Now);
        var separate = NivelAprobacion.Create(200.01m, 300m, "B", Now);

        Assert.True(first.Overlaps(overlapping));
        Assert.False(first.Overlaps(separate));
    }

    [Fact]
    public void OpenRangeContainsAnyAmountFromItsMinimum()
    {
        var open = NivelAprobacion.Create(10000000m, null, "Junta Directiva", Now);

        Assert.True(open.Contains(10000000m));
        Assert.True(open.Contains(decimal.MaxValue));
        Assert.False(open.Contains(9999999.99m));
    }

    [Fact]
    public void ClosedRangeContainsBothLimits()
    {
        var closed = NivelAprobacion.Create(1000000m, 9999999.99m, "Gerencia", Now);

        Assert.True(closed.Contains(1000000m));
        Assert.True(closed.Contains(9999999.99m));
    }
}

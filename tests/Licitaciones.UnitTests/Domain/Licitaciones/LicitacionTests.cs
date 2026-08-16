
using Licitaciones.Domain.Licitaciones;

namespace Licitaciones.UnitTests.Domain.Licitaciones;

public sealed class LicitacionTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 11, 15, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset FutureClose = Now.AddDays(5);

    [Fact]
    public void CreateValidTenderStartsAsDraft()
    {
        var licitacion = Licitacion.Create(" LIC-2026-001 ", "Compra de equipo", 1500000m, FutureClose, Now);

        Assert.Equal("LIC-2026-001", licitacion.Codigo);
        Assert.Equal("LIC-2026-001", licitacion.CodigoNormalizado);
        Assert.Equal("Compra de equipo", licitacion.Titulo);
        Assert.Equal(1500000m, licitacion.PresupuestoCrc);
        Assert.Equal(LicitacionEstado.Borrador, licitacion.Estado);
    }

    [Fact]
    public void CreateRejectsInvalidBaseData()
    {
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            Licitacion.Create(" ", "Compra", 1000m, FutureClose, Now)).Errors, e => e.Code == LicitacionErrors.CodigoRequerido);
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            Licitacion.Create("LIC/2026", "Compra", 1000m, FutureClose, Now)).Errors, e => e.Code == LicitacionErrors.CodigoCaracteresInvalidos);
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            Licitacion.Create("LIC-2026", "Compra", 0m, FutureClose, Now)).Errors, e => e.Code == LicitacionErrors.PresupuestoInvalido);
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            Licitacion.Create("LIC-2026", "Compra", 1000m, Now, Now)).Errors, e => e.Code == LicitacionErrors.FechaCierreInvalida);
    }

    [Theory]
    [InlineData("LIC-2026-001", " lic-2026-001 ")]
    [InlineData("LIC   2026", "lic 2026")]
    [InlineData("LIC-CAFÉ", "lic-cafe\u0301")]
    public void NormalizedCodeIgnoresCaseAndRepeatedSpaces(string left, string right)
    {
        Assert.Equal(
            LicitacionCodeNormalizer.NormalizeForComparison(left),
            LicitacionCodeNormalizer.NormalizeForComparison(right));
    }

    [Fact]
    public void CreateAcceptsMaximumCodeTitleAndMinimumPositiveBudget()
    {
        var code = new string('A', Licitacion.CodigoMaxLength);
        var title = new string('T', Licitacion.TituloMaxLength);

        var licitacion = Licitacion.Create(code, title, 0.01m, FutureClose, Now);

        Assert.Equal(Licitacion.CodigoMaxLength, licitacion.Codigo.Length);
        Assert.Equal(Licitacion.TituloMaxLength, licitacion.Titulo.Length);
        Assert.Equal(0.01m, licitacion.PresupuestoCrc);
    }

    public static TheoryData<string, string, decimal, string> InvalidLimitCases => new()
    {
        {
            new string('A', Licitacion.CodigoMaxLength + 1),
            "Compra",
            1000m,
            LicitacionErrors.CodigoLongitudMaxima
        },
        {
            "LIC-2026",
            new string('T', Licitacion.TituloMaxLength + 1),
            1000m,
            LicitacionErrors.TituloLongitudMaxima
        },
        { "LIC-2026", "Compra", -0.01m, LicitacionErrors.PresupuestoInvalido }
    };

    [Theory]
    [MemberData(nameof(InvalidLimitCases))]
    public void CreateRejectsCodeTitleAndBudgetOutsideLimits(
        string code,
        string title,
        decimal budget,
        string expectedError)
    {
        var exception = Assert.Throws<LicitacionValidationException>(() =>
            Licitacion.Create(code, title, budget, FutureClose, Now));

        Assert.Contains(exception.Errors, error => error.Code == expectedError);
    }

    [Fact]
    public void PublishAndCloseFollowAllowedTransitions()
    {
        var licitacion = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);

        licitacion.Publish(Now.AddHours(1));
        Assert.Equal(LicitacionEstado.Publicada, licitacion.Estado);

        var repeatedPublish = Assert.Throws<LicitacionValidationException>(() => licitacion.Publish(Now.AddHours(2)));
        Assert.Contains(repeatedPublish.Errors, e => e.Code == LicitacionErrors.TransicionInvalida);

        licitacion.Close(Now.AddHours(3));
        Assert.Equal(LicitacionEstado.Cerrada, licitacion.Estado);
    }

    [Fact]
    public void DraftTenderCanBeClosed()
    {
        var draft = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);

        draft.Close(Now.AddHours(1));

        Assert.Equal(LicitacionEstado.Cerrada, draft.Estado);
    }

    [Fact]
    public void InvalidTransitionsAndUpdatesAreRejected()
    {
        var draft = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);
        draft.Publish(Now.AddHours(1));
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            draft.Update("LIC-2026-002", "Compra actualizada", 2000m, FutureClose.AddDays(1), Now.AddHours(2))).Errors,
            e => e.Code == LicitacionErrors.EdicionNoPermitida);
    }

    [Fact]
    public void ChangeEstadoRejectsReturningToDraft()
    {
        var published = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);
        published.Publish(Now.AddHours(1));
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            published.ChangeEstado(LicitacionEstado.Borrador, Now.AddHours(2))).Errors,
            e => e.Code == LicitacionErrors.TransicionInvalida);

        var closed = Licitacion.Create("LIC-2026-002", "Compra", 1000m, FutureClose, Now);
        closed.Close(Now.AddHours(1));
        Assert.Contains(Assert.Throws<LicitacionValidationException>(() =>
            closed.ChangeEstado(LicitacionEstado.Borrador, Now.AddHours(2))).Errors,
            e => e.Code == LicitacionErrors.TransicionInvalida);
    }

    [Fact]
    public void ChangeEstadoToCurrentStateIsIdempotent()
    {
        var licitacion = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);
        var publishedAt = Now.AddHours(1);
        licitacion.Publish(publishedAt);

        licitacion.ChangeEstado(LicitacionEstado.Publicada, Now.AddHours(2));

        Assert.Equal(LicitacionEstado.Publicada, licitacion.Estado);
        Assert.Equal(publishedAt, licitacion.PublishedAt);
        Assert.Equal(publishedAt, licitacion.UpdatedAt);
    }

    [Fact]
    public void ChangeEstadoRejectsUndefinedEnumValue()
    {
        var licitacion = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);

        var exception = Assert.Throws<LicitacionValidationException>(() =>
            licitacion.ChangeEstado((LicitacionEstado)999, Now.AddHours(1)));

        Assert.Contains(exception.Errors, error => error.Code == LicitacionErrors.TransicionInvalida);
    }

    [Fact]
    public void PublishedExpiredTenderIsEffectivelyClosed()
    {
        var licitacion = Licitacion.Create("LIC-2026-001", "Compra", 1000m, FutureClose, Now);
        licitacion.Publish(Now.AddHours(1));

        Assert.Equal(LicitacionEstado.Cerrada, licitacion.GetEstadoEfectivo(FutureClose.AddMinutes(1)));
    }
}

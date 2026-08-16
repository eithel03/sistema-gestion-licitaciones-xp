using Microsoft.Playwright;

namespace Licitaciones.E2ETests;

[Collection(E2ETestGroup.Name)]
public sealed class LicitacionOfertaE2ETests(E2EHostFixture host) : E2ETestBase(host)
{
    [Fact]
    public async Task TenderCanBeCreatedPublishedAndClosedFromTheBrowser()
    {
        await ResetAsync();
        await Page.GotoAsync("/Licitaciones/Create");
        await Page.GetByLabel("Codigo").FillAsync("E2E-LIC-ESTADOS");
        await Page.GetByLabel("Titulo").FillAsync("Compra E2E estados");
        await Page.GetByLabel("Presupuesto CRC").FillAsync("2500,00");
        await Page.GetByLabel("Fecha de cierre").FillAsync("2035-08-25T10:00");
        await Page.Locator("form[action='/Licitaciones/Create']").EvaluateAsync("form => form.submit()");
        await Page.WaitForURLAsync("**/Licitaciones/Details/**");
        await Page.WaitForLoadStateAsync();
        if (!Page.Url.Contains("/Licitaciones/Details/", StringComparison.Ordinal))
        {
            throw new Xunit.Sdk.XunitException($"La creación no redirigió. URL: {Page.Url}\n{await Page.ContentAsync()}");
        }
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "E2E-LIC-ESTADOS", Exact = true })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Publicar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/Licitaciones/Details/**");
        await Expect(Page.GetByText("Publicada", new() { Exact = true }).First).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Cerrar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/Licitaciones/Details/**");
        await Expect(Page.GetByText("Cerrada", new() { Exact = true }).First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task OfferCanBeRegisteredAndInvalidAmountsAreRejectedVisibly()
    {
        await ResetAsync();
        var (licitacionId, proveedorId) = await Host.SeedPublishedOfferScenarioAsync();
        await Page.GotoAsync("/Ofertas/Create");
        await Page.GetByLabel("Licitacion").SelectOptionAsync(licitacionId.ToString());
        await Page.GetByLabel("Proveedor").SelectOptionAsync(proveedorId.ToString());
        await Page.GetByLabel("Monto ofertado CRC").FillAsync("1000.01");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Expect(Page.GetByText("no puede superar el presupuesto", new() { Exact = false })).ToBeVisibleAsync();

        await Page.GotoAsync("/Ofertas/Create");
        await Page.GetByLabel("Licitacion").SelectOptionAsync(licitacionId.ToString());
        await Page.GetByLabel("Proveedor").SelectOptionAsync(proveedorId.ToString());
        await Page.GetByLabel("Monto ofertado CRC").FillAsync("900");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/Ofertas/Details/**");
        await Expect(Page.GetByText("900,00", new() { Exact = false })).ToBeVisibleAsync();

        await Page.GotoAsync("/Ofertas/Create");
        await Page.GetByLabel("Licitacion").SelectOptionAsync(licitacionId.ToString());
        await Page.GetByLabel("Proveedor").SelectOptionAsync(proveedorId.ToString());
        await Page.GetByLabel("Monto ofertado CRC").FillAsync("800");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Expect(Page.GetByText("ya presento una oferta", new() { Exact = false })).ToBeVisibleAsync();
    }
}

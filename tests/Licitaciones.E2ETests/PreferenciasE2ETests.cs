using Microsoft.Playwright;

namespace Licitaciones.E2ETests;

[Collection(E2ETestGroup.Name)]
public sealed class PreferenciasE2ETests(E2EHostFixture host) : E2ETestBase(host)
{
    [Fact]
    public async Task ThemePreferenceChangesVisuallyAndSurvivesReload()
    {
        await ResetAsync();
        await Page.GotoAsync("/");
        await Expect(Page.Locator("html")).ToHaveAttributeAsync("data-bs-theme", "light");
        await Page.Locator("#themePreference").SelectOptionAsync("dark");
        await Expect(Page.Locator("html")).ToHaveAttributeAsync("data-bs-theme", "dark");
        await Page.ReloadAsync();
        await Expect(Page.Locator("html")).ToHaveAttributeAsync("data-bs-theme", "dark");
    }

    [Fact]
    public async Task CurrencyPreferenceShowsUsdConversionAndReturnsToOriginalCrc()
    {
        await ResetAsync();
        var licitacionId = await Host.SeedPublishedLicitacionAsync();
        await Page.GotoAsync("/TiposCambio/Create");
        await Page.GetByLabel("Fecha").FillAsync("2035-08-15");
        await Page.GetByLabel("CRC por USD").FillAsync("500");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/TiposCambio/Details/**");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Activar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/TiposCambio/Details/**");

        await Page.GotoAsync($"/Licitaciones/Details/{licitacionId}");
        await Expect(Page.GetByText("1 000,00", new() { Exact = false })).ToBeVisibleAsync();
        await Page.Locator("#currencyPreference").SelectOptionAsync("USD");
        await Expect(Page.GetByText("2,00", new() { Exact = false })).ToBeVisibleAsync();
        await Expect(Page.GetByText("2035-08-15", new() { Exact = false })).ToBeVisibleAsync();
        await Page.Locator("#currencyPreference").SelectOptionAsync("CRC");
        await Expect(Page.GetByText("1 000,00", new() { Exact = false })).ToBeVisibleAsync();
    }
}

using Microsoft.Playwright;

namespace Licitaciones.E2ETests;

[Collection(E2ETestGroup.Name)]
public sealed class NavigationE2ETests(E2EHostFixture host) : E2ETestBase(host)
{
    [Fact]
    public async Task LandingNavigatesToEveryMainModule()
    {
        await ResetAsync();
        await Page.GotoAsync("/");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Sistema de Gestion de Licitaciones" })).ToBeVisibleAsync();

        var destinations = new[]
        {
            ("Proveedores", "Proveedores"),
            ("Licitaciones", "Licitaciones"),
            ("Ofertas", "Ofertas"),
            ("Niveles de aprobacion", "Niveles de aprobacion"),
            ("Tipos de cambio", "Tipos de cambio")
        };

        foreach (var (linkName, heading) in destinations)
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = linkName, Exact = true }).ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = heading, Exact = true })).ToBeVisibleAsync();
            await Page.GotoAsync("/");
        }
    }
}

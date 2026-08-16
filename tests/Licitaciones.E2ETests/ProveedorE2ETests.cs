using Microsoft.Playwright;

namespace Licitaciones.E2ETests;

[Collection(E2ETestGroup.Name)]
public sealed class ProveedorE2ETests(E2EHostFixture host) : E2ETestBase(host)
{
    [Fact]
    public async Task ProviderCanBeCreatedEditedAndDuplicateIsRejectedVisibly()
    {
        await ResetAsync();
        await Page.GotoAsync("/Proveedores");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Crear proveedor", Exact = true }).ClickAsync();
        await Page.GetByLabel("Nombre").FillAsync("Proveedor E2E");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/Proveedores/Details/**");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Proveedor E2E", Exact = true })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Editar", Exact = true }).ClickAsync();
        await Page.GetByLabel("Nombre").FillAsync("Proveedor E2E Editado");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Page.WaitForURLAsync("**/Proveedores/Details/**");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Proveedor E2E Editado", Exact = true })).ToBeVisibleAsync();

        await Page.GotoAsync("/Proveedores/Create");
        await Page.GetByLabel("Nombre").FillAsync("Proveedor E2E Editado");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar", Exact = true }).ClickAsync();
        await Expect(Page.GetByText("Ya existe un proveedor", new() { Exact = false })).ToBeVisibleAsync();
    }
}

extern alias WebApp;

using System.Net;
using System.Text.RegularExpressions;
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(Iteration3MvcGroup.Name)]
public sealed partial class Iteration3MvcTests : IAsyncLifetime
{
    private static readonly DateTimeOffset Now = new(2030, 8, 12, 16, 0, 0, TimeSpan.Zero);
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_mvc_iteration3_tests")
        .WithUsername("iteration3_mvc")
        .WithPassword("iteration3_mvc")
        .Build();
    private WebApplicationFactory<WebApp::Program>? _factory;
    private Guid _licitacionId;
    private Guid _proveedorId;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _container.GetConnectionString());
        _factory = new WebApplicationFactory<WebApp::Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
                new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString() }));
        });
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LicitacionesDbContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        var licitacion = Licitacion.Create("LIC-MVC-OF", "Compra MVC", 1000m, Now.AddDays(1), Now.AddHours(-1));
        licitacion.Publish(Now.AddMinutes(-30));
        var proveedor = Proveedor.Create("Proveedor MVC", Now);
        context.AddRange(licitacion, proveedor);
        await context.SaveChangesAsync();
        _licitacionId = licitacion.Id;
        _proveedorId = proveedor.Id;
    }

    public async Task DisposeAsync()
    {
        _factory?.Dispose();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", null);
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task OfferMvcSupportsCreateFilterEditValidationProviderDetailAndDelete()
    {
        using var client = ClientWithoutRedirect();
        var createPage = await GetHtmlAsync(client, "/Ofertas/Create");
        Assert.Contains("LIC-MVC-OF", createPage);
        Assert.Contains("Proveedor MVC", createPage);

        using var create = await PostFormAsync(client, "/Ofertas/Create", createPage, new()
        {
            ["LicitacionId"] = _licitacionId.ToString(),
            ["ProveedorId"] = _proveedorId.ToString(),
            ["MontoOfertadoCrc"] = "900"
        });
        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);
        var detailPath = create.Headers.Location!.ToString();

        var filtered = await client.GetStringAsync($"/Ofertas?licitacionId={_licitacionId}&proveedorId={_proveedorId}");
        Assert.Contains("900,00", filtered);
        var providerDetail = await client.GetStringAsync($"/Proveedores/Details/{_proveedorId}");
        Assert.Contains("Ofertas asociadas", providerDetail);
        Assert.Contains("900,00", providerDetail);

        var editPath = detailPath.Replace("Details", "Edit", StringComparison.OrdinalIgnoreCase);
        var editPage = await client.GetStringAsync(editPath);
        using var edit = await PostFormAsync(client, editPath, editPage, new()
        {
            ["LicitacionId"] = _licitacionId.ToString(),
            ["ProveedorId"] = _proveedorId.ToString(),
            ["MontoOfertadoCrc"] = "800",
            ["Version"] = ExtractInput(editPage, "Version")
        });
        Assert.Equal(HttpStatusCode.Redirect, edit.StatusCode);

        var invalidPage = await client.GetStringAsync("/Ofertas/Create");
        using var invalid = await PostFormAsync(client, "/Ofertas/Create", invalidPage, new()
        {
            ["LicitacionId"] = _licitacionId.ToString(),
            ["ProveedorId"] = _proveedorId.ToString(),
            ["MontoOfertadoCrc"] = "0"
        });
        Assert.Equal(HttpStatusCode.OK, invalid.StatusCode);
        Assert.Contains("mayor que cero", await invalid.Content.ReadAsStringAsync());

        var deletePath = detailPath.Replace("Details", "Delete", StringComparison.OrdinalIgnoreCase);
        var deletePage = await client.GetStringAsync(deletePath);
        Assert.Contains("Confirmar eliminacion", deletePage);
        using var delete = await PostFormAsync(client, deletePath, deletePage, []);
        Assert.Equal(HttpStatusCode.Redirect, delete.StatusCode);
    }

    [Fact]
    public async Task ApprovalMvcSupportsCrudAndShowsOverlapValidation()
    {
        using var client = ClientWithoutRedirect();
        var createPage = await GetHtmlAsync(client, "/NivelesAprobacion/Create");
        using var create = await PostFormAsync(client, "/NivelesAprobacion/Create", createPage, new()
        {
            ["MontoMinimoCrc"] = "0,01",
            ["MontoMaximoCrc"] = "100",
            ["Aprobador"] = "Encargado MVC"
        });
        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);
        var detailPath = create.Headers.Location!.ToString();
        Assert.Contains("Encargado MVC", await client.GetStringAsync(detailPath));

        var overlapPage = await client.GetStringAsync("/NivelesAprobacion/Create");
        using var overlap = await PostFormAsync(client, "/NivelesAprobacion/Create", overlapPage, new()
        {
            ["MontoMinimoCrc"] = "100",
            ["MontoMaximoCrc"] = "200",
            ["Aprobador"] = "Gerencia MVC"
        });
        Assert.Equal(HttpStatusCode.OK, overlap.StatusCode);
        Assert.Contains("traslapa", await overlap.Content.ReadAsStringAsync());

        var editPath = detailPath.Replace("Details", "Edit", StringComparison.OrdinalIgnoreCase);
        var editPage = await client.GetStringAsync(editPath);
        using var edit = await PostFormAsync(client, editPath, editPage, new()
        {
            ["MontoMinimoCrc"] = "0,01",
            ["MontoMaximoCrc"] = "99,99",
            ["Aprobador"] = "Encargado Editado",
            ["Version"] = ExtractInput(editPage, "Version")
        });
        Assert.Equal(HttpStatusCode.Redirect, edit.StatusCode);

        var deletePath = detailPath.Replace("Details", "Delete", StringComparison.OrdinalIgnoreCase);
        var deletePage = await client.GetStringAsync(deletePath);
        using var delete = await PostFormAsync(client, deletePath, deletePage, []);
        Assert.Equal(HttpStatusCode.Redirect, delete.StatusCode);
    }

    private HttpClient ClientWithoutRedirect() => _factory!.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static async Task<string> GetHtmlAsync(HttpClient client, string path)
    {
        using var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, html);
        return html;
    }

    private static async Task<HttpResponseMessage> PostFormAsync(HttpClient client, string path, string html, Dictionary<string, string> values)
    {
        values["__RequestVerificationToken"] = ExtractAntiForgeryToken(html);
        return await client.PostAsync(path, new FormUrlEncodedContent(values));
    }

    private static string ExtractAntiForgeryToken(string html) => WebUtility.HtmlDecode(AntiForgeryTokenRegex().Match(html).Groups["token"].Value);
    private static string ExtractInput(string html, string name) => WebUtility.HtmlDecode(InputRegex(name).Match(html).Groups["value"].Value);

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>[^\"]+)\"")]
    private static partial Regex AntiForgeryTokenRegex();

    private static Regex InputRegex(string name) => new($"name=\"{name}\" type=\"hidden\" value=\"(?<value>[^\"]*)\"", RegexOptions.IgnoreCase);
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class Iteration3MvcGroup
{
    public const string Name = "Iteration 3 MVC tests";
}

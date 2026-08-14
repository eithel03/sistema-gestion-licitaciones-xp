extern alias WebApp;

using System.Net;
using System.Text.RegularExpressions;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(ProveedorMvcTestGroup.Name)]
public sealed partial class ProveedorMvcTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_mvc_tests")
        .WithUsername("licitaciones_mvc_tests")
        .WithPassword("licitaciones_mvc_tests")
        .Build();

    private WebApplicationFactory<WebApp::Program>? _factory;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _container.GetConnectionString());

        _factory = new WebApplicationFactory<WebApp::Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureAppConfiguration((_, configuration) =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString()
                    });
                });
            });

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LicitacionesDbContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        _factory?.Dispose();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", null);
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task LandingPageAndProviderListAreAvailable()
    {
        using var client = _factory!.CreateClient();

        var landing = await client.GetStringAsync("/");
        var providers = await client.GetStringAsync("/Proveedores");

        Assert.Contains("Sistema de Gestion de Licitaciones", landing);
        Assert.Contains("Proveedores", providers);
    }

    [Fact]
    public async Task CreateEditAndRejectDuplicateProviderThroughMvc()
    {
        using var client = _factory!.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var createPage = await client.GetStringAsync("/Proveedores/Create");
        var createToken = ExtractAntiForgeryToken(createPage);

        using var createResponse = await client.PostAsync(
            "/Proveedores/Create",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = createToken,
                ["Nombre"] = "Empresa Central"
            }));

        Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);
        var detailPath = createResponse.Headers.Location!.ToString();

        var detail = await client.GetStringAsync(detailPath);
        Assert.Contains("Empresa Central", detail);

        var editPage = await client.GetStringAsync(detailPath.Replace("Details", "Edit", StringComparison.OrdinalIgnoreCase));
        var editToken = ExtractAntiForgeryToken(editPage);

        using var editResponse = await client.PostAsync(
            detailPath.Replace("Details", "Edit", StringComparison.OrdinalIgnoreCase),
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = editToken,
                ["Nombre"] = "Empresa Nacional"
            }));

        Assert.Equal(HttpStatusCode.Redirect, editResponse.StatusCode);

        createPage = await client.GetStringAsync("/Proveedores/Create");
        createToken = ExtractAntiForgeryToken(createPage);

        using var duplicateResponse = await client.PostAsync(
            "/Proveedores/Create",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = createToken,
                ["Nombre"] = " empresa   nacional "
            }));

        Assert.Equal(HttpStatusCode.OK, duplicateResponse.StatusCode);
        var duplicateContent = await duplicateResponse.Content.ReadAsStringAsync();
        Assert.Contains("Ya existe un proveedor", duplicateContent);
    }

    [Theory]
    [InlineData("Tecnología Empresarial CR")]
    [InlineData("Compañía Nacional 2026")]
    [InlineData("Empresa Ñandú")]
    [InlineData("Servicios Técnicos, S.A.")]
    [InlineData("Soluciones (Costa Rica)")]
    public async Task CreateProviderThroughMvcAcceptsUnicodeLetters(string nombre)
    {
        using var client = _factory!.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        var createPage = await client.GetStringAsync("/Proveedores/Create");
        var createToken = ExtractAntiForgeryToken(createPage);

        using var createResponse = await client.PostAsync(
            "/Proveedores/Create",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = createToken,
                ["Nombre"] = nombre
            }));

        Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);
        var detail = await client.GetStringAsync(createResponse.Headers.Location!.ToString());
        Assert.Contains(nombre, WebUtility.HtmlDecode(detail));
    }

    [Theory]
    [InlineData("Empresa @ CR")]
    [InlineData("Proveedor #1")]
    [InlineData("Empresa / Servicios")]
    [InlineData("Proveedor & Asociados")]
    public async Task CreateProviderThroughMvcRejectsDisallowedSymbols(string nombre)
    {
        using var client = _factory!.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        var createPage = await client.GetStringAsync("/Proveedores/Create");
        var createToken = ExtractAntiForgeryToken(createPage);

        using var createResponse = await client.PostAsync(
            "/Proveedores/Create",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = createToken,
                ["Nombre"] = nombre
            }));

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        var content = await createResponse.Content.ReadAsStringAsync();
        Assert.Contains("solo puede contener letras", content);
    }

    [Fact]
    public async Task CreateProviderFormUsesUnicodeSafeClientValidation()
    {
        using var client = _factory!.CreateClient();

        var createPage = await client.GetStringAsync("/Proveedores/Create");
        var input = ProviderNameInputRegex().Match(createPage).Value;

        Assert.DoesNotContain("data-val-regex", input);
        Assert.Contains("data-val-proveedornombre", input);
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        var match = AntiForgeryTokenRegex().Match(html);
        Assert.True(match.Success, "Expected antiforgery token in MVC form.");

        return WebUtility.HtmlDecode(match.Groups["token"].Value);
    }

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>[^\"]+)\"")]
    private static partial Regex AntiForgeryTokenRegex();

    [GeneratedRegex("<input[^>]*name=\"Nombre\"[^>]*>", RegexOptions.IgnoreCase)]
    private static partial Regex ProviderNameInputRegex();
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ProveedorMvcTestGroup
{
    public const string Name = "Proveedor MVC tests";
}

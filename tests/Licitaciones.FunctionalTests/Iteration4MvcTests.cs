extern alias WebApp;

using System.Net;
using System.Text.RegularExpressions;
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(Iteration4MvcGroup.Name)]
public sealed partial class Iteration4MvcTests : IAsyncLifetime
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 10, 0, 0, TimeSpan.Zero);
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_mvc_iteration4_tests")
        .WithUsername("iteration4_mvc")
        .WithPassword("iteration4_mvc")
        .Build();
    private WebApplicationFactory<WebApp::Program>? _factory;
    private Guid _licitacionId;

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
        var licitacion = Licitacion.Create("LIC-MVC-TC", "Compra moneda", 1000m, Now.AddDays(1), Now.AddHours(-1));
        context.Add(licitacion);
        await context.SaveChangesAsync();
        _licitacionId = licitacion.Id;
    }

    public async Task DisposeAsync()
    {
        _factory?.Dispose();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", null);
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task ExchangeRateMvcSupportsCrudActivationAndCurrencyToggle()
    {
        using var client = ClientWithoutRedirect();
        var createPage = await GetHtmlAsync(client, "/TiposCambio/Create");
        using var create = await PostFormAsync(client, "/TiposCambio/Create", createPage, new()
        {
            ["Fecha"] = "2026-08-13",
            ["CrcPorUsd"] = "500"
        });
        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);
        var detailPath = create.Headers.Location!.ToString();
        var detail = await client.GetStringAsync(detailPath);
        Assert.Contains("500,00", detail);

        var index = await client.GetStringAsync("/TiposCambio");
        Assert.Contains("500,00", index);

        var editPath = detailPath.Replace("Details", "Edit", StringComparison.OrdinalIgnoreCase);
        var editPage = await client.GetStringAsync(editPath);
        using var edit = await PostFormAsync(client, editPath, editPage, new()
        {
            ["Fecha"] = "2026-08-14",
            ["CrcPorUsd"] = "510",
            ["Version"] = ExtractInput(editPage, "Version")
        });
        Assert.Equal(HttpStatusCode.Redirect, edit.StatusCode);
        detail = await client.GetStringAsync(detailPath);
        Assert.Contains("510,00", detail);
        Assert.Contains("Tipo de cambio actualizado correctamente", detail);

        using var activate = await PostFormAsync(client, detailPath.Replace("Details", "Activate", StringComparison.OrdinalIgnoreCase), detail, new()
        {
            ["Version"] = ExtractInput(detail, "Version")
        });
        Assert.Equal(HttpStatusCode.Redirect, activate.StatusCode);

        var activeDetail = await client.GetStringAsync(detailPath);
        Assert.DoesNotContain(">Activar</button>", activeDetail);

        using var preference = await client.PostAsync("/Preferencias/Moneda?moneda=USD&returnUrl=%2FLicitaciones%2FDetails%2F" + _licitacionId, null);
        Assert.Equal(HttpStatusCode.Redirect, preference.StatusCode);
        Assert.Contains("licitaciones.currency=USD", string.Join(';', preference.Headers.GetValues("Set-Cookie")));

        client.DefaultRequestHeaders.Add("Cookie", "licitaciones.currency=USD");
        var licitacion = await client.GetStringAsync($"/Licitaciones/Details/{_licitacionId}");
        Assert.Contains("USD", licitacion);
        Assert.Contains("1,96", licitacion);
        Assert.Contains("2026-08-14", licitacion);

        var deletePath = detailPath.Replace("Details", "Delete", StringComparison.OrdinalIgnoreCase);
        var deletePage = await client.GetStringAsync(deletePath);
        Assert.Contains("Confirmar eliminacion", deletePage);
        using var delete = await PostFormAsync(client, deletePath, deletePage, []);
        Assert.Equal(HttpStatusCode.Redirect, delete.StatusCode);
        Assert.DoesNotContain("510,00", await client.GetStringAsync("/TiposCambio"));
    }

    [Theory]
    [InlineData("500", "500,00")]
    [InlineData("500.00", "500,00")]
    [InlineData("500,00", "500,00")]
    [InlineData("520.50", "520,50")]
    [InlineData("520,50", "520,50")]
    public async Task ExchangeRateMvcAcceptsDotAndCommaDecimalFormats(string submittedValue, string expectedDisplay)
    {
        using var client = ClientWithoutRedirect();
        var createPage = await GetHtmlAsync(client, "/TiposCambio/Create");

        using var create = await PostFormAsync(client, "/TiposCambio/Create", createPage, new()
        {
            ["Fecha"] = "2026-09-15",
            ["CrcPorUsd"] = submittedValue
        });

        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);
        var detail = await client.GetStringAsync(create.Headers.Location!.ToString());
        Assert.Contains(expectedDisplay, detail);
    }

    [Theory]
    [InlineData("0", "mayor que cero")]
    [InlineData("-1", "mayor que cero")]
    [InlineData("abc", "numero valido")]
    public async Task ExchangeRateMvcRejectsInvalidDecimalFormats(string submittedValue, string expectedMessage)
    {
        using var client = ClientWithoutRedirect();
        var createPage = await GetHtmlAsync(client, "/TiposCambio/Create");

        using var create = await PostFormAsync(client, "/TiposCambio/Create", createPage, new()
        {
            ["Fecha"] = "2026-10-15",
            ["CrcPorUsd"] = submittedValue
        });

        Assert.Equal(HttpStatusCode.OK, create.StatusCode);
        Assert.Contains(expectedMessage, await create.Content.ReadAsStringAsync(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ExchangeRateCreateFormRendersLocalizedDecimalInputForBrowserValidation()
    {
        using var client = ClientWithoutRedirect();

        var createPage = await GetHtmlAsync(client, "/TiposCambio/Create");
        var input = CrcPorUsdInputRegex().Match(createPage).Value;
        var jqueryValidateIndex = createPage.IndexOf("/lib/jquery-validation/dist/jquery.validate.min.js", StringComparison.Ordinal);
        var siteIndex = createPage.IndexOf("/js/site.js", StringComparison.Ordinal);

        Assert.Contains("type=\"text\"", input);
        Assert.Contains("inputmode=\"decimal\"", input);
        Assert.DoesNotContain("type=\"number\"", input);
        Assert.Contains("value=\"0,00\"", input);
        Assert.True(jqueryValidateIndex >= 0, createPage);
        Assert.True(siteIndex > jqueryValidateIndex, createPage);
    }

    [Fact]
    public async Task InactiveExchangeRateDetailShowsActivateButton()
    {
        using var client = ClientWithoutRedirect();
        var createPage = await GetHtmlAsync(client, "/TiposCambio/Create");
        using var create = await PostFormAsync(client, "/TiposCambio/Create", createPage, new()
        {
            ["Fecha"] = "2026-09-30",
            ["CrcPorUsd"] = "500"
        });
        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);

        var detail = await client.GetStringAsync(create.Headers.Location!.ToString());

        Assert.Contains(">Activar</button>", detail);
    }

    [Fact]
    public async Task ExchangeRateMvcAllowsSameDateAndActivatesOnlyOneRate()
    {
        using var client = ClientWithoutRedirect();
        var firstPath = await CreateExchangeRateAsync(client, "2026-08-13", "500");
        var secondPath = await CreateExchangeRateAsync(client, "2026-08-13", "510");
        var thirdPath = await CreateExchangeRateAsync(client, "2026-08-13", "520");

        var index = await client.GetStringAsync("/TiposCambio");
        Assert.Contains("500,00", index);
        Assert.Contains("510,00", index);
        Assert.Contains("520,00", index);

        var secondDetail = await client.GetStringAsync(secondPath);
        using var activateSecond = await PostFormAsync(client, secondPath.Replace("Details", "Activate", StringComparison.OrdinalIgnoreCase), secondDetail, []);
        Assert.Equal(HttpStatusCode.Redirect, activateSecond.StatusCode);

        var activeSecond = await client.GetStringAsync(secondPath);
        var inactiveThird = await client.GetStringAsync(thirdPath);
        Assert.Contains("Activo</dt><dd>Si", activeSecond);
        Assert.Contains("Activo</dt><dd>No", inactiveThird);

        var thirdDetail = await client.GetStringAsync(thirdPath);
        using var activateThird = await PostFormAsync(client, thirdPath.Replace("Details", "Activate", StringComparison.OrdinalIgnoreCase), thirdDetail, []);
        Assert.Equal(HttpStatusCode.Redirect, activateThird.StatusCode);

        var inactiveSecond = await client.GetStringAsync(secondPath);
        var activeThird = await client.GetStringAsync(thirdPath);
        var firstDetail = await client.GetStringAsync(firstPath);
        Assert.Contains("Activo</dt><dd>No", firstDetail);
        Assert.Contains("Activo</dt><dd>No", inactiveSecond);
        Assert.Contains("Activo</dt><dd>Si", activeThird);
    }

    [Fact]
    public async Task ThemePreferenceIsStoredInCookieAndLayoutReflectsDarkTheme()
    {
        using var client = ClientWithoutRedirect();

        using var preference = await client.PostAsync("/Preferencias/Tema?tema=dark&returnUrl=%2F", null);

        Assert.Equal(HttpStatusCode.Redirect, preference.StatusCode);
        Assert.Contains("licitaciones.theme=dark", string.Join(';', preference.Headers.GetValues("Set-Cookie")));

        client.DefaultRequestHeaders.Add("Cookie", "licitaciones.theme=dark");
        var home = await client.GetStringAsync("/");
        Assert.Contains("data-bs-theme=\"dark\"", home);
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

    private static async Task<string> CreateExchangeRateAsync(HttpClient client, string fecha, string crcPorUsd)
    {
        var createPage = await GetHtmlAsync(client, "/TiposCambio/Create");
        using var create = await PostFormAsync(client, "/TiposCambio/Create", createPage, new()
        {
            ["Fecha"] = fecha,
            ["CrcPorUsd"] = crcPorUsd
        });

        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);
        return create.Headers.Location!.ToString();
    }

    private static string ExtractAntiForgeryToken(string html) => WebUtility.HtmlDecode(AntiForgeryTokenRegex().Match(html).Groups["token"].Value);
    private static string ExtractInput(string html, string name) => WebUtility.HtmlDecode(InputRegex(name).Match(html).Groups["value"].Value);

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>[^\"]+)\"")]
    private static partial Regex AntiForgeryTokenRegex();

    [GeneratedRegex("<input[^>]*name=\"CrcPorUsd\"[^>]*>", RegexOptions.IgnoreCase)]
    private static partial Regex CrcPorUsdInputRegex();

    private static Regex InputRegex(string name) => new($"name=\"{name}\" type=\"hidden\" value=\"(?<value>[^\"]*)\"", RegexOptions.IgnoreCase);
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class Iteration4MvcGroup
{
    public const string Name = "Iteration 4 MVC tests";
}

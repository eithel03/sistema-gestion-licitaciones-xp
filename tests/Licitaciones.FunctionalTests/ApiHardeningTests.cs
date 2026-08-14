using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Licitaciones.Application.TiposCambio;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(ApiHardeningGroup.Name)]
public sealed class ApiHardeningTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_api_hardening_tests")
        .WithUsername("api_hardening")
        .WithPassword("api_hardening")
        .Build();
    private WebApplicationFactory<Program>? _factory;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _container.GetConnectionString());
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
                new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString() }));
        });
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LicitacionesDbContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        _factory?.Dispose();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", null);
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task OpenApiDocumentIncludesVersionedContracts()
    {
        using var client = _factory!.CreateClient();

        using var response = await client.GetAsync("/swagger/v1/swagger.json");
        var document = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("3.0.1", document.GetProperty("openapi").GetString());
        Assert.True(document.GetProperty("paths").TryGetProperty("/api/v1/proveedores", out _));
        Assert.True(document.GetProperty("paths").TryGetProperty("/api/v1/tipos-cambio", out _));
        Assert.True(document.GetProperty("paths").TryGetProperty("/api/v1/tipos-cambio/{id}/activar", out var activatePath));
        Assert.True(activatePath.TryGetProperty("patch", out _));
        Assert.False(activatePath.TryGetProperty("post", out _));
        var estadoPatch = document.GetProperty("paths").GetProperty("/api/v1/licitaciones/{id}/estado").GetProperty("patch");
        var requestSchemaRef = estadoPatch.GetProperty("requestBody")
            .GetProperty("content")
            .GetProperty("application/json")
            .GetProperty("schema")
            .GetProperty("$ref")
            .GetString();
        Assert.Equal("#/components/schemas/CambiarEstadoLicitacionRequest", requestSchemaRef);
    }

    [Fact]
    public async Task OpenApiDocumentsOnlyRealHttpMethodsForVersionedRoutes()
    {
        using var client = _factory!.CreateClient();

        var document = await client.GetFromJsonAsync<JsonElement>("/swagger/v1/swagger.json");
        var paths = document.GetProperty("paths");

        AssertDocumentedMethods(paths, "/api/v1/proveedores", "get", "post");
        AssertDocumentedMethods(paths, "/api/v1/proveedores/{id}", "get", "put", "delete");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones", "get", "post");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones/{id}", "get", "put", "delete");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones/{id}/publish", "post");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones/{id}/close", "post");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones/{id}/estado", "patch");
        AssertDocumentedMethods(paths, "/api/v1/ofertas", "get", "post");
        AssertDocumentedMethods(paths, "/api/v1/ofertas/{id}", "get", "put", "delete");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones/{id}/ofertas", "get", "post");
        AssertDocumentedMethods(paths, "/api/v1/licitaciones/{id}/mejor-oferta", "get");
        AssertDocumentedMethods(paths, "/api/v1/niveles-aprobacion", "get", "post");
        AssertDocumentedMethods(paths, "/api/v1/niveles-aprobacion/{id}", "get", "put", "delete");
        AssertDocumentedMethods(paths, "/api/v1/niveles-aprobacion/aprobador", "get");
        AssertDocumentedMethods(paths, "/api/v1/tipos-cambio", "get", "post");
        AssertDocumentedMethods(paths, "/api/v1/tipos-cambio/activo", "get");
        AssertDocumentedMethods(paths, "/api/v1/tipos-cambio/{id}", "get", "put", "delete");
        AssertDocumentedMethods(paths, "/api/v1/tipos-cambio/{id}/activar", "patch");
        AssertDocumentedMethods(paths, "/api/v1/moneda/convertir", "get");
    }

    [Fact]
    public async Task SwaggerRouteServesInteractiveHtmlUi()
    {
        using var client = _factory!.CreateClient();

        using var response = await client.GetAsync("/swagger");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType!.MediaType);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("swagger-ui", html, StringComparison.OrdinalIgnoreCase);
        var initializer = await client.GetStringAsync("/swagger/swagger-initializer.js");
        Assert.Contains("swagger.json", initializer, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ProblemDetailsAndResponsesIncludeCorrelationId()
    {
        using var client = _factory!.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/tipos-cambio")
        {
            Content = JsonContent.Create(new CrearTipoCambioRequest(new DateOnly(2026, 8, 13), 0m))
        };
        request.Headers.Add("X-Correlation-ID", "test-correlation-id");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("test-correlation-id", response.Headers.GetValues("X-Correlation-ID").Single());
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("TipoCambio.ValorInvalido", body);
    }

    [Theory]
    [InlineData("/api/v1/licitaciones/00000000-0000-0000-0000-000000000001", "Licitacion.NoEncontrada")]
    [InlineData("/api/v1/proveedores/00000000-0000-0000-0000-000000000001", "Proveedor.NoEncontrado")]
    [InlineData("/api/v1/ofertas/00000000-0000-0000-0000-000000000001", "Oferta.NoEncontrada")]
    [InlineData("/api/v1/niveles-aprobacion/00000000-0000-0000-0000-000000000001", "NivelAprobacion.NoEncontrado")]
    [InlineData("/api/v1/tipos-cambio/00000000-0000-0000-0000-000000000001", "TipoCambio.NoEncontrado")]
    public async Task NotFoundProblemDetailsIncludesCodeCorrelationIdAndSafeBody(string path, string expectedCode)
    {
        using var client = _factory!.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Add("X-Correlation-ID", "problem-details-correlation-id");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        Assert.Equal("problem-details-correlation-id", response.Headers.GetValues("X-Correlation-ID").Single());

        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("title", out var title));
        Assert.False(string.IsNullOrWhiteSpace(title.GetString()));
        Assert.Equal(404, root.GetProperty("status").GetInt32());
        Assert.True(root.TryGetProperty("detail", out var detail));
        Assert.False(string.IsNullOrWhiteSpace(detail.GetString()));
        Assert.Equal(expectedCode, root.GetProperty("code").GetString());
        Assert.Equal("problem-details-correlation-id", root.GetProperty("correlationId").GetString());
        Assert.DoesNotContain("Exception", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("StackTrace", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(" at ", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ConnectionStrings", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Npgsql", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Licitaciones.Infrastructure", body, StringComparison.OrdinalIgnoreCase);
    }

    private static void AssertDocumentedMethods(JsonElement paths, string path, params string[] expectedMethods)
    {
        Assert.True(paths.TryGetProperty(path, out var pathDocument), $"Expected OpenAPI path '{path}'.");
        var actualMethods = pathDocument.EnumerateObject()
            .Select(property => property.Name)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(expectedMethods.Order(StringComparer.Ordinal).ToArray(), actualMethods);
    }
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ApiHardeningGroup
{
    public const string Name = "API hardening tests";
}

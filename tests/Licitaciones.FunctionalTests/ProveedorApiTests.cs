using System.Net;
using System.Net.Http.Json;
using Licitaciones.Application.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(ProveedorApiTestGroup.Name)]
public sealed class ProveedorApiTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_api_tests")
        .WithUsername("licitaciones_api_tests")
        .WithPassword("licitaciones_api_tests")
        .Build();

    private WebApplicationFactory<Program>? _factory;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _container.GetConnectionString());

        _factory = new WebApplicationFactory<Program>()
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
    public async Task CreateProviderReturnsCreatedAndCanBeRead()
    {
        using var client = _factory!.CreateClient();

        using var createResponse = await client.PostAsJsonAsync(
            "/api/v1/proveedores",
            new CrearProveedorRequest("Empresa Central"));

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<ProveedorResponse>();
        Assert.NotNull(created);

        using var getResponse = await client.GetAsync($"/api/v1/proveedores/{created.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateProviderApiAcceptsUnicodeLettersAndRejectsDisallowedSymbols()
    {
        using var client = _factory!.CreateClient();
        string[] validNames =
        [
            "Tecnología Empresarial CR",
            "Compañía Nacional 2026",
            "Empresa Ñandú",
            "Servicios Técnicos, S.A.",
            "Soluciones (Costa Rica)"
        ];
        string[] invalidNames =
        [
            "Empresa @ CR",
            "Proveedor #1",
            "Empresa / Servicios",
            "Proveedor & Asociados"
        ];

        foreach (var name in validNames)
        {
            using var response = await client.PostAsJsonAsync("/api/v1/proveedores", new CrearProveedorRequest(name));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        foreach (var name in invalidNames)
        {
            using var response = await client.PostAsJsonAsync("/api/v1/proveedores", new CrearProveedorRequest(name));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task CreateDuplicateProviderReturnsConflict()
    {
        using var client = _factory!.CreateClient();
        await client.PostAsJsonAsync("/api/v1/proveedores", new CrearProveedorRequest("Empresa Central"));

        using var duplicateResponse = await client.PostAsJsonAsync(
            "/api/v1/proveedores",
            new CrearProveedorRequest(" empresa   central "));

        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task CreateProviderWithTooLongNameReturnsBadRequest()
    {
        using var client = _factory!.CreateClient();

        using var response = await client.PostAsJsonAsync(
            "/api/v1/proveedores",
            new CrearProveedorRequest(new string('A', 201)));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task UpdateAndDeleteProviderUseExpectedStatusCodes()
    {
        using var client = _factory!.CreateClient();
        using var createResponse = await client.PostAsJsonAsync(
            "/api/v1/proveedores",
            new CrearProveedorRequest("Empresa Central"));
        var created = await createResponse.Content.ReadFromJsonAsync<ProveedorResponse>();

        using var updateResponse = await client.PutAsJsonAsync(
            $"/api/v1/proveedores/{created!.Id}",
            new ActualizarProveedorRequest("Empresa Nacional"));

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        using var deleteResponse = await client.DeleteAsync($"/api/v1/proveedores/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ProveedorApiTestGroup
{
    public const string Name = "Proveedor API tests";
}

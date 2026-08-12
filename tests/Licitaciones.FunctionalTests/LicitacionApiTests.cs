using System.Net;
using System.Net.Http.Json;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(LicitacionApiTestGroup.Name)]
public sealed class LicitacionApiTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16").WithDatabase("licitaciones_api_licitaciones_tests").WithUsername("licitaciones_api_licitaciones_tests").WithPassword("licitaciones_api_licitaciones_tests").Build();
    private WebApplicationFactory<Program>? _factory;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _container.GetConnectionString());
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString() }));
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
    public async Task CreatePublishCloseAndRejectInvalidTransitionThroughApi()
    {
        using var client = _factory!.CreateClient();
        var closeDate = DateTimeOffset.UtcNow.AddDays(5);

        using var createResponse = await client.PostAsJsonAsync("/api/v1/licitaciones", new CrearLicitacionRequest("LIC-2026-API", "Compra API", 2500m, closeDate));
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<LicitacionResponse>();
        Assert.NotNull(created);
        Assert.Equal(LicitacionEstado.Borrador, created!.Estado);

        using var publishResponse = await client.PostAsync($"/api/v1/licitaciones/{created.Id}/publish", null);
        Assert.Equal(HttpStatusCode.OK, publishResponse.StatusCode);

        using var repeatedPublish = await client.PostAsync($"/api/v1/licitaciones/{created.Id}/publish", null);
        Assert.Equal(HttpStatusCode.BadRequest, repeatedPublish.StatusCode);

        using var closeResponse = await client.PostAsync($"/api/v1/licitaciones/{created.Id}/close", null);
        Assert.Equal(HttpStatusCode.OK, closeResponse.StatusCode);
    }
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class LicitacionApiTestGroup
{
    public const string Name = "Licitacion API tests";
}

using System.Net;
using System.Net.Http.Json;
using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Aprobaciones;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Application.Ofertas;
using Licitaciones.Application.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(Iteration3ApiGroup.Name)]
public sealed class Iteration3ApiTests : IAsyncLifetime
{
    private static readonly DateTimeOffset Now = new(2026, 8, 12, 16, 0, 0, TimeSpan.Zero);
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_api_iteration3_tests")
        .WithUsername("iteration3_tests")
        .WithPassword("iteration3_tests")
        .Build();
    private readonly MutableClock _clock = new(Now);
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
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IClock>();
                services.AddSingleton<IClock>(_clock);
            });
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
    public async Task CreatesReadsFiltersAndEvaluatesBestOffer()
    {
        _clock.UtcNow = Now;
        using var client = _factory!.CreateClient();
        var licitacion = await CreatePublishedLicitacionAsync(client, "LIC-API-OF-1", 1000m);
        var firstProvider = await CreateProviderAsync(client, "Proveedor API Uno");
        var secondProvider = await CreateProviderAsync(client, "Proveedor API Dos");
        using var levelResponse = await client.PostAsJsonAsync("/api/v1/niveles-aprobacion", new CrearNivelAprobacionRequest(0.01m, null, "Gerencia Persistida"));
        Assert.Equal(HttpStatusCode.Created, levelResponse.StatusCode);

        using var firstCreate = await client.PostAsJsonAsync($"/api/v1/licitaciones/{licitacion.Id}/ofertas", new CrearOfertaLicitacionRequest(firstProvider.Id, 900m));
        using var secondCreate = await client.PostAsJsonAsync("/api/v1/ofertas", new CrearOfertaRequest(licitacion.Id, secondProvider.Id, 800m));
        Assert.Equal(HttpStatusCode.Created, firstCreate.StatusCode);
        Assert.Equal(HttpStatusCode.Created, secondCreate.StatusCode);
        var created = await secondCreate.Content.ReadFromJsonAsync<OfertaResponse>();

        using var detail = await client.GetAsync($"/api/v1/ofertas/{created!.Id}");
        var filtered = await client.GetFromJsonAsync<OfertaPage>($"/api/v1/ofertas?licitacionId={licitacion.Id}&proveedorId={secondProvider.Id}");
        var byLicitacion = await client.GetFromJsonAsync<OfertaPage>($"/api/v1/licitaciones/{licitacion.Id}/ofertas");
        var best = await client.GetFromJsonAsync<MejorOfertaResponse>($"/api/v1/licitaciones/{licitacion.Id}/mejor-oferta");

        Assert.Equal(HttpStatusCode.OK, detail.StatusCode);
        Assert.Single(filtered!.Items);
        Assert.Equal(2, byLicitacion!.TotalItems);
        Assert.Equal(800m, best!.MejorOferta!.MontoOfertadoCrc);
        Assert.Equal(20m, best.PorcentajeAhorro);
        Assert.Equal("Gerencia Persistida", best.Aprobador);
    }

    [Fact]
    public async Task RejectsDuplicateAboveBudgetAndExpiredOffer()
    {
        _clock.UtcNow = Now;
        using var client = _factory!.CreateClient();
        var licitacion = await CreatePublishedLicitacionAsync(client, "LIC-API-OF-2", 1000m);
        var provider = await CreateProviderAsync(client, "Proveedor API Tres");
        using var valid = await client.PostAsJsonAsync("/api/v1/ofertas", new CrearOfertaRequest(licitacion.Id, provider.Id, 900m));
        using var duplicate = await client.PostAsJsonAsync("/api/v1/ofertas", new CrearOfertaRequest(licitacion.Id, provider.Id, 800m));
        var another = await CreateProviderAsync(client, "Proveedor API Cuatro");
        using var above = await client.PostAsJsonAsync("/api/v1/ofertas", new CrearOfertaRequest(licitacion.Id, another.Id, 1000.01m));
        _clock.UtcNow = Now.AddDays(2);
        using var expired = await client.PostAsJsonAsync("/api/v1/ofertas", new CrearOfertaRequest(licitacion.Id, another.Id, 800m));

        Assert.Equal(HttpStatusCode.Created, valid.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, duplicate.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, above.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, expired.StatusCode);
    }

    [Fact]
    public async Task ApprovalLevelCrudRejectsOverlap()
    {
        _clock.UtcNow = Now;
        using var client = _factory!.CreateClient();
        using var createdResponse = await client.PostAsJsonAsync("/api/v1/niveles-aprobacion", new CrearNivelAprobacionRequest(1000000m, 9999999.99m, "Gerencia API"));
        Assert.Equal(HttpStatusCode.Created, createdResponse.StatusCode);
        var created = await createdResponse.Content.ReadFromJsonAsync<NivelAprobacionResponse>();

        using var overlap = await client.PostAsJsonAsync("/api/v1/niveles-aprobacion", new CrearNivelAprobacionRequest(9999999.99m, null, "Junta"));
        using var update = await client.PutAsJsonAsync($"/api/v1/niveles-aprobacion/{created!.Id}", new ActualizarNivelAprobacionRequest(1000000m, 9000000m, "Gerencia Editada", created.Version));
        using var detail = await client.GetAsync($"/api/v1/niveles-aprobacion/{created.Id}");
        using var delete = await client.DeleteAsync($"/api/v1/niveles-aprobacion/{created.Id}");

        Assert.Equal(HttpStatusCode.Conflict, overlap.StatusCode);
        Assert.Equal(HttpStatusCode.OK, update.StatusCode);
        Assert.Equal(HttpStatusCode.OK, detail.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    private static async Task<LicitacionResponse> CreatePublishedLicitacionAsync(HttpClient client, string code, decimal budget)
    {
        using var create = await client.PostAsJsonAsync("/api/v1/licitaciones", new CrearLicitacionRequest(code, "Compra API", budget, Now.AddDays(1)));
        var licitacion = await create.Content.ReadFromJsonAsync<LicitacionResponse>();
        using var publish = await client.PostAsync($"/api/v1/licitaciones/{licitacion!.Id}/publish", null);
        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        return licitacion;
    }

    private static async Task<ProveedorResponse> CreateProviderAsync(HttpClient client, string name)
    {
        using var response = await client.PostAsJsonAsync("/api/v1/proveedores", new CrearProveedorRequest(name));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProveedorResponse>())!;
    }

    private sealed class MutableClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; set; } = utcNow;
    }
}

public sealed record CrearOfertaLicitacionRequest(Guid ProveedorId, decimal MontoOfertadoCrc);

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class Iteration3ApiGroup
{
    public const string Name = "Iteration 3 API tests";
}

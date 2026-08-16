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
    private static readonly DateTimeOffset Now = new(2026, 8, 16, 10, 0, 0, TimeSpan.Zero);
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
        var closeDate = Now.AddDays(5);

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

    [Fact]
    public async Task PatchEstadoSupportsOfficialAllowedTransitions()
    {
        using var client = _factory!.CreateClient();

        var draftToPublished = await CreateTenderAsync(client, "LIC-PATCH-PUB");
        using var publish = await PatchEstadoAsync(client, draftToPublished.Id, "Publicada");
        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        var published = await publish.Content.ReadFromJsonAsync<LicitacionResponse>();
        Assert.Equal(LicitacionEstado.Publicada, published!.Estado);

        var draftToClosed = await CreateTenderAsync(client, "LIC-PATCH-CLOSED");
        using var closeDraft = await PatchEstadoAsync(client, draftToClosed.Id, "Cerrada");
        Assert.Equal(HttpStatusCode.OK, closeDraft.StatusCode);
        var closedFromDraft = await closeDraft.Content.ReadFromJsonAsync<LicitacionResponse>();
        Assert.Equal(LicitacionEstado.Cerrada, closedFromDraft!.Estado);

        var publishedToClosed = await CreateTenderAsync(client, "LIC-PATCH-PUB-CLOSED");
        using var publishFirst = await PatchEstadoAsync(client, publishedToClosed.Id, "Publicada");
        Assert.Equal(HttpStatusCode.OK, publishFirst.StatusCode);
        using var closePublished = await PatchEstadoAsync(client, publishedToClosed.Id, "Cerrada");
        Assert.Equal(HttpStatusCode.OK, closePublished.StatusCode);
        var closedFromPublished = await closePublished.Content.ReadFromJsonAsync<LicitacionResponse>();
        Assert.Equal(LicitacionEstado.Cerrada, closedFromPublished!.Estado);
    }

    [Fact]
    public async Task PatchEstadoRejectsOfficialInvalidTransitions()
    {
        using var client = _factory!.CreateClient();

        var publishedTender = await CreateTenderAsync(client, "LIC-PATCH-PUB-DRAFT");
        using var publish = await PatchEstadoAsync(client, publishedTender.Id, "Publicada");
        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        using var publishedToDraft = await PatchEstadoAsync(client, publishedTender.Id, "Borrador");
        Assert.Equal(HttpStatusCode.BadRequest, publishedToDraft.StatusCode);
        Assert.Equal("application/problem+json", publishedToDraft.Content.Headers.ContentType!.MediaType);

        var closedTender = await CreateTenderAsync(client, "LIC-PATCH-CLOSED-PUB");
        using var close = await PatchEstadoAsync(client, closedTender.Id, "Cerrada");
        Assert.Equal(HttpStatusCode.OK, close.StatusCode);
        using var closedToPublished = await PatchEstadoAsync(client, closedTender.Id, "Publicada");
        Assert.Equal(HttpStatusCode.BadRequest, closedToPublished.StatusCode);
        Assert.Equal("application/problem+json", closedToPublished.Content.Headers.ContentType!.MediaType);
        using var closedToDraft = await PatchEstadoAsync(client, closedTender.Id, "Borrador");
        Assert.Equal(HttpStatusCode.BadRequest, closedToDraft.StatusCode);
        Assert.Equal("application/problem+json", closedToDraft.Content.Headers.ContentType!.MediaType);
    }

    [Fact]
    public async Task PatchEstadoForUnknownTenderReturnsNotFoundProblemDetails()
    {
        using var client = _factory!.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/licitaciones/00000000-0000-0000-0000-000000000001/estado")
        {
            Content = JsonContent.Create(new CambiarEstadoLicitacionRequest("Publicada"))
        };
        request.Headers.Add("X-Correlation-ID", "licitacion-estado-not-found");

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        Assert.Equal("licitacion-estado-not-found", response.Headers.GetValues("X-Correlation-ID").Single());
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Licitacion.NoEncontrada", body);
        Assert.Contains("licitacion-estado-not-found", body);
    }

    private static async Task<LicitacionResponse> CreateTenderAsync(HttpClient client, string code)
    {
        using var createResponse = await client.PostAsJsonAsync(
            "/api/v1/licitaciones",
            new CrearLicitacionRequest(code, "Compra API", 2500m, Now.AddDays(5)));

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        return (await createResponse.Content.ReadFromJsonAsync<LicitacionResponse>())!;
    }

    private static async Task<HttpResponseMessage> PatchEstadoAsync(HttpClient client, Guid id, string estado)
    {
        using var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/licitaciones/{id}/estado")
        {
            Content = JsonContent.Create(new CambiarEstadoLicitacionRequest(estado))
        };

        return await client.SendAsync(request);
    }
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class LicitacionApiTestGroup
{
    public const string Name = "Licitacion API tests";
}

using System.Net;
using System.Net.Http.Json;
using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.TiposCambio;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Licitaciones.FunctionalTests;

[Collection(Iteration4ApiGroup.Name)]
public sealed class Iteration4ApiTests : IAsyncLifetime
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 10, 0, 0, TimeSpan.Zero);
    private static readonly DateOnly Fecha = new(2026, 8, 13);
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_api_iteration4_tests")
        .WithUsername("iteration4_tests")
        .WithPassword("iteration4_tests")
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
    public async Task ExchangeRateApiSupportsCrudActivationAndConversion()
    {
        using var client = _factory!.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/api/v1/tipos-cambio", new CrearTipoCambioRequest(Fecha, 500m));
        using var secondResponse = await client.PostAsJsonAsync("/api/v1/tipos-cambio", new CrearTipoCambioRequest(Fecha.AddDays(1), 525m));
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Created, secondResponse.StatusCode);
        var first = await firstResponse.Content.ReadFromJsonAsync<TipoCambioResponse>();
        var second = await secondResponse.Content.ReadFromJsonAsync<TipoCambioResponse>();

        using var firstActive = await client.PatchAsync($"/api/v1/tipos-cambio/{first!.Id}/activar", null);
        using var secondActive = await client.PatchAsync($"/api/v1/tipos-cambio/{second!.Id}/activar", null);
        using var postActivation = await client.PostAsync($"/api/v1/tipos-cambio/{second.Id}/activar", null);
        var active = await client.GetFromJsonAsync<TipoCambioResponse>("/api/v1/tipos-cambio/activo");
        var firstDetail = await client.GetFromJsonAsync<TipoCambioResponse>($"/api/v1/tipos-cambio/{first.Id}");
        var secondDetail = await client.GetFromJsonAsync<TipoCambioResponse>($"/api/v1/tipos-cambio/{second.Id}");
        var conversion = await client.GetFromJsonAsync<MontoVisualizadoResponse>("/api/v1/moneda/convertir?montoCrc=1050&moneda=USD");
        var listed = await client.GetFromJsonAsync<TipoCambioPage>("/api/v1/tipos-cambio");

        Assert.Equal(HttpStatusCode.OK, firstActive.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondActive.StatusCode);
        Assert.Equal(HttpStatusCode.MethodNotAllowed, postActivation.StatusCode);
        Assert.Equal(second.Id, active!.Id);
        Assert.False(firstDetail!.Activo);
        Assert.True(secondDetail!.Activo);
        Assert.Equal(2m, conversion!.MontoVisualizado);
        Assert.Equal(1050m, conversion.MontoOriginalCrc);
        Assert.Equal(2, listed!.TotalItems);
        Assert.Single(listed.Items, tipoCambio => tipoCambio.Activo);
    }

    [Fact]
    public async Task ExchangeRateApiUsesProblemDetailsForValidationErrors()
    {
        using var client = _factory!.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/v1/tipos-cambio", new CrearTipoCambioRequest(Fecha, 0m));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType!.MediaType);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("TipoCambio.ValorInvalido", body);
    }

    private sealed class MutableClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; set; } = utcNow;
    }
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class Iteration4ApiGroup
{
    public const string Name = "Iteration 4 API tests";
}

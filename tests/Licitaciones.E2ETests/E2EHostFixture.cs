using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Proveedores;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Licitaciones.E2ETests;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class E2ETestGroup : ICollectionFixture<E2EHostFixture>
{
    public const string Name = "Chromium E2E";
}

public sealed class E2EHostFixture : IAsyncLifetime
{
    public static readonly DateTimeOffset Now = new(2035, 8, 15, 10, 0, 0, TimeSpan.Zero);

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_e2e_tests")
        .WithUsername("licitaciones_e2e")
        .WithPassword("licitaciones_e2e")
        .Build();
    private Process? _webProcess;
    private Task<string>? _standardOutput;
    private Task<string>? _standardError;

    public string BaseUrl { get; private set; } = string.Empty;
    public string ConnectionString => _postgres.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        await InitializeDatabaseAsync();

        var port = FindFreePort();
        BaseUrl = $"http://127.0.0.1:{port}";
        var root = FindRepositoryRoot();
        var webProject = Path.Combine(root, "src", "Licitaciones.Web", "Licitaciones.Web.csproj");
        var startInfo = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = root,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(webProject);
        startInfo.ArgumentList.Add("--configuration");
        startInfo.ArgumentList.Add("Release");
        startInfo.ArgumentList.Add("--no-restore");
        startInfo.ArgumentList.Add("--no-launch-profile");
        startInfo.ArgumentList.Add("--urls");
        startInfo.ArgumentList.Add(BaseUrl);
        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Testing";
        startInfo.Environment["ConnectionStrings__DefaultConnection"] = ConnectionString;

        _webProcess = Process.Start(startInfo) ?? throw new InvalidOperationException("No se pudo iniciar Licitaciones.Web.");
        _standardOutput = _webProcess.StandardOutput.ReadToEndAsync();
        _standardError = _webProcess.StandardError.ReadToEndAsync();
        await WaitForWebAsync();
    }

    public async Task DisposeAsync()
    {
        if (_webProcess is { HasExited: false })
        {
            _webProcess.Kill(entireProcessTree: true);
            await _webProcess.WaitForExitAsync();
        }

        _webProcess?.Dispose();
        await _postgres.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        await using var context = new LicitacionesDbContext(options);
        await context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"Ofertas\", \"NivelesAprobacion\", \"Licitaciones\", \"Proveedores\", \"TiposCambio\" RESTART IDENTITY CASCADE;");
    }

    private async Task InitializeDatabaseAsync()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        await using var context = new LicitacionesDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    public async Task<(Guid LicitacionId, Guid ProveedorId)> SeedPublishedOfferScenarioAsync()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        await using var context = new LicitacionesDbContext(options);
        var licitacion = Licitacion.Create("E2E-LIC-OFERTA", "Compra E2E", 1000m, Now.AddDays(10), Now);
        licitacion.Publish(Now.AddHours(1));
        var proveedor = Proveedor.Create("Proveedor E2E Oferta", Now);
        context.AddRange(licitacion, proveedor);
        await context.SaveChangesAsync();
        return (licitacion.Id, proveedor.Id);
    }

    public async Task<Guid> SeedPublishedLicitacionAsync()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        await using var context = new LicitacionesDbContext(options);
        var licitacion = Licitacion.Create("E2E-LIC-MONEDA", "Compra moneda E2E", 1000m, Now.AddDays(10), Now);
        licitacion.Publish(Now.AddHours(1));
        context.Licitaciones.Add(licitacion);
        await context.SaveChangesAsync();
        return licitacion.Id;
    }

    private async Task WaitForWebAsync()
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        for (var attempt = 0; attempt < 120; attempt++)
        {
            if (_webProcess is { HasExited: true })
            {
                var error = _standardError is null ? string.Empty : await _standardError;
                throw new InvalidOperationException($"Licitaciones.Web terminó durante el arranque. {error}");
            }

            try
            {
                using var response = await client.GetAsync($"{BaseUrl}/health");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return;
                }
            }
            catch (HttpRequestException)
            {
            }
            catch (TaskCanceledException)
            {
            }

            await Task.Delay(500);
        }

        var output = _standardOutput is null ? string.Empty : await _standardOutput;
        var errorOutput = _standardError is null ? string.Empty : await _standardError;
        throw new TimeoutException($"Licitaciones.Web no respondió en el tiempo esperado.\n{output}\n{errorOutput}");
    }

    private static int FindFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Licitaciones.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new DirectoryNotFoundException("No se encontró la raíz del repositorio.");
    }
}

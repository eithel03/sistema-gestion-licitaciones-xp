using Licitaciones.Application;
using Licitaciones.Api;
using Licitaciones.Infrastructure;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var healthChecks = builder.Services.AddHealthChecks();

if (!builder.Environment.IsEnvironment("Testing"))
{
    healthChecks.AddInfrastructureHealthChecks(builder.Configuration);
}

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LicitacionesDbContext>();
    dbContext.Database.Migrate();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync(report.Status.ToString());
    }
});

app.MapProveedorEndpoints();
app.MapLicitacionEndpoints();
app.MapOfertaEndpoints();
app.MapNivelAprobacionEndpoints();

app.Run();

public partial class Program { }

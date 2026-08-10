using Licitaciones.Application;
using Licitaciones.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync(report.Status.ToString());
    }
});

app.Run();

public partial class Program { }

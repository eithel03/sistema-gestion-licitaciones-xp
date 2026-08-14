using Licitaciones.Application;
using Licitaciones.Api;
using Licitaciones.Infrastructure;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.HttpContext.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var correlationId))
        {
            context.ProblemDetails.Extensions["correlationId"] = correlationId;
        }
    };
});
var healthChecks = builder.Services.AddHealthChecks();

if (!builder.Environment.IsEnvironment("Testing"))
{
    healthChecks.AddInfrastructureHealthChecks(builder.Configuration);
}

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await Results.Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Ocurrio un error inesperado.").ExecuteAsync(context);
    });
});

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LicitacionesDbContext>();
    dbContext.Database.Migrate();
}

app.MapOpenApiEndpoints();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync(report.Status.ToString());
    }
});

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Gestion de Licitaciones API v1");
    options.DocumentTitle = "Sistema de Gestion de Licitaciones API";
});

app.MapProveedorEndpoints();
app.MapLicitacionEndpoints();
app.MapOfertaEndpoints();
app.MapNivelAprobacionEndpoints();
app.MapTipoCambioEndpoints();

app.Run();

public partial class Program { }

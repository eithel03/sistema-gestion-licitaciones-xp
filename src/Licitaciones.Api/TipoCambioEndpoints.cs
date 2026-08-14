using Licitaciones.Application.TiposCambio;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Api;

public static class TipoCambioEndpoints
{
    public static IEndpointRouteBuilder MapTipoCambioEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/tipos-cambio").WithTags("Tipos de cambio");
        group.MapGet("/", ListAsync);
        group.MapGet("/activo", GetActiveAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapPatch("/{id:guid}/activar", ActivateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);

        routes.MapGet("/api/v1/moneda/convertir", ConvertAsync).WithTags("Moneda");
        return routes;
    }

    private static async Task<IResult> ListAsync(ITipoCambioService service, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new TipoCambioQuery(page, pageSize), cancellationToken);
        return Results.Ok(result.Value!);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, ITipoCambioService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> GetActiveAsync(ITipoCambioService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.GetActiveAsync(cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> CreateAsync(CrearTipoCambioRequest request, ITipoCambioService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return result.Succeeded
            ? Results.Created($"/api/v1/tipos-cambio/{result.Value!.Id}", result.Value)
            : ToError(result, context);
    }

    private static async Task<IResult> UpdateAsync(Guid id, ActualizarTipoCambioRequest request, ITipoCambioService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> ActivateAsync(Guid id, ITipoCambioService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.ActivateAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> DeleteAsync(Guid id, ITipoCambioService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.Succeeded ? Results.NoContent() : ToError(result, context);
    }

    private static async Task<IResult> ConvertAsync(decimal montoCrc, MonedaVisualizacion moneda, IMonedaConversionService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.ConvertFromCrcAsync(montoCrc, moneda, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static IResult ToError<T>(TipoCambioResult<T> result, HttpContext context)
    {
        var status = result.Status switch
        {
            TipoCambioResultStatus.NotFound => 404,
            TipoCambioResultStatus.Conflict or TipoCambioResultStatus.ConcurrencyConflict => 409,
            _ => 400
        };
        return ApiProblemResults.Problem(context, status, result.ErrorMessage, result.ErrorMessage, result.ErrorCode ?? "TipoCambio.Error");
    }
}

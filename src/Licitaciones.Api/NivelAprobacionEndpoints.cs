using Licitaciones.Application.Aprobaciones;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Api;

public static class NivelAprobacionEndpoints
{
    public static IEndpointRouteBuilder MapNivelAprobacionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/niveles-aprobacion").WithTags("Niveles de aprobacion");
        group.MapGet("/", ListAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
        group.MapGet("/aprobador", FindApproverAsync);
        return routes;
    }

    private static async Task<IResult> ListAsync(INivelAprobacionService service, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new NivelAprobacionQuery(page, pageSize), cancellationToken);
        return Results.Ok(result.Value!);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, INivelAprobacionService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> CreateAsync(CrearNivelAprobacionRequest request, INivelAprobacionService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return result.Succeeded
            ? Results.Created($"/api/v1/niveles-aprobacion/{result.Value!.Id}", result.Value)
            : ToError(result, context);
    }

    private static async Task<IResult> UpdateAsync(Guid id, ActualizarNivelAprobacionRequest request, INivelAprobacionService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> DeleteAsync(Guid id, INivelAprobacionService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.Succeeded ? Results.NoContent() : ToError(result, context);
    }

    private static async Task<IResult> FindApproverAsync(decimal montoCrc, INivelAprobacionService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.FindApproverAsync(montoCrc, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static IResult ToError<T>(NivelAprobacionResult<T> result, HttpContext context)
    {
        var status = result.Status switch
        {
            NivelAprobacionResultStatus.NotFound => 404,
            NivelAprobacionResultStatus.Conflict or NivelAprobacionResultStatus.ConcurrencyConflict => 409,
            _ => 400
        };
        return ApiProblemResults.Problem(context, status, result.ErrorMessage, result.ErrorMessage, result.ErrorCode ?? "NivelAprobacion.Error");
    }
}

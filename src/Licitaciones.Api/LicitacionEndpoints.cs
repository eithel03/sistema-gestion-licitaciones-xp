using Licitaciones.Application.Licitaciones;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Api;

public static class LicitacionEndpoints
{
    public static RouteGroupBuilder MapLicitacionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/licitaciones").WithTags("Licitaciones");
        group.MapGet("/", ListAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);
        group.MapPost("/{id:guid}/publish", PublishAsync);
        group.MapPost("/{id:guid}/close", CloseAsync);
        return group;
    }

    private static async Task<IResult> ListAsync(ILicitacionService service, int page = 1, int pageSize = 10, string? search = null, string? sort = "code", CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new LicitacionQuery(page, pageSize, search, sort), cancellationToken);
        return Results.Ok(result.Value!);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, ILicitacionService service, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : Results.NotFound(CreateProblem(404, result));
    }

    private static async Task<IResult> CreateAsync(CrearLicitacionRequest request, ILicitacionService service, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        if (result.Succeeded) return Results.Created("/api/v1/licitaciones/" + result.Value!.Id, result.Value);
        return ToError(result);
    }

    private static async Task<IResult> UpdateAsync(Guid id, ActualizarLicitacionRequest request, ILicitacionService service, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result);
    }
    private static async Task<IResult> DeleteAsync(Guid id, ILicitacionService service, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.Succeeded ? Results.NoContent() : ToError(result);
    }
    private static async Task<IResult> PublishAsync(Guid id, ILicitacionService service, CancellationToken cancellationToken)
    {
        var result = await service.PublishAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result);
    }
    private static async Task<IResult> CloseAsync(Guid id, ILicitacionService service, CancellationToken cancellationToken)
    {
        var result = await service.CloseAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result);
    }
    private static IResult ToError(dynamic result)
    {
        int status = result.Status == LicitacionResultStatus.NotFound ? 404 : result.Status == LicitacionResultStatus.Conflict || result.Status == LicitacionResultStatus.ConcurrencyConflict ? 409 : 400;
        var problem = CreateProblem(status, result);
        return status == 404 ? Results.NotFound(problem) : status == 409 ? Results.Conflict(problem) : Results.BadRequest(problem);
    }
    private static ProblemDetails CreateProblem(int status, dynamic result)
    {
        var problem = new ProblemDetails { Status = status, Title = result.ErrorMessage, Detail = result.ErrorMessage };
        problem.Extensions["code"] = result.ErrorCode ?? "Licitacion.Error";
        return problem;
    }
}

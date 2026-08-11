using Licitaciones.Application.Proveedores;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Api;

public static class ProveedorEndpoints
{
    public static RouteGroupBuilder MapProveedorEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/proveedores")
            .WithTags("Proveedores");

        group.MapGet("/", ListAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);

        return group;
    }

    private static async Task<Ok<ProveedorPage>> ListAsync(
        IProveedorService service,
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string? sort = "name",
        CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new ProveedorQuery(page, pageSize, search, sort), cancellationToken);

        return TypedResults.Ok(result.Value!);
    }

    private static async Task<Results<Ok<ProveedorResponse>, NotFound<ProblemDetails>>> GetByIdAsync(
        Guid id,
        IProveedorService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);

        return result.Succeeded
            ? TypedResults.Ok(result.Value!)
            : TypedResults.NotFound(CreateProblem(StatusCodes.Status404NotFound, result));
    }

    private static async Task<Results<Created<ProveedorResponse>, BadRequest<ProblemDetails>, Conflict<ProblemDetails>>> CreateAsync(
        CrearProveedorRequest request,
        IProveedorService service,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        if (result.Succeeded)
        {
            return TypedResults.Created($"/api/v1/proveedores/{result.Value!.Id}", result.Value);
        }

        return result.Status == ProveedorResultStatus.Conflict
            ? TypedResults.Conflict(CreateProblem(StatusCodes.Status409Conflict, result))
            : TypedResults.BadRequest(CreateProblem(StatusCodes.Status400BadRequest, result));
    }

    private static async Task<Results<Ok<ProveedorResponse>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>, Conflict<ProblemDetails>>> UpdateAsync(
        Guid id,
        ActualizarProveedorRequest request,
        IProveedorService service,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);

        if (result.Succeeded)
        {
            return TypedResults.Ok(result.Value!);
        }

        return result.Status switch
        {
            ProveedorResultStatus.NotFound => TypedResults.NotFound(CreateProblem(StatusCodes.Status404NotFound, result)),
            ProveedorResultStatus.Conflict => TypedResults.Conflict(CreateProblem(StatusCodes.Status409Conflict, result)),
            _ => TypedResults.BadRequest(CreateProblem(StatusCodes.Status400BadRequest, result))
        };
    }

    private static async Task<Results<NoContent, NotFound<ProblemDetails>>> DeleteAsync(
        Guid id,
        IProveedorService service,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);

        return result.Succeeded
            ? TypedResults.NoContent()
            : TypedResults.NotFound(CreateProblem(StatusCodes.Status404NotFound, result));
    }

    private static ProblemDetails CreateProblem<T>(int statusCode, ProveedorResult<T> result)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = result.ErrorMessage,
            Detail = result.ErrorMessage,
            Extensions =
            {
                ["code"] = result.ErrorCode ?? "Proveedor.Error"
            }
        };
    }
}

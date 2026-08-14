using Licitaciones.Application.Proveedores;
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

    private static async Task<IResult> ListAsync(
        IProveedorService service,
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string? sort = "name",
        CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new ProveedorQuery(page, pageSize, search, sort), cancellationToken);

        return Results.Ok(result.Value!);
    }

    private static async Task<IResult> GetByIdAsync(
        Guid id,
        IProveedorService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);

        return result.Succeeded
            ? Results.Ok(result.Value!)
            : ToError(result, context);
    }

    private static async Task<IResult> CreateAsync(
        CrearProveedorRequest request,
        IProveedorService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        if (result.Succeeded)
        {
            return Results.Created($"/api/v1/proveedores/{result.Value!.Id}", result.Value);
        }

        return ToError(result, context);
    }

    private static async Task<IResult> UpdateAsync(
        Guid id,
        ActualizarProveedorRequest request,
        IProveedorService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);

        if (result.Succeeded)
        {
            return Results.Ok(result.Value!);
        }

        return ToError(result, context);
    }

    private static async Task<IResult> DeleteAsync(
        Guid id,
        IProveedorService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);

        return result.Succeeded
            ? Results.NoContent()
            : ToError(result, context);
    }

    private static IResult ToError<T>(ProveedorResult<T> result, HttpContext context)
    {
        var status = result.Status switch
        {
            ProveedorResultStatus.NotFound => 404,
            ProveedorResultStatus.Conflict => 409,
            _ => 400
        };
        return ApiProblemResults.Problem(context, status, result.ErrorMessage, result.ErrorMessage, result.ErrorCode ?? "Proveedor.Error");
    }
}

using Licitaciones.Application.Aprobaciones;
using Licitaciones.Application.Ofertas;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Api;

public sealed record CrearOfertaLicitacionRequest(Guid ProveedorId, decimal MontoOfertadoCrc);

public static class OfertaEndpoints
{
    public static IEndpointRouteBuilder MapOfertaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/ofertas").WithTags("Ofertas");
        group.MapGet("/", ListAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);

        routes.MapGet("/api/v1/licitaciones/{id:guid}/ofertas", ListByLicitacionAsync).WithTags("Ofertas");
        routes.MapPost("/api/v1/licitaciones/{id:guid}/ofertas", CreateForLicitacionAsync).WithTags("Ofertas");
        routes.MapGet("/api/v1/licitaciones/{id:guid}/mejor-oferta", GetBestAsync).WithTags("Ofertas");
        return routes;
    }

    private static async Task<IResult> ListAsync(
        IOfertaService service,
        int page = 1,
        int pageSize = 10,
        Guid? licitacionId = null,
        Guid? proveedorId = null,
        string? sort = "registered",
        CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new OfertaQuery(page, pageSize, licitacionId, proveedorId, sort), cancellationToken);
        return Results.Ok(result.Value!);
    }

    private static async Task<IResult> ListByLicitacionAsync(Guid id, IOfertaService service, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await service.ListAsync(new OfertaQuery(page, pageSize, LicitacionId: id), cancellationToken);
        return Results.Ok(result.Value!);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, IOfertaService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> CreateAsync(CrearOfertaRequest request, IOfertaService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return result.Succeeded
            ? Results.Created($"/api/v1/ofertas/{result.Value!.Id}", result.Value)
            : ToError(result, context);
    }

    private static Task<IResult> CreateForLicitacionAsync(Guid id, CrearOfertaLicitacionRequest request, IOfertaService service, HttpContext context, CancellationToken cancellationToken) =>
        CreateAsync(new CrearOfertaRequest(id, request.ProveedorId, request.MontoOfertadoCrc), service, context, cancellationToken);

    private static async Task<IResult> UpdateAsync(Guid id, ActualizarOfertaRequest request, IOfertaService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result.Succeeded ? Results.Ok(result.Value!) : ToError(result, context);
    }

    private static async Task<IResult> DeleteAsync(Guid id, IOfertaService service, HttpContext context, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.Succeeded ? Results.NoContent() : ToError(result, context);
    }

    private static async Task<IResult> GetBestAsync(
        Guid id,
        IOfertaService ofertas,
        INivelAprobacionService niveles,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await ofertas.GetBestAsync(id, cancellationToken);
        if (!result.Succeeded) return ToError(result, context);
        var best = result.Value!;
        if (!best.TieneOferta) return Results.Ok(best);
        var approver = await niveles.FindApproverAsync(best.MejorOferta!.MontoOfertadoCrc, cancellationToken);
        return Results.Ok(best with { Aprobador = approver.Succeeded ? approver.Value!.Aprobador : null });
    }

    private static IResult ToError<T>(OfertaResult<T> result, HttpContext context)
    {
        var status = result.Status switch
        {
            OfertaResultStatus.NotFound => 404,
            OfertaResultStatus.Conflict or OfertaResultStatus.ConcurrencyConflict => 409,
            _ => 400
        };
        return ApiProblemResults.Problem(context, status, result.ErrorMessage, result.ErrorMessage, result.ErrorCode ?? "Oferta.Error");
    }
}

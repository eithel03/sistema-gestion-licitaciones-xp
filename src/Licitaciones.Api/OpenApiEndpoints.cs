namespace Licitaciones.Api;

public static class OpenApiEndpoints
{
    public static IEndpointRouteBuilder MapOpenApiEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/swagger/v1/swagger.json", () => Results.Json(Document));
        return routes;
    }

    private static readonly object Document = new
    {
        openapi = "3.0.1",
        info = new
        {
            title = "Sistema de Gestion de Licitaciones API",
            version = "v1",
            description = "Contratos versionados para proveedores, licitaciones, ofertas, niveles de aprobacion y tipos de cambio."
        },
        paths = new Dictionary<string, object>
        {
            ["/api/v1/proveedores"] = Path("Catalogo de proveedores", "ProveedorResponse", "get", "post"),
            ["/api/v1/proveedores/{id}"] = Path("Proveedor por identificador", "ProveedorResponse", "get", "put", "delete"),
            ["/api/v1/licitaciones"] = Path("Gestion de licitaciones", "LicitacionResponse", "get", "post"),
            ["/api/v1/licitaciones/{id}"] = Path("Licitacion por identificador", "LicitacionResponse", "get", "put", "delete"),
            ["/api/v1/licitaciones/{id}/publish"] = Path("Publicar licitacion", "LicitacionResponse", "post"),
            ["/api/v1/licitaciones/{id}/close"] = Path("Cerrar licitacion", "LicitacionResponse", "post"),
            ["/api/v1/licitaciones/{id}/estado"] = PathWithRequest("Cambiar estado de licitacion", "CambiarEstadoLicitacionRequest", "LicitacionResponse", "patch"),
            ["/api/v1/ofertas"] = Path("Gestion de ofertas", "OfertaResponse", "get", "post"),
            ["/api/v1/ofertas/{id}"] = Path("Oferta por identificador", "OfertaResponse", "get", "put", "delete"),
            ["/api/v1/licitaciones/{id}/ofertas"] = Path("Ofertas por licitacion", "OfertaResponse", "get", "post"),
            ["/api/v1/licitaciones/{id}/mejor-oferta"] = Path("Mejor oferta por licitacion", "OfertaResponse", "get"),
            ["/api/v1/niveles-aprobacion"] = Path("Niveles de aprobacion", "NivelAprobacionResponse", "get", "post"),
            ["/api/v1/niveles-aprobacion/{id}"] = Path("Nivel de aprobacion por identificador", "NivelAprobacionResponse", "get", "put", "delete"),
            ["/api/v1/niveles-aprobacion/aprobador"] = Path("Aprobador por monto", "NivelAprobacionResponse", "get"),
            ["/api/v1/tipos-cambio"] = Path("Tipos de cambio CRC/USD", "TipoCambioResponse", "get", "post"),
            ["/api/v1/tipos-cambio/activo"] = Path("Tipo de cambio activo", "TipoCambioResponse", "get"),
            ["/api/v1/tipos-cambio/{id}"] = Path("Tipo de cambio por identificador", "TipoCambioResponse", "get", "put", "delete"),
            ["/api/v1/tipos-cambio/{id}/activar"] = Path("Activar un tipo de cambio y desactivar el anterior.", "TipoCambioResponse", "patch"),
            ["/api/v1/moneda/convertir"] = Path("Conversion visual desde CRC", "MontoVisualizadoResponse", "get")
        },
        components = new
        {
            schemas = new Dictionary<string, object>
            {
                ["ProblemDetails"] = Schema("Respuesta de error RFC 7807 con codigo de negocio y correlacion por header X-Correlation-ID."),
                ["ProveedorResponse"] = Schema("Proveedor registrado."),
                ["LicitacionResponse"] = Schema("Licitacion con presupuesto persistido en CRC."),
                ["CambiarEstadoLicitacionRequest"] = Schema("Solicitud para cambiar el estado de una licitacion."),
                ["OfertaResponse"] = Schema("Oferta con monto persistido en CRC."),
                ["NivelAprobacionResponse"] = Schema("Nivel de aprobacion por rango en CRC."),
                ["TipoCambioResponse"] = Schema("Tipo de cambio local CRC por USD."),
                ["MontoVisualizadoResponse"] = Schema("Monto original CRC y monto calculado para visualizacion.")
            }
        }
    };

    private static Dictionary<string, object> Path(string summary, string responseSchema, params string[] methods) =>
        methods.ToDictionary(method => method, _ => (object)Operation(summary, responseSchema));

    private static Dictionary<string, object> PathWithRequest(string summary, string requestSchema, string responseSchema, params string[] methods) =>
        methods.ToDictionary(method => method, _ => (object)Operation(summary, responseSchema, requestSchema));

    private static Dictionary<string, object> Operation(string summary, string responseSchema, string? requestSchema = null)
    {
        var operation = new Dictionary<string, object>
        {
            ["summary"] = summary,
            ["responses"] = new Dictionary<string, object>
            {
                ["200"] = Response("Operacion correcta.", responseSchema),
                ["400"] = Response("Solicitud invalida.", "ProblemDetails"),
                ["404"] = Response("Recurso no encontrado.", "ProblemDetails"),
                ["409"] = Response("Conflicto de negocio o concurrencia.", "ProblemDetails")
            }
        };

        if (requestSchema is not null)
        {
            operation["requestBody"] = new
            {
                required = true,
                content = new Dictionary<string, object>
                {
                    ["application/json"] = new
                    {
                        schema = new Dictionary<string, string> { ["$ref"] = $"#/components/schemas/{requestSchema}" }
                    }
                }
            };
        }

        return operation;
    }

    private static object Response(string description, string schema) => new
    {
        description,
        content = new Dictionary<string, object>
        {
            ["application/json"] = new
            {
                schema = new Dictionary<string, string> { ["$ref"] = $"#/components/schemas/{schema}" }
            }
        }
    };

    private static object Schema(string description) => new
    {
        type = "object",
        description
    };
}

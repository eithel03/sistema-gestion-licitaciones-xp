# API REST

## Estado actual

`Licitaciones.Api` es un host ASP.NET Core Minimal API separado de la interfaz MVC. Todas las operaciones de negocio están bajo `/api/v1`. El versionado es fijo por ruta; no existe negociación ni una biblioteca formal de versionado.

## Proveedores

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/v1/proveedores` | `ProveedorPage`. |
| GET | `/api/v1/proveedores/{id}` | `ProveedorResponse` o 404. |
| POST | `/api/v1/proveedores` | Crea y devuelve 201. |
| PUT | `/api/v1/proveedores/{id}` | Actualiza y devuelve 200. |
| DELETE | `/api/v1/proveedores/{id}` | Borrado lógico y 204. |

Listado: `page`, `pageSize`, `search`, `sort` (`name` o `name_desc`).

## Licitaciones

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/v1/licitaciones` | `LicitacionPage`. |
| GET | `/api/v1/licitaciones/{id}` | Detalle o 404. |
| POST | `/api/v1/licitaciones` | Crea en Borrador y devuelve 201. |
| PUT | `/api/v1/licitaciones/{id}` | Actualiza un borrador. |
| DELETE | `/api/v1/licitaciones/{id}` | Borrado lógico y 204. |
| POST | `/api/v1/licitaciones/{id}/publish` | Publica. |
| POST | `/api/v1/licitaciones/{id}/close` | Cierra. |
| PATCH | `/api/v1/licitaciones/{id}/estado` | Cambia estado mediante `CambiarEstadoLicitacionRequest`. |

Listado: `page`, `pageSize`, `search`, `sort` (`code`, `code_desc`, `close_desc`). Las transiciones reales son Borrador→Publicada, Borrador→Cerrada y Publicada→Cerrada.

## Ofertas y mejor oferta

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/v1/ofertas` | `OfertaPage`. |
| GET | `/api/v1/ofertas/{id}` | Detalle o 404. |
| POST | `/api/v1/ofertas` | Crea y devuelve 201. |
| PUT | `/api/v1/ofertas/{id}` | Actualiza monto. |
| DELETE | `/api/v1/ofertas/{id}` | Elimina y devuelve 204. |
| GET | `/api/v1/licitaciones/{id}/ofertas` | Ofertas paginadas de la licitación. |
| POST | `/api/v1/licitaciones/{id}/ofertas` | Crea oferta usando el id de ruta. |
| GET | `/api/v1/licitaciones/{id}/mejor-oferta` | Mejor oferta, ahorro, clasificación y aprobador opcional. |

Listado general: `page`, `pageSize`, `licitacionId`, `proveedorId`, `sort`. Los órdenes reconocidos son `registered`, `registered_desc`, `amount` y `amount_desc`.

El endpoint de mejor oferta devuelve 200 con `tieneOferta=false` cuando no hay ofertas. Cuando existe ganadora, consulta los niveles de aprobación y agrega el nombre del aprobador si se encuentra un rango.

## Niveles de aprobación

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/v1/niveles-aprobacion` | Página ordenada por mínimo. |
| GET | `/api/v1/niveles-aprobacion/{id}` | Detalle o 404. |
| POST | `/api/v1/niveles-aprobacion` | Crea y devuelve 201. |
| PUT | `/api/v1/niveles-aprobacion/{id}` | Actualiza. |
| DELETE | `/api/v1/niveles-aprobacion/{id}` | Elimina y devuelve 204. |
| GET | `/api/v1/niveles-aprobacion/aprobador?montoCrc=...` | Aprobador aplicable o 404. |

Listado: `page`, `pageSize`.

## Tipos de cambio y conversión

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/v1/tipos-cambio` | Página de tipos de cambio. |
| GET | `/api/v1/tipos-cambio/activo` | Registro activo o 404. |
| GET | `/api/v1/tipos-cambio/{id}` | Detalle o 404. |
| POST | `/api/v1/tipos-cambio` | Crea y devuelve 201. |
| PUT | `/api/v1/tipos-cambio/{id}` | Actualiza fecha y valor. |
| PATCH | `/api/v1/tipos-cambio/{id}/activar` | Activa y desactiva el anterior. |
| DELETE | `/api/v1/tipos-cambio/{id}` | Elimina y devuelve 204. |
| GET | `/api/v1/moneda/convertir?montoCrc=...&moneda=...` | Presentación en CRC o USD. |

Listado: `page`, `pageSize`. La conversión USD requiere un tipo activo; CRC se devuelve sin consultar una tasa.

## DTO

Los contratos de negocio están en Application y no exponen directamente entidades EF Core. Incluyen solicitudes de creación/actualización, respuestas, páginas y resultados de mejor oferta, aprobador y moneda. `CrearOfertaLicitacionRequest` vive en el proyecto API para la variante anidada.

## Paginación y filtros

- `page < 1` se normaliza a 1.
- `pageSize < 1` usa el valor por defecto del módulo.
- `pageSize > 100` se limita a 100.
- Proveedores: búsqueda por nombre y orden.
- Licitaciones: búsqueda por código/título y orden.
- Ofertas: filtros por licitación/proveedor y orden.
- Aprobaciones y tipos de cambio: paginación sin filtro textual.

## ProblemDetails y correlación

Los resultados controlados usan `application/problem+json` con:

- `status`;
- `title` y `detail`;
- extensión `code` con el código de negocio;
- extensión `correlationId`.

`CorrelationIdMiddleware` reutiliza `X-Correlation-ID` si el cliente lo envía o genera un GUID. El mismo valor se agrega al encabezado de respuesta. Las excepciones no controladas pasan por un manejador de 500 con un título genérico.

## Códigos HTTP

- 200: consultas y actualizaciones correctas.
- 201: creación correcta.
- 204: eliminación correcta.
- 400: validación o transición inválida.
- 404: recurso o configuración activa no encontrada.
- 409: duplicidad, traslape, activo único o concurrencia.
- 500: excepción inesperada manejada globalmente.

## Health check

`GET /health` devuelve texto con el estado. En ejecución normal, el health check de PostgreSQL se agrega cuando `HealthChecks:PostgreSQL:Enabled=true`; se omite en `Testing`.

## Swagger y OpenAPI

- Swagger UI: `/swagger`.
- Documento: `/swagger/v1/swagger.json`.

El documento OpenAPI actual es un objeto manual definido en `OpenApiEndpoints`. Enumera rutas y métodos principales, pero es superficial:

- los esquemas son objetos sin propiedades;
- la mayoría de POST/PUT no documenta su request body;
- las respuestas se generalizan y no reflejan siempre 201 o 204;
- algunos nombres de esquema de respuesta son aproximados.

Por tanto, Swagger UI existe y permite consultar el documento, pero no debe considerarse una descripción completa de DTO, cuerpos y respuestas. Generar un contrato OpenAPI derivado de los endpoints es trabajo técnico futuro y no se modifica en Fase 9.

## Pruebas relacionadas

`ProveedorApiTests`, `LicitacionApiTests`, `Iteration3ApiTests`, `Iteration4ApiTests`, `ApiHardeningTests` y pruebas E2E/funcionales indirectas. Los resultados numéricos disponibles son evidencia histórica descrita en [pruebas.md](pruebas.md).

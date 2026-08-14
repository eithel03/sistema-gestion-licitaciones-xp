# API Iteración 4

La API conserva las rutas versionadas bajo `/api/v1` y documenta únicamente los verbos HTTP implementados.

## Tipos de cambio y moneda

| Método | Ruta | Resultado principal |
| --- | --- | --- |
| GET | `/api/v1/tipos-cambio` | Lista paginada de tipos de cambio. |
| GET | `/api/v1/tipos-cambio/activo` | Tipo de cambio activo o 404 ProblemDetails. |
| GET | `/api/v1/tipos-cambio/{id}` | Detalle de un tipo de cambio. |
| POST | `/api/v1/tipos-cambio` | Crea un tipo de cambio local y devuelve 201 Created. |
| PUT | `/api/v1/tipos-cambio/{id}` | Actualiza fecha y valor. |
| PATCH | `/api/v1/tipos-cambio/{id}/activar` | Activa el registro, desactiva el activo anterior y mantiene un único activo. |
| DELETE | `/api/v1/tipos-cambio/{id}` | Elimina el registro cuando la regla de negocio lo permite. |
| GET | `/api/v1/moneda/convertir?montoCrc=...&moneda=USD` | Devuelve el monto CRC original y la representación USD calculada. |

`/api/v1/tipos-cambio/activo` solo admite GET. `/api/v1/tipos-cambio/{id}/activar` solo admite PATCH y `/api/v1/moneda/convertir` solo admite GET.

CRC permanece como fuente de verdad persistida. Para presentación se aplica `USD = CRC / CrcPorUsd` con el tipo de cambio activo.

## Estado de licitación

| Método | Ruta | Resultado principal |
| --- | --- | --- |
| PATCH | `/api/v1/licitaciones/{id}/estado` | Cambia el estado mediante las reglas de dominio existentes. |
| POST | `/api/v1/licitaciones/{id}/publish` | Publica una licitación; se conserva por compatibilidad. |
| POST | `/api/v1/licitaciones/{id}/close` | Cierra una licitación; se conserva por compatibilidad. |

DTO de cambio de estado:

```json
{
  "estado": "Publicada"
}
```

Transiciones permitidas:

- Borrador a Publicada.
- Borrador a Cerrada.
- Publicada a Cerrada.

Se rechazan Publicada a Borrador, Cerrada a Publicada y Cerrada a Borrador. Los datos y las transiciones inválidas producen 400 ProblemDetails; un identificador inexistente produce 404 ProblemDetails.

## Swagger y OpenAPI

- Interfaz Swagger UI interactiva: `/swagger`.
- Documento OpenAPI v1: `/swagger/v1/swagger.json`.
- Swagger UI consume el documento v1 y permite consultar operaciones, parámetros, esquemas, respuestas y ejecutar solicitudes con `Try it out`.
- El contrato documenta `PATCH /api/v1/tipos-cambio/{id}/activar` y `PATCH /api/v1/licitaciones/{id}/estado` con sus verbos reales.
- Las pruebas comparan rutas documentadas con métodos admitidos para evitar POST, PUT o DELETE inexistentes.

## ProblemDetails y correlación

Los errores controlados usan `Content-Type: application/problem+json` y contienen:

```json
{
  "title": "La licitación solicitada no existe.",
  "status": 404,
  "detail": "La licitación solicitada no existe.",
  "code": "Licitacion.NoEncontrada",
  "correlationId": "<identificador de la solicitud>"
}
```

El mismo identificador se envía en `X-Correlation-ID`. El valor del cuerpo coincide exactamente con el encabezado. Las respuestas no exponen stack traces, rutas internas, consultas ni secretos.

La infraestructura común cubre los endpoints de proveedores, licitaciones, ofertas, niveles de aprobación y tipos de cambio.

## Evidencia automatizada

- `Iteration4ApiTests`.
- `ApiHardeningTests`.
- `LicitacionApiTests`.
- Suite funcional final: 51/51.

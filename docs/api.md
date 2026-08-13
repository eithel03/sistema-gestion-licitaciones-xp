# API REST

## Proveedores - Iteracion 1

La API REST de proveedores esta disponible en el proyecto `Licitaciones.Api`.

| Metodo | Ruta | Resultado principal |
| --- | --- | --- |
| GET | `/api/v1/proveedores` | Lista paginada de proveedores activos |
| GET | `/api/v1/proveedores/{id}` | Detalle de proveedor |
| POST | `/api/v1/proveedores` | Crea proveedor y devuelve `201 Created` |
| PUT | `/api/v1/proveedores/{id}` | Actualiza proveedor y devuelve `200 OK` |
| DELETE | `/api/v1/proveedores/{id}` | Retira proveedor y devuelve `204 No Content` |

Parametros de listado:

- `page`: pagina solicitada.
- `pageSize`: tamano de pagina, maximo 100.
- `search`: filtro por nombre.
- `sort`: `name` o `name_desc`.

Errores esperados:

- `400 Bad Request`: datos invalidos.
- `404 Not Found`: proveedor inexistente o retirado.
- `409 Conflict`: nombre equivalente a otro proveedor activo.

### Contratos reales

Solicitudes:

- `CrearProveedorRequest`: `nombre`.
- `ActualizarProveedorRequest`: `nombre`.

Respuesta de proveedor:

- `id`
- `nombre`
- `nombreNormalizado`
- `createdAt`
- `updatedAt`
- `deletedAt`

Respuesta paginada:

- `items`
- `totalItems`
- `page`
- `pageSize`
- `totalPages`

### Evidencia Iteracion 1

- Rama: `feature/iteracion-01-landing-proveedores`.
- Commit: `5696a0f`.
- Pull Request: `#9`.
- Pruebas funcionales: `ProveedorApiTests`.


## Licitaciones - Iteracion 2

La API REST de licitaciones esta disponible bajo /api/v1/licitaciones.

Endpoints reales:
- GET /api/v1/licitaciones: lista paginada de licitaciones activas; responde 200.
- GET /api/v1/licitaciones/{id}: consulta detalle; responde 200 o 404.
- POST /api/v1/licitaciones: crea licitacion en Borrador; responde 201, 400 o 409.
- PUT /api/v1/licitaciones/{id}: actualiza licitacion permitida; responde 200, 400, 404 o 409.
- DELETE /api/v1/licitaciones/{id}: aplica borrado logico; responde 204, 400, 404 o 409.
- POST /api/v1/licitaciones/{id}/publish: publica una licitacion en Borrador y vigente; responde 200, 400, 404 o 409.
- POST /api/v1/licitaciones/{id}/close: cierra una licitacion Publicada; responde 200, 400, 404 o 409.

Parametros de listado reales:
- page, pageSize, search y sort.
- page menor que 1 se normaliza a 1; pageSize menor que 1 se normaliza a 10 y el maximo es 100.
- search filtra por Codigo o Titulo.
- sort acepta code, code_desc y close_desc.

Contratos de solicitud reales:
- CrearLicitacionRequest: codigo, titulo, presupuestoCrc, fechaCierreUtc.
- ActualizarLicitacionRequest: codigo, titulo, presupuestoCrc, fechaCierreUtc, version.

Respuesta LicitacionResponse:
- id, codigo, codigoNormalizado, titulo, presupuestoCrc, fechaCierreUtc.
- estado, estadoEfectivo, createdAt, updatedAt, publishedAt, closedAt, deletedAt, version.
- LicitacionPage: items, totalItems, page, pageSize, totalPages.

Errores esperados:
- 400 Bad Request: validaciones de dominio, datos invalidos o transiciones no permitidas.
- 404 Not Found: licitacion inexistente o retirada.
- 409 Conflict: codigo normalizado duplicado o conflicto de concurrencia.
- ProblemDetails incluye code con el codigo de error de aplicacion.

Publicacion, cierre, borrado logico y concurrencia:
- publish cambia Borrador a Publicada si la fecha de cierre sigue vigente.
- close cambia Publicada a Cerrada.
- DELETE marca DeletedAt y excluye la licitacion de listados activos.
- version via ActualizarLicitacionRequest y xmin de PostgreSQL detectan concurrencia.
- Una Publicada vencida se devuelve con estadoEfectivo Cerrada.

### Evidencia Iteracion 2
- Rama: feature/iteracion-02-licitaciones.
- Historias: HU-12 a HU-19.
- Pruebas: ejecutadas localmente; resultado global 64/64.
- PR: Pendiente.
- CI remoto: Pendiente.
- Merge: Pendiente.

## Ofertas y niveles de aprobacion - Iteracion 3

### Ofertas

| Metodo | Ruta | Parametros o contrato | Respuesta observable |
| --- | --- | --- | --- |
| GET | `/api/v1/ofertas` | Query `page`, `pageSize`, `licitacionId`, `proveedorId`, `sort` | `200 OK` con `OfertaPage`. |
| GET | `/api/v1/ofertas/{id}` | `id` UUID | `200 OK` con `OfertaResponse` o `404 Not Found`. |
| POST | `/api/v1/ofertas` | `CrearOfertaRequest` | `201 Created`; `400 Bad Request`, `404 Not Found` o `409 Conflict`. |
| PUT | `/api/v1/ofertas/{id}` | `ActualizarOfertaRequest` | `200 OK`; `400`, `404` o `409`. |
| DELETE | `/api/v1/ofertas/{id}` | `id` UUID | `204 No Content`; `400`, `404` o `409`. |
| GET | `/api/v1/licitaciones/{id}/ofertas` | `id` UUID; query `page`, `pageSize` | `200 OK` con `OfertaPage`. |
| POST | `/api/v1/licitaciones/{id}/ofertas` | `CrearOfertaLicitacionRequest` | `201 Created`; `400`, `404` o `409`. |
| GET | `/api/v1/licitaciones/{id}/mejor-oferta` | `id` UUID | `200 OK` con `MejorOfertaResponse` o error controlado. |

`CrearOfertaRequest` contiene `licitacionId`, `proveedorId` y `montoOfertadoCrc`. La variante anidada recibe `proveedorId` y `montoOfertadoCrc`; el identificador de licitacion proviene de la ruta. `ActualizarOfertaRequest` contiene `montoOfertadoCrc` y `version` opcional.

`OfertaResponse` expone `id`, `licitacionId`, `proveedorId`, `montoOfertadoCrc`, `fechaRegistro`, `updatedAt` y `version`. `MejorOfertaResponse` incluye `tieneOferta`, la mejor oferta opcional, ahorro CRC, porcentaje, clasificacion, descripcion y aprobador opcional. Sin ofertas devuelve `200 OK` con `tieneOferta = false` y descripcion `Sin ofertas validas`.

### Niveles de aprobacion

| Metodo | Ruta | Parametros o contrato | Respuesta observable |
| --- | --- | --- | --- |
| GET | `/api/v1/niveles-aprobacion` | Query `page`, `pageSize` | `200 OK` con `NivelAprobacionPage`. |
| GET | `/api/v1/niveles-aprobacion/{id}` | `id` UUID | `200 OK` con `NivelAprobacionResponse` o `404 Not Found`. |
| POST | `/api/v1/niveles-aprobacion` | `CrearNivelAprobacionRequest` | `201 Created`; `400 Bad Request` o `409 Conflict`. |
| PUT | `/api/v1/niveles-aprobacion/{id}` | `ActualizarNivelAprobacionRequest` | `200 OK`; `400`, `404` o `409`. |
| DELETE | `/api/v1/niveles-aprobacion/{id}` | `id` UUID | `204 No Content`; `404` o `409`. |
| GET | `/api/v1/niveles-aprobacion/aprobador?montoCrc=...` | Query decimal `montoCrc` | `200 OK` con `AprobadorResponse`; `400` o `404`. |

Los contratos de alta y actualizacion contienen `montoMinimoCrc`, `montoMaximoCrc` nullable y `aprobador`; la actualizacion agrega `version` opcional. La seleccion del aprobador consulta los rangos persistidos.

Los endpoints consumen servicios y DTO de Application. Los errores se traducen localmente a `ProblemDetails` con extension `code` y estados 400/404/409; no se incorporo el manejo global de `ProblemDetails` previsto para Iteracion 4.

Evidencia: rama `feature/iteracion-03-ofertas-aprobacion`, commit API `37bcb55` y `Iteration3ApiTests`. Pull Request, CI remoto, merge y tag `v0.3.0`: Pendientes.

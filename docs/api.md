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

| Metodo | Ruta | Resultado principal |
| --- | --- | --- |
| GET/POST | `/api/v1/ofertas` | Lista filtrable o crea oferta. |
| GET/PUT/DELETE | `/api/v1/ofertas/{id}` | Detalle, actualizacion o eliminacion permitida. |
| GET/POST | `/api/v1/licitaciones/{id}/ofertas` | Lista o registra en una licitacion. |
| GET | `/api/v1/licitaciones/{id}/mejor-oferta` | Ganadora, ahorro, porcentaje, clasificacion y aprobador persistido. |
| GET/POST | `/api/v1/niveles-aprobacion` | Lista o crea nivel. |
| GET/PUT/DELETE | `/api/v1/niveles-aprobacion/{id}` | CRUD por identificador. |
| GET | `/api/v1/niveles-aprobacion/aprobador?montoCrc=...` | Busca aprobador por monto. |

Filtros de ofertas: `licitacionId`, `proveedorId`, `page`, `pageSize` y `sort`. Se usan DTO de Application y errores 400/404/409 controlados. No se implemento el manejo global de `ProblemDetails` previsto para Iteracion 4.

Evidencia local: `Iteration3ApiTests`. PR, CI remoto, merge y tag: Pendientes.

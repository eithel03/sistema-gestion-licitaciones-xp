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

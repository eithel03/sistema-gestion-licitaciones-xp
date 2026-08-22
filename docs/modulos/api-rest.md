# Módulo API REST

## 1. Propósito

Exponer por HTTP los casos de uso del sistema para consumidores externos, con rutas versionadas y errores controlados.

## 2. Responsabilidades

- CRUD de proveedores, licitaciones, ofertas, niveles y tipos de cambio.
- Publicación, cierre y cambio de estado de licitaciones.
- Ofertas anidadas, mejor oferta y aprobador.
- Activación y conversión monetaria.
- Paginación y filtros.
- ProblemDetails, códigos HTTP y correlación.
- Servir Swagger UI y el documento OpenAPI manual.

## 3. Dependencias

- Host `Licitaciones.Api`.
- Servicios y DTO de Application.
- Repositorios/DbContext de Infrastructure.
- Middleware de correlación.
- ASP.NET Core ProblemDetails y Swashbuckle Swagger UI.

## 4. Entradas

- Rutas `/api/v1`.
- UUID en rutas.
- DTO JSON de creación, actualización y cambio de estado.
- Parámetros de página, búsqueda, filtro, orden, monto y moneda.
- Encabezado opcional `X-Correlation-ID`.

## 5. Salidas

- DTO y páginas JSON.
- 200, 201 y 204 en operaciones correctas.
- ProblemDetails 400, 404, 409 y 500.
- Encabezado `X-Correlation-ID`.
- Swagger UI en `/swagger` y JSON en `/swagger/v1/swagger.json`.

## 6. Reglas de negocio

La API no implementa reglas propias: delega en Application/Domain. El versionado actual es literal en la ruta. Los listados limitan `pageSize` a 100. La respuesta de mejor oferta consulta el aprobador después de evaluar ofertas.

## 7. Errores

- 400 para validación o transición inválida.
- 404 para recursos ausentes.
- 409 para duplicidad, traslape, activo único o concurrencia.
- 500 genérico para excepciones inesperadas.
- `code` y `correlationId` se agregan a ProblemDetails controlados.

Limitación OpenAPI: el documento se mantiene manualmente, tiene esquemas sin propiedades, omite la mayoría de request bodies y generaliza respuestas. Swagger existe, pero no representa por completo los contratos. Su mejora es trabajo técnico futuro.

## 8. Pruebas relacionadas

- `ProveedorApiTests`.
- `LicitacionApiTests`.
- `Iteration3ApiTests`.
- `Iteration4ApiTests`.
- `ApiHardeningTests`.
- `HealthEndpointTests`.

El inventario completo de rutas está en [api.md](../api.md).

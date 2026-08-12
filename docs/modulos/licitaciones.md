# Modulo Licitaciones

## Proposito

Gestionar el ciclo de vida de licitaciones del Sistema de Gestion de Licitaciones, desde su creacion hasta publicacion, cierre y retiro logico.

## Evidencia Iteracion 2

- Rama: `feature/iteracion-02-licitaciones`.
- Driver: Eithel.
- Navigator: Chavala.
- Historias: HU-12 a HU-19.
- Puntos planificados: 36.
- Issue: `#10`.
- Commit de implementacion: `cce95ad`.
- Commit de pruebas: `812b59c`.
- Commit documental: `ed89c5a`.
- Pull Request: `#12`.
- CI remoto: Aprobado.
- Version prevista: `v0.2.0`.
- Merge: Pendiente.

## Responsabilidades

- Crear licitaciones.
- Listar y consultar licitaciones activas.
- Editar licitaciones en estado Borrador.
- Retirar licitaciones mediante borrado logico.
- Publicar licitaciones vigentes.
- Cerrar licitaciones publicadas.
- Exponer operaciones equivalentes por API REST.
- Proteger auditoria, unicidad y concurrencia optimista.

## Reglas

- Codigo es requerido, tiene longitud maxima de 50 caracteres y se normaliza en el dominio.
- CodigoNormalizado compara sin distinguir mayusculas/minusculas ni espacios repetidos.
- Caracteres permitidos en el codigo: letras, numeros, espacios y guion.
- No se permiten licitaciones activas duplicadas por CodigoNormalizado.
- Titulo es requerido y tiene longitud maxima de 200 caracteres.
- PresupuestoCrc debe ser mayor que cero.
- FechaCierreUtc debe ser posterior al reloj inyectado al crear, editar o publicar.
- El borrado logico usa DeletedAt y excluye registros retirados de listados activos.
- Las reglas definitivas viven en Licitaciones.Domain.

## Estados

- Borrador: estado inicial. Permite editar, publicar y retirar.
- Publicada: permite cerrar. No permite editar ni retirar.
- Cerrada: estado final para la licitacion cerrada.

Transiciones permitidas:

- Borrador a Publicada.
- Publicada a Cerrada.

Transiciones rechazadas:

- Publicar una licitacion ya publicada o cerrada.
- Cerrar una licitacion en borrador o ya cerrada.
- Editar o retirar una licitacion que no esta en borrador.

## Entradas

- MVC: formulario de licitacion con Codigo, Titulo, PresupuestoCrc, FechaCierreLocal y Version.
- API: CrearLicitacionRequest con codigo, titulo, presupuestoCrc y fechaCierreUtc.
- API: ActualizarLicitacionRequest con codigo, titulo, presupuestoCrc, fechaCierreUtc y version.
- Listado: page, pageSize, search y sort.

## Salidas

- MVC: listado, detalle, creacion, edicion, confirmacion de retiro y acciones de estado.
- API: LicitacionResponse y LicitacionPage.
- Errores controlados mediante resultados de Application y ProblemDetails en API.

## Persistencia

Tabla real: Licitaciones.

Columnas principales:

- Id.
- Codigo.
- CodigoNormalizado.
- Titulo.
- PresupuestoCrc.
- FechaCierreUtc.
- Estado.
- CreatedAt.
- UpdatedAt.
- PublishedAt.
- ClosedAt.
- DeletedAt.
- xmin.

Indices reales:

- IX_Licitaciones_CodigoNormalizado: unico con filtro parcial DeletedAt IS NULL.
- IX_Licitaciones_Estado.

Migracion real:

- 20260812002104_CreateLicitaciones.

## Auditoria

- CreatedAt registra la creacion.
- UpdatedAt registra modificaciones.
- PublishedAt registra publicacion.
- ClosedAt registra cierre.
- DeletedAt registra borrado logico.

## Concurrencia

La concurrencia optimista se implementa con PostgreSQL xmin, expuesto como Version en los contratos de licitacion. Application y API traducen conflictos a errores controlados y la API responde 409 Conflict cuando corresponde.

## API REST

Base: /api/v1/licitaciones.

- GET /api/v1/licitaciones: listar licitaciones activas.
- GET /api/v1/licitaciones/{id}: consultar detalle.
- POST /api/v1/licitaciones: crear licitacion.
- PUT /api/v1/licitaciones/{id}: actualizar licitacion.
- DELETE /api/v1/licitaciones/{id}: retirar mediante borrado logico.
- POST /api/v1/licitaciones/{id}/publish: publicar.
- POST /api/v1/licitaciones/{id}/close: cerrar.

## Errores

- 400 Bad Request: validaciones de dominio, datos invalidos o transiciones no permitidas.
- 404 Not Found: licitacion inexistente o retirada.
- 409 Conflict: codigo normalizado duplicado o conflicto de concurrencia.

## Pruebas

- Unitarias: LicitacionTests cubre creacion, validaciones, normalizacion, estados, transiciones, vencimiento y borrado logico.
- Integracion: LicitacionPersistenceTests cubre guardado, listado, borrado logico y concurrencia con PostgreSQL real y Testcontainers.
- Funcionales: LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- Validacion manual MVC: crear, listar, detalle, editar, publicar, rechazar transicion invalida y cerrar.
- Resultado global conocido: 64/64 pruebas aprobadas.

## Ajustes de interfaz

- El campo PresupuestoCrc se ajusto para mantener coherencia con cultura es-CR y formato decimal.
- La vista de detalle muestra acciones segun estado: en Borrador, editar/publicar/retirar; en Publicada, cerrar; en Cerrada, sin acciones de transicion.
- Estas mejoras no sustituyen las validaciones del dominio.


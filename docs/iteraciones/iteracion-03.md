# Iteracion 3 - Ofertas, mejor oferta y aprobaciones

- Objetivo: entregar gestion de ofertas, evaluacion economica y niveles de aprobacion parametrizables.
- Driver: Chavala.
- Navigator: Eithel.
- Rama: `feature/iteracion-03-ofertas-aprobacion`.
- Version prevista: `v0.3.0`.
- Puntos planificados: 38.
- Historias: HU-11 y HU-20 a HU-29.

## Alcance implementado

- CRUD MVC y API de ofertas con filtros por licitacion y proveedor.
- Monto positivo, limite presupuestario, licitacion publicada y vigente, proveedor existente y una oferta por proveedor/licitacion.
- Edicion y eliminacion solo mientras la licitacion recibe ofertas.
- Mejor oferta por menor monto; desempate por `FechaRegistro` y luego por `Id` ascendente.
- Ahorro y clasificacion usando `decimal`.
- CRUD MVC y API de niveles de aprobacion.
- Aprobador resuelto desde rangos persistidos, sin cargos codificados en una cadena de decisiones.
- Consulta de ofertas asociadas desde el detalle de proveedor.
- Persistencia PostgreSQL y migracion `20260813011055_Iteration03OfertasAprobacion`.

## Decisiones

- `Oferta` delega el estado funcional y vencimiento a `Licitacion.GetEstadoEfectivo(utcNow)`.
- `FechaRegistro` y `UpdatedAt` usan `DateTimeOffset` UTC obtenido mediante `IClock` en Application.
- Los montos son `decimal` y se persisten como `numeric(18,2)`.
- Si dos ofertas empatan tambien en fecha, el menor `Guid` de `Id` gana; esto hace el resultado determinista sin introducir otro dato de negocio.
- Los limites de aprobacion son inclusivos. Por tanto, compartir un limite constituye traslape.
- PostgreSQL refuerza rangos con `CHECK`, indice parcial para un rango abierto y exclusion GiST sobre `numrange`.

## Ciclos TDD ejecutados

### Oferta y evaluacion

- ROJO: pruebas no compilaban porque no existian `Oferta`, errores, clasificacion ni evaluador.
- VERDE: reglas monetarias, estado efectivo, vencimiento, mutaciones, mejor oferta, desempate y clasificacion.
- REFACTOR: evaluacion economica separada en `EvaluadorOfertas` y orden estable monto/fecha/Id.

### Application

- ROJO: faltaban contratos, repositorio y servicio de ofertas.
- VERDE: duplicidad, relaciones, reloj, filtros, CRUD y resultado vacio controlado.
- REFACTOR: resultados estandarizados y controladores delgados.

### Niveles de aprobacion

- ROJO: faltaban entidad y servicio de rangos.
- VERDE: limites, rango abierto, traslape, CRUD y busqueda de aprobador.
- REFACTOR: comparacion de rangos en Domain y consultas persistidas en repositorio.

### PostgreSQL, API y MVC

- ROJO: no existian `DbSet`, tablas, rutas ni vistas.
- VERDE: configuraciones EF, repositorios, migracion, endpoints, controladores y vistas.
- REFACTOR: restricciones de exclusion para concurrencia, aislamiento de pruebas funcionales y validacion decimal compatible con `es-CR`.

## Pruebas locales ejecutadas durante el desarrollo

- Unitarias focalizadas de Domain Ofertas: 15 aprobadas.
- Unitarias focalizadas de Application Ofertas: 6 aprobadas.
- Unitarias focalizadas de Domain Aprobaciones: 7 aprobadas.
- Unitarias focalizadas de Application Aprobaciones: 5 aprobadas.
- Suite unitaria completa intermedia: 76 aprobadas.
- Integracion focalizada de Iteracion 3: 9 aprobadas con PostgreSQL 16/Testcontainers.
- Funcionales API focalizadas: 3 aprobadas con PostgreSQL 16/Testcontainers.
- Funcionales MVC focalizadas: 2 aprobadas con PostgreSQL 16/Testcontainers.

Resultados finales: build con 0 errores y 0 advertencias; UnitTests 76/76, IntegrationTests 22/22 y FunctionalTests 13/13. Total 111/111. El detalle, incluido el primer fallo TLS de restore en sandbox y su repeticion exitosa, esta en `docs/pruebas.md`.

## Evidencia pendiente

## Prueba manual local

Ejecutada el 2026-08-12 contra PostgreSQL 16 y API local con datos unicos generados para la ejecucion.

- Licitacion futura creada y publicada; estado publicado confirmado.
- Primera oferta CRC 900000 creada.
- Duplicada rechazada con HTTP 409.
- Segundo proveedor creado; oferta CRC 1000000.01 rechazada con HTTP 400.
- Segunda oferta CRC 800000 creada; listado devolvio 2 ofertas.
- Mejor oferta: CRC 800000, ahorro 20 %, `Oferta conveniente`.
- Nivel abierto creado, listado, consultado y editado a `Gerencia Manual Actualizada`.
- Mejor oferta devolvio ese aprobador persistido.
- Rango traslapado rechazado con HTTP 409.
- Nivel eliminado con HTTP 204.

El servidor MVC independiente no pudo dejarse disponible en este host: el perfil local encontro claves Data Protection/DPAPI no descifrables, acceso denegado al Event Log y un certificado HTTPS ausente o vencido. La cobertura MVC se ejecuto correctamente mediante `WebApplicationFactory` en la suite funcional (13/13 global, 2 flujos focalizados de Iteracion 3).

- Issue: Pendiente.
- Commits: Pendiente; Codex no realizo commits.
- Pull Request: Pendiente.
- CI remoto: Pendiente.
- Revision/aprobacion del Navigator: Pendiente.
- Merge: Pendiente.
- Tag `v0.3.0`: Pendiente.
- Velocidad observada y retroalimentacion del cliente: Pendientes de cierre por el equipo.

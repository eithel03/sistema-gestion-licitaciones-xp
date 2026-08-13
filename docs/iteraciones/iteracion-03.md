# Iteracion 3 - Ofertas, mejor oferta y aprobaciones

- Objetivo: entregar gestion de ofertas, evaluacion economica y niveles de aprobacion parametrizables.
- Driver: Chavala.
- Navigator: Eithel.
- Rama: `feature/iteracion-03-ofertas-aprobacion`.
- Version prevista: `v0.3.0`.
- Base: `fafcc66` (`Merge pull request #12 from eithel03/feature/iteracion-02-licitaciones`).
- Puntos planificados: 38.
- Historias: HU-11 y HU-20 a HU-29.

## Modalidad XP

Trabajo en pareja. Chavala actuo como Driver principal: ejecuto la implementacion con asistencia de Codex, reviso cambios, ejecuto comandos y pruebas, preparo los commits y actualizo evidencias. Eithel actuo como Navigator principal: reviso las reglas, criterios de aceptacion, estrategia TDD y posibles defectos. La revision formal del Navigator permanece pendiente.

## Alcance implementado

- CRUD MVC y API de ofertas con filtros por licitacion y proveedor.
- Monto positivo, limite presupuestario, licitacion publicada y vigente, proveedor existente y una oferta por proveedor/licitacion.
- Edicion y eliminacion solo mientras la licitacion recibe ofertas.
- Mejor oferta por menor monto; desempate por `FechaRegistro` y luego por `Id` ascendente.
- Ahorro y clasificacion usando `decimal`: `Oferta conveniente`, `Oferta aceptable`, `Oferta valida sin ahorro` o `Sin ofertas validas`.
- CRUD MVC y API de niveles de aprobacion.
- Aprobador resuelto desde rangos persistidos, sin cargos codificados en una cadena de decisiones.
- Consulta de ofertas asociadas desde el detalle de proveedor.
- Persistencia PostgreSQL y migracion `20260813011055_Iteration03OfertasAprobacion`.
- Contratos, resultados, validaciones, servicios, repositorios abstractos y excepciones controladas en `Licitaciones.Application.Ofertas` y `Licitaciones.Application.Aprobaciones`.
- Configuraciones `OfertaConfiguration` y `NivelAprobacionConfiguration`, repositorios EF Core, `LicitacionesDbContext` e inyeccion de dependencias actualizados en Infrastructure.

## Decisiones

- `Oferta` delega el estado funcional y vencimiento a `Licitacion.GetEstadoEfectivo(utcNow)`.
- `FechaRegistro` y `UpdatedAt` usan `DateTimeOffset` UTC obtenido mediante `IClock` en Application.
- Los montos son `decimal` y se persisten como `numeric(18,2)`.
- Si dos ofertas empatan tambien en fecha, el menor `Guid` de `Id` gana; esto hace el resultado determinista sin introducir otro dato de negocio.
- Los limites de aprobacion son inclusivos. Por tanto, compartir un limite constituye traslape.
- PostgreSQL refuerza rangos con `CHECK`, indice parcial para un rango abierto y exclusion GiST sobre `numrange`.
- Los endpoints y controladores MVC consumen servicios de Application; las reglas centrales no se implementan en los controladores.

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

## Commits reales

- `d6d6009` - `feat(ofertas): implementar reglas y evaluacion de ofertas`.
- `7e6a317` - `feat(ofertas): implementar casos de uso de ofertas`.
- `a20eb19` - `feat(aprobacion): implementar niveles y validacion de rangos`.
- `29e727c` - `feat(persistencia): agregar ofertas y niveles de aprobacion`.
- `37bcb55` - `feat(api): exponer ofertas y niveles de aprobacion`.
- `4faaf83` - `feat(web): agregar gestion MVC de ofertas y aprobacion`.
- `437cc37` - `docs(xp): documentar resultados de la iteracion 3`.

Los commits fueron organizados posteriormente por responsabilidad tecnica. La evidencia TDD se encuentra en las pruebas y ejecuciones registradas; no se afirma que cada paso ROJO tuviera un commit independiente.

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

## Estado y pendientes

Iteracion 3 tecnicamente implementada y validada localmente.

- Issue: Pendiente.
- Commits: registrados en esta rama.
- Pull Request: Pendiente.
- CI remoto: Pendiente.
- Revision formal del Navigator: Pendiente.
- Merge: Pendiente.
- Tag `v0.3.0`: Pendiente.
- Velocidad tecnica: 38 puntos implementados; cierre formal y retroalimentacion del cliente pendientes.

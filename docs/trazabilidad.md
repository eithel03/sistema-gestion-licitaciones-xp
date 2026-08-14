# Matriz de trazabilidad

Esta matriz registra la trazabilidad prevista entre historias, criterios de aceptacion, pruebas, documentacion, Issues, ramas, commits, Pull Requests y liberaciones. Los campos sin evidencia real permanecen como `Pendiente`.

| Historia | Iteracion | Prioridad | Puntos | Criterios | Pruebas previstas | Modulo | Issue | Rama | Commits | PR | Documentacion | Liberacion |
| --- | --- | --- | ---: | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| HU-01 | Iteracion 1 | Alta | 3 | Definidos en `historias-usuario.md` | `ProveedorMvcTests.LandingPageAndProviderListAreAvailable` | Interfaz | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `bitacora-xp.md` | v0.1.0 |
| HU-02 | Iteracion 1 | Alta | 2 | Definidos en `historias-usuario.md` | Revision responsive en vistas MVC y `ProveedorMvcTests` | Interfaz | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md` | v0.1.0 |
| HU-03 | Iteracion 1 | Alta | 2 | Definidos en `historias-usuario.md` | `ProveedorMvcTests.CreateEditAndRejectDuplicateProviderThroughMvc` | Interfaz | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-04 | Iteracion 1 | Media | 3 | Definidos en `historias-usuario.md` | `ProveedorServiceTests.ListFiltersSortsAndPaginatesProviders` | Interfaz, API REST | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-05 | Iteracion 1 | Alta | 3 | Definidos en `historias-usuario.md` | `ProveedorTests`, `ProveedorServiceTests.CreateReturnsCreatedProvider`, `ProveedorApiTests.CreateProviderReturnsCreatedAndCanBeRead` | Proveedores | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-06 | Iteracion 1 | Alta | 2 | Definidos en `historias-usuario.md` | `ProveedorPersistenceTests.SavesAndRetrievesProveedor`, `ProveedorMvcTests.LandingPageAndProviderListAreAvailable` | Proveedores | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-07 | Iteracion 1 | Alta | 3 | Definidos en `historias-usuario.md` | `ProveedorTests.RenameUpdatesNameAndTimestamp`, `ProveedorPersistenceTests.RetiresProveedorWithLogicalDelete`, `ProveedorApiTests.UpdateAndDeleteProviderUseExpectedStatusCodes` | Proveedores | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-08 | Iteracion 1 | Alta | 5 | Definidos en `historias-usuario.md` | `ProveedorTests.NormalizedNameIgnoresCaseAndRepeatedSpaces`, `ProveedorPersistenceTests.UniqueIndexRejectsEquivalentNormalizedName` | Proveedores, persistencia | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-09 | Iteracion 1 | Media | 2 | Definidos en `historias-usuario.md` | `ProveedorTests.CreateAcceptsAllowedCharacters`, `ProveedorTests.CreateRejectsDisallowedCharacters` | Proveedores | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md` | v0.1.0 |
| HU-10 | Iteracion 1 | Alta | 5 | Definidos en `historias-usuario.md` | `ProveedorApiTests` | Proveedores, API REST | Pendiente | `feature/iteracion-01-landing-proveedores` | `5696a0f` | `#9` | `iteracion-01.md`, `modulos/proveedores.md`, `api.md` | v0.1.0 |
| HU-11 | Iteracion 3 | Media | 2 | Definidos en `historias-usuario.md` | `Iteration3MvcTests`, `Iteration3PersistenceTests` | Proveedores, ofertas | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `7e6a317`, `29e727c`, `4faaf83`, `437cc37` | `#14` | `iteracion-03.md`, `modulos/ofertas.md`, `integracion-modulos.md` | `v0.3.0` (Prevista) |
| HU-12 | Iteración 2 | Alta | 5 | Definidos en `historias-usuario.md` | `LicitacionTests.CreateValidTenderStartsAsDraft`, `LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi` | Licitaciones | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `pruebas.md`, `bitacora-xp.md` | `v0.2.0` (Prevista) |
| HU-13 | Iteración 2 | Alta | 3 | Definidos en `historias-usuario.md` | `LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion`, `LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi` | Licitaciones | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `api.md`, `pruebas.md` | `v0.2.0` (Prevista) |
| HU-14 | Iteración 2 | Alta | 3 | Definidos en `historias-usuario.md` | `LicitacionTests.InvalidTransitionsAndUpdatesAreRejected`, `LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion` | Licitaciones | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `modelo-datos.md`, `pruebas.md` | `v0.2.0` (Prevista) |
| HU-15 | Iteración 2 | Alta | 5 | Definidos en `historias-usuario.md` | `LicitacionTests.CreateRejectsInvalidBaseData`, `LicitacionTests.NormalizedCodeIgnoresCaseAndRepeatedSpaces` | Licitaciones | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `pruebas.md` | `v0.2.0` (Prevista) |
| HU-16 | Iteración 2 | Alta | 5 | Definidos en `historias-usuario.md` | `LicitacionTests.PublishAndCloseFollowAllowedTransitions`, `LicitacionTests.PublishedExpiredTenderIsEffectivelyClosed`, `LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi` | Licitaciones | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `pruebas.md` | `v0.2.0` (Prevista) |
| HU-17 | Iteración 2 | Alta | 5 | Definidos en `historias-usuario.md` | `LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi` | Licitaciones, API REST | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `api.md`, `pruebas.md` | `v0.2.0` (Prevista) |
| HU-18 | Iteración 2 | Alta | 5 | Definidos en `historias-usuario.md` | `LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion`, migración `20260812002104_CreateLicitaciones` | Persistencia | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `modelo-datos.md`, `pruebas.md` | `v0.2.0` (Prevista) |
| HU-19 | Iteración 2 | Alta | 5 | Definidos en `historias-usuario.md` | `LicitacionPersistenceTests.ConcurrentUpdatesDetectStaleVersion`, manejo controlado de concurrencia en Application/API | Persistencia, API REST | `#10` | `feature/iteracion-02-licitaciones` | `cce95ad`, `812b59c`, `ed89c5a`, `c77343b` | `#12` | `iteracion-02.md`, `modulos/licitaciones.md`, `api.md`, `modelo-datos.md`, `pruebas.md`, `bitacora-xp.md` | `v0.2.0` (Prevista) |
| HU-20 | Iteracion 3 | Alta | 5 | Definidos en `historias-usuario.md` | `OfertaTests`, `OfertaServiceTests`, `Iteration3ApiTests`, `Iteration3MvcTests` | Ofertas | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37` | `#14` | `iteracion-03.md`, `modulos/ofertas.md`, `api.md`, `modelo-datos.md` | `v0.3.0` (Prevista) |
| HU-21 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | `OfertaServiceTests`, `Iteration3ApiTests`, `Iteration3MvcTests` | Ofertas, API REST | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `7e6a317`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37` | `#14` | `modulos/ofertas.md`, `api.md`, `iteracion-03.md` | `v0.3.0` (Prevista) |
| HU-22 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | `OfertaTests`, `OfertaServiceTests`, `Iteration3MvcTests` | Ofertas, licitaciones | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37` | `#14` | `modulos/ofertas.md`, `iteracion-03.md`, `pruebas.md` | `v0.3.0` (Prevista) |
| HU-23 | Iteracion 3 | Alta | 5 | Definidos en `historias-usuario.md` | `OfertaTests`, `OfertaServiceTests`, `Iteration3ApiTests` | Ofertas, licitaciones | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `29e727c`, `37bcb55`, `437cc37` | `#14` | `modulos/ofertas.md`, `modelo-datos.md`, `pruebas.md` | `v0.3.0` (Prevista) |
| HU-24 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | `OfertaTests`, `Iteration3ApiTests` | Ofertas, licitaciones | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `37bcb55`, `437cc37` | `#14` | `modulos/ofertas.md`, `api.md`, `pruebas.md` | `v0.3.0` (Prevista) |
| HU-25 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | `EvaluadorOfertasTests`, `Iteration3ApiTests` | Ofertas, licitaciones | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `37bcb55`, `437cc37` | `#14` | `modulos/ofertas.md`, `api.md`, `iteracion-03.md` | `v0.3.0` (Prevista) |
| HU-26 | Iteracion 3 | Media | 3 | Definidos en `historias-usuario.md` | `EvaluadorOfertasTests`, `Iteration3ApiTests` | Ofertas, licitaciones | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `37bcb55`, `437cc37` | `#14` | `modulos/ofertas.md`, `api.md`, `pruebas.md` | `v0.3.0` (Prevista) |
| HU-27 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | `NivelAprobacionServiceTests`, `Iteration3ApiTests`, `Iteration3MvcTests` | Niveles de aprobacion | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `a20eb19`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37` | `#14` | `modulos/niveles-aprobacion.md`, `api.md`, `iteracion-03.md` | `v0.3.0` (Prevista) |
| HU-28 | Iteracion 3 | Alta | 5 | Definidos en `historias-usuario.md` | `NivelAprobacionTests`, `NivelAprobacionServiceTests`, `Iteration3PersistenceTests`, `Iteration3ApiTests` | Niveles de aprobacion, ofertas | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `a20eb19`, `29e727c`, `37bcb55`, `437cc37` | `#14` | `modulos/niveles-aprobacion.md`, `modelo-datos.md`, `integracion-modulos.md` | `v0.3.0` (Prevista) |
| HU-29 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | `Iteration3ApiTests` | Ofertas, aprobaciones, API REST | Pendiente | `feature/iteracion-03-ofertas-aprobacion` | `d6d6009`, `7e6a317`, `a20eb19`, `29e727c`, `37bcb55`, `437cc37` | `#14` | `api.md`, `iteracion-03.md`, `modulos/ofertas.md`, `modulos/niveles-aprobacion.md` | `v0.3.0` (Prevista) |
| HU-30 | Iteracion 4 | Media | 5 | Definidos en `historias-usuario.md` | `TipoCambioTests`, `TipoCambioServiceTests`, `TipoCambioPersistenceTests`, `Iteration4ApiTests`, `Iteration4MvcTests` | Tipos de cambio | `#15` | `feature/iteracion-04-moneda-ux` | `40c4f5d`, `5cba6c2`, `9b0fa75`, `2e710d2` | `#16` | `iteraciones/iteracion-04.md`, `iteraciones/iteracion-04-evidencia.md`, `modulos/tipos-cambio.md`, `api-iteracion-04.md`, `pruebas-iteracion-04.md` | `v1.0.0-rc` (Prevista) |
| HU-31 | Iteracion 4 | Alta | 3 | Definidos en `historias-usuario.md` | `TipoCambioTests`, `TipoCambioServiceTests`, `TipoCambioPersistenceTests`, `Iteration4ApiTests` | Tipos de cambio, persistencia | `#15` | `feature/iteracion-04-moneda-ux` | `40c4f5d`, `38c5bf5`, `9b0fa75`, `2e710d2` | `#16` | `iteraciones/iteracion-04.md`, `modulos/tipos-cambio.md`, `api-iteracion-04.md` | `v1.0.0-rc` (Prevista) |
| HU-32 | Iteracion 4 | Media | 3 | Definidos en `historias-usuario.md` | `TipoCambioServiceTests`, `Iteration4ApiTests`, `Iteration4MvcTests` | Tipos de cambio, interfaz, licitaciones, ofertas | `#15` | `feature/iteracion-04-moneda-ux` | `40c4f5d`, `5cba6c2`, `9b0fa75`, `2e710d2` | `#16` | `iteraciones/iteracion-04.md`, `modulos/tipos-cambio.md`, `pruebas-iteracion-04.md` | `v1.0.0-rc` (Prevista) |
| HU-33 | Iteracion 4 | Media | 3 | Definidos en `historias-usuario.md` | `Iteration4MvcTests` y validacion manual de persistencia | Interfaz | `#15` | `feature/iteracion-04-moneda-ux` | `5cba6c2`, `9b0fa75`, `2e710d2` | `#16` | `iteraciones/iteracion-04.md`, `iteraciones/iteracion-04-evidencia.md` | `v1.0.0-rc` (Prevista) |
| HU-34 | Iteracion 4 | Alta | 5 | Definidos en `historias-usuario.md` | `ApiHardeningTests`, `Iteration4ApiTests`, `LicitacionApiTests` | API REST | `#15` | `feature/iteracion-04-moneda-ux` | `38c5bf5`, `9b0fa75`, `2e710d2` | `#16` | `api-iteracion-04.md`, `iteraciones/iteracion-04-evidencia.md`, `pruebas-iteracion-04.md` | `v1.0.0-rc` (Prevista) |
| HU-35 | Iteracion 4 | Alta | 5 | Definidos en `historias-usuario.md` | UnitTests 96/96, IntegrationTests 27/27, FunctionalTests 51/51; cobertura limpia 3x | Pruebas, integracion continua | `#15` | `feature/iteracion-04-moneda-ux` | `9b0fa75`, `2e710d2` | `#16` | `pruebas-iteracion-04.md`, `iteraciones/iteracion-04-evidencia.md` | `v1.0.0-rc` (Prevista) |
| HU-36 | Iteracion 4 | Media | 5 | Definidos en `historias-usuario.md` | Docker config, build, arranque, health y persistencia; Kustomize exitoso | Infraestructura, persistencia | `#15` | `feature/iteracion-04-moneda-ux` | `8103b12`, `2e710d2` | `#16` | `docker.md`, `kubernetes.md`, `iteraciones/iteracion-04-evidencia.md` | `v1.0.0-rc` (Prevista) |
| HU-37 | Iteracion 4 | Alta | 3 | Definidos en `historias-usuario.md` | Revision documental, enlaces, trazabilidad y `git diff --check` | Documentacion, XP | `#15` | `feature/iteracion-04-moneda-ux` | `2e710d2` | `#16` | `iteraciones/iteracion-04.md`, `iteraciones/iteracion-04-evidencia.md`, `bitacora-xp.md`, `uso-ia.md`, `trazabilidad.md` | `v1.0.0-rc` (Prevista) |
| FASE-02 | Preparación técnica | N/A | N/A | Inicialización técnica del monolito modular | `ArchitectureTests.cs`, `InfrastructureAssemblyTests.cs`, `HealthEndpointTests.cs` | `src/`, `tests/`, `docs/`, `.github/workflows/ci.yml` | `#3` | `chore/arquitectura-inicial` | `ad7913b`, `e0e5ad1`, `821ab9d`, `65a6afd` | `#4` | `docs/arquitectura-general.md`, `docs/bitacora-xp.md`, `docs/trazabilidad.md`, `docs/uso-ia.md`, `docs/README.md` | CI aprobado |
| FASE-03 | Preparación dominio/TDD | N/A | N/A | Convenciones mínimas de dominio y pruebas preparatorias | `EntityTests.cs`, `ValueObjectTests.cs`, `ValidationResultTests.cs`, `IClockTests.cs` | `Domain`, `Application`, `Infrastructure`, `UnitTests` | `#7` | `chore/fase-03-dominio-tdd` | `2200fe3` | `#6` | `docs/dominio-tdd.md`, `docs/pruebas.md`, `docs/arquitectura-general.md`, `docs/bitacora-xp.md`, `docs/uso-ia.md` | CI aprobado |
| FASE-04 | Preparacion persistencia | N/A | N/A | Infraestructura minima de PostgreSQL y EF Core sin tablas futuras | `PersistenceConventionsTests.cs`, `PostgreSqlContainerTests.cs`, restore, build, test, Docker Compose | `Infrastructure`, `IntegrationTests`, `compose.yaml`, documentacion | `#5` | `chore/preparacion-persistencia` | Pendiente | Pendiente | `docs/arquitectura-general.md`, `docs/modelo-datos.md`, `docs/pruebas.md`, `docs/bitacora-xp.md`, `docs/uso-ia.md`, `docs/README.md` | Pendiente |

## Totales

| Iteracion | Puntos |
| --- | ---: |
| Iteracion 1 | 30 |
| Iteracion 2 | 36 |
| Iteracion 3 | 38 |
| Iteracion 4 | 32 |
| Total | 136 |

## Evidencia comun de Iteracion 3

- Base: `fafcc66`.
- Rama: `feature/iteracion-03-ofertas-aprobacion`.
- Historias: HU-11 y HU-20 a HU-29.
- Commits: `d6d6009`, `7e6a317`, `a20eb19`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37`, `d349ccc`.
- Pruebas: UnitTests 76/76, IntegrationTests 22/22 y FunctionalTests 13/13; total 111/111.
- Documentacion: `iteraciones/iteracion-03.md`, `modulos/ofertas.md`, `modulos/niveles-aprobacion.md`, `api.md`, `modelo-datos.md`, `pruebas.md`, `integracion-modulos.md` y `bitacora-xp.md`.
- Version: `v0.3.0` prevista; tag pendiente.
- Issue: Pendiente.
- Pull Request: `#14`.
- Merge a `main`: `fe5317c`.
- CI remoto: Pendiente de evidencia confirmada.
- Tag `v0.3.0`: Pendiente.

## Actualizacion Iteracion 2 - HU-12 a HU-19.

Evidencia real local para las historias HU-12 a HU-19.
Rama común: `feature/iteracion-02-licitaciones`.
- HU-12: Licitaciones. Pruebas: LicitacionTests.CreateValidTenderStartsAsDraft y LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-13: Licitaciones. Pruebas: LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion y LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-14: Licitaciones. Pruebas: LicitacionTests.InvalidTransitionsAndUpdatesAreRejected y LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion.
- HU-15: Licitaciones. Pruebas: LicitacionTests.CreateRejectsInvalidBaseData y LicitacionTests.NormalizedCodeIgnoresCaseAndRepeatedSpaces.
- HU-16: Licitaciones. Pruebas: LicitacionTests.PublishAndCloseFollowAllowedTransitions, LicitacionTests.PublishedExpiredTenderIsEffectivelyClosed y LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-17: Licitaciones, API REST. Pruebas: LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-18: Persistencia. Pruebas: LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion y migracion 20260812002104_CreateLicitaciones.
- HU-19: Persistencia, API REST. Pruebas: LicitacionPersistenceTests.ConcurrentUpdatesDetectStaleVersion y manejo controlado de concurrencia en Application/API.
Documentacion relacionada: iteracion-02.md, modulos/licitaciones.md, api.md, modelo-datos.md, pruebas.md y bitacora-xp.md.
Evidencia formal: Issue `#10`, commits `cce95ad`, `812b59c`, `ed89c5a` y `c77343b`, Pull Request `#12` y merge a `main` `fafcc66`.
Pruebas finales: 64/64 aprobadas, 0 fallidas y 0 omitidas.
Liberación: `v0.2.0` (Prevista); tag pendiente.

## Evidencia común de Iteración 4

- Rama: `feature/iteracion-04-moneda-ux`.
- Driver: Eithel.
- Navigator: Chavala.
- Historias: HU-30 a HU-37; 32 puntos implementados y validados localmente.
- Pruebas: UnitTests 96/96, IntegrationTests 27/27 y FunctionalTests 51/51; total 174/174.
- Cobertura limpia: global 87.3%, Domain 91.4%, Application 83.8%, API 88.4%, Infrastructure 95.1% y Web 61.6%; `MultiReport (3x Cobertura)`.
- Docker: config, build, arranque, health checks y persistencia validados; PostgreSQL host 55432.
- Kubernetes: manifiestos preparados y renderizados; despliegue real pendiente de clúster activo.
- Swagger: UI interactiva en `/swagger` y documento en `/swagger/v1/swagger.json`.
- Documentación: `iteraciones/iteracion-04.md`, `iteraciones/iteracion-04-evidencia.md`, `api-iteracion-04.md`, `modulos/tipos-cambio.md`, `pruebas-iteracion-04.md`, `docker.md`, `kubernetes.md`, `bitacora-xp.md` y `uso-ia.md`.
- Issue: `#15 - ITER-04: Moneda, UX y consolidación técnica`.
- Pull Request: `#16`, desde `feature/iteracion-04-moneda-ux` hacia `main`.
- Commits: `40c4f5d`, `38c5bf5`, `5cba6c2`, `9b0fa75`, `8103b12`, `2e710d2`.
- Rama publicada y vinculada a `origin/feature/iteracion-04-moneda-ux`.
- CI remoto, revisión formal del Navigator, merge, tag y GitHub Release: Pendientes.
- Liberación: `v1.0.0-rc` (Prevista); el tag no existe todavía.

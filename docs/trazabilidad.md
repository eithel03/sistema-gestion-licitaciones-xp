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
| FASE-06 | Docker y Docker Compose | N/A | N/A | Docker multi-stage, Compose, PostgreSQL healthy, persistencia, migraciones, usuario no privilegiado, build y pruebas | Validación manual Docker; build Release; 218/218 pruebas; E2E 6/6 | Dockerfiles, `compose.yaml`, Infrastructure, Web, API, documentación | `#19` | `chore/fase-06-docker` | `b26d40a`, `5f16fa6` | `#20` | `docs/docker.md`, `docs/bitacora-xp.md`, `docs/pruebas.md`, `docs/uso-ia.md`, `docs/README.md` | Pendiente |
| FASE-07 | Kubernetes | N/A | N/A | Namespace, Deployments y Services de Web/API, ConfigMap, Secret de ejemplo, StatefulSet/Service/PVC de PostgreSQL, probes, requests y limits, migraciones, despliegue y persistencia | `kubectl kustomize k8s`; dry-run; apply; pods/svcs/pvc Bound; health checks `200 Healthy`; migraciones; persistencia tras reinicio de pod | `k8s/*` (7 archivos creados, 5 eliminados, `kustomization.yaml` modificado), `docs/kubernetes.md`, `docs/bitacora-xp.md` | `#21` | `chore/fase-07-kubernetes` | `2d75e38` | Pendiente | `docs/kubernetes.md`, `docs/bitacora-xp.md`, `docs/trazabilidad.md`, `docs/README.md` | Pendiente |

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

## Evidencia de Fase 5

- Rama: `chore/fase-05-pruebas-cobertura`.
- Driver: Chavala.
- Navigator: Eithel.
- Iteración 4 integrada en `main` mediante `ea9772f` (`Merge pull request #16 from eithel03/feature/iteracion-04-moneda-ux`).
- Commits: `ba3ce34`, `8f14743`, `1512dd8`, `e8c1ee0`, `0cf4cb5`, `7d0b716`, `b8f0dbb` y `92c0301`.
- Pruebas: UnitTests 121/121, IntegrationTests 37/37, FunctionalTests 54/54 y E2ETests 6/6; total 218/218.
- Cobertura: Domain 91,64 %, Application 88,60 %, Infrastructure 95,43 %, Api 90,03 %, Web 66,53 % y global 89,37 %.
- E2E: Chromium real con `Microsoft.Playwright.Xunit` 1.61.0, Kestrel y PostgreSQL Testcontainers.
- Documentación: `pruebas.md`, `bitacora-xp.md`, `uso-ia.md` y este documento.
- Issue: Pendiente; no existe evidencia confirmada de número de Issue.
- Pull Request: `#18`.
- GitHub Actions: existieron ejecuciones fallidas durante el desarrollo; posteriormente el workflow, el CI del PR y el merge a `main` quedaron en verde.
- Merge de Fase 5: `f79d22d` (`Merge pull request #18 from eithel03/chore/fase-05-pruebas-cobertura`).
- Tag: no existe tag de Fase 5.
- Estado: Fase 5 técnicamente completada y validada localmente.

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
- Merge de Iteración 4 a `main`: realizado mediante `ea9772f`. CI remoto y revisión formal del Navigator de Iteración 4, tag y GitHub Release: pendientes. Fase 5 fue integrada a `main` mediante `f79d22d`.
- Liberación: `v1.0.0-rc` (Prevista); el tag no existe todavía.

## Evidencia de Fase 6

- Rama: `chore/fase-06-docker`.
- Issue: `#19`.
- Dockerfiles multi-stage: Web y API construidos como `licitaciones-web:v1.0.0-rc` y `licitaciones-api:v1.0.0-rc`.
- Docker Compose: `docker compose config` y `docker compose up --build -d` exitosos.
- PostgreSQL: imagen `postgres:16`, puerto host `55432` y estado `healthy`.
- Servicios: Web inició en `8080`, API inició en `8081` y Swagger fue verificado manualmente.
- Persistencia: `docker compose restart` fue exitoso y el volumen conservó licitaciones, proveedores, ofertas y tipos de cambio.
- Migraciones: Web y API ejecutan `Database.Migrate()`; `__EFMigrationsHistory` registró con EF Core 9.0.18 las migraciones `20260810092133_CreateProveedores`, `20260811234653_MakeProveedorNameUniqueIndexPartial`, `20260812002104_CreateLicitaciones`, `20260813011055_Iteration03OfertasAprobacion`, `20260813205016_Iteration04TiposCambio` y `20260814014136_AllowDuplicateTipoCambioDates`.
- Usuario no privilegiado: `USER $APP_UID` en ambos Dockerfiles y UID `1654` confirmado en Web y API.
- Build Release: exitoso.
- Pruebas: 218/218 aprobadas, 0 fallidas y 0 omitidas; E2E 6/6 después de instalar Chromium de Playwright localmente.
- Docker y Testcontainers: disponibles durante la validación.
- `docker compose down -v`: no se ejecutó.
- Commits: `b26d40a` y `5f16fa6`.
- Pull Request: `#20`, desde `chore/fase-06-docker` hacia `main`.
- GitHub Actions / Checks: 2/2 exitosos en el Pull Request `#20`.
- Conflictos: ninguno con la rama base `main`; GitHub confirmó que el Pull Request puede integrarse automáticamente.
- Revisión formal final del Navigator: Aprobada por Luis Diego Chavala en el Pull Request `#20`.
- Merge del Pull Request `#20`: Realizado el 17 de agosto de 2026 mediante `7557e45`.
- Liberación/tag: Pendiente.

## Evidencia de Fase 7 — Kubernetes

- Fecha: 17 y 18 de agosto de 2026.
- Modalidad: programación en pareja XP.
- Driver: Luis Diego Chavala.
- Navigator: Eithel Herrera Rojas.
- Issue: `#21` — `feat(k8s): Fase 7 - Despliegue en Kubernetes con StatefulSet, PVC y Probes`.
- Rama: `chore/fase-07-kubernetes`.
- Commit: `2d75e38` — `feat(k8s): implementar despliegue Fase 7 con StatefulSet, PVC y probes (Closes #21)`, subido a `origin`.
- Pull Request: Pendiente.
- Liberación/tag: Pendiente.
- Estructura `/k8s` final: `namespace.yaml`, `app-deployment.yaml`, `app-service.yaml`, `app-configmap.yaml`, `app-secret.example.yaml`, `postgres-statefulset.yaml`, `postgres-service.yaml`, `postgres-pvc.yaml`, `kustomization.yaml`.
- Migración desde Iteración 4: `api.yaml` y `web.yaml` → `app-deployment.yaml`/`app-service.yaml`; `postgres.yaml` → `postgres-statefulset.yaml`/`postgres-service.yaml`/`postgres-pvc.yaml` (Deployment convertido a StatefulSet); `configmap.yaml` → `app-configmap.yaml`; `secret.example.yaml` → `app-secret.example.yaml`.
- Probes: startup/readiness/liveness en PostgreSQL, API y Web.
- Recursos: API y Web requests 100m/128Mi y limits 500m/512Mi; PostgreSQL requests 100m/256Mi y limits 1000m/1Gi.
- Validación: `kubectl kustomize k8s` exitoso y `kubectl apply --dry-run=client -k k8s` exitoso (10 objetos).
- Despliegue: `kubectl apply -k k8s` sobre Docker Desktop Kubernetes v1.34.1.

### Evidencia de despliegue

- Namespace `licitaciones`: Active.
- Pods: `postgres-0`, `licitaciones-api-*` y `licitaciones-web-*` en Running, 1/1, 0 reinicios.
- Deployments `licitaciones-api` y `licitaciones-web`: 1/1 Available; StatefulSet `postgres`: 1/1 Ready.
- Services: `licitaciones-api` ClusterIP `10.104.33.85:8080`, `licitaciones-web` NodePort `10.103.223.241:8080:30080`, `postgres` ClusterIP `10.106.37.125:5432`.
- PVC `postgres-data`: Bound (1Gi, RWO).
- ConfigMap `licitaciones-config`: 5 entradas; Secret `licitaciones-secret`: 2 entradas (ejemplo).
- Migraciones: las seis migraciones aplicadas en `__EFMigrationsHistory` dentro del clúster.

### Evidencia de health checks

- API: `GET http://localhost:8080/health` → `200 Healthy` (vía `kubectl port-forward -n licitaciones svc/licitaciones-api 8080:8080`).
- Web: `GET http://localhost:30080/health` → `200 Healthy` (Service NodePort).

### Evidencia de persistencia

1. Se creó el proveedor "Proveedor Prueba Persistencia K8s" vía API (ID: `5f534d0a-ebd1-4443-8da9-09c1fe0bd88c`).
2. Se ejecutó `kubectl delete pod -n licitaciones postgres-0`.
3. El StatefulSet recreó el pod `postgres-0` automáticamente en aproximadamente 15 segundos.
4. Se consultó el proveedor por su ID y los datos sobrevivieron al reinicio, confirmando que el PVC funciona correctamente.

- Problema encontrado: reinicio inicial de Web/API por `Migrate()` contra PostgreSQL no disponible; resuelto con initContainer `wait-for-postgres`.
- No se ejecutó `kubectl delete pvc postgres-data`; eliminar el PVC destruiría los datos.
- Pull Request y cierre formal con el Navigator: Pendientes.

# Matriz inicial de trazabilidad

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
| HU-11 | Iteracion 3 | Media | 2 | Definidos en `historias-usuario.md` | Funcional e integracion de ofertas relacionadas | Proveedores, ofertas | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-12 | Iteracion 2 | Alta | 5 | Definidos en historias-usuario.md | LicitacionTests.CreateValidTenderStartsAsDraft; LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi | Licitaciones | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, pruebas.md, bitacora-xp.md | v0.2.0 |
| HU-13 | Iteracion 2 | Alta | 3 | Definidos en historias-usuario.md | LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion; LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi | Licitaciones | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, api.md, pruebas.md | v0.2.0 |
| HU-14 | Iteracion 2 | Alta | 3 | Definidos en historias-usuario.md | LicitacionTests.InvalidTransitionsAndUpdatesAreRejected; LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion | Licitaciones | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, modelo-datos.md, pruebas.md | v0.2.0 |
| HU-15 | Iteracion 2 | Alta | 5 | Definidos en historias-usuario.md | LicitacionTests.CreateRejectsInvalidBaseData; LicitacionTests.NormalizedCodeIgnoresCaseAndRepeatedSpaces | Licitaciones | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, pruebas.md | v0.2.0 |
| HU-16 | Iteracion 2 | Alta | 5 | Definidos en historias-usuario.md | LicitacionTests.PublishAndCloseFollowAllowedTransitions; LicitacionTests.PublishedExpiredTenderIsEffectivelyClosed; LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi | Licitaciones | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, pruebas.md | v0.2.0 |
| HU-17 | Iteracion 2 | Alta | 5 | Definidos en historias-usuario.md | LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi | Licitaciones, API REST | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, api.md, pruebas.md | v0.2.0 |
| HU-18 | Iteracion 2 | Alta | 5 | Definidos en historias-usuario.md | LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion; migracion 20260812002104_CreateLicitaciones | Persistencia | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, modelo-datos.md, pruebas.md | v0.2.0 |
| HU-19 | Iteracion 2 | Alta | 5 | Definidos en historias-usuario.md | LicitacionPersistenceTests.ConcurrentUpdatesDetectStaleVersion; manejo controlado de concurrencia en Application/API | Persistencia, API REST | Pendiente | feature/iteracion-02-licitaciones | Pendiente | Pendiente | iteracion-02.md, modulos/licitaciones.md, api.md, modelo-datos.md, pruebas.md, bitacora-xp.md | v0.2.0 |
| HU-20 | Iteracion 3 | Alta | 5 | Definidos en `historias-usuario.md` | Unitarias e integracion | Ofertas | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-21 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | Filtros e integracion | Ofertas, API REST | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-22 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | Permisos e integracion | Ofertas, licitaciones | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-23 | Iteracion 3 | Alta | 5 | Definidos en `historias-usuario.md` | Duplicidad y vencimiento | Ofertas, licitaciones | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-24 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | Limite presupuestario | Ofertas, licitaciones | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-25 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | Mejor oferta y desempate | Ofertas, licitaciones | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-26 | Iteracion 3 | Media | 3 | Definidos en `historias-usuario.md` | Calculo de ahorro | Ofertas, licitaciones | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-27 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | CRUD niveles de aprobacion | Niveles de aprobacion | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-28 | Iteracion 3 | Alta | 5 | Definidos en `historias-usuario.md` | Rangos y aprobador | Niveles de aprobacion, ofertas | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-29 | Iteracion 3 | Alta | 3 | Definidos en `historias-usuario.md` | Endpoints de ofertas y aprobaciones | Ofertas, aprobaciones, API REST | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-03.md` | v0.3.0 |
| HU-30 | Iteracion 4 | Media | 5 | Definidos en `historias-usuario.md` | CRUD tipos de cambio | Tipos de cambio | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-31 | Iteracion 4 | Alta | 3 | Definidos en `historias-usuario.md` | Activacion unica | Tipos de cambio | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-32 | Iteracion 4 | Media | 3 | Definidos en `historias-usuario.md` | Conversion visual | Tipos de cambio, interfaz | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-33 | Iteracion 4 | Media | 3 | Definidos en `historias-usuario.md` | Preferencia visual | Interfaz | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-34 | Iteracion 4 | Alta | 5 | Definidos en `historias-usuario.md` | Contrato y errores API | API REST | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-35 | Iteracion 4 | Alta | 5 | Definidos en `historias-usuario.md` | Suites y cobertura | Pruebas, integracion continua | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-36 | Iteracion 4 | Media | 5 | Definidos en `historias-usuario.md` | Contenedores y manifiestos | Infraestructura | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md` | v1.0.0-rc |
| HU-37 | Iteracion 4 | Alta | 3 | Definidos en `historias-usuario.md` | Revision documental | Documentacion, XP | Pendiente | Pendiente | Pendiente | Pendiente | `historias-usuario.md`, `iteracion-04.md`, `bitacora-xp.md` | v1.0.0-rc |
| FASE-02 | PreparaciÃ³n tÃ©cnica | N/A | N/A | InicializaciÃ³n tÃ©cnica del monolito modular | `ArchitectureTests.cs`, `InfrastructureAssemblyTests.cs`, `HealthEndpointTests.cs` | `src/`, `tests/`, `docs/`, `.github/workflows/ci.yml` | `#3` | `chore/arquitectura-inicial` | `ad7913b`, `e0e5ad1`, `821ab9d`, `65a6afd` | `#4` | `docs/arquitectura-general.md`, `docs/bitacora-xp.md`, `docs/trazabilidad.md`, `docs/uso-ia.md`, `docs/README.md` | CI aprobado |

| FASE-03 | PreparaciÃ³n dominio/TDD | N/A | N/A | Convenciones mÃ­nimas de dominio y pruebas preparatorias | `EntityTests.cs`, `ValueObjectTests.cs`, `ValidationResultTests.cs`, `IClockTests.cs` | `Domain`, `Application`, `Infrastructure`, `UnitTests` | `#7` | `chore/fase-03-dominio-tdd` | `2200fe3` | `#6` | `docs/dominio-tdd.md`, `docs/pruebas.md`, `docs/arquitectura-general.md`, `docs/bitacora-xp.md`, `docs/uso-ia.md` | CI aprobado |
| FASE-04 | Preparacion persistencia | N/A | N/A | Infraestructura minima de PostgreSQL y EF Core sin tablas futuras | `PersistenceConventionsTests.cs`, `PostgreSqlContainerTests.cs`, restore, build, test, Docker Compose | `Infrastructure`, `IntegrationTests`, `compose.yaml`, documentacion | `#5` | `chore/preparacion-persistencia` | Pendiente | Pendiente | `docs/arquitectura-general.md`, `docs/modelo-datos.md`, `docs/pruebas.md`, `docs/bitacora-xp.md`, `docs/uso-ia.md`, `docs/README.md` | Pendiente |

## Totales

| Iteracion | Puntos |
| --- | ---: |
| Iteracion 1 | 30 |
| Iteracion 2 | 36 |
| Iteracion 3 | 38 |
| Iteracion 4 | 32 |
| Total | 136 |

## Actualizacion Iteracion 2 - HU-12 a HU-19.

Evidencia real local para las historias HU-12 a HU-19.
Rama comun: feature/iteracion-02-licitaciones.
- HU-12: Licitaciones. Pruebas: LicitacionTests.CreateValidTenderStartsAsDraft y LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-13: Licitaciones. Pruebas: LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion y LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-14: Licitaciones. Pruebas: LicitacionTests.InvalidTransitionsAndUpdatesAreRejected y LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion.
- HU-15: Licitaciones. Pruebas: LicitacionTests.CreateRejectsInvalidBaseData y LicitacionTests.NormalizedCodeIgnoresCaseAndRepeatedSpaces.
- HU-16: Licitaciones. Pruebas: LicitacionTests.PublishAndCloseFollowAllowedTransitions, LicitacionTests.PublishedExpiredTenderIsEffectivelyClosed y LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-17: Licitaciones, API REST. Pruebas: LicitacionApiTests.CreatePublishCloseAndRejectInvalidTransitionThroughApi.
- HU-18: Persistencia. Pruebas: LicitacionPersistenceTests.SavesListsAndLogicallyDeletesLicitacion y migracion 20260812002104_CreateLicitaciones.
- HU-19: Persistencia, API REST. Pruebas: LicitacionPersistenceTests.ConcurrentUpdatesDetectStaleVersion y manejo controlado de concurrencia en Application/API.
Documentacion relacionada: iteracion-02.md, modulos/licitaciones.md, api.md, modelo-datos.md, pruebas.md y bitacora-xp.md.
Campos pendientes para HU-12 a HU-19: Issue, commits definitivos y Pull Request.
Liberacion prevista: v0.2.0.

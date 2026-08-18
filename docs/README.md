# Documentación del proyecto

Índice oficial de la documentación del Sistema de Gestión de Licitaciones.

## Planificación XP

- [Visión y alcance](vision-alcance.md).
- [Historias de usuario](historias-usuario.md).
- [Plan XP](plan-xp.md).
- [Plan de liberación](plan-liberacion.md).
- [Bitácora XP](bitacora-xp.md).
- [Matriz de trazabilidad](trazabilidad.md).
- [Propuesta de GitHub Issues y Milestones](github-issues-propuestos.md).

## Iteraciones

- [Iteración 1](iteraciones/iteracion-01.md).
- [Iteración 2](iteraciones/iteracion-02.md).
- [Iteración 3](iteraciones/iteracion-03.md).
- [Iteración 4](iteraciones/iteracion-04.md).
- [Evidencia de Iteración 4](iteraciones/iteracion-04-evidencia.md).

## Documentación técnica

- [Arquitectura general](arquitectura-general.md).
- [Dominio y estrategia TDD](dominio-tdd.md).
- [Modelo de datos](modelo-datos.md).
- [API REST de iteraciones anteriores](api.md).
- [API de Iteración 4](api-iteracion-04.md).
- [Pruebas generales](pruebas.md).
- [Pruebas de Iteración 4](pruebas-iteracion-04.md).
- [Docker](docker.md).
- [Kubernetes](kubernetes.md).
- [Integración de módulos](integracion-modulos.md).
- [Documentación por módulos](modulos/README.md).
- [Módulo de tipos de cambio](modulos/tipos-cambio.md).

## Registros existentes

- [Flujo Git y GitHub](flujo-git.md).
- [Uso de inteligencia artificial](uso-ia.md).

## Estado de Fase 5

Fase 5 está técnicamente completada y validada localmente en `chore/fase-05-pruebas-cobertura`. Chavala actuó como Driver principal y Eithel como Navigator principal. Las suites finales aprobaron 218/218 pruebas: UnitTests 121/121, IntegrationTests 37/37, FunctionalTests 54/54 y E2ETests 6/6. La cobertura de líneas fue Domain 91,64 %, Application 88,60 % y global 89,37 %, superando los umbrales requeridos.

Se registraron los commits `ba3ce34`, `8f14743`, `1512dd8`, `e8c1ee0`, `0cf4cb5`, `7d0b716`, `b8f0dbb` y `92c0301`. El Pull Request `#18` fue integrado a `main` mediante `f79d22d` (`Merge pull request #18 from eithel03/chore/fase-05-pruebas-cobertura`). Existieron ejecuciones fallidas de GitHub Actions durante el desarrollo; posteriormente el workflow, el CI del PR y el merge quedaron en verde. La revisión formal del Navigator permanece pendiente y no existe tag de Fase 5.

## Estado de Iteración 4

Las historias HU-30 a HU-37 están implementadas y validadas localmente en `feature/iteracion-04-moneda-ux`. La suite final aprobó 174/174 pruebas y Docker fue validado con persistencia. Los manifiestos Kubernetes están preparados y renderizan correctamente; su despliegue real permanece pendiente de un clúster activo.

Issue `#15`, Pull Request `#16` y los commits `40c4f5d`, `38c5bf5`, `5cba6c2`, `9b0fa75`, `8103b12` y `2e710d2` están registrados. El merge de Iteración 4 a `main` está evidenciado por `ea9772f`. CI remoto, revisión formal final del Navigator, tag `v1.0.0-rc` y GitHub Release permanecen pendientes.

## Estado de Fase 6

La Fase 6 se validó localmente en la rama `chore/fase-06-docker`, asociada al Issue `#19`, con Eithel Herrera Rojas como Driver y Luis Diego Chavala como Navigator. Docker Compose inició correctamente Web en `8080`, API en `8081` y PostgreSQL 16 en `55432` con estado `healthy`; Swagger estuvo disponible y el volumen conservó licitaciones, proveedores, ofertas y tipos de cambio después del reinicio.

Web y API están configurados para aplicar automáticamente las migraciones pendientes mediante `Database.Migrate()`. La tabla `__EFMigrationsHistory` confirmó seis migraciones aplicadas correctamente.

El Issue `#19` fue realizado y se registraron los commits `b26d40a` y `5f16fa6`. El Pull Request `#20`, desde `chore/fase-06-docker` hacia `main`, fue integrado el 17 de agosto de 2026 mediante `7557e45` con 2/2 checks exitosos y la revisión formal final del Navigator aprobada por Luis Diego Chavala. La liberación/tag permanece pendiente.

## Estado de Fase 7

La Fase 7 se validó en la rama `chore/fase-07-kubernetes` (Issue `#21`), con Luis Diego Chavala como Driver y Eithel Herrera Rojas como Navigator. Los manifiestos de `/k8s` se reorganizaron a la estructura `namespace.yaml`, `app-deployment.yaml`, `app-service.yaml`, `app-configmap.yaml`, `app-secret.example.yaml`, `postgres-statefulset.yaml`, `postgres-service.yaml`, `postgres-pvc.yaml` y `kustomization.yaml`.

El despliegue se realizó sobre el clúster local de Docker Desktop (Kubernetes v1.34.1): Namespace `licitaciones` Active, tres pods en Running con 0 reinicios, PVC `postgres-data` Bound, health checks `200 Healthy` en Web (`http://localhost:30080/health`) y API (`http://localhost:8080/health`), seis migraciones aplicadas y persistencia comprobada después de eliminar y recrear el pod `postgres-0`. Los detalles están en [kubernetes.md](kubernetes.md) y [bitacora-xp.md](bitacora-xp.md). El commit `2d75e38` se subió a `origin/chore/fase-07-kubernetes`; el Pull Request y el cierre formal permanecen pendientes.

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

Se registraron los commits `ba3ce34`, `8f14743`, `1512dd8`, `e8c1ee0`, `0cf4cb5` y `7d0b716`. La revisión formal del Navigator, Pull Request, CI remoto y merge de Fase 5 permanecen pendientes. No existe tag de Fase 5.

## Estado de Iteración 4

Las historias HU-30 a HU-37 están implementadas y validadas localmente en `feature/iteracion-04-moneda-ux`. La suite final aprobó 174/174 pruebas y Docker fue validado con persistencia. Los manifiestos Kubernetes están preparados y renderizan correctamente; su despliegue real permanece pendiente de un clúster activo.

Issue `#15`, Pull Request `#16` y los commits `40c4f5d`, `38c5bf5`, `5cba6c2`, `9b0fa75`, `8103b12` y `2e710d2` están registrados. El merge de Iteración 4 a `main` está evidenciado por `ea9772f`. CI remoto, revisión formal final del Navigator, tag `v1.0.0-rc` y GitHub Release permanecen pendientes.

# Documentacion del proyecto

Indice oficial de la documentacion del Sistema de Gestion de Licitaciones.

## Planificacion XP

- [Vision y alcance](vision-alcance.md).
- [Historias de usuario](historias-usuario.md).
- [Plan XP](plan-xp.md).
- [Plan de liberacion](plan-liberacion.md).
- [Bitacora XP](bitacora-xp.md).
- [Trazabilidad inicial](trazabilidad.md).
- [Propuesta de GitHub Issues y Milestones](github-issues-propuestos.md).

## Iteraciones

- [Iteracion 1](iteraciones/iteracion-01.md).
- [Iteracion 2](iteraciones/iteracion-02.md).
- [Iteracion 3](iteraciones/iteracion-03.md).
- [Iteracion 4](iteraciones/iteracion-04.md).

## Documentacion tecnica

- [Arquitectura general](arquitectura-general.md) - documentada inicialmente en la Fase 2.
- [Dominio y estrategia TDD](dominio-tdd.md) - documentada inicialmente en la Fase 3.
- [Modelo de datos](modelo-datos.md) - incluye proveedores, licitaciones, ofertas y niveles de aprobacion.
- [API REST](api.md) - documenta proveedores, licitaciones, ofertas y niveles de aprobacion.
- [Pruebas](pruebas.md) - iniciada en la Fase 3.
- [Docker](docker.md) - pendiente como documento completo; PostgreSQL local queda preparado en Fase 4 mediante `compose.yaml`.
- [Kubernetes](kubernetes.md) - pendiente de una fase posterior.
- [Integracion de modulos](integracion-modulos.md) - documenta relaciones de ofertas en Iteracion 3.
- [Documentacion por modulos](modulos/README.md) - indice de modulos implementados.

## Registros existentes

- [Flujo Git y GitHub](flujo-git.md).
- [Uso de inteligencia artificial](uso-ia.md).

## Estado Iteracion 3

La Iteracion 3 esta tecnicamente implementada y validada localmente en `feature/iteracion-03-ofertas-aprobacion`. La revision formal del Navigator, Pull Request, CI remoto, merge a `main` y tag previsto `v0.3.0` permanecen pendientes.

# Documentación del proyecto

Índice canónico del Sistema de Gestión de Licitaciones. La línea base documental de Fase 9 es `main@36e89ec`, después de la integración del Pull Request `#24` de Fase 8.

## Estado actual

Estos documentos describen el comportamiento presente en el código:

- [Visión y alcance](vision-alcance.md)
- [Arquitectura general](arquitectura-general.md)
- [Modelo de datos](modelo-datos.md)
- [API REST](api.md)
- [Integración de módulos](integracion-modulos.md)
- [Pruebas](pruebas.md)
- [Docker](docker.md)
- [Kubernetes](kubernetes.md)
- [Uso de inteligencia artificial](uso-ia.md)
- [Matriz de trazabilidad](trazabilidad.md)
- [Documentación por módulos](modulos/README.md)
- [Recursos visuales y criterio de capturas](assets/README.md)

### Módulos

- [Proveedores](modulos/proveedores.md)
- [Licitaciones](modulos/licitaciones.md)
- [Ofertas](modulos/ofertas.md)
- [Niveles de aprobación](modulos/niveles-aprobacion.md)
- [Tipo de cambio](modulos/tipo-cambio.md)
- [Interfaz Web](modulos/interfaz-web.md)
- [API REST](modulos/api-rest.md)
- [Persistencia](modulos/persistencia.md)

### Estado de la liberación

- Fases 0 a 8: integradas en `main`.
- Fase 9: documentación actualizada localmente al 21 de agosto de 2026; revisión del equipo e integración aún no registradas.
- Fase 10: posterior a Fase 9; no iniciada ni evidenciada en esta línea base.
- Tag Git oficial: no existe.
- GitHub Release: no existe evidencia local de una release.
- `v1.0.0-rc`: nombre de imágenes Docker y versión prevista, no tag oficial.
- `v0.1.0` en la landing: texto antiguo que requiere una corrección técnica futura fuera de Fase 9.

## Planificación y registros XP

- [Historias de usuario HU-01 a HU-37](historias-usuario.md)
- [Plan XP](plan-xp.md)
- [Plan de liberación](plan-liberacion.md)
- [Bitácora XP](bitacora-xp.md)
- [Flujo Git](flujo-git.md)
- [Propuesta histórica de Issues](github-issues-propuestos.md)
- [Estrategia histórica de dominio y TDD](dominio-tdd.md)

## Histórico y evidencia de iteraciones

Los siguientes documentos conservan decisiones y resultados registrados en el momento de cada iteración. Cuando un estado histórico difiera del actual, prevalecen esta página, la bitácora de Fase 9 y la matriz de trazabilidad vigente.

- [Iteración 1 — landing y proveedores](iteraciones/iteracion-01.md)
- [Iteración 2 — licitaciones](iteraciones/iteracion-02.md)
- [Iteración 3 — ofertas y aprobaciones](iteraciones/iteracion-03.md)
- [Iteración 4 — moneda, UX y API](iteraciones/iteracion-04.md)
- [Evidencia de Iteración 4](iteraciones/iteracion-04-evidencia.md)
- [API de Iteración 4, referencia histórica](api-iteracion-04.md)
- [Pruebas de Iteración 4, referencia histórica](pruebas-iteracion-04.md)

## Criterio de evidencia

- **Estado actual:** verificado contra archivos presentes en `main@36e89ec`.
- **Evidencia histórica:** resultados registrados por el equipo en fases anteriores; no implica una nueva ejecución durante Fase 9.
- **Trabajo futuro:** limitaciones técnicas detectadas y no corregidas porque Fase 9 solo modifica documentación.

No se atribuyen a Fase 9 tags, releases, Issues, ramas, commits, Pull Requests ni resultados de pruebas que aún no existan.

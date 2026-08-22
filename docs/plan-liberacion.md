# Plan de liberación

## Objetivo

Entregar incrementalmente un Sistema de Gestión de Licitaciones funcional, probado y documentado mediante pequeñas liberaciones XP.

## Alcance implementado

- navegación, formularios, preferencias y mensajes;
- proveedores, licitaciones, ofertas, niveles de aprobación y tipos de cambio;
- mejor oferta, clasificación y aprobador;
- API REST `/api/v1` y errores controlados;
- EF Core/PostgreSQL;
- pruebas unitarias, integración, funcionales y E2E;
- Docker, Kubernetes y GitHub Actions.

Las limitaciones vigentes se describen en los documentos canónicos y no se ocultan como funcionalidades completas.

## Iteraciones y versiones previstas

| Iteración | Historias | Puntos | Nombre previsto |
|---|---|---:|---|
| 1 | HU-01 a HU-10 | 30 | `v0.1.0` |
| 2 | HU-12 a HU-19 | 36 | `v0.2.0` |
| 3 | HU-11 y HU-20 a HU-29 | 38 | `v0.3.0` |
| 4 | HU-30 a HU-37 | 32 | `v1.0.0-rc` |

Los cuatro bloques fueron integrados en `main`, pero los nombres continúan siendo propuestas de planificación. El repositorio no contiene tags Git y no hay evidencia local de una GitHub Release. `v1.0.0-rc` también se usa como etiqueta de imagen Docker; eso no la convierte en versión oficial.

## Estado de fases

- Fases 0–8: integradas antes de `main@36e89ec`.
- Fase 9: sincronización documental actual.
- Fase 10: posterior, no iniciada. Su alcance no se inventa en este documento.

## Prácticas XP

- Planning Game e historias de usuario.
- Programación en parejas con rotación de Driver/Navigator.
- TDD y pruebas de aceptación.
- Integración continua.
- Diseño simple y refactorización.
- Propiedad colectiva.
- Ritmo sostenible.
- Pequeñas liberaciones solo cuando son demostrables.

## Criterios de liberación oficial

Una liberación oficial futura requiere:

- aceptación del alcance;
- pruebas aplicables ejecutadas y evidencia conservada;
- limitaciones conocidas acordadas;
- documentación y trazabilidad actualizadas;
- revisión de la pareja;
- tag y release creados explícitamente por el equipo.

No se declara `v1.0.0` ni `v1.0.0-rc` como release oficial durante Fase 9.

## Riesgos para una liberación posterior

- inconsistencia visual de versión entre landing, Docker y Git;
- OpenAPI manual incompleto;
- migraciones ejecutadas por dos hosts;
- health checks parciales;
- secreto Kubernetes de ejemplo incluido en Kustomize;
- validaciones de formato/vulnerabilidades no bloqueantes;
- limitaciones de paginación y presentación MVC.

Estos riesgos requieren decisión técnica posterior; Fase 9 solo los documenta.

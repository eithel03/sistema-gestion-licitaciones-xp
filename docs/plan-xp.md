# Plan XP

## Metodología

Extreme Programming es la única metodología del proyecto. El trabajo se organiza mediante Planning Game, historias de usuario, iteraciones cortas, pequeñas liberaciones, programación en parejas, TDD, integración continua, diseño simple, refactorización y propiedad colectiva.

## Estado del plan al iniciar Fase 9

Línea base: `main@36e89ec`.

| Bloque | Estado Git comprobado |
|---|---|
| Fases 0–4 | Integradas en `main`. |
| Iteraciones 1–4 | Integradas mediante PR #9, #12, #14 y #16. |
| Fase 5 | Integrada mediante PR #18. |
| Fase 6 | Integrada mediante PR #20. |
| Fase 7 | Integrada mediante PR #22 y merge `82c8c58`. |
| Fase 8 | Integrada mediante PR #24 y merge `36e89ec`. |
| Fase 9 | Actualización documental; no existe todavía rama, Issue, commit o PR registrado. |
| Fase 10 | Posterior a Fase 9; no iniciada ni evidenciada. |

## Planning Game e historias

El alcance funcional está expresado en 37 historias, 136 puntos y cuatro iteraciones. Cada historia conserva prioridad, estimación, dependencias, criterios de aceptación, módulos y pruebas. La Fase 9 sincroniza documentación; no crea historias funcionales nuevas.

## Iteraciones y pequeñas liberaciones

- Iteración 1: landing y proveedores.
- Iteración 2: licitaciones.
- Iteración 3: ofertas y niveles de aprobación.
- Iteración 4: tipo de cambio, UX, API e infraestructura inicial.

Los nombres `v0.1.0`, `v0.2.0`, `v0.3.0` y `v1.0.0-rc` son versiones previstas en el plan. No existen tags Git oficiales.

## Programación en parejas y rotación

| Trabajo | Driver | Navigator |
|---|---|---|
| Iteración 1 | Luis Diego Chavala | Eithel Herrera Rojas |
| Iteración 2 | Eithel Herrera Rojas | Luis Diego Chavala |
| Iteración 3 | Luis Diego Chavala | Eithel Herrera Rojas |
| Iteración 4 | Eithel Herrera Rojas | Luis Diego Chavala |
| Fase 5 | Luis Diego Chavala | Eithel Herrera Rojas |
| Fase 6 | Eithel Herrera Rojas | Luis Diego Chavala |
| Fase 7 | Luis Diego Chavala | Eithel Herrera Rojas |
| Fase 8 | Eithel Herrera Rojas | Luis Diego Chavala |
| Fase 9 | Luis Diego Chavala | Eithel Herrera Rojas |

El Driver opera el entorno y prepara cambios. El Navigator revisa estrategia, exactitud, omisiones y coherencia. La responsabilidad final permanece en ambos estudiantes.

## TDD

Para cambios funcionales se sigue rojo–verde–refactorización:

1. prueba que demuestra la regla faltante;
2. código mínimo;
3. refactorización con pruebas en verde;
4. evidencia en bitácora y trazabilidad.

Fase 9 no modifica código ni crea pruebas funcionales; valida documentación mediante inspección, enlaces y correspondencia con la línea base.

## Integración continua

Cada Pull Request ejecuta las validaciones disponibles. El workflow actual incluye build, cuatro suites, cobertura, formato informativo, vulnerabilidades informativas, construcción Docker y renderizado Kustomize. Sus limitaciones se documentan en [pruebas.md](pruebas.md#integración-continua).

## Diseño simple y refactorización

El diseño resuelve las historias presentes sin anticipar complejidad innecesaria. Las refactorizaciones funcionales requieren pruebas. En Fase 9 solo se refactoriza estructura documental; las limitaciones técnicas se registran como trabajo futuro.

## Propiedad colectiva y estándares

- Ambos integrantes pueden revisar y mejorar cualquier documento o componente.
- .NET 9 y convenciones idiomáticas para código.
- Conventional Commits.
- Errores sin datos sensibles.
- Decisiones técnicas documentadas.
- No inventar Issues, PR, commits, tags, releases ni resultados.

## Ritmo sostenible y velocidad

La velocidad XP se calcula con puntos terminados por iteración. Las cuatro iteraciones planificaron 136 puntos. Fase 9 es una fase documental y no altera retrospectivamente la estimación de las historias.

## Definición de terminado

Una historia funcional está terminada cuando cumple criterios, pruebas aplicables, revisión de pareja, integración y trazabilidad. Fase 9 termina cuando:

- la documentación representa `main@36e89ec`;
- los documentos canónicos separan estado actual, evidencia histórica y trabajo futuro;
- los ocho módulos contienen las secciones requeridas;
- enlaces y nombres se validan;
- Bitácora, trazabilidad y uso de IA registran Fase 9;
- no se modifica código, infraestructura ni pruebas;
- rama, Issue, commits y PR se registran únicamente cuando existan.

## Convenciones de ramas y commits

Ramas: `feature/`, `test/`, `fix/`, `refactor/`, `docs/` o `chore/`. Commits: `feat`, `test`, `fix`, `refactor`, `docs`, `chore`.

Flujo XP: historia o tarea → rama → trabajo en pareja → pruebas/validación aplicable → commits pequeños → Pull Request → integración continua → revisión → merge → bitácora y trazabilidad.

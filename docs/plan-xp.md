# Plan XP

## Metodologia

Extreme Programming es la unica metodologia usada en este proyecto. Todas las decisiones de planificacion, ejecucion, revision y mejora se expresan con terminos y practicas de XP.

## Planning Game

El Planning Game permite que el cliente exprese necesidades mediante historias de usuario y que los programadores estimen el esfuerzo relativo. El resultado inicial queda documentado en historias priorizadas, plan de liberacion y planes de iteracion.

## Historias de usuario

Cada historia incluye codigo unico, prioridad, estimacion, dependencias, iteracion asignada, criterios de aceptacion, pruebas previstas, modulo relacionado y campos de trazabilidad para Issue, Pull Request, commits y pruebas.

## Iteraciones cortas y pequenas liberaciones

El proyecto se organiza en cuatro iteraciones uniformes. Cada iteracion debe producir un resultado demostrable, aunque sea pequeno, y registrar la velocidad XP observada para ajustar el plan.

## TDD

Las reglas de negocio se implementaran siguiendo TDD:

1. Escribir una prueba que falle por la regla requerida.
2. Implementar el codigo minimo para hacerla pasar.
3. Refactorizar manteniendo las pruebas en verde.
4. Registrar evidencia del ciclo cuando la historia lo requiera.

## Programacion en parejas

El equipo trabajara en pareja con driver y navigator. El driver opera el entorno y el navigator revisa estrategia, calidad, omisiones y coherencia con la historia.

## Rotacion de driver y navigator

- Iteracion 1: Driver Chavala, navigator Eithel.
- Iteracion 2: Driver Eithel, navigator Chavala.
- Iteracion 3: Driver Chavala, navigator Eithel.
- Iteracion 4: Driver Eithel, navigator Chavala.

## Integracion continua

Cada Pull Request debera ejecutar las verificaciones automatizadas disponibles. Al avanzar el proyecto, la integracion continua incorporara pruebas unitarias, pruebas de integracion con PostgreSQL real, pruebas funcionales E2E, cobertura y revision de dependencias.

## Diseno simple

El diseno debe resolver la historia actual sin anticipar complejidad innecesaria. Las abstracciones se agregaran cuando eliminen duplicacion real, mejoren claridad o protejan reglas compartidas.

## Refactorizacion

La refactorizacion se realizara de forma continua, respaldada por pruebas. Debe mejorar estructura interna sin cambiar comportamiento esperado.

## Propiedad colectiva

Ambos integrantes pueden modificar cualquier parte del codigo o documentacion, siempre con revision de la pareja y trazabilidad clara.

## Estandares de codigo

- Usar convenciones idiomaticas de .NET 9.
- Mantener nombres claros y consistentes.
- Evitar duplicacion innecesaria.
- Mantener errores controlados y sin datos sensibles.
- Documentar decisiones tecnicas cuando afecten arquitectura, API, persistencia o pruebas.

## Ritmo sostenible

La planificacion se ajustara segun velocidad XP observada. No se compensara baja velocidad con trabajo oculto ni con reduccion de calidad.

## Pruebas de aceptacion

Cada historia contiene criterios verificables. Una historia no se considera terminada si sus pruebas de aceptacion no pueden demostrarse o si falta trazabilidad minima.

## Velocidad XP

La velocidad XP se calcula con puntos de historias terminadas por iteracion. Solo cuentan historias con criterios aceptados, pruebas aplicables ejecutadas y documentacion actualizada.

## Reglas de trabajo

- No trabajar directamente sobre `main`.
- Crear ramas pequenas alineadas con historias o tareas tecnicas.
- Mantener commits pequenos y descriptivos.
- Registrar decisiones y evidencias en la bitacora XP.
- No inventar Issues, Pull Requests, commits ni resultados de pruebas.
- Priorizar calidad verificable sobre cantidad de cambios.

## Definicion de terminado

Una historia esta terminada cuando:

- Sus criterios de aceptacion fueron cumplidos.
- Las pruebas previstas aplicables fueron ejecutadas.
- La pareja reviso el cambio.
- La integracion continua no reporta fallos bloqueantes.
- La documentacion y trazabilidad fueron actualizadas.
- La bitacora XP registra resultado, driver y navigator.

## Convenciones de ramas

- `feature/HU-XX-descripcion`
- `test/HU-XX-descripcion`
- `fix/HU-XX-descripcion`
- `refactor/HU-XX-descripcion`
- `docs/descripcion`
- `chore/descripcion`

## Convenciones de commits

Se usaran Conventional Commits:

- `feat`: funcionalidad.
- `test`: pruebas.
- `fix`: correccion.
- `refactor`: mejora interna sin cambio funcional.
- `docs`: documentacion.
- `chore`: mantenimiento o configuracion.

## Flujo de trabajo

Issue -> rama -> TDD -> commits pequenos -> Pull Request -> integracion continua -> revision de pareja -> merge -> bitacora XP.

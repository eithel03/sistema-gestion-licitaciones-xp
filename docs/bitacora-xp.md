# Bitácora de Extreme Programming

## Sesión 001 — Preparación inicial

- Fecha: 2 de agosto de 2026.
- Fase: Fase 0 — Organización inicial y preparación del equipo.
- Modalidad: Programación en parejas.
- Driver: Eithel Herrera Rojas.
- Navigator: Luis Diego Chavala.
- Rama: `chore/configuracion-inicial`.

### Actividades realizadas

- Creación del repositorio de GitHub.
- Incorporación del segundo integrante como colaborador.
- Creación del README inicial.
- Clonación del repositorio.
- Definición de la estructura documental inicial.
- Definición preliminar de las reglas de trabajo.

### Responsabilidades del driver

- Operar la computadora.
- Crear archivos y carpetas.
- Ejecutar comandos Git.
- Realizar los commits.
- Subir la rama.
- Crear el Pull Request.

### Responsabilidades del navigator

- Revisar los cambios en tiempo real.
- Comparar la estructura con el enunciado.
- Verificar nombres y contenido.
- Detectar omisiones.
- Revisar el Pull Request.

### Resultado esperado

Repositorio preparado para iniciar la planificación XP.

### Observaciones

En esta fase no se aplicó TDD porque no se implementaron reglas de negocio ni comportamiento funcional.

## Sesión 002 — Fase 1: Planning Game y planificación XP

- **Fecha:** 3 de agosto de 2026.
- **Duración:** 40 minutos.
- **Fase:** Fase 1 — Planning Game, historias de usuario y planificación XP.
- **Modalidad:** Programación en parejas.
- **Driver:** Luis Diego Chavala.
- **Navigator:** Eithel Herrera Rojas.
- **Rama:** `docs/fase-1-planificacion-xp`.
- **Commits principales:**
  - `docs(xp): mover HU-11 a la iteracion 3`
  - `docs(xp): completar plan de liberacion e iteraciones`
  - `docs(xp): completar vision y plan de trabajo`
  - `docs: organizar estructura documental requerida`
  - `docs(xp): registrar planificacion de fase 1`
  - `Merge pull request #2 from eithel03/docs/fase-1-planificacion-xp`

### Objetivo de la sesión

Definir el alcance funcional inicial del Sistema de Gestión de Licitaciones y organizar el trabajo mediante Extreme Programming, estableciendo historias de usuario, prioridades, estimaciones, criterios de aceptación, plan de liberación y distribución en cuatro iteraciones.

### Actividades realizadas

- Revisión del enunciado oficial del proyecto.
- Identificación de los módulos y funcionalidades requeridas.
- Redacción de las historias de usuario.
- Asignación de códigos, prioridades y estimaciones.
- Definición de criterios de aceptación verificables.
- Identificación de dependencias entre historias.
- Distribución de historias en cuatro iteraciones.
- Creación del plan XP.
- Creación del plan de liberación.
- Preparación de los documentos de cada iteración.
- Creación de la matriz inicial de trazabilidad.
- Preparación de las propuestas de GitHub Issues.
- Actualización del índice documental en `docs/README.md`.
- Revisión conjunta de la documentación elaborada.

### Responsabilidades del driver

Luis Diego Chavala realizó las acciones directas durante la sesión:

- Redacción y edición de los archivos Markdown.
- Organización de las historias de usuario.
- Distribución de las historias entre las iteraciones.
- Creación de los commits.
- Subida de la rama.
- Creación del Pull Request.

### Responsabilidades del navigator

Eithel Herrera Rojas participó como navigator mediante:

- Revisión en tiempo real de las historias.
- Comparación del contenido con el enunciado del docente.
- Verificación de los CRUD y módulos obligatorios.
- Revisión de prioridades, estimaciones y dependencias.
- Validación de los criterios de aceptación.
- Confirmación de que se utilizara terminología de XP.
- Revisión de la distribución de historias entre iteraciones.
- Revisión final del Pull Request.

### Archivos creados o actualizados

- `docs/historias-usuario.md`
- `docs/plan-xp.md`
- `docs/plan-liberacion.md`
- `docs/iteraciones/iteracion-01.md`
- `docs/iteraciones/iteracion-02.md`
- `docs/iteraciones/iteracion-03.md`
- `docs/iteraciones/iteracion-04.md`
- `docs/trazabilidad.md`
- `docs/github-issues-propuestos.md`
- `docs/vision-alcance.md`
- `docs/README.md`
- `docs/bitacora-xp.md`
- `docs/uso-ia.md`

### Pruebas y validaciones realizadas

No se ejecutaron pruebas automatizadas porque esta fase corresponde a planificación y documentación, no a implementación de comportamiento de software.

Se realizaron las siguientes validaciones manuales:

- Revisión de que todas las funcionalidades del enunciado estuvieran representadas.
- Verificación de que las historias tuvieran criterios de aceptación.
- Confirmación de que existieran cuatro iteraciones.
- Revisión de dependencias entre historias.
- Verificación de que no se utilizara terminología de Scrum o Kanban.
- Revisión de enlaces internos y estructura documental.
- Comparación del plan de liberación con el alcance oficial del proyecto.

### Aplicación de TDD

No se aplicó TDD en esta fase porque no se implementaron reglas de negocio ni comportamiento ejecutable.

TDD se aplicará a partir de las fases de dominio e implementación, mediante el ciclo:

1. Escribir una prueba que falle.
2. Implementar el código mínimo.
3. Ejecutar las pruebas.
4. Refactorizar cuando corresponda.

### Refactorizaciones

No se realizaron refactorizaciones de código porque en esta fase no se implementó software.

Sin embargo, se realizaron ajustes documentales y de planificación:

- Reorganización de la estructura documental requerida.
- Ajuste del plan de liberación.
- Redistribución de historias entre las iteraciones.
- Movimiento de la historia HU-11 a la Iteración 3.

Estos cambios corresponden a mejoras de planificación y documentación, no a refactorizaciones de código.

### Velocidad

No se calculó velocidad XP en puntos porque esta fase corresponde a planificación y todavía no se ha ejecutado una iteración funcional.

La sesión tuvo una duración de **40 minutos**.

La velocidad XP comenzará a medirse al cierre de la Iteración 1, comparando los puntos planificados con los puntos realmente completados.

### Retroalimentación

Durante la revisión de la planificación, el navigator propuso mover la historia **HU-11 a la Iteración 3**, debido a su relación con las funcionalidades previstas para esa iteración.

La propuesta fue revisada por ambos integrantes y se actualizó la distribución de historias en:

- `docs/historias-usuario.md`
- `docs/plan-liberacion.md`
- `docs/iteraciones/iteracion-03.md`
- `docs/trazabilidad.md`

El navigator también confirmó que la planificación cubre los principales requisitos funcionales y técnicos del enunciado.

### Pequeña liberación

No se generó una liberación ejecutable en esta fase porque todavía no existe una implementación funcional del sistema.

La primera pequeña liberación está planificada para el cierre de la Iteración 1 y será identificada como:

```text
v0.1.0
```
### Resultado

La Fase 1 quedó completada con:

- Historias de usuario definidas.
- Prioridades y estimaciones iniciales.
- Criterios de aceptación.
- Plan XP.
- Plan de liberación.
- Cuatro iteraciones planificadas.
- Documentación de trazabilidad inicial.
- Propuestas de GitHub Issues.
- Documentación revisada por ambos integrantes.

## Sesión 003 — Fase 2: inicialización técnica del monolito modular

- Fecha: 4 de agosto de 2026.
- Fase: Fase 2 — Inicialización técnica del monolito modular.
- Modalidad: Programación en parejas.
- Driver: Eithel Herrera Rojas.
- Navigator: Luis Diego Chavala.
- Rama: `chore/arquitectura-inicial`.
- Duración: 2 horas.
- Issue: `#3 — FASE-02: Inicialización técnica del monolito modular`.
- Pull Request: `#4 Fase 2: inicialización técnica del monolito modular`.
- Commits:
  - `chore(arquitectura): crear solucion modular inicial`.
  - `ci: configurar compilacion y pruebas iniciales`.
  - `docs(arquitectura): documentar fase 2 y evidencias xp`.

### Objetivo de la sesión

Crear el esqueleto técnico del monolito modular y dejar la solución preparada para implementar historias verticalmente.

### Actividades realizadas

- Creación de la solución.
- Creación de cinco proyectos de producción.
- Creación de tres proyectos de pruebas.
- Configuración de referencias.
- Activación de nullable e implicit usings.
- Configuración común con `Directory.Build.props`.
- Preparación de inyección de dependencias.
- Creación de página MVC mínima.
- Creación de `GET /health`.
- Creación de pruebas técnicas.
- Creación de CI inicial.
- Compilación y ejecución de pruebas.

### Responsabilidades del driver

Eithel Herrera Rojas ejecutó las acciones técnicas, creó y configuró la solución, revisó los archivos generados, ejecutó restauración, compilación y pruebas, y validó el arranque de Web y API.

### Responsabilidades del navigator

Luis Diego Chavala revisó las dependencias entre proyectos, confirmó la independencia de `Domain`, revisó la estructura modular, verificó los resultados de compilación y pruebas, y revisó el diagrama y la documentación de arquitectura.

### Archivos creados o actualizados

- `Licitaciones.sln`
- `Directory.Build.props`
- `src/Licitaciones.Application/DependencyInjection.cs`
- `src/Licitaciones.Infrastructure/DependencyInjection.cs`
- `src/Licitaciones.Web/Program.cs`
- `src/Licitaciones.Web/Views/Home/Index.cshtml`
- `src/Licitaciones.Api/Program.cs`
- `tests/Licitaciones.UnitTests/ArchitectureTests.cs`
- `tests/Licitaciones.IntegrationTests/InfrastructureAssemblyTests.cs`
- `tests/Licitaciones.FunctionalTests/HealthEndpointTests.cs`
- `.github/workflows/ci.yml`
- `docs/arquitectura-general.md`
- `docs/bitacora-xp.md`
- `docs/trazabilidad.md`
- `docs/uso-ia.md`
- `global.json`

### Pruebas y validaciones realizadas

- `dotnet restore Licitaciones.sln`: exitoso.
- `dotnet build Licitaciones.sln --configuration Release --no-restore`: exitoso.
- Resultado: `0` errores y `0` advertencias.
- `dotnet test Licitaciones.sln --configuration Release --no-build`: exitoso.
- `UnitTests`: `1` prueba exitosa.
- `IntegrationTests`: `1` prueba exitosa.
- `FunctionalTests`: `1` prueba exitosa.
- Web inició correctamente.
- API inició correctamente.
- `/health` respondió `200 OK` y `Healthy`.
- Se creó `global.json` para fijar el SDK del proyecto en .NET `9.0.305`.

### Aplicación de TDD

No se aplicó TDD a reglas de negocio porque todavía no se implementaron.

Sí se crearon pruebas técnicas para validar arquitectura y funcionamiento.

El TDD funcional comenzará con las historias de las iteraciones.

### Refactorizaciones

Se realizaron mejoras técnicas reales:

- Centralización de configuración común en `Directory.Build.props`.
- Extracción de registros de dependencias a los métodos `AddApplication` y `AddInfrastructure`.
- Organización de proyectos en `src/` y `tests/`.

No hubo refactorización de lógica de negocio.

### Velocidad

La velocidad XP en puntos comenzará a calcularse al cierre de la Iteración 1.

### Retroalimentación

Durante la revisión conjunta, el navigator confirmó que la estructura del monolito modular respeta la dirección de dependencias definida y que `Licitaciones.Domain` permanece independiente de Infrastructure, Web y API.

También verificó que:

- La solución compila correctamente.
- Las pruebas técnicas pasan.
- La aplicación Web y la API pueden iniciar.
- El endpoint `/health` responde correctamente.
- La organización en `src/` y `tests/` facilita el desarrollo incremental.
- No se implementaron funcionalidades correspondientes a fases posteriores.

Como observación, el navigator recomendó mantener esta misma separación de responsabilidades en las siguientes fases y evitar agregar lógica de negocio directamente en Web, API o Infrastructure.

### Pequeña liberación

No se generó una pequeña liberación funcional porque esta fase solamente prepara la arquitectura.

La primera liberación funcional planificada sigue siendo:

```text
v0.1.0
```

### Resultado

- La solución compila.
- Las pruebas pasan.
- Web y API inician.
- `Domain` permanece independiente.
- El CI inicial quedó preparado localmente y su ejecución remota quedó pendiente del push.
- No se implementó funcionalidad de fases posteriores.
- GitHub Actions ejecutó correctamente restore, build y test.
- El workflow CI finalizó sin errores.

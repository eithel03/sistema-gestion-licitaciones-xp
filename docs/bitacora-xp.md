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

## Sesion 004 - Fase 3: preparacion del dominio y estrategia TDD

- Fecha: 8 de agosto de 2026.
- Fase: Fase 3 - Preparacion del dominio y estrategia TDD.
- Modalidad: Programacion en parejas.
- Driver: Luis Diego Chavala.
- Navigator: Eithel Herrera Rojas.
- Rama: chore/fase-03-dominio-tdd.
- Issue: #7 — FASE-03: Preparación del dominio y estrategia TDD.
- Pull Request: #6 — Fase 3: preparación del dominio y estrategia TDD.
- Commit principal:
  - `2200fe3` — `feat(domain): preparar dominio y estrategia TDD`.
  
### Objetivo de la sesion

Preparar convenciones minimas de dominio y pruebas para iniciar las historias mediante TDD sin adelantar reglas de negocio de iteraciones futuras.

### Actividades realizadas

- Revision de la estructura actual de la solucion.
- Creacion de tipos base para entidades, objetos de valor, excepciones y validacion.
- Creacion de una guarda transversal minima.
- Creacion de la abstraccion `IClock` para tiempo inyectable.
- Creacion de `SystemClock` y registro en `AddInfrastructure`.
- Creacion de pruebas unitarias de ejemplo para el ciclo TDD.
- Actualizacion de documentacion tecnica, pruebas, trazabilidad y uso de IA.

### Responsabilidades del driver

Luis Diego Chavala realiza las acciones directas de implementacion, revisa los cambios y prepara la evidencia de la fase.

### Responsabilidades del navigator

Eithel Herrera Rojas revisa que la base no anticipe reglas futuras y que mantenga la separacion entre `Domain`, `Application` e `Infrastructure`.

### Pruebas y validaciones realizadas

Durante la sesión original, el entorno local del driver no disponía del SDK .NET `9.0.305` requerido por `global.json`.

Posteriormente, el navigator realizó la validación en un entorno compatible:

- `dotnet --version`: `9.0.305`.
- `dotnet restore Licitaciones.sln`: exitoso.
- `dotnet build Licitaciones.sln --configuration Release --no-restore`: exitoso.
- `dotnet test Licitaciones.sln --configuration Release --no-build`: exitoso.
- Total de pruebas ejecutadas: `11`.
- Pruebas aprobadas: `11`.
- Pruebas fallidas: `0`.
- Pruebas omitidas: `0`.
- GitHub Actions ejecutó correctamente el workflow de CI.

### Aplicacion de TDD

Se dejaron ejemplos minimos para guiar ciclos rojo-verde-refactorizacion:

- Igualdad de entidades por identidad.
- Igualdad de objetos de valor por componentes.
- Resultados de validacion exitosos y fallidos.
- Reloj reemplazable en pruebas.

### Restricciones respetadas

No se implementaron reglas de normalizacion de proveedores, estados de licitaciones, ofertas, aprobaciones ni moneda. Esas reglas permanecen asignadas a sus iteraciones.

### Resultado

La Fase 3 quedó preparada y validada correctamente a nivel de código, documentación y pruebas automatizadas. La solución compiló correctamente, las `11` pruebas ejecutadas fueron aprobadas y GitHub Actions confirmó exitosamente la integración continua.

## Sesion 005 - Fase 4: preparacion de persistencia

- Fecha: 9 de agosto de 2026.
- Fase: Fase 4 - Preparacion de persistencia.
- Modalidad: Programacion en parejas.
- Driver: Eithel Herrera Rojas.
- Navigator: Luis Diego Chavala.
- Rama: `chore/preparacion-persistencia`.
- Issue: `#5 - FASE-04: Preparacion de persistencia`.
- Pull Request: Pendiente.
- Commits: Pendiente.

### Objetivo

Configurar la infraestructura minima de PostgreSQL, Entity Framework Core y Testcontainers para permitir que la persistencia crezca de forma incremental durante las iteraciones, sin construir por adelantado tablas ni reglas de historias futuras.

### Actividades realizadas

- Instalacion de paquetes EF Core 9, Npgsql y health checks EF en `Licitaciones.Infrastructure`.
- Instalacion de Testcontainers PostgreSQL en `Licitaciones.IntegrationTests`.
- Creacion de `LicitacionesDbContext` vacio, preparado para configuraciones separadas mediante `IEntityTypeConfiguration`.
- Creacion de fabrica de diseno para comandos de migraciones.
- Registro de `LicitacionesDbContext` con PostgreSQL desde `AddInfrastructure`.
- Conservacion del registro existente de `IClock` y `SystemClock`.
- Configuracion de `ConnectionStrings:DefaultConnection` con placeholders seguros y soporte de variables de entorno.
- Creacion de `compose.yaml` para PostgreSQL 16 local con volumen y health check.
- Creacion de `.env.example` con valores de ejemplo no secretos.
- Preparacion de convenciones reutilizables para `CreatedAt`, `UpdatedAt`, `DeletedAt`, `Version` y precision monetaria explicita `numeric(18,2)`.
- Creacion de prueba de convenciones de persistencia.
- Creacion de prueba tecnica con Testcontainers para levantar PostgreSQL 16 y abrir conexion desde `LicitacionesDbContext`.
- Configuracion opcional de health check PostgreSQL, deshabilitado por defecto y omitido en entorno `Testing`.
- Actualizacion de documentacion de Fase 4.

### Responsabilidades del driver

Eithel Herrera Rojas ejecuta los cambios de infraestructura, configura paquetes, prepara Docker Compose, ejecuta validaciones locales y registra evidencia tecnica.

### Responsabilidades del navigator

Luis Diego Chavala revisa que la fase no adelante entidades ni reglas futuras, verifica la direccion de dependencias y valida que la documentacion mantenga la trazabilidad XP.

### Archivos creados o actualizados

- `src/Licitaciones.Infrastructure/Persistence/LicitacionesDbContext.cs`
- `src/Licitaciones.Infrastructure/Persistence/LicitacionesDbContextFactory.cs`
- `src/Licitaciones.Infrastructure/Persistence/Conventions/PersistencePropertyNames.cs`
- `src/Licitaciones.Infrastructure/Persistence/Conventions/PersistenceModelBuilderExtensions.cs`
- `src/Licitaciones.Infrastructure/Persistence/Conventions/MoneyPropertyBuilderExtensions.cs`
- `tests/Licitaciones.IntegrationTests/Persistence/PersistenceConventionsTests.cs`
- `tests/Licitaciones.IntegrationTests/Persistence/PostgreSqlContainerTests.cs`
- `compose.yaml`
- `.env.example`
- `src/Licitaciones.Infrastructure/Licitaciones.Infrastructure.csproj`
- `src/Licitaciones.Application/Licitaciones.Application.csproj`
- `tests/Licitaciones.IntegrationTests/Licitaciones.IntegrationTests.csproj`
- `src/Licitaciones.Infrastructure/DependencyInjection.cs`
- `src/Licitaciones.Api/Program.cs`
- `src/Licitaciones.Api/appsettings.json`
- `src/Licitaciones.Api/appsettings.Development.json`
- `src/Licitaciones.Web/appsettings.json`
- `src/Licitaciones.Web/appsettings.Development.json`
- `docs/arquitectura-general.md`
- `docs/modelo-datos.md`
- `docs/pruebas.md`
- `docs/trazabilidad.md`
- `docs/uso-ia.md`
- `docs/README.md`

### Pruebas y validaciones realizadas

- `dotnet restore Licitaciones.sln`: exitoso.
- `dotnet build Licitaciones.sln --configuration Release --no-restore`: exitoso, 0 errores y 0 advertencias.
- `dotnet test Licitaciones.sln --configuration Release --no-build`: exitoso.
- Resultado final de pruebas: 13 pruebas aprobadas.
- `docker --version`: Docker `29.6.2`.
- `docker compose version`: Docker Compose `v5.3.1`.
- `docker compose config`: exitoso.
- `docker compose up -d`: exitoso.
- `docker compose ps`: PostgreSQL `postgres:16` alcanzo estado `healthy`.
- `docker compose down`: ejecutado sin eliminar volumenes.

### Aplicacion de TDD

No se forzo TDD en archivos declarativos. Se agregaron pruebas donde habia comportamiento verificable: convenciones del modelo EF Core y conexion real de `LicitacionesDbContext` contra PostgreSQL 16 con Testcontainers. Durante la implementacion se corrigio el fixture de Testcontainers hasta dejar build y suite en verde.

### Decisiones tecnicas

- Se eligio `ConnectionStrings:DefaultConnection` como convencion unica de cadena de conexion.
- La persistencia queda dentro de `Licitaciones.Infrastructure`; `Domain` no recibe dependencias de EF Core.
- No se crea migracion inicial porque el `DbContext` no tiene entidades reales todavia.
- La precision monetaria se aplica mediante `HasMoneyPrecision()` en propiedades explicitamente configuradas, para no afectar decimales que no sean dinero.
- Auditoria y concurrencia quedan preparadas por convenciones de nombres, sin agregar propiedades a entidades inexistentes.
- El health check PostgreSQL queda opt-in para evitar fallos en pruebas funcionales sin base real.

### Restricciones respetadas

No se implementaron CRUD, entidades completas, proveedores, licitaciones, ofertas, niveles de aprobacion, tipos de cambio, reglas de moneda, endpoints funcionales nuevos ni interfaz funcional nueva.

### Resultado actual

La Fase 4 deja EF Core, Npgsql, PostgreSQL 16 local, Testcontainers y convenciones basicas preparados para crecer durante las iteraciones. PR, commits, merge y CI remoto quedan pendientes para ejecucion manual del equipo.

## Iteración 1 — Landing page y proveedores

- Fecha: 11 de agosto de 2026.
- Fase/Iteracion: Iteracion 1 - Landing page y proveedores.
- Modalidad: Programacion en parejas.
- Driver: Chavala.
- Navigator: Eithel.
- Rama: `feature/iteracion-01-landing-proveedores`.
- Pull Request: `#9 - feat: completar Iteración 1 - Landing page y proveedores`.
- Destino del PR: `main`.
- Origen del PR: `feature/iteracion-01-landing-proveedores`.
- Estado del PR: Open / Ready to merge, segun dato proporcionado por el equipo; verificacion remota pendiente porque `gh` no esta disponible en el entorno local.
- Commit principal: `5696a0f` - `feat(proveedores): completar iteracion 1 de landing y gestion de proveedores`.
- Historias trabajadas: HU-01, HU-02, HU-03, HU-04, HU-05, HU-06, HU-07, HU-08, HU-09 y HU-10.
- Ciclos TDD:
  - Dominio de proveedores: ROJO por tipos inexistentes; VERDE con entidad, normalizador y validaciones; REFACTOR para conservar presentacion sin forzar Title Case.
  - Application de proveedores: ROJO por contratos/servicio inexistentes; VERDE con DTO, servicio y repositorio abstracto; REFACTOR de resultado para evitar advertencias.
  - Persistencia: ROJO por repositorio/configuracion faltantes; VERDE con EF Core, indice unico y migracion; REFACTOR generando migracion real con `dotnet ef`.
  - Funcionales API/MVC: ROJO por configuracion de conexion de pruebas; VERDE con Testcontainers inyectado en `WebApplicationFactory`.
- Pruebas:
  - `dotnet restore Licitaciones.sln --force`: exitoso.
  - `dotnet build Licitaciones.sln --no-restore`: exitoso, 0 errores y 0 advertencias.
  - `dotnet test tests/Licitaciones.UnitTests/Licitaciones.UnitTests.csproj --no-restore`: 34 pruebas aprobadas.
  - `dotnet test tests/Licitaciones.IntegrationTests/Licitaciones.IntegrationTests.csproj --no-restore`: 9 pruebas aprobadas.
  - `dotnet test tests/Licitaciones.FunctionalTests/Licitaciones.FunctionalTests.csproj --no-restore`: 6 pruebas aprobadas.
- Refactorizaciones:
  - Separacion de reglas en dominio, servicio en Application y repositorio EF en Infrastructure.
  - Uso de borrado logico para mantener historial.
  - Generacion de migracion EF real despues de detectar diferencia en snapshot manual.
- Decisiones tecnicas:
  - `Proveedor` queda en `Licitaciones.Domain.Proveedores` y mantiene reglas de nombre, normalizacion, auditoria y borrado logico.
  - `ProveedorService` concentra los casos de uso y traduce validaciones a resultados de aplicacion.
  - `ProveedorRepository` usa EF Core, filtro global para excluir retirados e indice unico sobre `NombreNormalizado`.
  - MVC y API consumen `IProveedorService`; no acceden directamente a EF Core.
- Resultado: implementacion funcional de landing y gestion de proveedores. La aplicacion permite administrar proveedores mediante MVC y API, utilizando persistencia en PostgreSQL y las validaciones asociadas al modulo.
- Retroalimentacion: pendiente de registrar revision real del navigator.
- Velocidad: 30 puntos implementados en la rama de Iteracion 1, pendientes de cierre formal despues de integracion a `main`.

## Iteracion 2 - Licitaciones y persistencia base

- Driver: Eithel.
- Navigator: Chavala.
- Rama: feature/iteracion-02-licitaciones.
- Historias: HU-12 a HU-19.
- Puntos: 36.
- Versión prevista: `v0.2.0`; tag pendiente.

### Objetivo

Implementar gestion de licitaciones con reglas de codigo, presupuesto, fecha, estados, persistencia, API, auditoria y concurrencia.

### Actividades realizadas

- Entidad y reglas de licitacion en Domain.
- Casos de uso y contratos en Application.
- Repositorio y configuracion EF Core en Infrastructure.
- Migracion real 20260812002104_CreateLicitaciones.
- CRUD MVC y acciones de publicar/cerrar.
- API REST bajo /api/v1/licitaciones.
- Pruebas unitarias, integracion con PostgreSQL/Testcontainers y funcionales.
- Validacion manual del flujo MVC de licitaciones.

### TDD

#### Dominio

- ROJO: tipos y reglas de licitacion aun inexistentes.
- VERDE: entidad, normalizador, validaciones, estados, publicacion, cierre y vencimiento.
- REFACTOR: reglas mantenidas en Domain y separacion de Application.

#### Persistencia

- ROJO: faltaba persistencia del modulo.
- VERDE: repositorio, configuracion EF, tabla, migracion e indices.
- REFACTOR: EF Core permanece en Infrastructure y Domain no depende de persistencia.

#### Concurrencia

- ROJO: dos actualizaciones con versiones obsoletas no tenian deteccion comprobable.
- VERDE: concurrencia optimista mediante PostgreSQL xmin y prueba de integracion.
- REFACTOR: traduccion de conflicto a resultado controlado de Application/API.

### Pruebas

- dotnet restore Licitaciones.sln.
- dotnet build Licitaciones.sln --configuration Release --no-restore.
- dotnet test Licitaciones.sln --configuration Release --no-build.
- Resultado: 64 ejecutadas, 64 aprobadas, 0 fallidas, 0 omitidas.

### Validacion manual

Flujo validado: crear, listar, detalle, editar, publicar, transicion invalida rechazada y cerrar.

### Retroalimentacion del navigator

El navigator propuso corregir la coherencia del formato decimal de PresupuestoCrc con cultura Costa Rica y simplificar las acciones de interfaz segun estado. Ambas observaciones fueron aplicadas.

### Refactorizaciones y ajustes

- Cultura es-CR y ajuste de presupuesto decimal en MVC.
- Compatibilidad de validacion cliente del codigo.
- Botones de acciones visibles segun estado.
- Separacion Domain/Application/Infrastructure.
- Uso de IClock para reglas temporales.
- Concurrencia controlada con xmin.
- Los ajustes visuales no reemplazan validaciones del dominio.

### Velocidad

36 puntos implementados, validados e integrados a `main` en la Iteración 2.

### Cierre formal

- Issue: `#10`.
- Commits asociados: `cce95ad`, `812b59c`, `ed89c5a`, `c77343b`.
- Pull Request: `#12 - feat: completar Iteración 2 - Licitaciones y persistencia base`.
- Pruebas: 64/64 aprobadas, 0 fallidas y 0 omitidas.
- Merge a `main`: realizado.
- Commit de merge/base resultante: `fafcc66`.
- CI remoto: Pendiente de evidencia confirmada.
- Tag `v0.2.0`: Pendiente.

## Iteracion 3 - Ofertas, mejor oferta y aprobaciones

- Modalidad: programacion en pareja mediante Extreme Programming.
- Driver principal: Chavala.
- Navigator principal: Eithel.
- Rama: `feature/iteracion-03-ofertas-aprobacion`.
- Base: `fafcc66`, integracion de Iteracion 2 en `main`.
- Historias: HU-11 y HU-20 a HU-29. Version prevista: `v0.3.0`.
- Responsabilidad del Driver: ejecucion asistida de la implementacion, revision de cambios, comandos y pruebas, preparacion de commits, evidencias y Pull Request.
- Responsabilidad del Navigator: revision de reglas, criterios de aceptacion, TDD, defectos y resultados.

### Ciclos TDD

- Oferta/evaluacion: ROJO por tipos y reglas inexistentes; VERDE con monto, estado efectivo, vencimiento, mutaciones, mejor oferta y clasificacion; REFACTOR hacia `EvaluadorOfertas` y orden estable por monto, fecha e `Id`.
- Casos de uso: ROJO por contratos, repositorio y servicio inexistentes; VERDE con CRUD, filtros, duplicidad, relaciones y reloj; REFACTOR con resultados controlados y consumidores delgados.
- Aprobaciones: ROJO por entidad y servicio inexistentes; VERDE con limites, traslapes, rango abierto, CRUD y busqueda; REFACTOR separando comparacion de Domain y consultas persistidas.
- Persistencia/API/MVC: ROJO por tablas, rutas y vistas ausentes; VERDE con configuraciones, repositorios, migracion, endpoints y vistas; REFACTOR con restricciones PostgreSQL, aislamiento funcional y entrada decimal `es-CR`.

Estos ciclos se respaldan con las pruebas y ejecuciones registradas. Los commits se organizaron posteriormente por responsabilidad tecnica; no se atribuye un commit separado a cada paso ROJO.

### Decisiones y refactorizaciones

- Reutilizar `Licitacion.GetEstadoEfectivo(IClock.UtcNow)` para no duplicar reglas de estado y vencimiento.
- Usar `decimal` y `numeric(18,2)` para dinero.
- Resolver empates por menor monto, fecha mas temprana y finalmente `Id` como criterio determinista.
- Resolver el aprobador desde rangos persistidos, sin una cadena fija de cargos.
- Mantener Domain libre de EF Core, MVC y API, y traducir errores mediante Application.
- Reforzar integridad con FKs restrictivas, indice unico de oferta, checks, un rango abierto, exclusion GiST de traslapes y concurrencia `xmin`.

### Pruebas y resultado

- Build: correcto, 0 errores y 0 advertencias.
- UnitTests: 76 ejecutadas, 76 aprobadas, 0 fallidas, 0 omitidas.
- IntegrationTests: 22 ejecutadas, 22 aprobadas, 0 fallidas, 0 omitidas; PostgreSQL 16 mediante Testcontainers.
- FunctionalTests: 13 ejecutadas, 13 aprobadas, 0 fallidas, 0 omitidas.
- Total: 111/111 aprobadas.
- Prueba manual API: duplicidad 409, presupuesto excedido 400, recuperacion de ofertas, mejor oferta, 20 % de ahorro, clasificacion conveniente, aprobador persistido/editado, traslape 409 y eliminacion 204.
- Limitacion manual MVC: DPAPI, Event Log y certificado HTTPS del host impidieron sostener el proceso independiente; los flujos MVC automatizados mediante `WebApplicationFactory` fueron satisfactorios.

### Commits

- `d6d6009` - `feat(ofertas): implementar reglas y evaluacion de ofertas`.
- `7e6a317` - `feat(ofertas): implementar casos de uso de ofertas`.
- `a20eb19` - `feat(aprobacion): implementar niveles y validacion de rangos`.
- `29e727c` - `feat(persistencia): agregar ofertas y niveles de aprobacion`.
- `37bcb55` - `feat(api): exponer ofertas y niveles de aprobacion`.
- `4faaf83` - `feat(web): agregar gestion MVC de ofertas y aprobacion`.
- `437cc37` - `docs(xp): documentar resultados de la iteracion 3`.
- `d349ccc` - commit asociado al cierre de la Iteración 3.

### Resultado y cierre formal

Iteración 3 implementada, validada e integrada a `main`. Velocidad técnica: 38 puntos.

- Issue: Pendiente.
- Pull Request: `#14`.
- Merge a `main`: realizado mediante `fe5317c`.
- CI remoto: Pendiente de evidencia confirmada.
- Revisión formal final del Navigator: Pendiente.
- Tag `v0.3.0`: Pendiente.
- Retroalimentación de cierre: Pendiente.

## Iteración 4 - Moneda, UX y consolidación técnica

- Modalidad: programación en pareja mediante Extreme Programming.
- Driver principal: Eithel.
- Navigator principal: Chavala.
- Rama: `feature/iteracion-04-moneda-ux`.
- Historias: HU-30 a HU-37.
- Puntos planificados: 32.
- Versión candidata prevista: `v1.0.0-rc`.
- Duración: Pendiente de completar por el equipo.

### Objetivo

Completar la administración local de tipos de cambio, la presentación CRC/USD, las preferencias visuales, el endurecimiento de API, la suite automatizada y la infraestructura reproducible, cerrando la trazabilidad documental sin inventar evidencia Git/GitHub.

### Actividades realizadas

- Implementación y validación del CRUD de tipos de cambio.
- Activación de un único registro con respaldo de PostgreSQL.
- Conversión visual CRC/USD sin modificar montos CRC persistidos.
- Tema claro y oscuro con preferencia persistida.
- Swagger UI interactivo, OpenAPI v1, ProblemDetails y correlación.
- Endpoint oficial `PATCH /api/v1/tipos-cambio/{id}/activar`.
- Endpoint `PATCH /api/v1/licitaciones/{id}/estado` con reglas de dominio.
- Consolidación de pruebas, cobertura, Docker y manifiestos Kubernetes.
- Actualización de documentación, evidencia, trazabilidad y uso de IA.

### Ciclos TDD

- Tipos de cambio: ROJO por entidad y servicios ausentes; VERDE con validaciones, CRUD, activación y conversión; REFACTOR con contratos reutilizables.
- Persistencia: ROJO por modelo y migración ausentes; VERDE con EF Core, repositorio e índices; REFACTOR con activación consistente y garantía parcial única.
- Fechas duplicadas: ROJO por índice único; VERDE con `20260814014136_AllowDuplicateTipoCambioDates`; REFACTOR dejando `IX_TiposCambio_Fecha` normal.
- MVC monetario: ROJO para coma decimal y acción redundante; VERDE con entrada decimal flexible y visibilidad condicional; REFACTOR de normalización.
- Proveedores: ROJO por regresión cliente con Unicode; VERDE con patrón compatible; REFACTOR conservando la regla de dominio existente.
- API: ROJO por verbo de activación, Swagger UI, contrato OpenAPI y correlación incompletos; VERDE con PATCH, UI interactiva y ProblemDetails estándar; REFACTOR centralizado.
- Licitaciones: ROJO por ausencia del PATCH de estado; VERDE reutilizando transiciones existentes; REFACTOR manteniendo `publish` y `close` por compatibilidad.

### Decisiones técnicas

- CRC permanece como fuente de verdad y USD se calcula con `USD = CRC / CrcPorUsd`.
- Se permiten varias tasas con la misma fecha; solo la activación es única.
- `IX_TiposCambio_UnicoActivo` permanece como índice parcial único.
- Swagger documenta exclusivamente los métodos HTTP reales.
- ProblemDetails se produce de forma común y segura para los módulos API.
- PostgreSQL 16 real se conserva en pruebas mediante Testcontainers.
- Docker usa el puerto host 55432 para PostgreSQL y conserva el volumen.
- Kubernetes se mantuvo básico, reproducible y sin afirmar un despliegue no realizado.

### Defectos y ajustes encontrados

- Se eliminó la unicidad accidental de fecha mediante una migración real.
- Se corrigió la entrada MVC para aceptar punto y coma decimal.
- Se ocultó el botón Activar cuando el registro ya está activo.
- Se corrigió la validación cliente de nombres Unicode de proveedores.
- Se cambió la activación API de POST a PATCH.
- Se agregó `correlationId` al cuerpo ProblemDetails y se ajustó `application/problem+json`.
- Se agregó Swagger UI y se eliminaron verbos falsos del documento.
- Se agregó el PATCH oficial de estado de licitación.
- Un intento fallido del PATCH con cuerpo mal escapado se identificó como quoting de PowerShell, no como defecto de API.

### Pruebas y cobertura

- Build Release: 0 errores y 0 advertencias.
- UnitTests: 96/96.
- IntegrationTests: 27/27 con PostgreSQL real.
- FunctionalTests: 51/51.
- Total: 174/174, 0 fallidas y 0 omitidas.
- Cobertura limpia: `Parser: MultiReport (3x Cobertura)`.
- Líneas: global 87.3%, Domain 91.4%, Application 83.8%, API 88.4%, Infrastructure 95.1% y Web 61.6%.
- Branch coverage: 59%.
- Method coverage: 84%.
- Umbrales global, Domain y Application: cumplidos.

Los mensajes de conexión posteriores a `DROP DATABASE ... WITH (FORCE)` corresponden al reinicio intencional de bases temporales de Testcontainers y no son fallos de prueba.

### Docker y Kubernetes

- Docker Compose: configuración, build, arranque, health checks, Web 8080, API 8081 y PostgreSQL 55432 validados.
- Persistencia Docker: proveedores, licitaciones, ofertas y tipos de cambio permanecieron después de reiniciar contenedores.
- `docker compose down -v`: no ejecutado.
- `kubectl kustomize k8s`: exitoso.
- El dry-run no completó la consulta porque el API server local no estaba disponible en `kubernetes.docker.internal:6443`.
- Despliegue, pods, logs y persistencia real en Kubernetes: Pendientes.

### Resultado

32 puntos implementados y validados localmente. El sistema queda técnicamente preparado para la versión candidata prevista `v1.0.0-rc`, sujeta al cierre formal del flujo Git/GitHub.

### Evidencia Git/GitHub

- Issue: `#15 - ITER-04: Moneda, UX y consolidación técnica`.
- Pull Request: `#16`.
- Rama origen: `feature/iteracion-04-moneda-ux`.
- Rama destino: `main`.
- La rama fue publicada mediante `git push -u origin feature/iteracion-04-moneda-ux` y quedó vinculada a `origin/feature/iteracion-04-moneda-ux`.

### Commits

- `40c4f5d` - `feat(moneda): implementar tipos de cambio y persistencia`.
- `38c5bf5` - `feat(api): consolidar contratos y manejo de errores`.
- `5cba6c2` - `feat(web): agregar moneda visual y preferencias de interfaz`.
- `9b0fa75` - `test(iteracion-04): consolidar pruebas y cobertura`.
- `8103b12` - `chore(deploy): preparar Docker y Kubernetes`.
- `2e710d2` - `docs(xp): documentar cierre tecnico de iteracion 4`.

El workflow de CI está preparado dentro de `9b0fa75`, pero su ejecución remota para el Pull Request permanece pendiente de evidencia confirmada. Esta actualización documental posterior todavía no tiene commit.

### Pendientes formales

- CI remoto del Pull Request: Pendiente de evidencia confirmada.
- Revisión formal final del Navigator: Pendiente.
- Retroalimentación de cierre: Pendiente.
- Merge a `main`: Pendiente.
- Tag `v1.0.0-rc`: Pendiente.
- GitHub Release: Pendiente.

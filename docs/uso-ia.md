# Registro del uso de inteligencia artificial

## Información general

Las herramientas de inteligencia artificial se utilizan como apoyo para analizar el enunciado, organizar el trabajo, aclarar conceptos técnicos y preparar propuestas que posteriormente son revisadas y validadas por los integrantes.

## Registro de uso

### Registro 001

- Fecha: 2 de agosto de 2026.
- Herramienta: ChatGPT.
- Participantes: Eithel Herrera Rojas y Luis Diego Chavala.
- Finalidad: análisis del enunciado y elaboración del plan de implementación.
- Resultado: se definió una arquitectura de monolito modular, cuatro iteraciones XP y una rotación de los roles de driver y navigator.
- Validación realizada: Revisamos la propuesta y la comparamos con el enunciado proporcionado por el docente.
- Código generado: ninguno en esta etapa.

### Registro 002

- Fecha: 3 de agosto de 2026.
- Herramienta: Codex.
- Participantes: Luis Diego Chavala y Eithel Herrera Rojas.
- Finalidad: apoyar la creación inicial de historias de usuario, planes de iteración y trazabilidad, ademas de algunos .md más.
- Archivos asistidos: `historias-usuario.md`, `plan-xp.md`, `plan-liberacion.md` e iteraciones.
- Validación realizada: ambos comparamos los documentos con el enunciado oficial y se corrigieron la distribución de historias.
- Código generado: ninguno; únicamente documentación.

### Registro 003

- Fecha: 4 de agosto de 2026.
- Herramienta: Codex.
- Participantes: Eithel Herrera Rojas y Luis Diego Chavala.
- Finalidad: apoyo en la creación del esqueleto técnico de la Fase 2.
- Componentes asistidos: solución y proyectos; referencias; configuración común; inyección de dependencias; página MVC mínima; endpoint `/health`; pruebas técnicas; GitHub Actions.
- Código generado: estructura técnica, configuración inicial, pruebas y workflow de CI.
- Validaciones realizadas: revisión del código; `restore`; `build`; `test`; arranque de Web; arranque de API; prueba de `/health`.
- Intervención humana: el driver autorizó comandos técnicos, revisó los cambios y comprobó los resultados,ademas se realizó ajuste en la documentación tanto en la bitácora y en la del uso-ia; el navigator realizo la revisión conjunta.
- Limitaciones: Codex no realizó commits, push, merge ni Pull Request y no implementó reglas de negocio.
- Resultado: propuesta técnica funcional pendiente de integración manual al repositorio.

### Registro 004

- Fecha: 8 de agosto de 2026.
- Herramienta: Codex.
- Participantes: Luis Diego Chavala y Eithel Herrera Rojas.
- Finalidad: apoyo para revisar el proyecto y preparar la Fase 3 de dominio y estrategia TDD.
- Componentes asistidos: convenciones de dominio, resultado de validacion, excepcion de dominio, reloj inyectable, pruebas unitarias de ejemplo y documentacion TDD.
- Codigo generado: base minima en `Domain`, abstraccion `IClock` en `Application`, implementacion `SystemClock` en `Infrastructure` y pruebas preparatorias.
- Validaciones realizadas: revisión de estructura, validación posterior con SDK .NET `9.0.305`, restore, build, ejecución de `11` pruebas automatizadas y validación mediante GitHub Actions.
- Limitaciones: durante la sesión original el entorno del driver no disponía del SDK .NET `9.0.305`; posteriormente la validación fue completada por el navigator en un entorno compatible.
- Resultado: Fase 3 preparada y validada correctamente, con `11` pruebas aprobadas y sin implementar reglas de negocio correspondientes a iteraciones futuras.

### Registro 005

- Fecha: 9 de agosto de 2026.
- Herramienta: Codex.
- Participantes: Eithel Herrera Rojas y Luis Diego Chavala.
- Finalidad: apoyar la preparacion de persistencia de la Fase 4 sin adelantar funcionalidades de iteraciones futuras.
- Componentes asistidos: EF Core, Npgsql, `LicitacionesDbContext`, registro DI, Docker Compose PostgreSQL 16, Testcontainers, convenciones de persistencia, health check opcional y documentacion XP.
- Codigo generado: infraestructura de persistencia en `Licitaciones.Infrastructure`, pruebas de integracion, `compose.yaml`, `.env.example` y actualizaciones de configuracion segura.
- Validaciones realizadas: `dotnet restore`, `dotnet build`, `dotnet test`, verificacion de Docker, `docker compose config`, `docker compose up -d`, health check saludable y `docker compose down`.
- Intervencion humana: el equipo define alcance, revisa decisiones, conserva PR/commits pendientes para ejecucion manual y valida que no se adelanten entidades ni historias futuras.
- Limitaciones: no se realizaron commits, push, merge ni Pull Request; no se creo migracion inicial porque no hay modelo persistente real; CI remoto queda pendiente.
- Resultado: Fase 4 preparada y validada localmente con 13 pruebas aprobadas y PostgreSQL 16 saludable en Docker Compose.

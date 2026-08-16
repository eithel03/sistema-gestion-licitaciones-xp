# Registro del uso de inteligencia artificial

## Información general

Las herramientas de inteligencia artificial se utilizan como apoyo para analizar el enunciado, organizar el trabajo, aclarar conceptos técnicos y preparar propuestas que posteriormente son revisadas y validadas por los integrantes.

## Registro 008 - Fase 5

- Fecha: 16 de agosto de 2026.
- Herramienta: OpenAI Codex.
- Participantes responsables: Chavala como Driver y Eithel como Navigator.
- Rama: `chore/fase-05-pruebas-cobertura`.
- Evidencia de integración previa: Iteración 4 fue integrada en `main` mediante `ea9772f`; el merge pendiente referido en registros anteriores corresponde al estado documental previo a esa integración.
- Finalidad: auditoría previa, identificación de brechas, generación y revisión de pruebas unitarias, análisis de concurrencia PostgreSQL, pruebas funcionales HTTP/MVC, suite Playwright y cobertura reproducible.
- Componentes asistidos: `LicitacionService`, `TipoCambio`, `TipoCambioService`, dominio de licitaciones, `EvaluadorOfertas`, repositorio de tipos de cambio, pruebas Testcontainers, pruebas funcionales, Playwright, ReportGenerator y verificador de umbrales.
- Validación humana: revisión de cambios; ejecución de UnitTests 121/121, IntegrationTests 37/37, FunctionalTests 54/54 y E2ETests 6/6; revisión de los ROJOS reales de transacción y paginación; revisión del arreglo transaccional; revisión de la suite E2E y de la cobertura combinada.
- Resultado: Fase 5 validada localmente con 218/218 pruebas y cobertura de líneas Domain 91,64 %, Application 88,60 % y global 89,37 %.
- Responsabilidad: Codex se utilizó como herramienta de asistencia y no constituye un tercer integrante. Driver y Navigator mantienen la responsabilidad final de comprender, revisar, validar y defender el trabajo.
- Limitaciones: revisión formal del Navigator, Pull Request, CI remoto y merge posterior permanecen pendientes.

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

### Registro 006 - Iteracion 3

- Fecha: 12 de agosto de 2026.
- Herramienta: OpenAI Codex.
- Participantes responsables: Chavala como Driver principal y Eithel como Navigator principal.
- Finalidad: apoyo en analisis del repositorio, asistencia en la implementacion de Iteracion 3, generacion y revision de pruebas, documentacion e identificacion de problemas.
- Modulos asistidos: Ofertas, Niveles de aprobacion, persistencia, API, MVC, pruebas y documentacion.
- Validaciones realizadas por los estudiantes: revision del codigo; build manual; 76 pruebas unitarias; 22 pruebas de integracion; 13 pruebas funcionales; revision de la migracion; revision de `EvaluadorOfertas`; revision de restricciones PostgreSQL.
- Resultado: Iteracion 3 tecnicamente implementada y validada localmente con 111/111 pruebas aprobadas.
- Responsabilidad: Codex se utilizo como herramienta de asistencia y no constituye un tercer integrante. Driver y Navigator mantienen la responsabilidad del trabajo y deben comprender, revisar y defender el codigo.
- Limitaciones de evidencia: revision formal del Navigator, Pull Request, CI remoto, merge y tag permanecen pendientes.

### Registro 007 - Iteración 4

- Fecha: Pendiente de completar por el equipo.
- Herramienta: OpenAI Codex.
- Participantes responsables: Eithel como Driver y Chavala como Navigator.
- Finalidad: asistencia en la implementación y consolidación de tipos de cambio, presentación monetaria, UX, API, pruebas, infraestructura y documentación de Iteración 4.
- Componentes asistidos: tipos de cambio, conversión CRC/USD, tema claro/oscuro, Swagger/OpenAPI, ProblemDetails, correlación, pruebas automatizadas, cobertura, Docker, Kubernetes, corrección de defectos y documentación.
- Correcciones asistidas: fecha duplicada de tipo de cambio, entrada decimal con punto o coma, acción Activar redundante, validación cliente Unicode de proveedores, activación API por PATCH, verbos OpenAPI exactos y PATCH de estado de licitación.
- Validaciones realizadas por los estudiantes: revisión del código; suite automatizada 174/174; pruebas manuales de CRUD, moneda, tema, Swagger y API; cobertura limpia; build; Docker, health checks y persistencia; renderizado de manifiestos Kubernetes.
- Resultado: Iteración 4 implementada y validada localmente, con cobertura global de líneas de 87.3% y Docker operativo. El despliegue real en Kubernetes y el cierre Git/GitHub permanecen pendientes.
- Responsabilidad: Codex se utilizó como herramienta de asistencia y no constituye un tercer integrante. Driver y Navigator mantienen la responsabilidad final de comprender, revisar, validar y defender el trabajo.
- Evidencia Git/GitHub: Issue `#15`, Pull Request `#16` y commits `40c4f5d`, `38c5bf5`, `5cba6c2`, `9b0fa75`, `8103b12` y `2e710d2` registrados. El merge de Iteración 4 a `main` se realizó mediante `ea9772f`; CI remoto, revisión formal final del Navigator, tag `v1.0.0-rc` y GitHub Release permanecen pendientes.

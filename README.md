# Sistema de Gestión de Licitaciones

Proyecto académico del curso ITI-822 — Metodologías Ágiles de Desarrollo de Software de la Universidad Técnica Nacional. El equipo aplica exclusivamente prácticas de Extreme Programming (XP).

## Objetivo

Administrar proveedores, licitaciones, ofertas económicas, niveles de aprobación y tipos de cambio CRC/USD mediante una interfaz ASP.NET Core MVC, una API REST y persistencia PostgreSQL.

## Estado actual

La línea base documental de la Fase 9 es `main@36e89ec`, posterior a la integración de la Fase 8 mediante el Pull Request `#24`.

El código actual implementa:

- landing y navegación adaptable;
- CRUD de proveedores, licitaciones, ofertas, niveles de aprobación y tipos de cambio;
- reglas de unicidad, estados, vencimiento, presupuesto, mejor oferta, clasificación y rangos de aprobación;
- presentación monetaria CRC/USD y tema claro/oscuro persistidos en cookies;
- API bajo `/api/v1`, ProblemDetails, correlación y Swagger UI;
- persistencia EF Core con PostgreSQL y seis migraciones;
- pruebas unitarias, de integración, funcionales y E2E;
- Docker Compose, manifiestos Kubernetes y GitHub Actions.

La aplicación no tiene un tag Git ni una GitHub Release oficial. `v1.0.0-rc` es el nombre usado por las imágenes Docker y una versión prevista en la planificación; no constituye una versión Git publicada. La landing todavía muestra `v0.1.0`, inconsistencia visual registrada como trabajo técnico futuro.

## Tecnologías

- .NET 9 y ASP.NET Core 9.
- ASP.NET Core MVC y Minimal API.
- Entity Framework Core 9 y Npgsql.
- PostgreSQL 16.
- xUnit, Testcontainers y Playwright.
- Docker y Docker Compose.
- Kubernetes y Kustomize.
- GitHub Actions.

## Arquitectura

La solución modular comparte Domain, Application, Infrastructure y PostgreSQL, pero tiene dos hosts ejecutables independientes:

- `Licitaciones.Web`: interfaz MVC.
- `Licitaciones.Api`: API REST.

Ambos hosts consumen las mismas reglas y servicios y acceden a la misma persistencia. La solución también contiene cuatro proyectos de pruebas. La descripción completa está en [arquitectura-general.md](docs/arquitectura-general.md).

## Módulos

- [Proveedores](docs/modulos/proveedores.md)
- [Licitaciones](docs/modulos/licitaciones.md)
- [Ofertas](docs/modulos/ofertas.md)
- [Niveles de aprobación](docs/modulos/niveles-aprobacion.md)
- [Tipo de cambio](docs/modulos/tipo-cambio.md)
- [Interfaz Web](docs/modulos/interfaz-web.md)
- [API REST](docs/modulos/api-rest.md)
- [Persistencia](docs/modulos/persistencia.md)

## Pruebas y evidencia

El repositorio contiene pruebas unitarias, de integración PostgreSQL, funcionales MVC/API y E2E con Playwright. La auditoría de Fase 9 identificó 170 declaraciones `[Fact]`/`[Theory]`; una teoría puede producir varios casos.

La evidencia histórica anterior a Fase 9 registra 218/218 casos aprobados y cobertura de líneas de 91,64 % para Domain, 88,60 % para Application y 89,37 % global. Esos resultados no fueron regenerados durante Fase 9. Véase [pruebas.md](docs/pruebas.md).

## Infraestructura

- [Docker](docs/docker.md): PostgreSQL, Web y API con volumen persistente.
- [Kubernetes](docs/kubernetes.md): Namespace, Deployments, StatefulSet, Services, PVC, probes y recursos.
- [GitHub Actions](docs/pruebas.md#integración-continua): build, pruebas, cobertura, validaciones Docker y renderizado Kustomize, con las limitaciones documentadas.

## Documentación

El índice canónico está en [docs/README.md](docs/README.md). La carpeta `/docs` separa el estado actual de la evidencia histórica de fases e iteraciones XP.

## Integrantes

- Eithel Herrera Rojas.
- Luis Diego Chavala.

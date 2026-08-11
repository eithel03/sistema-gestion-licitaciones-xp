# Arquitectura general

## Propósito

Se eligió un monolito modular para mantener una sola solución desplegable, pero con separación clara de responsabilidades, límites de dependencia y una base ordenada para crecer por iteraciones sin mezclar capas técnicas con reglas de negocio.

## Estructura real

```text
Licitaciones.sln

src/
 Licitaciones.Domain/
 Licitaciones.Application/
 Licitaciones.Infrastructure/
 Licitaciones.Web/
 Licitaciones.Api/

tests/
 Licitaciones.UnitTests/
 Licitaciones.IntegrationTests/
 Licitaciones.FunctionalTests/
```

## Responsabilidad de los proyectos

- `Licitaciones.Domain`: reglas y conceptos centrales del dominio. No depende de otros proyectos de la solución.
- `Licitaciones.Application`: casos de uso, contratos y coordinación de operaciones. Depende de `Domain`.
- `Licitaciones.Infrastructure`: implementaciones técnicas futuras, persistencia e integración externa. Depende de `Application` y `Domain`.
- `Licitaciones.Web`: interfaz MVC. Depende de `Application` e `Infrastructure`.
- `Licitaciones.Api`: API REST. Depende de `Application` e `Infrastructure`.
- `Licitaciones.UnitTests`: pruebas unitarias y validaciones arquitectónicas.
- `Licitaciones.IntegrationTests`: pruebas técnicas y futuras pruebas con infraestructura real.
- `Licitaciones.FunctionalTests`: pruebas funcionales mediante el arranque real de la API con `WebApplicationFactory`.

La base tecnica inicial fue ampliada en la Iteracion 1 con el modulo de proveedores: entidad de dominio, casos de uso de aplicacion, repositorio EF Core, persistencia PostgreSQL, API REST y MVC.

## Dirección de dependencias

- `Application -> Domain`
- `Infrastructure -> Application, Domain`
- `Web -> Application, Infrastructure`
- `Api -> Application, Infrastructure`
- `UnitTests -> Domain, Application`
- `IntegrationTests -> Infrastructure`
- `FunctionalTests -> Api` mediante `WebApplicationFactory`

Estas dependencias mantienen a `Domain` independiente de `Infrastructure`, ASP.NET Core, Entity Framework Core, Web y API.

## Inyección de dependencias

Se prepararon métodos de extensión mínimos para la composición futura:

```csharp
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

Desde la Iteracion 1 registran servicios y repositorios de proveedores sin acoplar `Domain` a infraestructura.

## Health check

La API expone `GET /health` usando los health checks nativos de ASP.NET Core.

El endpoint valida la disponibilidad basica de la API y responde con `200 OK` y el estado `Healthy`. Desde la Fase 4 existe un health check PostgreSQL opcional mediante `HealthChecks:PostgreSQL:Enabled`; permanece deshabilitado por defecto y no se registra en entorno `Testing` para no acoplar las pruebas funcionales a una base real.

## Configuración común

La solución quedó configurada con:

- `TargetFramework` `net9.0`.
- `Nullable` habilitado.
- `ImplicitUsings` habilitado.
- Configuración común centralizada en `Directory.Build.props`.
- Reglas base de formato y análisis mantenidas en `.editorconfig`.

## Pruebas técnicas iniciales

Se agregaron tres validaciones técnicas mínimas:

- Prueba arquitectónica de independencia de `Domain`.
- Smoke test del ensamblado de `Infrastructure`.
- Prueba funcional de `GET /health`, que espera `200 OK` y contenido `Healthy`.

## Base de dominio de Fase 3

Se agregaron convenciones minimas para iniciar el desarrollo por TDD:

- `Entity<TId>` para entidades con identidad.
- `ValueObject` para objetos de valor con igualdad por componentes.
- `DomainException` para violaciones de invariantes del dominio.
- `ValidationError` y `ValidationResult` para validaciones acumulables.
- `Guard` para validaciones transversales pequenas.
- `IClock` en `Application` y `SystemClock` en `Infrastructure` para controlar dependencias de tiempo.

Estas piezas sirvieron como base para implementar las reglas de proveedores en la Iteracion 1. Las reglas de licitaciones, ofertas, aprobaciones y moneda quedan diferidas a sus iteraciones.


## Persistencia preparada en Fase 4

La persistencia inicial vive en `Licitaciones.Infrastructure` y se compone de:

- `LicitacionesDbContext`, basado en EF Core 9 y preparado para crecer mediante configuraciones separadas de entidades.
- Provider `Npgsql.EntityFrameworkCore.PostgreSQL` para PostgreSQL 16.
- Registro del contexto desde `AddInfrastructure(builder.Configuration)` usando `ConnectionStrings:DefaultConnection`.
- Fabrica de diseno `LicitacionesDbContextFactory` para comandos de migraciones versionadas en Infrastructure.
- Convenciones reutilizables para propiedades `CreatedAt`, `UpdatedAt`, `DeletedAt`, `Version` y precision monetaria explicita con `HasMoneyPrecision()`.
- Pruebas de integracion con Testcontainers para levantar PostgreSQL 16 temporalmente y abrir conexion desde el `DbContext`.

Desde la Iteracion 1 existe `DbSet<Proveedor>`, configuracion Fluent API de proveedores y la migracion real `20260810092133_CreateProveedores`. Todavia no existen `DbSet` ni configuraciones de licitaciones, ofertas, niveles de aprobacion o tipos de cambio.
## Integración continua

El archivo `.github/workflows/ci.yml` ejecuta:

- Checkout del repositorio.
- Configuración de .NET 9.
- Restore.
- Build en `Release`.
- Test.

El workflow de GitHub Actions fue ejecutado correctamente y las validaciones de restore, build y test finalizaron exitosamente.

## Diagrama Mermaid

```mermaid
flowchart TD
    Web[Licitaciones.Web] --> Application[Licitaciones.Application]
    Web --> Infrastructure[Licitaciones.Infrastructure]

    Api[Licitaciones.Api] --> Application
    Api --> Infrastructure

    Infrastructure --> Application
    Infrastructure --> Domain[Licitaciones.Domain]

    Application --> Domain

    UnitTests[Licitaciones.UnitTests] --> Application
    UnitTests --> Domain

    IntegrationTests[Licitaciones.IntegrationTests] --> Infrastructure
    FunctionalTests[Licitaciones.FunctionalTests] --> Api
```

## Restricciones actuales

Todavia no se implementaron:

- CRUD de licitaciones, ofertas, niveles de aprobacion ni tipos de cambio.
- Entidades completas fuera del modulo de proveedores.
- Reglas de negocio fuera del modulo de proveedores.
- Dockerfile de aplicacion.
- Kubernetes.
- Modulos funcionales completos fuera de proveedores.

La Fase 4 agrego infraestructura de persistencia, PostgreSQL local y Testcontainers. La Iteracion 1 agrego la primera tabla funcional y reglas de proveedores.

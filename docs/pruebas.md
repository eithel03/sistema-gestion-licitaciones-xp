# Pruebas

## Estado actual

La Fase 3 preparo la suite unitaria para sostener TDD en las siguientes historias. La base actual incluye pruebas tecnicas de arquitectura y ejemplos minimos de dominio.

## Pruebas unitarias agregadas en Fase 3

- `EntityTests`: igualdad por identidad y rechazo de identificadores por defecto.
- `ValueObjectTests`: igualdad por componentes.
- `ValidationResultTests`: resultados exitosos, errores y proteccion contra fallos sin errores.
- `IClockTests`: reemplazo del reloj por una implementacion fija en pruebas.

## Comandos previstos

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build
```

## Resultado de validación

Durante la sesión original del driver no fue posible ejecutar la suite con .NET 9 debido a que su entorno no disponía del SDK requerido por `global.json`.

Posteriormente, la Fase 3 fue validada por el navigator en un entorno con SDK .NET `9.0.305`.

Se ejecutaron correctamente:

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build
```

## Separacion actual de suites

### Pruebas unitarias

Validan comportamiento de dominio y abstracciones sin infraestructura externa. No dependen de Docker ni PostgreSQL.

Resultado validado en Fase 4: 9 pruebas aprobadas.

### Pruebas funcionales

Validan el arranque real de la API mediante `WebApplicationFactory` y el endpoint `/health`. El entorno `Testing` no registra el health check PostgreSQL para mantener la suite estable sin base real.

Resultado validado en Fase 4: 1 prueba aprobada.

### Pruebas de integracion con PostgreSQL y Testcontainers

Se agregaron:

- `PersistenceConventionsTests`: valida convenciones reutilizables de EF Core sin abrir conexion externa.
- `PostgreSqlContainerTests`: levanta PostgreSQL 16 mediante Testcontainers, crea `LicitacionesDbContext` y abre la conexion correctamente.

Resultado validado en Fase 4: 3 pruebas aprobadas.

Estas pruebas requieren Docker disponible. En el entorno local se verifico que `postgres:16` existia y que Testcontainers podia abrir conexion. El intento inicial fallo porque Docker no pudo descargar `testcontainers/ryuk:0.14.0`; el fixture fue ajustado para deshabilitar Ryuk en esta prueba tecnica y cerrar el contenedor mediante `DisposeAsync`.

## Comandos de Fase 4

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build
```

Docker:

```bash
docker --version
docker compose version
docker compose config
docker compose up -d
docker compose ps
docker compose down
```

No usar `docker compose down -v` salvo que se quiera borrar manualmente el volumen local de desarrollo.

## Resultado de validacion Fase 4

- Restore: exitoso.
- Build Release: exitoso, 0 errores y 0 advertencias.
- Test Release: exitoso, 13 pruebas aprobadas.
- Docker Compose: configuracion valida.
- PostgreSQL local: contenedor `postgres:16` alcanzo estado `healthy`.
- Contenedores de Compose: detenidos con `docker compose down` sin borrar volumenes.

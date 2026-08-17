# Docker

Infraestructura reproducible validada para la Iteración 4.

## Servicios

`compose.yaml` define:

- `postgres`: PostgreSQL 16 con volumen persistente `postgres_data` y health check `pg_isready`.
- `api`: imagen `licitaciones-api:v1.0.0-rc`, Dockerfile multi-stage en `src/Licitaciones.Api/Dockerfile`, puerto externo `${API_PORT:-8081}`.
- `web`: imagen `licitaciones-web:v1.0.0-rc`, Dockerfile multi-stage en `src/Licitaciones.Web/Dockerfile`, puerto externo `${WEB_PORT:-8080}`.

La cadena de conexión se configura mediante `ConnectionStrings__DefaultConnection` y apunta al servicio `postgres` dentro de la red de Compose.

PostgreSQL se publica en el puerto host `55432` para no interferir con otra instalación local que utilice `5432`.

## Variables

El repositorio incluye `.env.example` con valores de desarrollo y sin secretos reales. El equipo debe crear su archivo `.env` local antes del arranque cuando necesite personalizar esos valores.

Comandos de ejecución:

```bash
docker compose config
docker compose build
docker compose up -d
docker compose ps
```

## Validación local

- `docker compose config`: exitoso.
- Construcción de las imágenes Web y API: exitosa.
- Arranque de servicios: exitoso.
- PostgreSQL 16: saludable.
- API: disponible en `http://localhost:8081` y health check operativo.
- Web: disponible en `http://localhost:8080` y health check operativo.
- PostgreSQL: accesible desde el host por el puerto `55432`.
- Volumen `postgres_data`: operativo.

La persistencia se comprobó creando datos y reiniciando los contenedores. Proveedores, licitaciones, ofertas y tipos de cambio permanecieron disponibles después del reinicio.

## Conservación de datos

No se ejecutó `docker compose down -v`. No usar ese comando salvo decisión explícita del equipo, porque elimina el volumen local de PostgreSQL y sus datos persistidos.

## Cierre y validación de Fase 6

La Fase 6 consolidó y validó la infraestructura Docker existente. Web y API conservan Dockerfiles multi-stage y se construyeron como `licitaciones-web:v1.0.0-rc` y `licitaciones-api:v1.0.0-rc` mediante `docker compose up --build -d`.

### Evidencia de servicios y persistencia

- `docker compose config`: exitoso.
- PostgreSQL: imagen `postgres:16`, puerto host `55432` y estado `healthy`.
- Web: contenedor iniciado correctamente en el puerto host `8080`.
- API: contenedor iniciado correctamente en el puerto host `8081`.
- Swagger de la API: verificado manualmente y disponible.
- Volumen persistente de PostgreSQL: operativo.
- `docker compose restart`: exitoso.
- Persistencia posterior al reinicio: se conservaron licitaciones, proveedores, ofertas y tipos de cambio.
- `docker compose down -v`: no se ejecutó.

### Migraciones automáticas

`src/Licitaciones.Api/Program.cs` y `src/Licitaciones.Web/Program.cs` ejecutan `dbContext.Database.Migrate()` al iniciar. La tabla PostgreSQL `__EFMigrationsHistory` registró con EF Core 9.0.18 las seis migraciones aplicadas:

- `20260810092133_CreateProveedores`.
- `20260811234653_MakeProveedorNameUniqueIndexPartial`.
- `20260812002104_CreateLicitaciones`.
- `20260813011055_Iteration03OfertasAprobacion`.
- `20260813205016_Iteration04TiposCambio`.
- `20260814014136_AllowDuplicateTipoCambioDates`.

### Usuario no privilegiado

Los Dockerfiles de Web y API declaran `USER $APP_UID`. La validación mediante `docker inspect` devolvió UID `1654` para ambos contenedores, lo que confirma que no se ejecutan como root.

### Comandos reproducibles

```bash
docker compose config
docker compose up --build -d
docker compose ps
docker compose restart
docker inspect $(docker compose ps -q api) --format '{{.Config.User}}'
docker inspect $(docker compose ps -q web) --format '{{.Config.User}}'
```

Para validar el historial de migraciones desde PostgreSQL puede consultarse la tabla `__EFMigrationsHistory`. No se incluye `docker compose down -v` porque no fue ejecutado y eliminaría el volumen persistente.

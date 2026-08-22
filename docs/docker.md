# Docker

## Estado actual

`compose.yaml` define PostgreSQL, API y Web. Los Dockerfiles de aplicación son multi-stage con SDK/runtime .NET 9 y ejecutan el proceso final como usuario no privilegiado mediante `$APP_UID`.

## Servicios y puertos

| Servicio | Imagen/construcción | Puerto host | Puerto contenedor |
|---|---|---:|---:|
| PostgreSQL | `postgres:16` | `${POSTGRES_PORT:-55432}` | 5432 |
| API | `licitaciones-api:v1.0.0-rc` | `${API_PORT:-8081}` | 8080 |
| Web | `licitaciones-web:v1.0.0-rc` | `${WEB_PORT:-8080}` | 8080 |

`v1.0.0-rc` es una etiqueta local de imagen; no existe un tag Git oficial con ese nombre.

## Persistencia

El volumen nombrado `postgres_data` se monta en `/var/lib/postgresql/data`. El health check de PostgreSQL usa `pg_isready`. Web y API esperan la condición `service_healthy` antes de iniciar.

La evidencia histórica de Fase 6 registra que los datos sobrevivieron a `docker compose restart`. Ese resultado no se volvió a ejecutar en Fase 9.

## Variables

`.env.example` define:

- `ASPNETCORE_ENVIRONMENT`;
- `POSTGRES_DB`;
- `POSTGRES_USER`;
- `POSTGRES_PASSWORD`;
- `POSTGRES_PORT`;
- `WEB_PORT`;
- `API_PORT`.

Web y API reciben `ConnectionStrings__DefaultConnection` apuntando a `Host=postgres`. API habilita además el health check PostgreSQL.

`change_this_password` es solo un valor de ejemplo y debe sustituirse fuera de un entorno académico/local.

## Construcción de imágenes

Cada Dockerfile:

1. restaura el proyecto correspondiente desde `mcr.microsoft.com/dotnet/sdk:9.0`;
2. publica en Release;
3. copia la salida a `mcr.microsoft.com/dotnet/aspnet:9.0`;
4. expone 8080;
5. ejecuta con `USER $APP_UID`.

## Migraciones

Web y API llaman `Database.Migrate()` al iniciar fuera de `Testing`. Compose no contiene un servicio migrador. Si ambos hosts arrancan al mismo tiempo, pueden intentar aplicar migraciones de forma concurrente.

## Health checks

- PostgreSQL: health check explícito de Compose.
- API: endpoint `/health`, pero Compose no lo usa como healthcheck del servicio.
- Web: endpoint `/health`, pero Compose no lo usa como healthcheck del servicio.
- El endpoint Web no comprueba PostgreSQL.

## Limitaciones actuales

- Web y API no tienen `healthcheck` propio en Compose.
- No existe migrador independiente.
- Las migraciones pueden iniciarse desde ambos hosts.
- Existe una contraseña predeterminada de ejemplo.
- No hay `restart` policy.
- Las imágenes se construyen en CI, pero no se publican en un registry.

Estas limitaciones son trabajo técnico futuro y no se corrigieron en Fase 9.

## Comandos de referencia

No se ejecutaron durante Fase 9:

```bash
docker compose config
docker compose build
docker compose up -d
docker compose ps
docker compose restart
```

No debe ejecutarse `docker compose down -v` si se desea conservar el volumen.

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

# Modelo de datos

## Estado en Fase 4

La Fase 4 inicia la documentacion de persistencia, pero no define todavia un modelo completo de tablas. No existen tablas de proveedores, licitaciones, ofertas, niveles de aprobacion ni tipos de cambio porque esas piezas pertenecen a las iteraciones funcionales.

## Motor

- Motor local previsto: PostgreSQL 16.
- Desarrollo local: `compose.yaml` con servicio `postgres`, volumen persistente y health check `pg_isready`.
- Pruebas de integracion: Testcontainers con imagen `postgres:16`.

## Estrategia EF Core

- `LicitacionesDbContext` vive en `Licitaciones.Infrastructure.Persistence`.
- EF Core 9 y Npgsql se instalan solo en `Licitaciones.Infrastructure`.
- `Domain` no depende de EF Core.
- `Application` no depende directamente de EF Core.
- Las configuraciones futuras deben agregarse como clases separadas, preferiblemente mediante `IEntityTypeConfiguration<T>`.

## Cadena de conexion

Convencion unica elegida:

```text
ConnectionStrings:DefaultConnection
ConnectionStrings__DefaultConnection
```

PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=55432;Database=licitaciones_dev;Username=licitaciones_app;Password=change_this_password"
```

Git Bash:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=55432;Database=licitaciones_dev;Username=licitaciones_app;Password=change_this_password'
```

Docker Compose usa variables del archivo `.env` local no versionado. El repositorio incluye `.env.example`:

```bash
cp .env.example .env
```

Variables de Compose:

```text
POSTGRES_DB=licitaciones_dev
POSTGRES_USER=licitaciones_app
POSTGRES_PASSWORD=change_this_password
POSTGRES_PORT=55432
```

Los valores son ejemplos de desarrollo y deben reemplazarse localmente. No se deben versionar secretos reales.

## Convenciones preparadas

- `CreatedAt`, `UpdatedAt` y `DeletedAt`: si una entidad futura define estas propiedades como `DateTimeOffset`, EF las configura como `timestamp with time zone`.
- `Version`: si una entidad futura define esta propiedad, EF la marca como token de concurrencia optimista.
- Dinero: las propiedades monetarias deben configurarse explicitamente con `HasMoneyPrecision()` para usar precision `numeric(18,2)`. No se aplica una precision global a todos los `decimal`.

## Migraciones

La estrategia preparada ubica migraciones versionadas dentro de `Licitaciones.Infrastructure`.

Comando previsto para la primera migracion real:

```bash
dotnet ef migrations add NombreMigracion --project src/Licitaciones.Infrastructure --startup-project src/Licitaciones.Api --output-dir Persistence/Migrations
```

Aplicacion local de migraciones:

```bash
dotnet ef database update --project src/Licitaciones.Infrastructure --startup-project src/Licitaciones.Api
```

No se crea una migracion inicial en Fase 4 porque el `DbContext` no tiene entidades persistentes reales. Crear una tabla ficticia solo para generar una migracion adelantaria diseno fuera del alcance.

## Ampliacion progresiva prevista

- Proveedores: Iteracion 1.
- Licitaciones, auditoria y concurrencia funcional: Iteracion 2.
- Ofertas y niveles de aprobacion: Iteracion 3.
- Tipos de cambio: Iteracion 4.

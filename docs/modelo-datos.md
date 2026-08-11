# Modelo de datos

## Estado actual

La Fase 4 preparo la persistencia base. La Iteracion 1 agrega el modelo persistente de proveedores mediante EF Core y la migracion `20260810092133_CreateProveedores`.

No existen todavia tablas de licitaciones, ofertas, niveles de aprobacion ni tipos de cambio.

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

Migracion real existente:

- `20260810092133_CreateProveedores`

## Tabla `Proveedores`

Entidad de dominio: `Licitaciones.Domain.Proveedores.Proveedor`.

| Campo | Tipo EF/PostgreSQL | Requerido | Observaciones |
| --- | --- | --- | --- |
| `Id` | `uuid` | Si | Identificador generado por dominio; EF usa `ValueGeneratedNever`. |
| `Nombre` | `character varying(200)` | Si | Nombre de presentacion normalizado para mostrar. |
| `NombreNormalizado` | `character varying(200)` | Si | Clave normalizada para comparacion de duplicados. |
| `CreatedAt` | `timestamp with time zone` | Si | Convencion de auditoria. |
| `UpdatedAt` | `timestamp with time zone` | Si | Convencion de auditoria. |
| `DeletedAt` | `timestamp with time zone` | No | Borrado logico. |
| `Version` | `bigint` | Si | Campo preparado por convencion de persistencia. |

Restricciones e indices reales:

- Llave primaria: `PK_Proveedores` sobre `Id`.
- Indice unico: `IX_Proveedores_NombreNormalizado` sobre `NombreNormalizado`.
- Filtro global EF Core: excluye proveedores con `DeletedAt` distinto de `null`.

Normalizacion y duplicidad:

- `Nombre` se recorta, normaliza Unicode en Form C y reduce espacios repetidos.
- `NombreNormalizado` usa normalizacion Form KC y mayusculas invariantes para comparacion.
- La aplicacion valida duplicidad antes de guardar.
- PostgreSQL refuerza la regla con el indice unico sobre `NombreNormalizado`.

Relaciones:

- La Iteracion 1 no define relaciones desde `Proveedores` hacia otras tablas.

## Ampliacion progresiva prevista

- Proveedores: Iteracion 1.
- Licitaciones, auditoria y concurrencia funcional: Iteracion 2.
- Ofertas y niveles de aprobacion: Iteracion 3.
- Tipos de cambio: Iteracion 4.

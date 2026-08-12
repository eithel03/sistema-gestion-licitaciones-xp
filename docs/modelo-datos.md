# Modelo de datos

## Estado actual

La Fase 4 preparo la persistencia base mediante PostgreSQL, Entity Framework Core y Testcontainers.

La Iteracion 1 agrego el modelo persistente de proveedores mediante EF Core y la migracion `20260810092133_CreateProveedores`.

Durante la Iteracion 2 se agrego el modelo persistente de licitaciones, junto con las reglas de auditoria, borrado logico, estados y concurrencia optimista.

Actualmente existen las tablas:

- `Proveedores`
- `Licitaciones`

Aun no existen tablas de ofertas, niveles de aprobacion ni tipos de cambio, ya que corresponden a iteraciones posteriores.

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

- `CreatedAt`, `UpdatedAt` y `DeletedAt`: se utilizan para auditoria y borrado logico cuando corresponde.
- `Version`: se mantiene como convencion de concurrencia para entidades que utilizan este mecanismo.
- En licitaciones, la concurrencia optimista se implementa mediante la columna de sistema `xmin` de PostgreSQL.
- Dinero: las propiedades monetarias se configuran explicitamente con precision `numeric(18,2)`. No se aplica una precision global a todos los `decimal`.

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
- `20260811234653_MakeProveedorNameUniqueIndexPartial`
- `20260812002104_CreateLicitaciones`


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
- Indice unico parcial: `IX_Proveedores_NombreNormalizado` sobre `NombreNormalizado`, aplicado solamente cuando `DeletedAt IS NULL`.
- Filtro global EF Core: excluye proveedores con `DeletedAt` distinto de `null`.

Normalizacion y duplicidad:

- `Nombre` se recorta, normaliza Unicode en Form C y reduce espacios repetidos.
- `NombreNormalizado` usa normalizacion Form KC y mayusculas invariantes para comparacion.
- La aplicacion valida duplicidad antes de guardar.
- PostgreSQL refuerza la regla mediante un indice unico parcial sobre `NombreNormalizado` para proveedores activos.
- Esto permite retirar un proveedor mediante borrado logico y posteriormente registrar nuevamente el mismo nombre normalizado.

Relaciones:

- La Iteracion 1 no define relaciones desde `Proveedores` hacia otras tablas.

## Tabla `Licitaciones`

Entidad de dominio: `Licitaciones.Domain.Licitaciones.Licitacion`.

La tabla fue agregada durante la Iteración 2 mediante la migración:

```text
20260812002104_CreateLicitaciones
```

| Campo | Tipo EF/PostgreSQL | Requerido | Observaciones |
| :--- | :--- | :--- | :--- |
| **Id** | `uuid` | Sí | Identificador único de la licitación. |
| **Codigo** | `character varying(50)` | Sí | Código de presentación de la licitación. |
| **CodigoNormalizado** | `character varying(50)` | Sí | Código utilizado para comparación de duplicados. |
| **Titulo** | `character varying(200)` | Sí | Título descriptivo de la licitación. |
| **PresupuestoCrc** | `numeric(18,2)` | Sí | Presupuesto almacenado en colones costarricenses. |
| **FechaCierreUtc** | `timestamp with time zone` | Sí | Fecha de cierre almacenada en UTC. |
| **Estado** | `character varying(20)` | Sí | Estado actual de la licitación. |
| **CreatedAt** | `timestamp with time zone` | Sí | Fecha de creación para auditoría. |
| **UpdatedAt** | `timestamp with time zone` | Sí | Fecha de última modificación. |
| **PublishedAt** | `timestamp with time zone` | No | Fecha en que la licitación fue publicada. |
| **ClosedAt** | `timestamp with time zone` | No | Fecha en que la licitación fue cerrada. |
| **DeletedAt** | `timestamp with time zone` | No | Borrado lógico de la licitación. |
| **xmin** | Columna de sistema PostgreSQL | Sí | Token utilizado para concurrencia optimista. |

### Restricciones e índices

* **Llave primaria:** `PK_Licitaciones` sobre `Id`.
* **Índice único parcial:** `IX_Licitaciones_CodigoNormalizado` sobre `CodigoNormalizado` cuando `DeletedAt IS NULL`.
* **Índice:** `IX_Licitaciones_Estado` sobre `Estado`.
* **Filtro global de EF Core:** para excluir licitaciones retiradas mediante borrado lógico.
* **PresupuestoCrc:** utiliza precisión `numeric(18,2)`.

### Normalización y duplicidad

* El código se normaliza antes de realizar comparaciones.
* La aplicación verifica duplicidad antes de guardar.
* PostgreSQL refuerza la unicidad mediante el índice parcial sobre `CodigoNormalizado`.
* El borrado lógico permite conservar el historial sin considerar registros retirados como activos.

### Estados

Los estados persistidos actualmente son:

* **Borrador**
* **Publicada**
* **Cerrada**

Las transiciones válidas son controladas por el dominio.

### Auditoría

La tabla registra:

* `CreatedAt`
* `UpdatedAt`
* `PublishedAt`
* `ClosedAt`
* `DeletedAt`

Estos campos permiten mantener evidencia del ciclo de vida de cada licitación.

### Concurrencia optimista

* Las licitaciones utilizan `xmin`, columna de sistema de PostgreSQL, como token de concurrencia optimista.
* Durante una actualización, EF Core verifica que la versión leída inicialmente coincida con la versión actual de la fila.
* Esto evita que una modificación obsoleta sobrescriba silenciosamente cambios realizados por otra operación.
* Los conflictos son tratados de forma controlada por la capa de aplicación.

### Relaciones

* La Iteración 2 todavía no agrega relaciones con ofertas.
* Las relaciones entre licitaciones, proveedores y ofertas se incorporarán en las iteraciones correspondientes.

## Ampliacion progresiva

- Proveedores: implementado en Iteracion 1.
- Licitaciones, auditoria y concurrencia: implementado en Iteracion 2.
- Ofertas y niveles de aprobacion: previsto para Iteracion 3.
- Tipos de cambio: previsto para Iteracion 4.

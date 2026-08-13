# Pruebas

## Estado actual

La Fase 3 preparo la suite unitaria para sostener TDD. La Fase 4 agrego pruebas de persistencia base. La Iteracion 1 incorporo pruebas de proveedores en Domain, Application, Infrastructure, API y MVC.

La Iteracion 2 amplio la cobertura con pruebas de licitaciones, reglas de estado, validaciones, persistencia relacional, concurrencia optimista y API REST, manteniendo PostgreSQL real mediante Testcontainers para las pruebas de integracion y funcionales.

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

## Resultado de validacion Iteracion 1

Evidencia registrada en `docs/iteraciones/iteracion-01.md`:

- `dotnet restore Licitaciones.sln --force`: exitoso.
- `dotnet build Licitaciones.sln --no-restore`: exitoso, 0 errores y 0 advertencias.
- `dotnet test tests/Licitaciones.UnitTests/Licitaciones.UnitTests.csproj --no-restore`: 34 pruebas aprobadas.
- `dotnet test tests/Licitaciones.IntegrationTests/Licitaciones.IntegrationTests.csproj --no-restore`: 9 pruebas aprobadas con PostgreSQL real.
- `dotnet test tests/Licitaciones.FunctionalTests/Licitaciones.FunctionalTests.csproj --no-restore`: 6 pruebas aprobadas con API, MVC y PostgreSQL real.

Pruebas de proveedores verificables en el repositorio:

- Unitarias de dominio: `tests/Licitaciones.UnitTests/Domain/Proveedores/ProveedorTests.cs`.
- Unitarias de aplicacion: `tests/Licitaciones.UnitTests/Application/Proveedores/ProveedorServiceTests.cs`.
- Integracion de persistencia: `tests/Licitaciones.IntegrationTests/Persistence/ProveedorPersistenceTests.cs`.
- Funcionales API: `tests/Licitaciones.FunctionalTests/ProveedorApiTests.cs`.
- Funcionales MVC: `tests/Licitaciones.FunctionalTests/ProveedorMvcTests.cs`.

## Resultado de validación Iteración 2

Durante la Iteración 2 se validaron las historias HU-12 a HU-19 mediante pruebas unitarias, de integración y funcionales.

Se ejecutaron:

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build
```

### Resultado final

* **Restore:** exitoso.
* **Build Release:** exitoso.
* **Pruebas ejecutadas:** 64.
* **Pruebas aprobadas:** 64.
* **Pruebas fallidas:** 0.
* **Pruebas omitidas:** 0.
* **PostgreSQL 16:** disponible mediante Docker.
* **Testcontainers:** ejecutó correctamente las pruebas con PostgreSQL real.

Las pruebas validaron la aplicación de las migraciones:

* `20260810092133_CreateProveedores`
* `20260811234653_MakeProveedorNameUniqueIndexPartial`
* `20260812002104_CreateLicitaciones`

## Pruebas de licitaciones - Iteracion 2

### Pruebas unitarias

Se validan reglas de dominio y Application sin depender de infraestructura externa.

Cobertura principal:

- Creacion de licitaciones.
- Normalizacion del codigo.
- Rechazo de codigos invalidos.
- Validacion de presupuesto mayor que cero.
- Validacion de fecha de cierre.
- Estado inicial `Borrador`.
- Publicacion de licitaciones.
- Cierre de licitaciones.
- Rechazo de transiciones invalidas.
- Comportamiento de licitaciones vencidas.
- Uso de `IClock` para controlar reglas temporales.

### Pruebas de integracion

Se utiliza PostgreSQL 16 real mediante Testcontainers.

Se valida:

- Persistencia de licitaciones.
- Lectura y actualizacion de registros.
- Indice unico parcial sobre `CodigoNormalizado`.
- Borrado logico.
- Precision monetaria `numeric(18,2)`.
- Migraciones EF Core.
- Auditoria.
- Concurrencia optimista mediante `xmin`.
- Conflictos de actualizacion controlados.

### Pruebas funcionales API

Se valida la API REST de licitaciones mediante `WebApplicationFactory` y PostgreSQL real.

Operaciones verificadas:

- Crear licitacion.
- Consultar licitacion.
- Listar licitaciones.
- Editar licitacion.
- Retirar licitacion.
- Publicar licitacion.
- Cerrar licitacion.
- Rechazar datos invalidos.
- Rechazar recursos inexistentes.
- Manejar conflictos controlados.

Base de endpoints:

```text
/api/v1/licitaciones
```

## Validacion manual de Iteracion 2

Ademas de las pruebas automatizadas, se realizo una validacion manual completa del flujo MVC de licitaciones.

Flujo ejecutado:

1. Crear una licitacion.
2. Verificar su aparicion en el listado.
3. Consultar el detalle.
4. Editarla mientras se encuentra en estado `Borrador`.
5. Publicarla.
6. Intentar una transicion invalida y verificar su rechazo.
7. Cerrar la licitacion.

La validacion manual confirmo que las reglas de estado se aplican correctamente.

Durante esta revision tambien se detectaron y corrigieron dos aspectos de interfaz:

- Formato de entrada de `PresupuestoCrc` para mantener coherencia con la cultura `es-CR`.
- Visibilidad de acciones segun el estado actual de la licitacion.

Las reglas del dominio permanecen como validacion definitiva independientemente de los controles mostrados en la interfaz.

## Estado actual de la suite

La suite completa se encuentra en verde al cierre tecnico local de la Iteracion 2.

```text
UnitTests        Exitosas
IntegrationTests Exitosas
FunctionalTests  Exitosas

Total:     64
Aprobadas: 64
Fallidas:   0
Omitidas:   0
```

## Pruebas de Iteracion 3

- Unitarias: `OfertaTests`, `EvaluadorOfertasTests`, `OfertaServiceTests`, `NivelAprobacionTests` y `NivelAprobacionServiceTests`.
- Integracion: `Iteration3PersistenceTests` cubre persistencia, actualizacion, ambas FKs, unicidad, checks, rango abierto, exclusion de traslapes y migraciones.
- Funcionales API: `Iteration3ApiTests` cubre alta, consulta, filtros, duplicidad, presupuesto, vencimiento, mejor oferta, clasificacion, aprobador y CRUD de niveles.
- Funcionales MVC: `Iteration3MvcTests` cubre acceso, selectores, alta, filtro, edicion, validacion, proveedor relacionado, confirmacion/eliminacion y CRUD/traslape de niveles.
- Base real: PostgreSQL 16 mediante Testcontainers; no se usa SQLite.

Ejecuciones focalizadas reales: 9 integraciones, 3 funcionales API y 2 funcionales MVC aprobadas.

Validacion final exacta del 2026-08-12:

- `dotnet restore Licitaciones.sln`: primer intento en sandbox fallo con `NU1301` por autenticacion TLS de NuGet; repeticion exacta fuera del sandbox exitosa para 8 proyectos.
- `dotnet build Licitaciones.sln --no-restore`: exitoso, 0 errores y 0 advertencias.
- `dotnet test tests/Licitaciones.UnitTests/Licitaciones.UnitTests.csproj --no-restore`: total 76, aprobadas 76, fallidas 0, omitidas 0.
- `dotnet test tests/Licitaciones.IntegrationTests/Licitaciones.IntegrationTests.csproj --no-restore`: total 22, aprobadas 22, fallidas 0, omitidas 0; PostgreSQL 16 mediante Testcontainers.
- `dotnet test tests/Licitaciones.FunctionalTests/Licitaciones.FunctionalTests.csproj --no-restore`: total 13, aprobadas 13, fallidas 0, omitidas 0.
- Total: 111/111 aprobadas, 0 fallidas, 0 omitidas.

Prueba manual local de Iteracion 3: completada contra PostgreSQL 16 y API local. Se observaron 409 para duplicidad, 400 para presupuesto excedido, mejor oferta CRC 800000 con 20 % y clasificacion conveniente, aprobador persistido/editado, 409 para traslape y 204 al eliminar el nivel. Identificadores y flujo completo: `docs/iteraciones/iteracion-03.md`.

Limitacion del entorno de ejecucion manual MVC: claves DPAPI no descifrables, Event Log sin permisos y certificado HTTPS local ausente o vencido impidieron mantener un servidor MVC independiente. Esto no representa una falla funcional de Iteracion 3: las pruebas MVC automatizadas con `WebApplicationFactory` y PostgreSQL real fueron satisfactorias.

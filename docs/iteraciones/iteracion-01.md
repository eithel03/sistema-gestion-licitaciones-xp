# Iteracion 1 - Landing page y proveedores

> Registro histórico de la Iteración 1. El PR `#9` fue integrado posteriormente a `main` mediante `10cb32b`. La versión `v0.1.0` fue prevista, pero no existe un tag oficial. Las frases de planificación que siguen describen el momento de la iteración.

- Objetivo: entregar una primera version funcional con landing page, navegacion y administracion completa de proveedores por MVC y API REST.
- Driver principal: Chavala.
- Navigator principal: Eithel.
- Rama de trabajo: `feature/iteracion-01-landing-proveedores`.
- Commit principal: `5696a0f` - `feat(proveedores): completar iteracion 1 de landing y gestion de proveedores`.
- Pull Request: `#9 - feat: completar Iteración 1 - Landing page y proveedores`.
- Estado actual del PR: integrado a `main` mediante `10cb32b`.
- Version prevista: `v0.1.0`.
- Puntos planificados: 30.
- Estado de la iteracion: implementada e integrada a `main`.

## Historias seleccionadas

| Historia | Prioridad | Puntos | Resultado |
| --- | --- | ---: | --- |
| HU-01 | Alta | 3 | Implementada |
| HU-02 | Alta | 2 | Implementada |
| HU-03 | Alta | 2 | Implementada |
| HU-04 | Media | 3 | Implementada |
| HU-05 | Alta | 3 | Implementada |
| HU-06 | Alta | 2 | Implementada |
| HU-07 | Alta | 3 | Implementada |
| HU-08 | Alta | 5 | Implementada |
| HU-09 | Media | 2 | Implementada |
| HU-10 | Alta | 5 | Implementada |

## Criterios de aceptacion verificados

- La pagina inicial presenta el sistema y enlaces a modulos principales.
- La navegacion incluye Inicio, Licitaciones, Proveedores, Ofertas, Niveles de aprobacion, Tipo de cambio y API / Swagger.
- Los modulos futuros muestran estado planificado sin adelantar CRUD.
- Proveedores permite crear, listar, consultar, editar y retirar mediante borrado logico.
- El listado permite buscar por nombre, ordenar y paginar.
- El servidor valida nombre requerido, caracteres permitidos y duplicidad normalizada.
- PostgreSQL protege la unicidad mediante indice unico sobre `NombreNormalizado`.
- La API REST expone `GET`, `POST`, `PUT` y `DELETE` bajo `/api/v1/proveedores`.
- La API usa DTO y devuelve `201`, `200`, `204`, `400`, `404` y `409` segun corresponda.

## Ciclos TDD realizados

1. ROJO: pruebas unitarias de dominio fallaron porque no existian `Proveedor` ni normalizador.
   VERDE: se implementaron entidad, normalizador, excepcion y reglas de caracteres.
   REFACTOR: se mantuvo la presentacion del nombre sin forzar Title Case y se centralizaron errores.

2. ROJO: pruebas unitarias de Application fallaron porque no existian contratos, repositorio ni servicio.
   VERDE: se implemento `ProveedorService`, DTO, resultado de aplicacion y validacion de duplicados.
   REFACTOR: se separaron fabricas de resultado para evitar advertencias de analisis.

3. ROJO: pruebas de persistencia fallaron por ausencia de repositorio/configuracion EF.
   VERDE: se agregaron `DbSet`, configuracion EF, repositorio e indice unico.
   REFACTOR: se genero migracion real con `dotnet ef` y se valido con `Database.Migrate`.

4. ROJO: pruebas funcionales API/MVC detectaron configuracion de conexion de pruebas.
   VERDE: se inyecto PostgreSQL de Testcontainers en `WebApplicationFactory`.
   REFACTOR: se aislaron colecciones de prueba para evitar interferencia entre contenedores.

## Pruebas realizadas

- `dotnet restore Licitaciones.sln --force`: exitoso.
- `dotnet build Licitaciones.sln --no-restore`: exitoso, 0 errores y 0 advertencias.
- `dotnet test tests/Licitaciones.UnitTests/Licitaciones.UnitTests.csproj --no-restore`: 34 pruebas aprobadas.
- `dotnet test tests/Licitaciones.IntegrationTests/Licitaciones.IntegrationTests.csproj --no-restore`: 9 pruebas aprobadas con PostgreSQL real.
- `dotnet test tests/Licitaciones.FunctionalTests/Licitaciones.FunctionalTests.csproj --no-restore`: 6 pruebas aprobadas con API, MVC y PostgreSQL real.

## Resultado

La Iteracion 1 quedó técnicamente implementada y luego se integró a `main` mediante `10cb32b`. La aplicación permite administrar proveedores mediante MVC y API, utilizando persistencia en PostgreSQL y las validaciones asociadas al módulo. El tag `v0.1.0` no existe.

## Velocidad observada

30 puntos registrados como implementados en la evidencia histórica. El PR fue integrado; esta fase documental no volvió a ejecutar su CI.

## Retroalimentacion

Pendiente de registrar despues de la revision real del navigator.

## Ajustes

- Se mantuvo la API / Swagger como pagina informativa en MVC porque Swagger formal esta planificado para Iteracion 4.
- Se uso borrado logico para proveedores segun HU-07.
- Se reutilizo `LicitacionesDbContext`, PostgreSQL y Testcontainers de Fase 4.

## Commits y Pull Requests

- Commit principal: `5696a0f` - `feat(proveedores): completar iteracion 1 de landing y gestion de proveedores`.
- Pull Request: `#9 - feat: completar Iteración 1 - Landing page y proveedores`.
- Base: `main`.
- Rama origen: `feature/iteracion-01-landing-proveedores`.
- Estado: integrado a `main` mediante `10cb32b`.

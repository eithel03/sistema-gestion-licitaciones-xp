# Modulo Niveles de Aprobacion

## Responsabilidad

Parametrizar mediante datos persistidos el aprobador aplicable a un monto, con CRUD MVC y API.

## Reglas

- `MontoMinimoCrc` es mayor que cero.
- `MontoMaximoCrc` es nullable y, cuando existe, no es menor que el minimo.
- Los limites son inclusivos.
- Los rangos no se traslapan.
- Solo existe un rango abierto.
- El aprobador es requerido y tiene longitud maxima de 200 caracteres.

## Persistencia

`NivelesAprobacion` usa montos `numeric(18,2)`, auditoria, `xmin`, `CHECK` de limites, indice parcial unico `NULLS NOT DISTINCT` para el rango abierto y exclusion GiST con `numrange &&` para impedir traslapes concurrentes.

## Capas y evidencia

- Domain: `NivelAprobacion`.
- Application: `NivelAprobacionService` y repositorio abstracto.
- Infrastructure: configuracion y repositorio EF Core.
- MVC/API: CRUD y consulta de aprobador.
- Historias: HU-27 a HU-29.
- Pruebas: `NivelAprobacionTests`, `NivelAprobacionServiceTests`, `Iteration3PersistenceTests`, `Iteration3ApiTests` e `Iteration3MvcTests`.
- Commit, PR, CI, merge y tag: Pendientes.

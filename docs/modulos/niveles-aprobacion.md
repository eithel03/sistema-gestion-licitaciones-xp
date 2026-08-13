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
- Application: contratos, consultas, resultados, validaciones, `INivelAprobacionService`, `NivelAprobacionService`, repositorio abstracto y excepciones controladas.
- Infrastructure: `NivelAprobacionConfiguration`, `NivelAprobacionRepository`, registro en `LicitacionesDbContext`, inyeccion de dependencias y migracion `20260813011055_Iteration03OfertasAprobacion`.
- MVC: `NivelesAprobacionController` y vistas `Index`, `Create`, `Details`, `Edit` y `Delete`.
- API: CRUD y consulta de aprobador por monto. Los consumidores usan Application; la logica de rangos no reside en controladores.
- Historias: HU-27 a HU-29.
- Pruebas: `NivelAprobacionTests`, `NivelAprobacionServiceTests`, `Iteration3PersistenceTests`, `Iteration3ApiTests` e `Iteration3MvcTests`.
- Commits: `a20eb19`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37`.
- Rama: `feature/iteracion-03-ofertas-aprobacion`.
- Pull Request, CI remoto, merge, revision formal del Navigator y tag `v0.3.0`: Pendientes.

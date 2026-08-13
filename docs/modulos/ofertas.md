# Modulo Ofertas

## Responsabilidad

Registrar, consultar, filtrar, modificar y eliminar ofertas mientras la licitacion publicada permanezca vigente. El modulo tambien obtiene la mejor oferta y calcula el ahorro.

## Reglas

- `MontoOfertadoCrc` es `decimal`, mayor que cero y no supera `PresupuestoCrc`.
- Solo una licitacion con estado efectivo `Publicada` recibe ofertas.
- `ahora >= FechaCierreUtc` produce estado efectivo `Cerrada`.
- Un proveedor presenta como maximo una oferta por licitacion.
- Una oferta cerrada no puede editarse ni eliminarse y permanece como evidencia.
- Mejor oferta: menor monto, luego fecha de registro mas temprana y finalmente menor `Id`.
- Ahorro: `((PresupuestoCrc - MejorOfertaCrc) / PresupuestoCrc) * 100`.
- Clasificaciones devueltas: ahorro desde 10 %, `Oferta conveniente`; ahorro mayor que 0 % y menor que 10 %, `Oferta aceptable`; ahorro igual a 0 %, `Oferta valida sin ahorro`.
- Sin ofertas, la evaluacion devuelve el resultado funcional `Sin ofertas validas`.

## Capas

- Domain: `Oferta`, `EvaluadorOfertas` y errores.
- Application: contratos, consultas, resultados, validaciones, `IOfertaService`, `IOfertaRepository`, `OfertaService` y excepciones controladas.
- Infrastructure: `OfertaConfiguration`, `OfertaRepository`, registro en `LicitacionesDbContext` e inyeccion de dependencias.
- MVC: `OfertasController`, modelos y vistas CRUD.
- API: `OfertaEndpoints`.

## Persistencia

`Ofertas` usa PK `Id`, FKs restrictivas a `Licitaciones` y `Proveedores`, monto `numeric(18,2)`, `CHECK` positivo, indice unico `IX_Ofertas_LicitacionId_ProveedorId` y `xmin` para concurrencia. La migracion es `20260813011055_Iteration03OfertasAprobacion`.

## MVC

`OfertasController` y las vistas `Index`, `Create`, `Details`, `Edit` y `Delete` consumen `IOfertaService`. El listado admite filtros por licitacion y proveedor; los formularios incluyen selectores, monto decimal, validaciones y confirmacion de eliminacion. El detalle de proveedor integra sus ofertas relacionadas para HU-11.

## Evidencia

- Historias: HU-11 y HU-20 a HU-26, HU-29.
- Pruebas: `OfertaTests`, `EvaluadorOfertasTests`, `OfertaServiceTests`, `Iteration3PersistenceTests`, `Iteration3ApiTests` e `Iteration3MvcTests`.
- Commits: `d6d6009`, `7e6a317`, `29e727c`, `37bcb55`, `4faaf83`, `437cc37`.
- Rama: `feature/iteracion-03-ofertas-aprobacion`.
- Pull Request, CI remoto, merge, revision formal del Navigator y tag `v0.3.0`: Pendientes.

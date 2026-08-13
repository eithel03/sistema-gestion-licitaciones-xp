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
- Clasificaciones: conveniente desde 10 %, aceptable entre 0 y 10 %, valida sin ahorro en 0 %.

## Capas

- Domain: `Oferta`, `EvaluadorOfertas` y errores.
- Application: contratos, resultados, `IOfertaService`, `IOfertaRepository` y `OfertaService`.
- Infrastructure: `OfertaConfiguration` y `OfertaRepository`.
- MVC: `OfertasController`, modelos y vistas CRUD.
- API: `OfertaEndpoints`.

## Persistencia

`Ofertas` usa PK `Id`, FKs restrictivas a `Licitaciones` y `Proveedores`, monto `numeric(18,2)`, `CHECK` positivo, indice unico `(LicitacionId, ProveedorId)` y `xmin` para concurrencia.

## Evidencia

- Historias: HU-11 y HU-20 a HU-26, HU-29.
- Pruebas: `OfertaTests`, `EvaluadorOfertasTests`, `OfertaServiceTests`, `Iteration3PersistenceTests`, `Iteration3ApiTests` e `Iteration3MvcTests`.
- Commit, PR, CI, merge y tag: Pendientes.

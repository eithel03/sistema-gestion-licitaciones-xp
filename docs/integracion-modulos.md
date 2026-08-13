# Integracion de modulos

## Iteracion 3

Flujo implementado:

`Proveedor -> Licitacion -> Oferta -> EvaluadorOfertas -> Clasificacion -> NivelAprobacion`

- `Proveedor` identifica al oferente y su detalle MVC consulta `IOfertaService` por `ProveedorId`; muestra la tabla relacionada o un estado vacio. Esta integracion satisface HU-11.
- `Licitacion` conserva presupuesto, estado y fecha de cierre. `OfertaService` obtiene licitacion y proveedor mediante repositorios antes de crear la oferta.
- `Oferta` relaciona ambos agregados mediante FKs restrictivas. Reutiliza `Licitacion.GetEstadoEfectivo(IClock.UtcNow)` para decidir si se permiten altas, ediciones o eliminaciones.
- `EvaluadorOfertas` selecciona el menor monto; desempata por `FechaRegistro` y finalmente por `Id`. Calcula ahorro y clasificacion con `decimal`.
- `NivelAprobacion` determina el aprobador consultando rangos persistidos. No existe una cadena fija de cargos en controladores ni endpoints.
- Los endpoints anidados de Licitacion consultan ofertas y mejor oferta mediante `IOfertaService`; al existir ganadora, consultan `INivelAprobacionService` con su monto.

No se agregaron integraciones de tipo de cambio, CRC/USD ni preferencias de Iteracion 4.

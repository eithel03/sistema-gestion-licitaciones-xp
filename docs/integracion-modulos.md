# Integracion de modulos

## Iteracion 3

- Oferta referencia mediante FKs restrictivas a Licitacion y Proveedor.
- `OfertaService` obtiene ambas entidades por sus repositorios antes de crear.
- Oferta reutiliza `Licitacion.GetEstadoEfectivo(IClock.UtcNow)` para estado y vencimiento.
- El detalle MVC de Proveedor consulta `IOfertaService` con `ProveedorId` y muestra tabla o estado vacio.
- Los endpoints anidados de Licitacion consultan ofertas y mejor oferta mediante `IOfertaService`.
- La respuesta de mejor oferta consulta `INivelAprobacionService` con el monto ganador y obtiene un aprobador persistido.

No se agregaron integraciones de tipo de cambio, CRC/USD ni preferencias de Iteracion 4.

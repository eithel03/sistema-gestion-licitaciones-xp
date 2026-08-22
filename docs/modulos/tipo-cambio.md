# Módulo Tipo de Cambio

## 1. Propósito

Administrar tasas locales CRC por USD y convertir montos CRC únicamente para presentación.

## 2. Responsabilidades

- CRUD de tipos de cambio.
- Mantener un único registro activo.
- Activar transaccionalmente una tasa y desactivar la anterior.
- Convertir CRC a USD sin modificar el monto persistido.
- Exponer fecha y valor de la tasa utilizada.
- Integrarse con preferencias MVC y API.

## 3. Dependencias

- Domain: `TipoCambio` y errores.
- Application: `ITipoCambioService`, `IMonedaConversionService`, contratos, repositorio e `IClock`.
- Infrastructure: `TipoCambioRepository`, EF Core, PostgreSQL y transacciones.
- Web: `TiposCambioController`, modelos, vistas, preferencias y `_MoneyDisplay`.
- API: `TipoCambioEndpoints`.

## 4. Entradas

- Fecha.
- Valor decimal `CrcPorUsd`.
- `Version` opcional al actualizar.
- Id al activar o eliminar.
- Listado: `page`, `pageSize`.
- Conversión: `montoCrc` y moneda `CRC` o `USD`.
- MVC acepta punto o coma decimal mediante `FlexibleDecimalModelBinder`.

## 5. Salidas

- `TipoCambioResponse` y `TipoCambioPage`.
- `MontoVisualizadoResponse`: monto original CRC, monto mostrado, moneda, id/fecha/valor de tasa.
- Vistas CRUD y acción de activación.
- API con 200/201/204 o ProblemDetails.

## 6. Reglas de negocio

- Fecha requerida; varios registros pueden tener la misma fecha.
- `CrcPorUsd` mayor que cero.
- Un único registro activo, reforzado por índice parcial.
- Activar desactiva los demás dentro de una transacción del repositorio.
- USD se calcula como `CRC / CrcPorUsd` y se redondea a dos decimales alejándose de cero.
- CRC se devuelve sin requerir tipo activo.
- La cookie `licitaciones.currency` decide la presentación MVC y no altera persistencia.
- Concurrencia por `xmin`.

## 7. Errores

- Fecha o valor inválido.
- Tipo de cambio no encontrado.
- Tipo activo no encontrado para conversión USD.
- Conflicto de concurrencia.
- Conflicto de activo único.

La eliminación es física; el código actual no contiene una regla que prohíba eliminar el registro activo.

## 8. Pruebas relacionadas

- Unitarias: `TipoCambioTests`, `TipoCambioServiceTests`.
- Integración: `TipoCambioPersistenceTests`.
- Funcionales: `Iteration4ApiTests`, `Iteration4MvcTests`.
- E2E: preferencia monetaria en `PreferenciasE2ETests`.

Limitación MVC: el listado está paginado en backend, pero no tiene controles de navegación visibles.

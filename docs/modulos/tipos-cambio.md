# Tipos de cambio

Módulo implementado en la Iteración 4 para administrar conversiones locales entre CRC y USD.

## Alcance

- Crear, listar, consultar, editar y eliminar tipos de cambio.
- Activar un único tipo de cambio vigente.
- Operar sin servicios externos ni conexión a Internet.
- Usar CRC como fuente de verdad persistida.
- Convertir a USD únicamente para presentación.

## Reglas principales

- `Fecha` es requerida.
- Se permiten múltiples tipos de cambio con la misma fecha.
- `CrcPorUsd` debe ser mayor que cero.
- Solo puede existir un registro activo al mismo tiempo.
- Activar uno nuevo desactiva el activo anterior.
- PostgreSQL refuerza el activo único con el índice parcial único `IX_TiposCambio_UnicoActivo`.

## Entrada y presentación

El formulario MVC admite entradas decimales equivalentes con punto o coma:

- `500`, `500.00` y `500,00` representan 500.00.
- `520.50` y `520,50` representan 520.50.
- Cero, negativos y texto no son válidos.

La interfaz presenta el valor con formato costarricense, por ejemplo `500,00`.

## Integración

- Application expone `ITipoCambioService` para CRUD y activación.
- Application expone `IMonedaConversionService` para conversión visual desde CRC.
- API usa `/api/v1/tipos-cambio` y `/api/v1/moneda/convertir`.
- La activación oficial usa `PATCH /api/v1/tipos-cambio/{id}/activar`.
- MVC usa `TiposCambioController` y el parcial `_MoneyDisplay`.
- La conversión visual aplica `USD = CRC / CrcPorUsd` y muestra la fecha de la tasa utilizada.

## Persistencia

Migraciones reales:

```text
20260813205016_Iteration04TiposCambio
20260814014136_AllowDuplicateTipoCambioDates
```

La migración inicial crea la tabla `TiposCambio`, el valor `CrcPorUsd` como `numeric(18,2)`, auditoría `CreatedAt`/`UpdatedAt`, concurrencia optimista `xmin` y restricciones de negocio.

`20260814014136_AllowDuplicateTipoCambioDates` elimina la unicidad de `IX_TiposCambio_Fecha` y recrea ese índice como índice normal. `IX_TiposCambio_UnicoActivo` permanece como índice parcial único aplicado a los registros activos.

## Evidencia

- CRUD validado manualmente.
- Múltiples registros con la misma fecha validados.
- Activación sucesiva validada dejando exactamente un registro activo.
- Persistencia validada con PostgreSQL real y Testcontainers.
- Conversión CRC/USD validada sin alterar los montos CRC persistidos.

# Módulo Niveles de Aprobación

## 1. Propósito

Parametrizar el aprobador aplicable a un monto CRC mediante rangos persistidos.

## 2. Responsabilidades

- CRUD de niveles de aprobación.
- Validar límites y aprobador.
- Evitar rangos solapados y más de un rango abierto.
- Encontrar el aprobador de un monto.
- Integrarse con la respuesta de mejor oferta.

## 3. Dependencias

- Domain: `NivelAprobacion` y errores.
- Application: `INivelAprobacionService`, contratos, resultados, repositorio e `IClock`.
- Infrastructure: `NivelAprobacionRepository`, PostgreSQL, índice parcial y exclusión GiST.
- Web: `NivelesAprobacionController`, modelos y vistas.
- API: `NivelAprobacionEndpoints` y `OfertaEndpoints`.

## 4. Entradas

- `MontoMinimoCrc`.
- `MontoMaximoCrc` opcional.
- `Aprobador`.
- `Version` opcional al actualizar.
- Listado: `page`, `pageSize`.
- Consulta de aprobador: `montoCrc`.

## 5. Salidas

- `NivelAprobacionResponse` y `NivelAprobacionPage`.
- `AprobadorResponse`.
- Vistas CRUD.
- API con 200/201/204 o ProblemDetails.

## 6. Reglas de negocio

- Mínimo mayor que cero.
- Máximo nulo o mayor/igual al mínimo.
- Límites inclusivos.
- Máximo nulo representa rango abierto.
- Solo un rango abierto.
- Ningún rango puede solaparse con otro, incluso en el límite compartido.
- Aprobador requerido, máximo 200 caracteres.
- Concurrencia por `xmin`.
- Application valida antes de guardar y PostgreSQL refuerza simultaneidad mediante restricciones.

## 7. Errores

- Mínimo inválido.
- Máximo inválido.
- Aprobador requerido o demasiado largo.
- Rango traslapado.
- Segundo rango abierto.
- Nivel o aprobador no encontrado.
- Conflicto de concurrencia.

## 8. Pruebas relacionadas

- Unitarias: `NivelAprobacionTests`, `NivelAprobacionServiceTests`.
- Integración: `Iteration3PersistenceTests`.
- Funcionales: `Iteration3ApiTests`, `Iteration3MvcTests`.
- No existe un flujo E2E exclusivo del CRUD de niveles.

Limitación MVC: la consulta está paginada en backend, pero la vista no ofrece navegación entre páginas. La integración con mejor oferta se expone por API, no como pantalla MVC.

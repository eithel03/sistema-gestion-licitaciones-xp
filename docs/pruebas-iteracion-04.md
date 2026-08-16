# Pruebas Iteración 4

## Actualización de Fase 5

La medición de Iteración 4 documentada abajo es histórica. La consolidación posterior de Fase 5 se ejecutó en `chore/fase-05-pruebas-cobertura` y obtuvo UnitTests 121/121, IntegrationTests 37/37, FunctionalTests 54/54 y E2ETests 6/6. La cobertura combinada vigente es Domain 91,64 %, Application 88,60 % y global 89,37 %.

## Suite final

| Suite | Aprobadas | Fallidas | Omitidas |
| --- | ---: | ---: | ---: |
| UnitTests | 96 | 0 | 0 |
| IntegrationTests | 27 | 0 | 0 |
| FunctionalTests | 51 | 0 | 0 |
| Total | 174 | 0 | 0 |

- Restore: exitoso.
- Build Release: exitoso, 0 errores y 0 advertencias.
- Integración y funcionales usan PostgreSQL real mediante Testcontainers y `WebApplicationFactory`.

## Pruebas agregadas y consolidadas

- `TipoCambioTests`: valor válido, rechazo de cero y negativos, estado y reglas de dominio.
- `TipoCambioServiceTests`: CRUD, activación única y conversión sin modificar CRC.
- `TipoCambioPersistenceTests`: persistencia PostgreSQL, fechas duplicadas e índice de activo único.
- `Iteration4ApiTests`: CRUD, activación PATCH y conversión monetaria.
- `Iteration4MvcTests`: formularios, formato decimal localizado, selector CRC/USD y temas.
- `ApiHardeningTests`: OpenAPI, Swagger UI, verbos reales, ProblemDetails y correlación.
- `LicitacionApiTests`: `PATCH /api/v1/licitaciones/{id}/estado`, transiciones válidas e inválidas y 404.
- `ProveedorTests` y `ProveedorMvcTests`: nombres Unicode válidos y símbolos no permitidos.

Los casos de entrada decimal cubren `500`, `500.00`, `500,00`, `520.50` y `520,50`; también rechazan `0`, `-1` y `abc`.

## Cobertura definitiva

El reporte limpio usa únicamente los tres archivos actuales de cobertura:

```text
Parser: MultiReport (3x Cobertura)
```

| Componente | Cobertura de líneas |
| --- | ---: |
| Global | 87.3% |
| Licitaciones.Domain | 91.4% |
| Licitaciones.Application | 83.8% |
| Licitaciones.Api | 88.4% |
| Licitaciones.Infrastructure | 95.1% |
| Licitaciones.Web | 61.6% |

- Branch coverage: 59%.
- Method coverage: 84%.

La evidencia definitiva utiliza únicamente los tres reportes actuales y no mezcla ejecuciones históricas.

## Umbrales

| Umbral obligatorio | Resultado | Estado |
| --- | ---: | --- |
| Global >= 70% | 87.3% | Cumplido |
| Domain >= 80% | 91.4% | Cumplido |
| Application >= 80% | 83.8% | Cumplido |

## Testcontainers

Las suites de integración y funcionales levantan PostgreSQL real mediante Testcontainers. Algunos logs muestran `fail: Microsoft.EntityFrameworkCore.Database.Connection` inmediatamente después de `DROP DATABASE ... WITH (FORCE)`; esto ocurre durante el reinicio intencional de las bases temporales y no representa pruebas fallidas.

## Integración continua

El workflow de GitHub Actions está preparado para ejecutar restore, build, pruebas y recolección de cobertura. La ejecución local final está validada; la evidencia de CI remoto posterior al push permanece Pendiente.

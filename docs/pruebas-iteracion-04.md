# Pruebas de Iteración 4 — evidencia histórica

Este documento conserva la medición obtenida al cerrar técnicamente la Iteración 4. No representa una ejecución realizada durante Fase 9.

## Medición registrada al cierre de la iteración

| Suite | Casos reportados |
|---|---:|
| UnitTests | 96/96 |
| IntegrationTests | 27/27 |
| FunctionalTests | 51/51 |
| **Total** | **174/174** |

Cobertura de líneas registrada entonces: global 87,3 %, Domain 91,4 %, Application 83,8 %, API 88,4 %, Infrastructure 95,1 % y Web 61,6 %. También se registró 59 % de ramas y 84 % de métodos.

## Casos incorporados

- dominio y servicio de tipos de cambio;
- persistencia, fecha repetida y activo único;
- CRUD/activación/conversión por API;
- formularios, formato decimal, moneda y tema por MVC;
- rutas OpenAPI, Swagger UI, ProblemDetails y correlación;
- PATCH de estado de licitación;
- Unicode de proveedores.

## Actualización histórica posterior

Fase 5 amplió las suites y registró 218/218 casos, con cobertura Domain 91,64 %, Application 88,60 % y global 89,37 %. La explicación vigente, las limitaciones y el conteo estático actual están en [pruebas.md](pruebas.md).

## Limitación OpenAPI conocida

Las pruebas de Iteración 4 validan rutas y métodos, pero el documento OpenAPI actual sigue siendo manual y superficial. No demuestra DTO, request bodies ni respuestas completos.

# Iteracion 4 - Tipos de cambio, API endurecida e infraestructura

- Objetivo: entregar tipos de cambio, visualizacion CRC/USD, modo claro y oscuro, contrato API completo, pruebas E2E iniciales, automatizacion e infraestructura de version candidata.
- Duracion uniforme propuesta: Pendiente de completar por el equipo.
- Fecha prevista: Pendiente de completar por el equipo.
- Driver principal: Eithel.
- Navigator principal: Chavala.
- Version prevista: v1.0.0-rc.
- Puntos planificados: 32.

## Historias seleccionadas

| Historia | Puntos | Proposito |
| --- | ---: | --- |
| HU-30 | 5 | Administrar tipos de cambio |
| HU-31 | 3 | Activar un unico tipo de cambio |
| HU-32 | 3 | Alternar visualmente entre CRC y USD |
| HU-33 | 3 | Modo claro y oscuro con preferencia persistida |
| HU-34 | 5 | Swagger/OpenAPI, versionado y errores controlados |
| HU-35 | 5 | Pruebas, cobertura e integracion continua |
| HU-36 | 5 | Infraestructura de despliegue |
| HU-37 | 3 | Documentacion XP, trazabilidad y defensa |

## Dependencias

- HU-30 precede a HU-31 y HU-32.
- HU-33 depende de la interfaz base adaptable.
- HU-34 depende de las APIs de proveedores, licitaciones, ofertas y aprobaciones.
- HU-35 depende de persistencia, API y reglas principales.
- HU-36 depende de persistencia y pruebas automatizadas.
- HU-37 se actualiza con evidencias reales de todas las iteraciones.

## Criterios de aceptacion principales

- Solo existe un tipo de cambio activo.
- CRC permanece como moneda persistida y USD como visualizacion calculada.
- La fecha del tipo de cambio se muestra al usuario.
- La preferencia de modo claro u oscuro persiste.
- La API esta documentada, versionada y usa ProblemDetails con correlacion.
- La integracion continua ejecuta pruebas y validaciones de dependencias.
- La infraestructura incluye contenedores, health checks y manifiestos Kubernetes.

## Pruebas previstas

- Pruebas unitarias de conversion y activacion de tipo de cambio.
- Pruebas funcionales de alternancia CRC/USD y modo visual.
- Pruebas de integracion de contrato API, ProblemDetails y correlacion.
- Pruebas E2E iniciales.
- Validacion de contenedores, health checks y manifiestos.

## Riesgos

- Endurecer API al final puede revelar inconsistencias de contratos anteriores.
- Pruebas E2E e infraestructura pueden requerir ajustes del entorno local.
- La documentacion final depende de registrar evidencias reales durante las iteraciones previas.

## Resultado demostrable esperado

Version candidata con tipos de cambio, preferencias visuales, API documentada, pruebas automatizadas iniciales e infraestructura lista para demostracion.

## Velocidad observada

Pendiente de completar por el equipo.

## Retroalimentacion del cliente

Pendiente de completar por el equipo.

## Ajustes

Pendiente de completar por el equipo.

## Ciclos TDD

Pendiente de completar por el equipo.

## Refactorizaciones

Pendiente de completar por el equipo.

## Commits y Pull Requests

- Commits: Pendiente.
- Pull Request: Pendiente.

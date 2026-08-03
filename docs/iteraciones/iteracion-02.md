# Iteracion 2 - Licitaciones y persistencia base

- Objetivo: entregar gestion de licitaciones con reglas de codigo, presupuesto, fecha de cierre, estados, vencimiento, persistencia relacional base y API relacionada.
- Duracion uniforme propuesta: Pendiente de completar por el equipo.
- Fecha prevista: Pendiente de completar por el equipo.
- Driver principal: Eithel.
- Navigator principal: Chavala.
- Version prevista: v0.2.0.
- Puntos planificados: 36.

## Historias seleccionadas

| Historia | Puntos | Proposito |
| --- | ---: | --- |
| HU-12 | 5 | Crear licitaciones |
| HU-13 | 3 | Listar y consultar licitaciones |
| HU-14 | 3 | Editar y aplicar borrado logico de licitaciones |
| HU-15 | 5 | Codigo unico, presupuesto y fecha de cierre |
| HU-16 | 5 | Publicar, cerrar y rechazar transiciones invalidas |
| HU-17 | 5 | API REST de licitaciones y cambios de estado |
| HU-18 | 5 | Persistencia relacional base |
| HU-19 | 5 | Auditoria, concurrencia y errores de persistencia |

## Dependencias

- HU-12 precede a HU-13, HU-14, HU-15, HU-16 y HU-17.
- HU-15 prepara reglas requeridas para publicar y cerrar licitaciones.
- HU-18 prepara almacenamiento relacional para HU-19 y modulos posteriores.

## Criterios de aceptacion principales

- Las licitaciones registran presupuesto en CRC y fecha de cierre.
- El codigo unico se valida despues de normalizacion.
- Las transiciones de estado invalidas son rechazadas.
- Una licitacion vencida se trata como cerrada para nuevas ofertas.
- La persistencia base incluye migraciones, datos semilla, restricciones e indices.
- La concurrencia y errores de persistencia se manejan de forma controlada.

## Pruebas previstas

- Pruebas unitarias de normalizacion de codigo, presupuesto, fechas y estados.
- Pruebas de integracion con PostgreSQL real.
- Pruebas de integracion de API de licitaciones y cambios de estado.
- Pruebas de concurrencia optimista y transacciones.

## Riesgos

- Casos de fecha y hora dificiles de reproducir sin estrategia de reloj controlado.
- Reglas de estado incompletas si no se acuerdan ejemplos con el cliente.
- Configuracion de PostgreSQL real puede tomar mas tiempo del previsto.

## Resultado demostrable esperado

Segunda pequena liberacion con licitaciones persistidas, estados verificables, API relacionada y base relacional preparada para ofertas.

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

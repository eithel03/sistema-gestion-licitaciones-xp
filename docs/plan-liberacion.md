# Plan de liberacion

## Objetivo de la liberacion

Entregar incrementalmente un Sistema de Gestion de Licitaciones funcional, probado y defendible, iniciando con catalogos basicos y avanzando hacia reglas de licitacion, ofertas, aprobacion, moneda, API, pruebas automatizadas e infraestructura.

Las versiones indicadas son propuestas de planificacion. Las etiquetas de Git se crearan unicamente cuando cada pequena liberacion sea funcional, demostrable y aceptada por el cliente.

## Alcance funcional

- Navegacion inicial, diseno adaptable y mensajes de usuario.
- Gestion de proveedores con normalizacion, validacion, duplicidad y consulta de ofertas relacionadas.
- Gestion de licitaciones con codigo unico, presupuesto, fecha de cierre, estados, publicacion, cierre y vencimiento.
- Gestion de ofertas con reglas de registro, duplicidad, presupuesto, mejor oferta, desempate y clasificacion.
- Gestion de niveles de aprobacion parametrizables.
- Gestion de tipos de cambio, activacion unica y visualizacion CRC/USD.
- API REST versionada, documentada y con errores controlados.
- Persistencia, pruebas, integracion continua e infraestructura de entrega.

## Restricciones tecnicas

- La implementacion futura usara .NET 9.
- PostgreSQL sera la base de datos persistente.
- CRC sera la moneda persistida y fuente de verdad.
- El sistema debe funcionar sin conexion a Internet para tipos de cambio.
- Los secretos se manejaran mediante variables de entorno o mecanismos equivalentes.
- La Fase 1 no implementa codigo funcional, entidades, controladores, migraciones, contenedores ni manifiestos.

## Practicas XP aplicables

- Planning Game para acordar alcance, prioridad y estimaciones.
- Historias de usuario con pruebas de aceptacion.
- Iteraciones cortas y de duracion uniforme.
- Pequenas liberaciones funcionales.
- TDD con evidencia rojo-verde-refactorizacion.
- Programacion en parejas con driver y navigator.
- Integracion continua.
- Diseno simple.
- Refactorizacion continua.
- Propiedad colectiva del codigo.
- Ritmo sostenible.

## Escala de estimacion

Se usa una escala simple de 1, 2, 3, 5 y 8 puntos. La estimacion representa tamano relativo, incertidumbre y esfuerzo de validacion.

## Prioridades utilizadas

- Alta: necesaria para cumplir el flujo central o una restriccion critica.
- Media: importante para usabilidad, calidad o completitud, pero ajustable segun velocidad XP observada.
- Baja: mejora planificable si existe capacidad disponible.

## Capacidad inicial estimada

La capacidad inicial se define de forma conservadora en cuatro iteraciones uniformes. La planificacion reparte 136 puntos totales:

| Iteracion | Version prevista | Puntos planificados |
| --- | --- | ---: |
| Iteracion 1 | v0.1.0 | 30 |
| Iteracion 2 | v0.2.0 | 36 |
| Iteracion 3 | v0.3.0 | 38 |
| Iteracion 4 | v1.0.0-rc | 32 |

Entrega final posterior propuesta: v1.0.0.

## Distribucion de historias

| Iteracion | Historias |
| --- | --- |
| Iteracion 1 | HU-01, HU-02, HU-03, HU-04, HU-05, HU-06, HU-07, HU-08, HU-09, HU-10 |
| Iteracion 2 | HU-12, HU-13, HU-14, HU-15, HU-16, HU-17, HU-18, HU-19 |
| Iteracion 3 | HU-11, HU-20, HU-21, HU-22, HU-23, HU-24, HU-25, HU-26, HU-27, HU-28, HU-29 |
| Iteracion 4 | HU-30, HU-31, HU-32, HU-33, HU-34, HU-35, HU-36, HU-37 |

## Dependencias principales

- La navegacion base precede a modulos visuales: HU-01 antes de HU-02, HU-03 y HU-33.
- Proveedores precede a ofertas: HU-05 y HU-06 antes de HU-20.
- HU-11 depende de HU-06, HU-20 y HU-21.
- Licitaciones precede a ofertas: HU-12, HU-15 y HU-16 antes de HU-20.
- Persistencia base precede a auditoria, tipos de cambio e infraestructura: HU-18 antes de HU-19, HU-30 y HU-36.
- Mejor oferta y clasificacion preceden a aprobaciones: HU-25 y HU-26 antes de HU-27 y HU-28.
- Endurecimiento de API depende de APIs previas: HU-10, HU-17 y HU-29 antes de HU-34.
- Automatizacion e infraestructura dependen de pruebas y persistencia: HU-18, HU-19, HU-34 y HU-35 antes de HU-36.

## Riesgos principales

- Subestimar reglas de normalizacion Unicode y unicidad.
- Dificultad para probar vencimientos y concurrencia de forma determinista.
- Complejidad acumulada en API si DTO, ProblemDetails y versionado se dejan para muy tarde.
- Sobrecarga de integracion con PostgreSQL real en entornos locales.
- Evidencias incompletas de TDD o programacion en parejas si no se registran durante el trabajo.
- Ajustes de alcance requeridos si la velocidad XP observada difiere de la estimada.

## Criterios para considerar terminada una historia

- Criterios de aceptacion cumplidos y revisados por el cliente o representante disponible.
- Pruebas previstas implementadas y ejecutadas cuando exista codigo funcional.
- Evidencia de TDD registrada para reglas nuevas.
- Codigo integrado mediante Pull Request revisado por la pareja.
- Documentacion y trazabilidad actualizadas.
- Sin errores conocidos de prioridad alta relacionados con la historia.
- Bitacora XP actualizada con driver, navigator, decisiones y resultado.

## Estrategia de pequenas liberaciones

- v0.1.0: primera liberacion pequena con navegacion, proveedores, validaciones basicas y API de proveedores.
- v0.2.0: segunda liberacion pequena con licitaciones, estados, persistencia base y API relacionada.
- v0.3.0: tercera liberacion pequena con ofertas, consulta de ofertas por proveedor, mejor oferta, clasificacion y aprobaciones.
- v1.0.0-rc: version candidata con tipos de cambio, modo visual, API endurecida, pruebas E2E iniciales e infraestructura.
- v1.0.0: entrega final posterior, creada solo si la version candidata es aceptada y demostrable.

## Calculo de velocidad XP

La velocidad XP se calculara al cierre de cada iteracion sumando los puntos de historias terminadas. Una historia cuenta solo si cumple sus criterios de aceptacion, pruebas previstas aplicables y trazabilidad minima.

## Ajuste de planificacion

Despues de cada iteracion, el equipo comparara puntos planificados contra puntos terminados. Si la velocidad XP observada es menor, se reducira alcance de menor prioridad o se moveran historias dependientes. Si es mayor, se podran adelantar historias preparatorias sin romper el plan de liberacion ni ocultar trabajo al cliente.

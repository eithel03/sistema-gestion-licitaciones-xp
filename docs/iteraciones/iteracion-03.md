# Iteracion 3 - Ofertas, mejor oferta y aprobaciones

- Objetivo: entregar gestion de ofertas con reglas de registro, mejor oferta, desempate, clasificacion del ahorro, niveles de aprobacion y API relacionada.
- Duracion uniforme propuesta: Pendiente de completar por el equipo.
- Fecha prevista: Pendiente de completar por el equipo.
- Driver principal: Chavala.
- Navigator principal: Eithel.
- Version prevista: v0.3.0.
- Puntos planificados: 38.

## Historias seleccionadas

| Historia | Puntos | Proposito |
| --- | ---: | --- |
| HU-11 | 2 | Consultar ofertas relacionadas con proveedor |
| HU-20 | 5 | Crear ofertas |
| HU-21 | 3 | Listar, consultar y filtrar ofertas |
| HU-22 | 3 | Editar y eliminar ofertas cuando este permitido |
| HU-23 | 5 | Rechazar duplicadas, vencidas o no publicadas |
| HU-24 | 3 | Validar ofertas contra presupuesto |
| HU-25 | 3 | Determinar mejor oferta y resolver empates |
| HU-26 | 3 | Calcular clasificacion del ahorro |
| HU-27 | 3 | Administrar niveles de aprobacion |
| HU-28 | 5 | Evitar traslapes y determinar aprobador |
| HU-29 | 3 | API REST de ofertas y aprobaciones |

## Dependencias

- HU-20 depende de proveedores y licitaciones publicadas.
- HU-11 depende de HU-06, HU-20 y HU-21.
- HU-21 y HU-22 dependen de HU-20.
- HU-23 y HU-24 dependen de las reglas de estado y presupuesto.
- HU-25 y HU-26 preparan HU-27 y HU-28.
- HU-29 depende de ofertas, mejor oferta y aprobaciones.

## Criterios de aceptacion principales

- Solo se aceptan ofertas validas para licitaciones publicadas y no vencidas.
- Se rechazan ofertas duplicadas y superiores al presupuesto.
- Una oferta igual al presupuesto es permitida si cumple las demas reglas.
- No se permiten cambios en ofertas cerradas.
- La mejor oferta se determina por menor monto y empate por fecha de registro.
- El detalle de proveedor permite consultar sus ofertas relacionadas y muestra un estado vacio cuando no existen.
- Los rangos de aprobacion evitan traslapes y permiten un solo rango abierto.

## Pruebas previstas

- Pruebas unitarias de reglas de oferta, presupuesto, duplicidad y vencimiento.
- Pruebas unitarias de mejor oferta, desempate, ahorro y aprobador.
- Pruebas de integracion de CRUD de ofertas y aprobaciones.
- Pruebas funcionales de filtros y consulta de mejor oferta.
- Prueba funcional e integracion de consulta de ofertas relacionadas desde el proveedor.

## Riesgos

- Interacciones complejas entre estado de licitacion, vencimiento y edicion de ofertas.
- Rangos de aprobacion con limites abiertos pueden generar casos borde.
- La consulta de mejor oferta debe permanecer consistente con concurrencia.

## Resultado demostrable esperado

Tercera pequena liberacion con ofertas registrables, consulta de ofertas por proveedor, reglas economicas verificables, mejor oferta calculada, aprobador determinado y API relacionada.

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

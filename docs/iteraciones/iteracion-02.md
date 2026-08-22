# Iteracion 2 - Licitaciones y persistencia base

> Registro histórico de la Iteración 2. El PR `#12` fue integrado a `main` mediante `fafcc66`. La versión `v0.2.0` fue prevista, pero no existe un tag oficial. Los resultados de ejecución son evidencia histórica y no fueron regenerados en la Fase 9.

- Objetivo: entregar gestion de licitaciones con reglas de codigo, presupuesto, fecha de cierre, estados, vencimiento, persistencia relacional base y API relacionada.
- Duracion uniforme propuesta: Pendiente de completar por el equipo.
- Fecha prevista: Pendiente de completar por el equipo.
- Driver principal: Eithel.
- Navigator principal: Chavala.
- Version prevista: v0.2.0.
- Puntos planificados: 36.
- Rama de trabajo: `feature/iteracion-02-licitaciones`.
- Issue: `#10 - ITER-02: Implementar gestion de licitaciones y persistencia base`.
- Estado de la iteracion: implementada e integrada a `main`; no existe tag oficial.

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


## Evidencia de validación

Se ejecutaron correctamente:

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build
```

### Resultados

* **Restore:** exitoso.
* **Build Release:** exitoso.
* **Pruebas ejecutadas:** 64.
* **Pruebas aprobadas:** 64.
* **Pruebas fallidas:** 0.
* **Pruebas omitidas:** 0.
* **PostgreSQL 16:** disponible mediante Docker.
* **Testcontainers:** ejecutó correctamente las pruebas con PostgreSQL real.
* **Migraciones:** fueron aplicadas correctamente sobre la base local.

### Validación manual MVC

* Crear licitación.
* Verla en el listado.
* Consultar detalle.
* Editarla en estado Borrador.
* Publicarla.
* Intentar una transición inválida y verificar su rechazo.
* Cerrarla correctamente.

### Validación manual API

* `/api/v1/proveedores`: respuesta correcta.
* `/api/v1/licitaciones`: respuesta correcta.
* `/health`: Healthy.

## Resultado demostrable esperado

Segunda pequena liberacion con licitaciones persistidas, estados verificables, API relacionada y base relacional preparada para ofertas.

## Velocidad observada

Los 36 puntos planificados correspondientes a HU-12 hasta HU-19 fueron implementados y validados localmente.

La velocidad definitiva de la iteracion quedara cerrada despues de la revision del navigator, Pull Request, CI remoto y merge a `main`.

## Retroalimentacion del navigator

Durante la revision de la Iteracion 2, el navigator propuso dos mejoras a partir de la validacion manual de la interfaz:

- Ajustar el campo `PresupuestoCrc` para que el formato de entrada fuera coherente con la cultura de Costa Rica (`es-CR`) y permitiera trabajar correctamente con valores decimales mostrados con coma.
- Mejorar la vista de detalle de las licitaciones para mostrar solamente las acciones validas de acuerdo con el estado actual de la licitacion, evitando presentar botones que llevarian a transiciones no permitidas.

Las observaciones fueron revisadas y aplicadas durante la iteracion.

Como resultado:

- En estado `Borrador` se muestran las acciones Editar, Publicar y Retirar.
- En estado `Publicada` se muestra la accion Cerrar.
- En estado `Cerrada` no se muestran acciones de cambio de estado.
- Las reglas de dominio continúan validando y rechazando cualquier transicion invalida independientemente de los controles mostrados en la interfaz.
- El formulario de presupuesto mantiene el valor como `decimal` y utiliza una presentacion coherente con la cultura `es-CR`.

Estas mejoras no modificaron las reglas principales del dominio, sino que mejoraron la consistencia y experiencia de uso de la interfaz.

## Ajustes

- Se corrigio la validacion cliente del codigo de licitacion despues de detectar durante la prueba manual que un codigo valido como `LIC-2026-001` era rechazado por el navegador.
- Se mantuvo la validacion definitiva del codigo dentro de Domain y se ajusto la expresion utilizada por MVC para compatibilidad con la validacion cliente.
- Se configuro la cultura `es-CR` en la aplicacion Web para mantener coherencia con el formato utilizado en Costa Rica.
- Se ajusto el campo `PresupuestoCrc` para permitir la entrada de valores decimales de forma coherente con la cultura configurada.
- Se mejoro la vista de detalle para mostrar solamente las acciones validas segun el estado actual de la licitacion.
- Se aplicaron correctamente las migraciones de proveedores y licitaciones sobre PostgreSQL local.
- Se valido manualmente el flujo completo de crear, listar, consultar, editar, publicar, rechazar una transicion invalida y cerrar una licitacion.

## Ciclos TDD

### Dominio de licitaciones

1. ROJO: las pruebas inicialmente fallaron porque no existian la entidad de licitacion, normalizacion de codigo ni reglas de estado.
2. VERDE: se implementaron la entidad, normalizador, validaciones de codigo, presupuesto y fecha, junto con las operaciones de publicacion y cierre.
3. REFACTOR: se mantuvieron las reglas de negocio dentro de Domain y se separaron los casos de uso en Application.

### Persistencia

1. ROJO: las pruebas requerian persistencia real de licitaciones, restricciones e indices.
2. VERDE: se agregaron configuracion EF Core, repositorio, tabla `Licitaciones`, migracion e indices.
3. REFACTOR: se mantuvo EF Core dentro de Infrastructure y se uso PostgreSQL real mediante Testcontainers para las pruebas de integracion.

### Concurrencia

Se implemento concurrencia optimista mediante `xmin` de PostgreSQL y se tradujeron los conflictos de actualizacion a resultados controlados.

## Refactorizaciones

- Se mantuvieron las reglas de negocio de licitaciones dentro de Domain y los casos de uso dentro de Application.
- Se separo la persistencia mediante repositorio y configuracion EF Core especifica del modulo.
- Se reutilizo `IClock` para las reglas temporales y vencimiento.
- Se ajusto la validacion cliente del codigo de licitacion sin modificar la validacion definitiva del dominio.
- Se configuro la cultura `es-CR` en MVC para mantener una presentacion coherente de valores monetarios.
- Se ajusto el campo `PresupuestoCrc` para evitar inconsistencias entre coma y punto decimal.
- Se simplifico la vista de detalle para mostrar solo las acciones validas segun el estado:
  - Borrador: Editar, Publicar y Retirar.
  - Publicada: Cerrar.
  - Cerrada: sin acciones de cambio de estado.
- Las reglas del dominio continuan rechazando las transiciones invalidas aunque se intente ejecutar una operacion directamente.

## Commits y Pull Requests

- Issue: `#10 - ITER-02: Implementar gestion de licitaciones y persistencia base`.
- Rama: `feature/iteracion-02-licitaciones`.
- Commit de implementacion: `cce95ad` - `feat(licitaciones): implementar gestion de licitaciones`.
- Commit de pruebas: `812b59c` - `test(licitaciones): completar pruebas de iteracion 2`.
- Commit documental: `ed89c5a` - `docs(xp): documentar iteracion 2 y evidencias`.
- Pull Request: `#12 - feat: completar Iteración 2 - Licitaciones y persistencia base`.
- CI remoto: Aprobado.
- Merge a `main`: realizado mediante `fafcc66`.
- Tag previsto históricamente: `v0.2.0`; no existe.

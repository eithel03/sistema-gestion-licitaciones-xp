# GitHub Issues y Milestones propuestos

`gh` no esta disponible en el entorno usado para preparar esta fase. Por esa razon no se crearon Milestones ni Issues reales en GitHub. Este documento deja la propuesta lista para que Chavala la revise y la cree posteriormente si corresponde.

## Milestones propuestos

| Milestone | Version prevista | Objetivo |
| --- | --- | --- |
| Iteracion 1 - v0.1.0 | v0.1.0 | Base navegable y proveedores |
| Iteracion 2 - v0.2.0 | v0.2.0 | Licitaciones y persistencia base |
| Iteracion 3 - v0.3.0 | v0.3.0 | Ofertas, mejor oferta y aprobaciones |
| Iteracion 4 - v1.0.0-rc | v1.0.0-rc | Tipos de cambio, API endurecida e infraestructura |

## Comandos opcionales

```powershell
gh milestone create "Iteracion 1 - v0.1.0" --description "Base navegable y proveedores"
gh milestone create "Iteracion 2 - v0.2.0" --description "Licitaciones y persistencia base"
gh milestone create "Iteracion 3 - v0.3.0" --description "Ofertas, mejor oferta y aprobaciones"
gh milestone create "Iteracion 4 - v1.0.0-rc" --description "Tipos de cambio, API endurecida e infraestructura"
```

Para crear cada Issue despues de revisar el cuerpo:

```powershell
gh issue create --title "HU-XX - Titulo" --milestone "Iteracion N - version" --body-file ruta-del-cuerpo.md
```

## Issues propuestos

### HU-01 - Consultar landing page y navegacion principal

```markdown
## Historia
Como visitante, quiero consultar una landing page y moverme entre los modulos principales, para entender el sistema y acceder rapidamente a cada area.

## Criterios de aceptacion
1. La pagina inicial muestra el nombre del sistema y accesos a los modulos definidos.
2. Cada enlace de navegacion lleva al destino correspondiente o a un estado planificado claramente identificable.

## Estimacion
3 puntos.

## Dependencias
Ninguna.

## Iteracion
Iteracion 1.

## Pruebas previstas
Prueba funcional de navegacion y prueba E2E inicial de enlaces principales.
```

### HU-02 - Usar diseno adaptable en la interfaz base

```markdown
## Historia
Como usuario, quiero usar el sistema desde escritorio y dispositivos pequenos, para trabajar sin depender de un unico tamano de pantalla.

## Criterios de aceptacion
1. La navegacion y el contenido principal se mantienen utilizables en vista de escritorio y movil.
2. Las tablas o listados no ocultan acciones criticas en pantallas pequenas.

## Estimacion
2 puntos.

## Dependencias
HU-01.

## Iteracion
Iteracion 1.

## Pruebas previstas
Revision visual en al menos dos anchos de pantalla y prueba E2E de navegacion adaptable.
```

### HU-03 - Visualizar mensajes de exito, advertencia y error

```markdown
## Historia
Como usuario, quiero recibir mensajes claros de exito, advertencia y error, para conocer el resultado de mis acciones.

## Criterios de aceptacion
1. Las operaciones exitosas muestran un mensaje de confirmacion visible.
2. Las validaciones y errores controlados muestran mensajes comprensibles sin datos sensibles.

## Estimacion
2 puntos.

## Dependencias
HU-01.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas funcionales de mensajes en formularios y respuestas de error.
```

### HU-04 - Aplicar paginacion, filtrado y ordenamiento base

```markdown
## Historia
Como usuario, quiero paginar, filtrar y ordenar listados, para encontrar informacion sin revisar todos los registros manualmente.

## Criterios de aceptacion
1. Un listado permite cambiar pagina, aplicar un filtro textual y ordenar por una columna permitida.
2. Los parametros seleccionados se reflejan de forma verificable en la consulta o respuesta.

## Estimacion
3 puntos.

## Dependencias
HU-01.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas funcionales de listado y pruebas de integracion para parametros de consulta.
```

### HU-05 - Crear proveedores

```markdown
## Historia
Como usuario administrativo, quiero registrar proveedores, para asociarlos posteriormente con ofertas.

## Criterios de aceptacion
1. Un proveedor valido queda registrado y disponible para consulta.
2. Un proveedor invalido es rechazado con mensajes de validacion verificables.

## Estimacion
3 puntos.

## Dependencias
HU-01.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas unitarias de validacion y pruebas de integracion de creacion.
```

### HU-06 - Listar y consultar proveedores

```markdown
## Historia
Como usuario administrativo, quiero listar y consultar proveedores, para revisar sus datos antes de usarlos en ofertas.

## Criterios de aceptacion
1. El listado muestra proveedores registrados con datos suficientes para identificarlos.
2. La consulta de detalle muestra un proveedor existente o informa que no fue encontrado.

## Estimacion
2 puntos.

## Dependencias
HU-05.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas funcionales de listado y consulta, pruebas de integracion de busqueda por identificador.
```

### HU-07 - Editar y aplicar borrado logico de proveedores

```markdown
## Historia
Como usuario administrativo, quiero editar proveedores y retirarlos mediante borrado logico, para mantener el catalogo actualizado sin perder historial.

## Criterios de aceptacion
1. Un proveedor existente puede actualizar sus datos permitidos.
2. Un proveedor retirado deja de aparecer como seleccionable sin perder su registro historico.

## Estimacion
3 puntos.

## Dependencias
HU-05, HU-06.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas de integracion de actualizacion y borrado logico.
```

### HU-08 - Validar nombre unico y normalizado de proveedor

```markdown
## Historia
Como usuario administrativo, quiero que el sistema valide nombres de proveedor equivalentes, para evitar duplicados por espacios, Unicode o mayusculas y minusculas.

## Criterios de aceptacion
1. Dos nombres equivalentes despues de normalizarse no pueden registrarse como proveedores distintos.
2. El nombre almacenado conserva una presentacion definida y una clave normalizada para comparacion.

## Estimacion
5 puntos.

## Dependencias
HU-05.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas unitarias con casos de espacios, Unicode y mayusculas; prueba de integracion de restriccion unica.
```

### HU-09 - Validar caracteres permitidos en proveedores

```markdown
## Historia
Como usuario administrativo, quiero que los nombres de proveedor acepten solo caracteres permitidos, para mejorar la calidad de los datos.

## Criterios de aceptacion
1. Un nombre con caracteres permitidos es aceptado.
2. Un nombre con caracteres no permitidos es rechazado con un mensaje verificable.

## Estimacion
2 puntos.

## Dependencias
HU-05.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas unitarias de caracteres validos e invalidos.
```

### HU-10 - Exponer API REST basica de proveedores

```markdown
## Historia
Como consumidor de API, quiero crear, consultar, actualizar y retirar proveedores mediante endpoints REST, para integrar el catalogo con otros componentes.

## Criterios de aceptacion
1. La API permite operaciones CRUD de proveedores usando DTO de entrada y salida.
2. Las respuestas usan codigos HTTP adecuados y no exponen datos sensibles en errores.

## Estimacion
5 puntos.

## Dependencias
HU-05, HU-06, HU-07, HU-08.

## Iteracion
Iteracion 1.

## Pruebas previstas
Pruebas de integracion de endpoints de proveedores.
```

### HU-11 - Consultar ofertas relacionadas con proveedor

```markdown
## Historia
Como usuario administrativo, quiero consultar las ofertas asociadas a un proveedor, para revisar su participacion en licitaciones.

## Criterios de aceptacion
1. El detalle de un proveedor muestra las ofertas asociadas.
2. Si el proveedor no tiene ofertas, la seccion muestra un estado vacio comprensible.
3. La consulta respeta las relaciones persistidas entre proveedor y oferta.

## Estimacion
2 puntos.

## Dependencias
HU-06, HU-20, HU-21.

## Iteracion
Iteracion 3.

## Pruebas previstas
Prueba funcional e integracion de consulta de ofertas relacionadas desde el proveedor.
```

### HU-12 - Crear licitaciones

```markdown
## Historia
Como usuario administrativo, quiero crear licitaciones, para publicar oportunidades de compra con presupuesto y fecha de cierre.

## Criterios de aceptacion
1. Una licitacion valida se registra con presupuesto en CRC y estado inicial definido.
2. Una licitacion incompleta o invalida es rechazada con mensajes verificables.

## Estimacion
5 puntos.

## Dependencias
HU-01.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas unitarias de reglas base y pruebas de integracion de creacion.
```

### HU-13 - Listar y consultar licitaciones

```markdown
## Historia
Como usuario administrativo, quiero listar y consultar licitaciones, para revisar su informacion y seguimiento.

## Criterios de aceptacion
1. El listado muestra codigo, presupuesto, fecha de cierre y estado.
2. El detalle informa una licitacion existente o devuelve una respuesta controlada si no existe.

## Estimacion
3 puntos.

## Dependencias
HU-12, HU-04.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas funcionales y pruebas de integracion de consulta.
```

### HU-14 - Editar y aplicar borrado logico de licitaciones

```markdown
## Historia
Como usuario administrativo, quiero editar licitaciones y retirarlas cuando corresponda, para corregir informacion sin romper el historial.

## Criterios de aceptacion
1. Una licitacion en estado permitido puede modificar campos editables.
2. Una licitacion retirada deja de participar en operaciones activas sin perder trazabilidad.

## Estimacion
3 puntos.

## Dependencias
HU-12, HU-13.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas unitarias de permisos de edicion y pruebas de integracion.
```

### HU-15 - Validar codigo unico, presupuesto y fecha de cierre

```markdown
## Historia
Como usuario administrativo, quiero validar codigo unico normalizado, presupuesto y fecha de cierre, para crear licitaciones consistentes.

## Criterios de aceptacion
1. Dos codigos equivalentes despues de normalizarse no pueden repetirse.
2. La fecha y hora de cierre se seleccionan de forma verificable y el presupuesto debe ser valido.

## Estimacion
5 puntos.

## Dependencias
HU-12.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas unitarias de normalizacion, presupuesto y fechas; prueba funcional de selector de fecha y hora.
```

### HU-16 - Publicar, cerrar y rechazar transiciones invalidas

```markdown
## Historia
Como usuario administrativo, quiero publicar y cerrar licitaciones con reglas de estado, para controlar cuando reciben ofertas.

## Criterios de aceptacion
1. Una licitacion puede pasar a publicada o cerrada solo si cumple las reglas definidas.
2. Una licitacion vencida se considera cerrada para nuevas ofertas aunque su cambio formal este pendiente.

## Estimacion
5 puntos.

## Dependencias
HU-12, HU-15.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas unitarias de maquina de estados y pruebas de integracion de cambio de estado.
```

### HU-17 - Exponer API REST de licitaciones y cambios de estado

```markdown
## Historia
Como consumidor de API, quiero administrar licitaciones y cambiar su estado mediante endpoints REST, para integrar el proceso de licitacion.

## Criterios de aceptacion
1. La API permite CRUD de licitaciones y cambio de estado con codigos HTTP correctos.
2. Las transiciones invalidas devuelven errores controlados sin datos sensibles.

## Estimacion
5 puntos.

## Dependencias
HU-12, HU-13, HU-14, HU-16.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas de integracion de endpoints y casos de error.
```

### HU-18 - Preparar persistencia relacional base

```markdown
## Historia
Como programador, quiero preparar persistencia con PostgreSQL, migraciones, datos semilla, restricciones e indices, para sostener las reglas de los modulos iniciales.

## Criterios de aceptacion
1. Las tablas planificadas cuentan con migraciones, restricciones e indices coherentes con las reglas.
2. Los datos semilla minimos permiten ejecutar pruebas de integracion reproducibles.

## Estimacion
5 puntos.

## Dependencias
HU-05, HU-12.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas de integracion con PostgreSQL real y verificacion de migraciones.
```

### HU-19 - Manejar auditoria, concurrencia y errores de persistencia

```markdown
## Historia
Como programador, quiero manejar auditoria, concurrencia optimista, transacciones y errores de persistencia, para proteger la consistencia de los datos.

## Criterios de aceptacion
1. Las operaciones criticas registran auditoria y detectan conflictos de concurrencia.
2. Los errores de persistencia se transforman en respuestas controladas sin detalles internos.

## Estimacion
5 puntos.

## Dependencias
HU-18.

## Iteracion
Iteracion 2.

## Pruebas previstas
Pruebas de integracion de concurrencia, transacciones y errores controlados.
```

### HU-20 - Crear ofertas

```markdown
## Historia
Como proveedor o usuario administrativo, quiero registrar ofertas para una licitacion publicada, para participar en el proceso de seleccion.

## Criterios de aceptacion
1. Una oferta valida queda asociada a una licitacion publicada y a un proveedor existente.
2. Una oferta invalida es rechazada con criterios de error verificables.

## Estimacion
5 puntos.

## Dependencias
HU-05, HU-12, HU-16.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de reglas de registro y pruebas de integracion de creacion.
```

### HU-21 - Listar, consultar y filtrar ofertas

```markdown
## Historia
Como usuario administrativo, quiero listar, consultar y filtrar ofertas por licitacion y proveedor, para analizar la participacion registrada.

## Criterios de aceptacion
1. El listado permite filtrar por licitacion y proveedor.
2. El detalle muestra proveedor, licitacion, monto y fecha de registro.

## Estimacion
3 puntos.

## Dependencias
HU-20, HU-04.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas funcionales de filtros y pruebas de integracion de consulta.
```

### HU-22 - Editar y eliminar ofertas cuando este permitido

```markdown
## Historia
Como usuario administrativo, quiero editar o eliminar ofertas solo cuando las reglas lo permitan, para corregir errores sin afectar licitaciones cerradas.

## Criterios de aceptacion
1. Una oferta abierta puede modificarse o retirarse segun reglas definidas.
2. Una oferta cerrada o asociada a licitacion cerrada rechaza cambios.

## Estimacion
3 puntos.

## Dependencias
HU-20, HU-16.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de permisos y pruebas de integracion.
```

### HU-23 - Rechazar ofertas duplicadas, vencidas o no publicadas

```markdown
## Historia
Como usuario administrativo, quiero que el sistema rechace ofertas duplicadas, vencidas o sobre licitaciones no publicadas, para asegurar competencia valida.

## Criterios de aceptacion
1. Una segunda oferta del mismo proveedor para la misma licitacion es rechazada.
2. Una oferta vencida o asociada a una licitacion no publicada es rechazada.

## Estimacion
5 puntos.

## Dependencias
HU-20, HU-16.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de duplicidad, vencimiento y estado; pruebas de integracion.
```

### HU-24 - Validar ofertas contra presupuesto

```markdown
## Historia
Como usuario administrativo, quiero rechazar ofertas superiores al presupuesto y aceptar ofertas iguales, para cumplir la regla economica de la licitacion.

## Criterios de aceptacion
1. Una oferta mayor al presupuesto es rechazada.
2. Una oferta igual al presupuesto es aceptada si cumple las demas reglas.

## Estimacion
3 puntos.

## Dependencias
HU-20, HU-12.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de limite presupuestario y pruebas de integracion.
```

### HU-25 - Determinar mejor oferta y resolver empates

```markdown
## Historia
Como usuario administrativo, quiero determinar la mejor oferta y resolver empates por fecha de registro, para identificar la opcion ganadora de forma transparente.

## Criterios de aceptacion
1. La mejor oferta corresponde al menor monto valido de la licitacion.
2. Si dos ofertas tienen el mismo monto, gana la de fecha de registro mas temprana.

## Estimacion
3 puntos.

## Dependencias
HU-20, HU-21.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de seleccion y desempate; prueba de integracion de consulta.
```

### HU-26 - Calcular clasificacion del ahorro

```markdown
## Historia
Como usuario administrativo, quiero calcular la clasificacion del ahorro de una oferta, para interpretar el impacto economico frente al presupuesto.

## Criterios de aceptacion
1. El sistema calcula el ahorro en CRC y su porcentaje respecto al presupuesto.
2. El resultado muestra una clasificacion verificable segun la regla definida.

## Estimacion
3 puntos.

## Dependencias
HU-24, HU-25.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de calculo y prueba funcional de visualizacion.
```

### HU-27 - Administrar niveles de aprobacion

```markdown
## Historia
Como usuario administrativo, quiero crear, listar, consultar, editar y eliminar niveles de aprobacion, para parametrizar quien aprueba segun el ahorro o monto definido.

## Criterios de aceptacion
1. El sistema permite crear, listar, consultar, editar y eliminar niveles de aprobacion cuando la integridad de los datos lo permita.
2. Una eliminacion no debe romper relaciones ni dejar datos inconsistentes.
3. Los errores de integridad se muestran mediante mensajes controlados.

## Estimacion
3 puntos.

## Dependencias
HU-26.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas de integracion de CRUD y validaciones basicas.
```

### HU-28 - Evitar traslapes y determinar aprobador

```markdown
## Historia
Como usuario administrativo, quiero evitar traslapes, permitir solo un rango abierto y determinar el aprobador desde una tabla parametrizable, para mantener reglas de aprobacion consistentes.

## Criterios de aceptacion
1. Un rango traslapado con otro existente es rechazado.
2. Para una oferta evaluada, el sistema determina el aprobador usando el rango vigente.

## Estimacion
5 puntos.

## Dependencias
HU-27.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas unitarias de rangos, rango abierto y seleccion de aprobador.
```

### HU-29 - Exponer API REST de ofertas y aprobaciones

```markdown
## Historia
Como consumidor de API, quiero administrar ofertas, consultar ofertas de una licitacion, consultar mejor oferta y administrar niveles de aprobacion, para integrar el analisis de adjudicacion.

## Criterios de aceptacion
1. La API expone CRUD de ofertas y niveles de aprobacion.
2. La API permite consultar ofertas por licitacion y obtener mejor oferta con clasificacion y aprobador.

## Estimacion
3 puntos.

## Dependencias
HU-21, HU-25, HU-27, HU-28.

## Iteracion
Iteracion 3.

## Pruebas previstas
Pruebas de integracion de endpoints y casos de error.
```

### HU-30 - Administrar tipos de cambio

```markdown
## Historia
Como usuario administrativo, quiero crear, listar, consultar, editar y eliminar tipos de cambio, para mantener conversiones CRC/USD administradas por el sistema.

## Criterios de aceptacion
1. El sistema permite CRUD de tipos de cambio con fecha y valor.
2. El modulo funciona con datos administrados localmente sin requerir Internet.

## Estimacion
5 puntos.

## Dependencias
HU-18.

## Iteracion
Iteracion 4.

## Pruebas previstas
Pruebas de integracion de CRUD y pruebas unitarias de validacion.
```

### HU-31 - Activar un unico tipo de cambio

```markdown
## Historia
Como usuario administrativo, quiero activar un tipo de cambio y garantizar que solo exista uno activo, para tener una conversion vigente unica.

## Criterios de aceptacion
1. Al activar un tipo de cambio, ningun otro queda activo.
2. Si no hay tipo de cambio activo, el sistema informa la situacion de forma controlada.

## Estimacion
3 puntos.

## Dependencias
HU-30.

## Iteracion
Iteracion 4.

## Pruebas previstas
Pruebas unitarias de activacion unica y pruebas de integracion.
```

### HU-32 - Alternar visualmente entre CRC y USD

```markdown
## Historia
Como usuario, quiero alternar visualmente montos entre CRC y USD, para interpretar presupuestos y ofertas en ambas monedas.

## Criterios de aceptacion
1. El usuario puede alternar montos visibles entre CRC y USD.
2. La interfaz muestra la fecha del tipo de cambio usado y mantiene CRC como valor persistido.

## Estimacion
3 puntos.

## Dependencias
HU-30, HU-31.

## Iteracion
Iteracion 4.

## Pruebas previstas
Pruebas unitarias de conversion y prueba funcional de alternancia visual.
```

### HU-33 - Alternar modo claro y oscuro con preferencia persistida

```markdown
## Historia
Como usuario, quiero alternar entre modo claro y oscuro y conservar mi preferencia, para usar el sistema con comodidad visual.

## Criterios de aceptacion
1. El usuario puede cambiar entre modo claro y modo oscuro desde la interfaz.
2. La preferencia seleccionada se conserva al recargar o volver a entrar al sistema.

## Estimacion
3 puntos.

## Dependencias
HU-01, HU-02.

## Iteracion
Iteracion 4.

## Pruebas previstas
Prueba funcional de alternancia y persistencia local de preferencia.
```

### HU-34 - Documentar y endurecer la API REST

```markdown
## Historia
Como consumidor de API, quiero contar con Swagger/OpenAPI, versionado, ProblemDetails, codigos HTTP correctos, identificador de correlacion y errores sin datos sensibles, para integrar la API con confianza.

## Criterios de aceptacion
1. Swagger/OpenAPI documenta endpoints versionados y DTO principales.
2. Los errores usan ProblemDetails, codigos HTTP correctos, correlacion y omiten datos sensibles.

## Estimacion
5 puntos.

## Dependencias
HU-10, HU-17, HU-29.

## Iteracion
Iteracion 4.

## Pruebas previstas
Pruebas de integracion de contrato, errores y correlacion.
```

### HU-35 - Automatizar pruebas, cobertura e integracion continua

```markdown
## Historia
Como programador, quiero ejecutar pruebas unitarias, integracion con PostgreSQL real, funcionales E2E y cobertura minima en integracion continua, para sostener TDD y calidad verificable.

## Criterios de aceptacion
1. La integracion continua ejecuta pruebas unitarias, integracion y E2E iniciales con resultado visible.
2. La evidencia de TDD, cobertura y revision de dependencias queda registrada en la documentacion correspondiente.

## Estimacion
5 puntos.

## Dependencias
HU-18, HU-19, HU-34.

## Iteracion
Iteracion 4.

## Pruebas previstas
Ejecucion automatizada de suites de prueba y reporte de cobertura.
```

### HU-36 - Preparar infraestructura de despliegue

```markdown
## Historia
Como programador, quiero preparar Dockerfile multi-stage, Docker Compose, PostgreSQL persistente, health checks y manifiestos Kubernetes, para entregar una version candidata ejecutable.

## Criterios de aceptacion
1. La aplicacion puede ejecutarse con contenedores y PostgreSQL persistente usando variables de entorno.
2. Los manifiestos de Kubernetes incluyen configuracion, secretos, probes, volumen persistente y recursos definidos.

## Estimacion
5 puntos.

## Dependencias
HU-18, HU-35.

## Iteracion
Iteracion 4.

## Pruebas previstas
Prueba de arranque con contenedores, health checks y validacion de manifiestos.
```

### HU-37 - Mantener documentacion XP, trazabilidad y preparacion de defensa

```markdown
## Historia
Como equipo, quiero mantener documentacion modular, bitacora XP, evidencias de pareja, trazabilidad, uso de inteligencia artificial y preparacion de entrega, para defender las decisiones del proyecto.

## Criterios de aceptacion
1. La documentacion permite navegar historias, planes, bitacora, trazabilidad y documentos tecnicos previstos.
2. Las evidencias reales se registran sin inventar Issues, Pull Requests, commits ni pruebas.

## Estimacion
3 puntos.

## Dependencias
Ninguna.

## Iteracion
Iteracion 4.

## Pruebas previstas
Revision documental de enlaces, trazabilidad y consistencia metodologica XP.
```

## Pendiente de creacion real

- Milestones: pendientes.
- Issues HU-01 a HU-37: pendientes.
- Asignacion a Pull Requests: pendiente.
- Cierre de Issues: pendiente hasta implementar y aceptar cada historia.

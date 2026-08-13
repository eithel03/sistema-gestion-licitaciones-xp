# Historias de usuario

Estas historias fueron identificadas, divididas, priorizadas y estimadas durante el Planning Game de XP del Sistema de Gestion de Licitaciones. La planificacion inicial busca que cada historia pueda ser entendida por el cliente, implementada por los programadores en una iteracion corta y verificada mediante pruebas de aceptacion.

## Escala y convenciones

- Estimacion: 1, 2, 3, 5 u 8 puntos de historia.
- Prioridad: Alta, Media o Baja.
- Estado inicial: Planificada.
- Evidencias reales: Issue, Pull Request, commits y pruebas se completaran cuando la historia sea implementada.

## Historias

### HU-01 - Consultar landing page y navegacion principal

- Historia: Como visitante, quiero consultar una landing page y moverme entre los modulos principales, para entender el sistema y acceder rapidamente a cada area.
- Descripcion: Incluye entrada inicial, menu principal y enlaces a proveedores, licitaciones, ofertas, aprobaciones, tipos de cambio y documentacion visible para el equipo.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: Ninguna.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. La pagina inicial muestra el nombre del sistema y accesos a los modulos definidos.
  2. Cada enlace de navegacion lleva al destino correspondiente o a un estado planificado claramente identificable.
- Pruebas previstas: Prueba funcional de navegacion y prueba E2E inicial de enlaces principales.
- Modulos relacionados: Interfaz, navegacion.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorMvcTests.LandingPageAndProviderListAreAvailable`.
- Estado: Implementada en Iteracion 1.

### HU-02 - Usar diseno adaptable en la interfaz base

- Historia: Como usuario, quiero usar el sistema desde escritorio y dispositivos pequenos, para trabajar sin depender de un unico tamano de pantalla.
- Descripcion: Define comportamiento adaptable para navegacion, tablas, formularios y mensajes iniciales.
- Prioridad: Alta.
- Estimacion: 2 puntos.
- Dependencias: HU-01.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. La navegacion y el contenido principal se mantienen utilizables en vista de escritorio y movil.
  2. Las tablas o listados no ocultan acciones criticas en pantallas pequenas.
- Pruebas previstas: Revision visual en al menos dos anchos de pantalla y prueba E2E de navegacion adaptable.
- Modulos relacionados: Interfaz, navegacion.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: revision responsive en vistas MVC y cobertura funcional mediante `ProveedorMvcTests`.
- Estado: Implementada en Iteracion 1.

### HU-03 - Visualizar mensajes de exito, advertencia y error

- Historia: Como usuario, quiero recibir mensajes claros de exito, advertencia y error, para conocer el resultado de mis acciones.
- Descripcion: Establece un patron comun de mensajes para operaciones de formularios, validaciones y errores controlados.
- Prioridad: Alta.
- Estimacion: 2 puntos.
- Dependencias: HU-01.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. Las operaciones exitosas muestran un mensaje de confirmacion visible.
  2. Las validaciones y errores controlados muestran mensajes comprensibles sin datos sensibles.
- Pruebas previstas: Pruebas funcionales de mensajes en formularios y respuestas de error.
- Modulos relacionados: Interfaz, validaciones.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorMvcTests.CreateEditAndRejectDuplicateProviderThroughMvc`.
- Estado: Implementada en Iteracion 1.

### HU-04 - Aplicar paginacion, filtrado y ordenamiento base

- Historia: Como usuario, quiero paginar, filtrar y ordenar listados, para encontrar informacion sin revisar todos los registros manualmente.
- Descripcion: Define el comportamiento comun para listados y su posterior exposicion por API.
- Prioridad: Media.
- Estimacion: 3 puntos.
- Dependencias: HU-01.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. Un listado permite cambiar pagina, aplicar un filtro textual y ordenar por una columna permitida.
  2. Los parametros seleccionados se reflejan de forma verificable en la consulta o respuesta.
- Pruebas previstas: Pruebas funcionales de listado y pruebas de integracion para parametros de consulta.
- Modulos relacionados: Interfaz, API REST.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorServiceTests.ListFiltersSortsAndPaginatesProviders`.
- Estado: Implementada en Iteracion 1.

### HU-05 - Crear proveedores

- Historia: Como usuario administrativo, quiero registrar proveedores, para asociarlos posteriormente con ofertas.
- Descripcion: Incluye formulario o contrato de entrada con nombre y datos minimos definidos por el equipo.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-01.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. Un proveedor valido queda registrado y disponible para consulta.
  2. Un proveedor invalido es rechazado con mensajes de validacion verificables.
- Pruebas previstas: Pruebas unitarias de validacion y pruebas de integracion de creacion.
- Modulos relacionados: Proveedores.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorTests`, `ProveedorServiceTests.CreateReturnsCreatedProvider`, `ProveedorApiTests.CreateProviderReturnsCreatedAndCanBeRead`.
- Estado: Implementada en Iteracion 1.

### HU-06 - Listar y consultar proveedores

- Historia: Como usuario administrativo, quiero listar y consultar proveedores, para revisar sus datos antes de usarlos en ofertas.
- Descripcion: Incluye listado paginado y vista de detalle de proveedor.
- Prioridad: Alta.
- Estimacion: 2 puntos.
- Dependencias: HU-05.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. El listado muestra proveedores registrados con datos suficientes para identificarlos.
  2. La consulta de detalle muestra un proveedor existente o informa que no fue encontrado.
- Pruebas previstas: Pruebas funcionales de listado y consulta, pruebas de integracion de busqueda por identificador.
- Modulos relacionados: Proveedores.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorPersistenceTests.SavesAndRetrievesProveedor`, `ProveedorMvcTests.LandingPageAndProviderListAreAvailable`.
- Estado: Implementada en Iteracion 1.

### HU-07 - Editar y aplicar borrado logico de proveedores

- Historia: Como usuario administrativo, quiero editar proveedores y retirarlos mediante borrado logico, para mantener el catalogo actualizado sin perder historial.
- Descripcion: El borrado logico evita eliminar datos relacionados con ofertas o auditoria.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-05, HU-06.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. Un proveedor existente puede actualizar sus datos permitidos.
  2. Un proveedor retirado deja de aparecer como seleccionable sin perder su registro historico.
- Pruebas previstas: Pruebas de integracion de actualizacion y borrado logico.
- Modulos relacionados: Proveedores, persistencia.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorTests.RenameUpdatesNameAndTimestamp`, `ProveedorPersistenceTests.RetiresProveedorWithLogicalDelete`, `ProveedorApiTests.UpdateAndDeleteProviderUseExpectedStatusCodes`.
- Estado: Implementada en Iteracion 1.

### HU-08 - Validar nombre unico y normalizado de proveedor

- Historia: Como usuario administrativo, quiero que el sistema valide nombres de proveedor equivalentes, para evitar duplicados por espacios, Unicode o mayusculas y minusculas.
- Descripcion: La normalizacion considera espacios redundantes, equivalencia Unicode y comparacion sin sensibilidad a mayusculas.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-05.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. Dos nombres equivalentes despues de normalizarse no pueden registrarse como proveedores distintos.
  2. El nombre almacenado conserva una presentacion definida y una clave normalizada para comparacion.
- Pruebas previstas: Pruebas unitarias con casos de espacios, Unicode y mayusculas; prueba de integracion de restriccion unica.
- Modulos relacionados: Proveedores, persistencia.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorTests.NormalizedNameIgnoresCaseAndRepeatedSpaces`, `ProveedorPersistenceTests.UniqueIndexRejectsEquivalentNormalizedName`.
- Estado: Implementada en Iteracion 1.

### HU-09 - Validar caracteres permitidos en proveedores

- Historia: Como usuario administrativo, quiero que los nombres de proveedor acepten solo caracteres permitidos, para mejorar la calidad de los datos.
- Descripcion: Define reglas explicitas para letras, numeros, espacios y signos aceptados por el dominio.
- Prioridad: Media.
- Estimacion: 2 puntos.
- Dependencias: HU-05.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. Un nombre con caracteres permitidos es aceptado.
  2. Un nombre con caracteres no permitidos es rechazado con un mensaje verificable.
- Pruebas previstas: Pruebas unitarias de caracteres validos e invalidos.
- Modulos relacionados: Proveedores, validaciones.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorTests.CreateAcceptsAllowedCharacters`, `ProveedorTests.CreateRejectsDisallowedCharacters`.
- Estado: Implementada en Iteracion 1.

### HU-10 - Exponer API REST basica de proveedores

- Historia: Como consumidor de API, quiero crear, consultar, actualizar y retirar proveedores mediante endpoints REST, para integrar el catalogo con otros componentes.
- Descripcion: Incluye DTO, codigos HTTP correctos y errores controlados para proveedores.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-05, HU-06, HU-07, HU-08.
- Iteracion asignada: Iteracion 1.
- Criterios de aceptacion:
  1. La API permite operaciones CRUD de proveedores usando DTO de entrada y salida.
  2. Las respuestas usan codigos HTTP adecuados y no exponen datos sensibles en errores.
- Pruebas previstas: Pruebas de integracion de endpoints de proveedores.
- Modulos relacionados: Proveedores, API REST.
- Issue: Pendiente.
- Pull Request: `#9`.
- Commits: `5696a0f`.
- Pruebas ejecutadas: `ProveedorApiTests`.
- Estado: Implementada en Iteracion 1.

### HU-11 - Consultar ofertas relacionadas con proveedor

- Historia: Como usuario administrativo, quiero consultar las ofertas asociadas a un proveedor, para revisar su participacion en licitaciones.
- Descripcion: Permite revisar desde el detalle del proveedor las ofertas persistidas que se relacionan con ese proveedor.
- Prioridad: Media.
- Estimacion: 2 puntos.
- Dependencias: HU-06, HU-20, HU-21.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. El detalle de un proveedor muestra las ofertas asociadas.
  2. Si el proveedor no tiene ofertas, la seccion muestra un estado vacio comprensible.
  3. La consulta respeta las relaciones persistidas entre proveedor y oferta.
- Pruebas previstas: Prueba funcional e integracion de consulta de ofertas relacionadas desde el proveedor.
- Modulos relacionados: Proveedores, ofertas.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `Iteration3MvcTests`, `Iteration3PersistenceTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-12 - Crear licitaciones

- Historia: Como usuario administrativo, quiero crear licitaciones, para publicar oportunidades de compra con presupuesto y fecha de cierre.
- Descripcion: Incluye datos base, presupuesto en CRC y estado inicial no publicado.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-01.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. Una licitacion valida se registra con presupuesto en CRC y estado inicial definido.
  2. Una licitacion incompleta o invalida es rechazada con mensajes verificables.
- Pruebas previstas: Pruebas unitarias de reglas base y pruebas de integracion de creacion.
- Modulos relacionados: Licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-13 - Listar y consultar licitaciones

- Historia: Como usuario administrativo, quiero listar y consultar licitaciones, para revisar su informacion y seguimiento.
- Descripcion: Incluye listado con paginacion, filtros basicos, ordenamiento y detalle.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-12, HU-04.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. El listado muestra codigo, presupuesto, fecha de cierre y estado.
  2. El detalle informa una licitacion existente o devuelve una respuesta controlada si no existe.
- Pruebas previstas: Pruebas funcionales y pruebas de integracion de consulta.
- Modulos relacionados: Licitaciones, API REST.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-14 - Editar y aplicar borrado logico de licitaciones

- Historia: Como usuario administrativo, quiero editar licitaciones y retirarlas cuando corresponda, para corregir informacion sin romper el historial.
- Descripcion: Las ediciones se limitan segun estado y relaciones existentes.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-12, HU-13.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. Una licitacion en estado permitido puede modificar campos editables.
  2. Una licitacion retirada deja de participar en operaciones activas sin perder trazabilidad.
- Pruebas previstas: Pruebas unitarias de permisos de edicion y pruebas de integracion.
- Modulos relacionados: Licitaciones, persistencia.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-15 - Validar codigo unico, presupuesto y fecha de cierre

- Historia: Como usuario administrativo, quiero validar codigo unico normalizado, presupuesto y fecha de cierre, para crear licitaciones consistentes.
- Descripcion: Incluye seleccion de fecha y hora mediante calendario y comparacion normalizada del codigo.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-12.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. Dos codigos equivalentes despues de normalizarse no pueden repetirse.
  2. La fecha y hora de cierre se seleccionan de forma verificable y el presupuesto debe ser valido.
- Pruebas previstas: Pruebas unitarias de normalizacion, presupuesto y fechas; prueba funcional de selector de fecha y hora.
- Modulos relacionados: Licitaciones, validaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-16 - Publicar, cerrar y rechazar transiciones invalidas

- Historia: Como usuario administrativo, quiero publicar y cerrar licitaciones con reglas de estado, para controlar cuando reciben ofertas.
- Descripcion: Rechaza transiciones no permitidas y trata como cerrada una licitacion vencida.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-12, HU-15.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. Una licitacion puede pasar a publicada o cerrada solo si cumple las reglas definidas.
  2. Una licitacion vencida se considera cerrada para nuevas ofertas aunque su cambio formal este pendiente.
- Pruebas previstas: Pruebas unitarias de maquina de estados y pruebas de integracion de cambio de estado.
- Modulos relacionados: Licitaciones, ofertas.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-17 - Exponer API REST de licitaciones y cambios de estado

- Historia: Como consumidor de API, quiero administrar licitaciones y cambiar su estado mediante endpoints REST, para integrar el proceso de licitacion.
- Descripcion: Incluye CRUD, endpoints de publicacion y cierre, DTO y respuestas controladas.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-12, HU-13, HU-14, HU-16.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. La API permite CRUD de licitaciones y cambio de estado con codigos HTTP correctos.
  2. Las transiciones invalidas devuelven errores controlados sin datos sensibles.
- Pruebas previstas: Pruebas de integracion de endpoints y casos de error.
- Modulos relacionados: Licitaciones, API REST.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-18 - Preparar persistencia relacional base

- Historia: Como programador, quiero preparar persistencia con PostgreSQL, migraciones, datos semilla, restricciones e indices, para sostener las reglas de los modulos iniciales.
- Descripcion: Historia tecnica preparatoria para persistir proveedores y licitaciones con integridad.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-05, HU-12.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. Las tablas planificadas cuentan con migraciones, restricciones e indices coherentes con las reglas.
  2. Los datos semilla minimos permiten ejecutar pruebas de integracion reproducibles.
- Pruebas previstas: Pruebas de integracion con PostgreSQL real y verificacion de migraciones.
- Modulos relacionados: Persistencia, proveedores, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-19 - Manejar auditoria, concurrencia y errores de persistencia

- Historia: Como programador, quiero manejar auditoria, concurrencia optimista, transacciones y errores de persistencia, para proteger la consistencia de los datos.
- Descripcion: Incluye campos de auditoria, control de concurrencia y mensajes controlados ante conflictos.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-18.
- Iteracion asignada: Iteracion 2.
- Criterios de aceptacion:
  1. Las operaciones criticas registran auditoria y detectan conflictos de concurrencia.
  2. Los errores de persistencia se transforman en respuestas controladas sin detalles internos.
- Pruebas previstas: Pruebas de integracion de concurrencia, transacciones y errores controlados.
- Modulos relacionados: Persistencia, API REST.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-20 - Crear ofertas

- Historia: Como proveedor o usuario administrativo, quiero registrar ofertas para una licitacion publicada, para participar en el proceso de seleccion.
- Descripcion: La oferta relaciona proveedor, licitacion, monto en CRC y fecha de registro.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-05, HU-12, HU-16.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. Una oferta valida queda asociada a una licitacion publicada y a un proveedor existente.
  2. Una oferta invalida es rechazada con criterios de error verificables.
- Pruebas previstas: Pruebas unitarias de reglas de registro y pruebas de integracion de creacion.
- Modulos relacionados: Ofertas, proveedores, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `OfertaTests`, `OfertaServiceTests`, `Iteration3ApiTests`, `Iteration3MvcTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-21 - Listar, consultar y filtrar ofertas

- Historia: Como usuario administrativo, quiero listar, consultar y filtrar ofertas por licitacion y proveedor, para analizar la participacion registrada.
- Descripcion: Incluye filtros por licitacion, proveedor y ordenamientos definidos.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-20, HU-04.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. El listado permite filtrar por licitacion y proveedor.
  2. El detalle muestra proveedor, licitacion, monto y fecha de registro.
- Pruebas previstas: Pruebas funcionales de filtros y pruebas de integracion de consulta.
- Modulos relacionados: Ofertas, API REST.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `OfertaServiceTests`, `Iteration3ApiTests`, `Iteration3MvcTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-22 - Editar y eliminar ofertas cuando este permitido

- Historia: Como usuario administrativo, quiero editar o eliminar ofertas solo cuando las reglas lo permitan, para corregir errores sin afectar licitaciones cerradas.
- Descripcion: Impide cambios en ofertas de licitaciones cerradas o vencidas.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-20, HU-16.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. Una oferta abierta puede modificarse o retirarse segun reglas definidas.
  2. Una oferta cerrada o asociada a licitacion cerrada rechaza cambios.
- Pruebas previstas: Pruebas unitarias de permisos y pruebas de integracion.
- Modulos relacionados: Ofertas, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `OfertaTests`, `OfertaServiceTests`, `Iteration3MvcTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-23 - Rechazar ofertas duplicadas, vencidas o no publicadas

- Historia: Como usuario administrativo, quiero que el sistema rechace ofertas duplicadas, vencidas o sobre licitaciones no publicadas, para asegurar competencia valida.
- Descripcion: Una combinacion proveedor-licitacion no puede repetirse y solo se aceptan ofertas dentro del periodo permitido.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-20, HU-16.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. Una segunda oferta del mismo proveedor para la misma licitacion es rechazada.
  2. Una oferta vencida o asociada a una licitacion no publicada es rechazada.
- Pruebas previstas: Pruebas unitarias de duplicidad, vencimiento y estado; pruebas de integracion.
- Modulos relacionados: Ofertas, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `OfertaTests`, `OfertaServiceTests`, `Iteration3ApiTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-24 - Validar ofertas contra presupuesto

- Historia: Como usuario administrativo, quiero rechazar ofertas superiores al presupuesto y aceptar ofertas iguales, para cumplir la regla economica de la licitacion.
- Descripcion: La comparacion se realiza contra el presupuesto persistido en CRC.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-20, HU-12.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. Una oferta mayor al presupuesto es rechazada.
  2. Una oferta igual al presupuesto es aceptada si cumple las demas reglas.
- Pruebas previstas: Pruebas unitarias de limite presupuestario y pruebas de integracion.
- Modulos relacionados: Ofertas, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `OfertaTests`, `Iteration3ApiTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-25 - Determinar mejor oferta y resolver empates

- Historia: Como usuario administrativo, quiero determinar la mejor oferta y resolver empates por fecha de registro, para identificar la opcion ganadora de forma transparente.
- Descripcion: La mejor oferta es la de menor monto; ante empate se selecciona la registrada primero.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-20, HU-21.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. La mejor oferta corresponde al menor monto valido de la licitacion.
  2. Si dos ofertas tienen el mismo monto, gana la de fecha de registro mas temprana.
- Pruebas previstas: Pruebas unitarias de seleccion y desempate; prueba de integracion de consulta.
- Modulos relacionados: Ofertas, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `EvaluadorOfertasTests`, `Iteration3ApiTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-26 - Calcular clasificacion del ahorro

- Historia: Como usuario administrativo, quiero calcular la clasificacion del ahorro de una oferta, para interpretar el impacto economico frente al presupuesto.
- Descripcion: La clasificacion se deriva de la diferencia entre presupuesto y mejor oferta segun rangos acordados por el equipo.
- Prioridad: Media.
- Estimacion: 3 puntos.
- Dependencias: HU-24, HU-25.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. El sistema calcula el ahorro en CRC y su porcentaje respecto al presupuesto.
  2. El resultado muestra una clasificacion verificable segun la regla definida.
- Pruebas previstas: Pruebas unitarias de calculo y prueba funcional de visualizacion.
- Modulos relacionados: Ofertas, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `EvaluadorOfertasTests`, `Iteration3ApiTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-27 - Administrar niveles de aprobacion

- Historia: Como usuario administrativo, quiero crear, listar, consultar, editar y eliminar niveles de aprobacion, para parametrizar quien aprueba segun el ahorro o monto definido.
- Descripcion: Gestiona niveles de aprobacion con aprobador asociado sin definir todavia una estrategia tecnica de eliminacion.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-26.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. El sistema permite crear, listar, consultar, editar y eliminar niveles de aprobacion cuando la integridad de los datos lo permita.
  2. Una eliminacion no debe romper relaciones ni dejar datos inconsistentes.
  3. Los errores de integridad se muestran mediante mensajes controlados.
- Pruebas previstas: Pruebas de integracion de CRUD y validaciones basicas.
- Modulos relacionados: Niveles de aprobacion.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `NivelAprobacionServiceTests`, `Iteration3ApiTests`, `Iteration3MvcTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-28 - Evitar traslapes y determinar aprobador

- Historia: Como usuario administrativo, quiero evitar traslapes, permitir solo un rango abierto y determinar el aprobador desde una tabla parametrizable, para mantener reglas de aprobacion consistentes.
- Descripcion: Un rango abierto no tiene limite superior y debe ser unico.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-27.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. Un rango traslapado con otro existente es rechazado.
  2. Para una oferta evaluada, el sistema determina el aprobador usando el rango vigente.
- Pruebas previstas: Pruebas unitarias de rangos, rango abierto y seleccion de aprobador.
- Modulos relacionados: Niveles de aprobacion, ofertas.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `NivelAprobacionTests`, `NivelAprobacionServiceTests`, `Iteration3PersistenceTests`, `Iteration3ApiTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-29 - Exponer API REST de ofertas y aprobaciones

- Historia: Como consumidor de API, quiero administrar ofertas, consultar ofertas de una licitacion, consultar mejor oferta y administrar niveles de aprobacion, para integrar el analisis de adjudicacion.
- Descripcion: Incluye endpoints REST, DTO, filtros, codigos HTTP y errores controlados.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-21, HU-25, HU-27, HU-28.
- Iteracion asignada: Iteracion 3.
- Criterios de aceptacion:
  1. La API expone CRUD de ofertas y niveles de aprobacion.
  2. La API permite consultar ofertas por licitacion y obtener mejor oferta con clasificacion y aprobador.
- Pruebas previstas: Pruebas de integracion de endpoints y casos de error.
- Modulos relacionados: Ofertas, niveles de aprobacion, API REST.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: `Iteration3ApiTests`.
- Estado: Implementada localmente en Iteracion 3; commit, PR y CI pendientes.

### HU-30 - Administrar tipos de cambio

- Historia: Como usuario administrativo, quiero crear, listar, consultar, editar y eliminar tipos de cambio, para mantener conversiones CRC/USD administradas por el sistema.
- Descripcion: No depende de servicios externos y permite operar sin conexion a Internet.
- Prioridad: Media.
- Estimacion: 5 puntos.
- Dependencias: HU-18.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. El sistema permite CRUD de tipos de cambio con fecha y valor.
  2. El modulo funciona con datos administrados localmente sin requerir Internet.
- Pruebas previstas: Pruebas de integracion de CRUD y pruebas unitarias de validacion.
- Modulos relacionados: Tipos de cambio.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-31 - Activar un unico tipo de cambio

- Historia: Como usuario administrativo, quiero activar un tipo de cambio y garantizar que solo exista uno activo, para tener una conversion vigente unica.
- Descripcion: Al activar un registro se desactiva cualquier otro tipo de cambio activo.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: HU-30.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. Al activar un tipo de cambio, ningun otro queda activo.
  2. Si no hay tipo de cambio activo, el sistema informa la situacion de forma controlada.
- Pruebas previstas: Pruebas unitarias de activacion unica y pruebas de integracion.
- Modulos relacionados: Tipos de cambio, persistencia.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-32 - Alternar visualmente entre CRC y USD

- Historia: Como usuario, quiero alternar visualmente montos entre CRC y USD, para interpretar presupuestos y ofertas en ambas monedas.
- Descripcion: CRC permanece como moneda persistida y fuente de verdad; USD es una representacion visual calculada con fecha del tipo de cambio.
- Prioridad: Media.
- Estimacion: 3 puntos.
- Dependencias: HU-30, HU-31.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. El usuario puede alternar montos visibles entre CRC y USD.
  2. La interfaz muestra la fecha del tipo de cambio usado y mantiene CRC como valor persistido.
- Pruebas previstas: Pruebas unitarias de conversion y prueba funcional de alternancia visual.
- Modulos relacionados: Tipos de cambio, interfaz, ofertas, licitaciones.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-33 - Alternar modo claro y oscuro con preferencia persistida

- Historia: Como usuario, quiero alternar entre modo claro y oscuro y conservar mi preferencia, para usar el sistema con comodidad visual.
- Descripcion: La preferencia debe persistir entre visitas sin alterar la informacion de negocio.
- Prioridad: Media.
- Estimacion: 3 puntos.
- Dependencias: HU-01, HU-02.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. El usuario puede cambiar entre modo claro y modo oscuro desde la interfaz.
  2. La preferencia seleccionada se conserva al recargar o volver a entrar al sistema.
- Pruebas previstas: Prueba funcional de alternancia y persistencia local de preferencia.
- Modulos relacionados: Interfaz.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-34 - Documentar y endurecer la API REST

- Historia: Como consumidor de API, quiero contar con Swagger/OpenAPI, versionado, ProblemDetails, codigos HTTP correctos, identificador de correlacion y errores sin datos sensibles, para integrar la API con confianza.
- Descripcion: Historia transversal de contrato y comportamiento de errores para todos los endpoints.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-10, HU-17, HU-29.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. Swagger/OpenAPI documenta endpoints versionados y DTO principales.
  2. Los errores usan ProblemDetails, codigos HTTP correctos, correlacion y omiten datos sensibles.
- Pruebas previstas: Pruebas de integracion de contrato, errores y correlacion.
- Modulos relacionados: API REST.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-35 - Automatizar pruebas, cobertura e integracion continua

- Historia: Como programador, quiero ejecutar pruebas unitarias, integracion con PostgreSQL real, funcionales E2E y cobertura minima en integracion continua, para sostener TDD y calidad verificable.
- Descripcion: Incluye evidencia rojo-verde-refactorizacion, umbral de cobertura acordado, validacion de dependencias y vulnerabilidades.
- Prioridad: Alta.
- Estimacion: 5 puntos.
- Dependencias: HU-18, HU-19, HU-34.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. La integracion continua ejecuta pruebas unitarias, integracion y E2E iniciales con resultado visible.
  2. La evidencia de TDD, cobertura y revision de dependencias queda registrada en la documentacion correspondiente.
- Pruebas previstas: Ejecucion automatizada de suites de prueba y reporte de cobertura.
- Modulos relacionados: Pruebas, integracion continua.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-36 - Preparar infraestructura de despliegue

- Historia: Como programador, quiero preparar Dockerfile multi-stage, Docker Compose, PostgreSQL persistente, health checks y manifiestos Kubernetes, para entregar una version candidata ejecutable.
- Descripcion: Incluye ConfigMap, Secret, probes, PVC, limites, solicitudes de recursos, variables de entorno y manejo de secretos.
- Prioridad: Media.
- Estimacion: 5 puntos.
- Dependencias: HU-18, HU-35.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. La aplicacion puede ejecutarse con contenedores y PostgreSQL persistente usando variables de entorno.
  2. Los manifiestos de Kubernetes incluyen configuracion, secretos, probes, volumen persistente y recursos definidos.
- Pruebas previstas: Prueba de arranque con contenedores, health checks y validacion de manifiestos.
- Modulos relacionados: Infraestructura, persistencia.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

### HU-37 - Mantener documentacion XP, trazabilidad y preparacion de defensa

- Historia: Como equipo, quiero mantener documentacion modular, bitacora XP, evidencias de pareja, trazabilidad, uso de inteligencia artificial y preparacion de entrega, para defender las decisiones del proyecto.
- Descripcion: Consolida documentos de planificacion, arquitectura futura, API, pruebas, infraestructura, integracion de modulos y defensa oral.
- Prioridad: Alta.
- Estimacion: 3 puntos.
- Dependencias: Ninguna.
- Iteracion asignada: Iteracion 4.
- Criterios de aceptacion:
  1. La documentacion permite navegar historias, planes, bitacora, trazabilidad y documentos tecnicos previstos.
  2. Las evidencias reales se registran sin inventar Issues, Pull Requests, commits ni pruebas.
- Pruebas previstas: Revision documental de enlaces, trazabilidad y consistencia metodologica XP.
- Modulos relacionados: Documentacion, XP.
- Issue: Pendiente.
- Pull Request: Pendiente.
- Commits: Pendiente.
- Pruebas ejecutadas: Pendiente.
- Estado: Planificada.

## Resumen de puntos

| Iteracion | Historias | Puntos |
| --- | --- | ---: |
| Iteracion 1 | HU-01 a HU-10 | 30 |
| Iteracion 2 | HU-12 a HU-19 | 36 |
| Iteracion 3 | HU-11, HU-20 a HU-29 | 38 |
| Iteracion 4 | HU-30 a HU-37 | 32 |
| Total | 37 historias | 136 |

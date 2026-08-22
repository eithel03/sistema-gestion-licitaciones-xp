# Módulo Ofertas

## 1. Propósito

Registrar propuestas económicas de proveedores para licitaciones vigentes y evaluar la mejor propuesta disponible.

## 2. Responsabilidades

- CRUD de ofertas mientras la licitación recibe ofertas.
- Verificar licitación y proveedor.
- Impedir duplicidad por licitación/proveedor.
- Validar monto contra presupuesto y vencimiento.
- Filtrar, ordenar y paginar.
- Calcular mejor oferta, ahorro y clasificación.
- Integrar la mejor oferta con el aprobador por API.

## 3. Dependencias

- Domain: `Oferta`, `EvaluadorOfertas`, clasificación y errores.
- Application: `IOfertaService`, repositorio, `ILicitacionRepository`, `IProveedorRepository` e `IClock`.
- Infrastructure: `OfertaRepository`, EF Core, PostgreSQL y FKs.
- Web: `OfertasController`, modelos, vistas y `_MoneyDisplay`.
- API: `OfertaEndpoints` e `INivelAprobacionService` para el aprobador.

## 4. Entradas

- Crear: `LicitacionId`, `ProveedorId`, `MontoOfertadoCrc`.
- Crear anidada: id de licitación en ruta, proveedor y monto en cuerpo.
- Actualizar: monto y `Version` opcional.
- Listado: `page`, `pageSize`, `licitacionId`, `proveedorId`, `sort`.
- Mejor oferta: id de licitación.

## 5. Salidas

- `OfertaResponse` y `OfertaPage`.
- `MejorOfertaResponse` con oferta opcional, ahorro, porcentaje, clasificación, descripción y aprobador opcional.
- Vistas CRUD y listado filtrado.
- API con 200/201/204 o ProblemDetails.

## 6. Reglas de negocio

- Monto mayor que cero y menor o igual al presupuesto.
- Solo una licitación no retirada con estado efectivo Publicada recibe ofertas.
- Al llegar `FechaCierreUtc`, la licitación deja de recibir, editar o eliminar ofertas.
- Proveedor existente y no retirado.
- Una oferta por proveedor y licitación, reforzada por índice único.
- Mejor oferta: menor monto; empate por fecha de registro más temprana; después menor UUID.
- Ahorro: presupuesto menos monto ganador.
- Clasificación:
  - ahorro ≥ 10 %: conveniente;
  - ahorro > 0 % y < 10 %: aceptable;
  - ahorro = 0 %: válida sin ahorro;
  - sin ofertas: resultado funcional sin ofertas válidas.
- Eliminación física, permitida solo mientras la licitación recibe ofertas.
- Concurrencia por `xmin`.

## 7. Errores

- Monto inválido o superior al presupuesto.
- Licitación no disponible, no encontrada o retirada.
- Proveedor inválido, no encontrado o retirado.
- Oferta duplicada o no encontrada.
- Conflicto de concurrencia.
- Sin ofertas se devuelve como resultado 200 del evaluador, no como excepción HTTP.

## 8. Pruebas relacionadas

- Unitarias: `OfertaTests`, `EvaluadorOfertasTests`, `OfertaServiceTests`.
- Integración: `Iteration3PersistenceTests`.
- Funcionales: `Iteration3ApiTests`, `Iteration3MvcTests`.
- E2E: `LicitacionOfertaE2ETests`.

Limitaciones MVC: el backend pagina, pero el listado no ofrece botones Anterior/Siguiente; la mejor oferta, clasificación y aprobador no tienen una pantalla MVC integrada. Estas capacidades existen en Application/API y se registran como trabajo técnico futuro de interfaz.

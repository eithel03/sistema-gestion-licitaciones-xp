# Módulo Licitaciones

## 1. Propósito

Administrar el ciclo de una licitación desde su creación en borrador hasta su publicación, cierre o retiro lógico.

## 2. Responsabilidades

- CRUD de licitaciones activas.
- Normalización y unicidad del código.
- Validación de título, presupuesto y fecha de cierre.
- Gestión de estado y estado efectivo por vencimiento.
- Auditoría y concurrencia optimista.
- Operaciones equivalentes en MVC y API.

## 3. Dependencias

- Domain: `Licitacion`, `LicitacionEstado`, normalizador y errores.
- Application: `ILicitacionService`, contratos, consultas, resultados, repositorio abstracto e `IClock`.
- Infrastructure: `LicitacionRepository`, configuración EF Core y PostgreSQL.
- Web: `LicitacionesController`, modelos y vistas.
- API: `LicitacionEndpoints`.
- Ofertas consume este módulo para validar disponibilidad y presupuesto.

## 4. Entradas

- Código, título, presupuesto CRC y fecha de cierre.
- Actualización opcionalmente incluye `Version`.
- Cambio de estado API: texto `estado`.
- Listado: `page`, `pageSize`, `search`, `sort`.
- MVC recibe fecha local de Costa Rica y la convierte a UTC.

## 5. Salidas

- `LicitacionResponse` con estado almacenado, estado efectivo, auditoría y versión.
- `LicitacionPage`.
- Vistas de listado, detalle, creación y edición; acciones POST de publicar, cerrar y retirar.
- API con 200/201/204 o ProblemDetails.

## 6. Reglas de negocio

- Código requerido, máximo 50, en mayúsculas para presentación y comparación normalizada.
- Código permite letras, números, espacios y guion.
- Código normalizado único entre licitaciones no retiradas.
- Título requerido, máximo 200.
- Presupuesto CRC mayor que cero.
- Fecha de cierre posterior al reloj al crear y editar; una licitación vencida no puede publicarse.
- Transiciones reales:
  - `Borrador → Publicada`.
  - `Borrador → Cerrada`.
  - `Publicada → Cerrada`.
- No se puede volver a Borrador ni publicar una Cerrada.
- Una Publicada vencida se presenta con estado efectivo Cerrada aunque su estado almacenado siga siendo Publicada.
- Solo Borrador puede editarse o retirarse.
- Borrado lógico mediante `DeletedAt`.
- Concurrencia por `xmin`, expuesto como `Version`.

## 7. Errores

- Código requerido, inválido, demasiado largo o duplicado.
- Título requerido o demasiado largo.
- Presupuesto o fecha inválidos.
- Transición inválida o edición no permitida.
- Licitación no encontrada.
- Conflicto de concurrencia, traducido a 409 por API.

## 8. Pruebas relacionadas

- Unitarias: `LicitacionTests`, `LicitacionServiceTests`.
- Integración: `LicitacionPersistenceTests`.
- Funcionales: `LicitacionApiTests` y pruebas MVC relacionadas.
- E2E: `LicitacionOfertaE2ETests`.

Limitaciones de interfaz: el backend acepta ordenamiento, pero la vista no ofrece selector; la landing todavía muestra una versión antigua. Ambas correcciones de Web quedan fuera de Fase 9.

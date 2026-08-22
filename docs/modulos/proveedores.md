# Módulo Proveedores

## 1. Propósito

Mantener el catálogo de organizaciones que pueden presentar ofertas en una licitación.

## 2. Responsabilidades

- Crear, consultar, listar y editar proveedores.
- Retirar proveedores mediante borrado lógico.
- Normalizar nombres y evitar duplicados activos.
- Exponer operaciones MVC y API.
- Mostrar en el detalle MVC las ofertas relacionadas.

## 3. Dependencias

- Domain: `Proveedor`, normalizador y errores.
- Application: `IProveedorService`, contratos y `IProveedorRepository`.
- Infrastructure: `ProveedorRepository`, configuración EF Core y PostgreSQL.
- Web: `ProveedoresController`, modelos y vistas.
- API: `ProveedorEndpoints`.
- Ofertas: `IOfertaService` para el detalle relacionado.

## 4. Entradas

- MVC: `ProveedorFormViewModel.Nombre`.
- API: `CrearProveedorRequest` y `ActualizarProveedorRequest`.
- Listado: `page`, `pageSize`, `search`, `sort`.

## 5. Salidas

- MVC: listado, detalle, alta, edición y confirmación de retiro.
- API: `ProveedorResponse`, `ProveedorPage`, 201/200/204 o ProblemDetails.
- Detalle MVC: proveedor y hasta 100 ofertas relacionadas consultadas por `ProveedorId`.

## 6. Reglas de negocio

- Nombre requerido y máximo 200 caracteres.
- Espacios laterales eliminados y espacios repetidos reducidos.
- Normalización Unicode Form C para presentación y Form KC/mayúsculas para comparación.
- Caracteres permitidos: letras Unicode, números, espacios, punto, coma y paréntesis.
- Unicidad entre proveedores activos por `NombreNormalizado`.
- Borrado lógico mediante `DeletedAt`; el nombre puede reutilizarse después.
- El listado filtra retirados, busca con `ILIKE`, ordena por nombre y pagina hasta 100 registros.

## 7. Errores

- `Proveedor.NombreRequerido`.
- `Proveedor.NombreCaracteresInvalidos`.
- `Proveedor.NombreLongitudMaxima`.
- `Proveedor.NombreDuplicado`, traducido normalmente a 409 por API.
- `Proveedor.NoEncontrado`, traducido a 404.

Limitación: los contratos no exponen `Version`; el repositorio no convierte explícitamente una carrera concurrente del índice único ni un conflicto de concurrencia en resultado controlado.

## 8. Pruebas relacionadas

- Unitarias: `ProveedorTests`, `ProveedorServiceTests`.
- Integración: `ProveedorPersistenceTests`.
- Funcionales: `ProveedorApiTests`, `ProveedorMvcTests`.
- E2E: `ProveedorE2ETests`.

Los resultados numéricos son evidencia histórica consolidada en [pruebas.md](../pruebas.md), no una ejecución de Fase 9.

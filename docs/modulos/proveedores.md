# Modulo Proveedores

## Proposito

Mantener el catalogo inicial de proveedores del Sistema de Gestion de Licitaciones. Este modulo permite registrar organizaciones que luego podran participar en ofertas.

## Responsabilidades

- Crear proveedores.
- Consultar detalle de proveedor.
- Listar proveedores activos con busqueda, ordenamiento y paginacion.
- Editar el nombre del proveedor.
- Retirar proveedores mediante borrado logico.
- Exponer operaciones equivalentes por API REST.

## Reglas

- `Id` se genera automaticamente y no se edita desde la interfaz.
- `Nombre` es requerido.
- `NombreNormalizado` se calcula en el servidor.
- La normalizacion elimina espacios laterales, reduce espacios repetidos, normaliza Unicode y compara sin distinguir mayusculas/minusculas.
- Caracteres permitidos: letras, numeros, espacios, punto, coma y parentesis.
- No se permiten proveedores activos duplicados por nombre normalizado.
- PostgreSQL refuerza la unicidad con un indice unico sobre `NombreNormalizado`.
- La eliminacion se implementa como borrado logico mediante `DeletedAt`.

## Entradas

- MVC: formulario de proveedor con campo `Nombre`.
- API: `CrearProveedorRequest` y `ActualizarProveedorRequest`.
- Listado: `page`, `pageSize`, `search` y `sort`.

## Salidas

- MVC: vistas de listado, detalle, creacion, edicion y confirmacion de eliminacion.
- API: `ProveedorResponse` y `ProveedorPage`.
- Errores controlados con mensajes comprensibles.

## Dependencias

- `Licitaciones.Domain.Proveedores`.
- `Licitaciones.Application.Proveedores`.
- `Licitaciones.Infrastructure.Persistence.LicitacionesDbContext`.
- PostgreSQL configurado por Fase 4.

## Errores

- Nombre requerido: rechazo con validacion de servidor.
- Caracteres invalidos: rechazo con validacion de servidor.
- Duplicado normalizado: API devuelve `409 Conflict`; MVC muestra error junto al campo.
- Proveedor no encontrado: API devuelve `404 Not Found`; MVC responde `NotFound`.

## Pruebas

- Unitarias: normalizacion, caracteres, creacion, edicion, borrado logico y duplicidad en Application.
- Integracion: guardar, recuperar, actualizar, indice unico, normalizacion persistida, migracion y borrado logico con PostgreSQL real.
- Funcionales: landing, listado MVC, crear/editar/rechazar duplicado por MVC, y endpoints API de proveedores.

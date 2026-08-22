# Módulo Interfaz Web

## 1. Propósito

Ofrecer una interfaz MVC navegable para administrar los módulos y presentar montos, validaciones y mensajes al usuario.

## 2. Responsabilidades

- Landing y navegación principal.
- Vistas CRUD de los cinco módulos de negocio.
- Formularios y validación cliente/servidor.
- Cultura `es-CR` y conversión de fechas locales de licitación a UTC.
- Tema claro/oscuro y preferencia CRC/USD.
- Presentar mensajes de éxito y error.
- Diseño adaptable mediante Bootstrap y CSS.

## 3. Dependencias

- ASP.NET Core MVC y Razor.
- Bootstrap, jQuery Validation y validación unobtrusive.
- Servicios de Application e Infrastructure registrados por el host.
- Cookies HTTP para preferencias.
- Parcial `_MoneyDisplay` e `IMonedaConversionService`.

## 4. Entradas

- Formularios de proveedores, licitaciones, ofertas, niveles y tipos de cambio.
- Parámetros de búsqueda, filtro, orden y página.
- Acciones POST de publicar, cerrar, activar y eliminar.
- Cookies `licitaciones.theme` y `licitaciones.currency`.

## 5. Salidas

- HTML Razor adaptable.
- Tablas, formularios y detalles.
- Mensajes mediante `TempData` y `ModelState`.
- Montos CRC o USD con fecha del tipo de cambio cuando aplica.
- Respuestas 404 para recursos no encontrados.

## 6. Reglas de negocio

La interfaz repite algunas validaciones para experiencia de usuario, pero Domain y Application conservan la autoridad. Tema y moneda solo cambian presentación. Las cookies usan `SameSite=Lax`, son esenciales y vencen en un año.

La cultura predeterminada es `es-CR`. Las fechas de licitación ingresadas como hora local de Costa Rica se convierten a UTC.

## 7. Errores

- Errores de modelo junto a campos o resumen.
- Mensajes de dominio convertidos en errores de formulario.
- Mensajes temporales de éxito/error.
- Vista global de error fuera de Development/Testing.

Limitaciones actuales:

- La landing muestra `v0.1.0` y texto de Iteración 1.
- La vista informativa API/Swagger afirma que Swagger estaba pendiente.
- No existe pantalla MVC de mejor oferta, clasificación y aprobador.
- Ofertas, niveles de aprobación y tipos de cambio no tienen botones completos de paginación.
- Licitaciones acepta orden en backend, pero la vista no ofrece selector.

Estas limitaciones requieren cambios en Web y no se corrigen en Fase 9.

## 8. Pruebas relacionadas

- Funcionales: `ProveedorMvcTests`, `Iteration3MvcTests`, `Iteration4MvcTests`.
- E2E: `NavigationE2ETests`, `ProveedorE2ETests`, `LicitacionOfertaE2ETests`, `PreferenciasE2ETests`.
- Unitarias indirectas de servicios y validaciones de dominio.

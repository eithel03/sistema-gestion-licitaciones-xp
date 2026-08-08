# Dominio y estrategia TDD

Este documento registra la base acordada en la Fase 3. Su alcance es preparar convenciones y herramientas minimas para desarrollar las historias mediante TDD, sin implementar reglas futuras de proveedores, licitaciones, ofertas, aprobaciones ni moneda.

## Convenciones de dominio

- Entidades: heredan de `Entity<TId>` cuando tienen identidad estable. La igualdad se basa en tipo concreto e identificador.
- Objetos de valor: heredan de `ValueObject` cuando no tienen identidad propia. La igualdad se basa en sus componentes.
- Enumeraciones: se usaran `enum` cuando el conjunto de valores sea estable y simple. Si un estado requiere comportamiento, transiciones o metadatos, se modelara con reglas explicitas en el dominio durante la historia correspondiente.
- Excepciones de dominio: se usara `DomainException` para violaciones de invariantes que no puedan representarse como validacion recuperable.
- Resultados de validacion: se usara `ValidationResult` con `ValidationError` para validaciones acumulables que deban devolverse como mensajes controlados.
- Guardas: `Guard` contiene validaciones transversales pequenas. No debe convertirse en un repositorio de reglas de negocio.

## Reloj inyectable

La abstraccion `IClock` vive en `Application` para que los casos de uso dependan de tiempo inyectable y verificable.

La implementacion `SystemClock` vive en `Infrastructure` y se registra desde `AddInfrastructure`. En pruebas se reemplaza con implementaciones fijas.

## Estrategia TDD

Para cada regla verificable:

1. Escribir una prueba que falle por la regla requerida.
2. Implementar el codigo minimo para pasar la prueba.
3. Refactorizar manteniendo la suite en verde.
4. Registrar evidencia del ciclo en la bitacora o documento de iteracion cuando la historia lo requiera.

## Ejemplos minimos creados

- `ValidationResultTests`: demuestra el ciclo para resultado exitoso, errores y proteccion contra fallos sin errores.
- `ValueObjectTests`: demuestra igualdad por componentes.
- `EntityTests`: demuestra igualdad por identidad y rechazo de identificadores por defecto.
- `IClockTests`: demuestra reemplazo de reloj por una implementacion fija en pruebas.

## Limites de Fase 3

No se implementaron:

- Normalizacion de proveedores.
- Estados o vencimiento de licitaciones.
- Reglas de ofertas, mejor oferta o aprobaciones.
- Conversion CRC/USD.
- Persistencia, migraciones o restricciones de base de datos.

Estas reglas se implementaran en sus iteraciones asignadas para mantener diseno simple y TDD incremental.

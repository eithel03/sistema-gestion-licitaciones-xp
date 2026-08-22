# Módulo Persistencia

## 1. Propósito

Persistir el estado de negocio en PostgreSQL y materializar las abstracciones de repositorio de Application.

## 2. Responsabilidades

- Configurar EF Core/Npgsql y `LicitacionesDbContext`.
- Implementar repositorios.
- Mapear entidades, relaciones, índices y restricciones.
- Aplicar auditoría, precisión monetaria y concurrencia.
- Mantener migraciones versionadas.
- Gestionar transacciones de activación de tipo de cambio.
- Exponer health check PostgreSQL opcional.

## 3. Dependencias

- PostgreSQL 16.
- Entity Framework Core 9.0.18.
- Npgsql EF Core 9.0.4.
- Domain y contratos de Application.
- Configuración `ConnectionStrings:DefaultConnection`.

## 4. Entradas

- Entidades y consultas desde servicios de Application.
- Cadena de conexión.
- Operaciones `Add`, `Get`, `List`, `Remove`, comprobación de existencia y `SaveChanges`.
- Migraciones invocadas por los hosts.

## 5. Salidas

- Entidades y páginas proyectadas.
- Cambios persistidos.
- Excepciones controladas de duplicidad, traslape, activo único o concurrencia en los repositorios que las traducen.
- Estado de health check PostgreSQL cuando se habilita.

## 6. Reglas de negocio

Las reglas principales viven en Domain/Application. PostgreSQL refuerza:

- unicidad parcial de proveedores y licitaciones activas;
- una oferta por proveedor/licitación y FKs restrictivas;
- montos positivos;
- rango de aprobación válido, sin solapamiento y único rango abierto;
- un único tipo de cambio activo;
- concurrencia mediante tokens `Version`, incluido `xmin` en licitaciones, ofertas, aprobaciones y tipos de cambio.

`TipoCambioRepository` abre una transacción cuando activa una tasa fuera de una transacción existente, desactiva las demás y confirma o revierte al guardar.

## 7. Errores

- `DbUpdateConcurrencyException` traducida por repositorios de licitaciones, ofertas, aprobaciones y tipos de cambio.
- Violaciones PostgreSQL de unicidad o exclusión traducidas donde existe manejo específico.
- Errores de conexión o migración pueden terminar el arranque.
- Proveedores no tiene traducción específica de carreras de unicidad/concurrencia.

Web y API ejecutan `Database.Migrate()` al iniciar fuera de `Testing`. No existe un migrador independiente ni un Job Kubernetes; ambos hosts pueden competir por las migraciones. Esta limitación se documenta sin modificar infraestructura.

## 8. Pruebas relacionadas

- `PostgreSqlContainerTests`.
- `PersistenceConventionsTests`.
- `ProveedorPersistenceTests`.
- `LicitacionPersistenceTests`.
- `Iteration3PersistenceTests`.
- `TipoCambioPersistenceTests`.

Las pruebas usan PostgreSQL 16 mediante Testcontainers y cubren migraciones, restricciones, índices, transacciones y concurrencia.

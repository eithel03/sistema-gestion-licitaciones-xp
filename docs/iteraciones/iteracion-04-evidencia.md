# Evidencia Iteración 4

- Rama: `feature/iteracion-04-moneda-ux`.
- Driver principal: Eithel.
- Navigator principal: Chavala.
- Historias: HU-30 a HU-37.
- Puntos planificados: 32.
- Versión candidata prevista: `v1.0.0-rc`.

## Estado por historia

| Historia | Evidencia local |
| --- | --- |
| HU-30 | CRUD local de tipos de cambio validado; se permiten fechas repetidas |
| HU-31 | Activación única validada por MVC, API y persistencia |
| HU-32 | Conversión visual CRC/USD validada sin alterar montos CRC persistidos |
| HU-33 | Tema claro/oscuro y persistencia de preferencia validados manualmente |
| HU-34 | Swagger UI, OpenAPI exacto, ProblemDetails y correlación validados |
| HU-35 | 174/174 pruebas y cobertura limpia validadas localmente; CI remoto pendiente |
| HU-36 | Docker operativo; Kubernetes renderizado y pendiente de clúster activo |
| HU-37 | Documentación y trazabilidad actualizadas localmente; cierre formal pendiente |

## Evidencia funcional

- Crear, listar, consultar, editar y eliminar tipos de cambio: correcto.
- Se permiten varios registros con la misma fecha.
- `PATCH /api/v1/tipos-cambio/{id}/activar` deja exactamente un registro activo.
- La migración `20260814014136_AllowDuplicateTipoCambioDates` eliminó la unicidad de `IX_TiposCambio_Fecha` y lo recreó como índice normal.
- El índice parcial único `IX_TiposCambio_UnicoActivo` permanece como garantía PostgreSQL.
- CRC continúa persistido como fuente de verdad; USD se calcula con `USD = CRC / CrcPorUsd`.
- Con tipo de cambio 500 se validaron CRC 1,000,000 = USD 2,000.00 y CRC 750,000 = USD 1,500.00.
- La fecha del tipo de cambio utilizado se muestra en la interfaz.
- La preferencia de tema claro/oscuro se conservó después de reiniciar contenedores.

## Correcciones durante la consolidación

- Entrada decimal MVC: se aceptan `500`, `500.00`, `500,00`, `520.50` y `520,50`; se rechazan `0`, `-1` y `abc`.
- Proveedores Unicode: se corrigió la validación cliente que rechazaba `Tecnología Empresarial CR`; la regla de dominio ya era correcta y `Empresa @ CR` continúa inválido.
- Activación: la acción redundante no se muestra en el detalle de un registro ya activo.
- API: activación corregida a PATCH y agregado `PATCH /api/v1/licitaciones/{id}/estado`.
- OpenAPI: se eliminaron verbos documentados que no existen realmente.
- Swagger: `/swagger` sirve la interfaz interactiva y `/swagger/v1/swagger.json` sirve el documento v1.
- ProblemDetails: `application/problem+json`, campos seguros y `correlationId` igual al encabezado `X-Correlation-ID`.

## Prueba del estado de licitación

`PATCH /api/v1/licitaciones/{id}/estado` reutiliza las transiciones de dominio:

- Borrador a Publicada.
- Borrador a Cerrada.
- Publicada a Cerrada.
- No se permite regresar a Borrador ni salir de Cerrada.

Una solicitud con GUID inexistente devolvió 404 ProblemDetails. La ruta existe; un intento previo con cuerpo mal escapado correspondió al quoting de PowerShell y no a un defecto de la API.

## Suite automatizada final

| Suite | Aprobadas | Fallidas | Omitidas |
| --- | ---: | ---: | ---: |
| UnitTests | 96 | 0 | 0 |
| IntegrationTests | 27 | 0 | 0 |
| FunctionalTests | 51 | 0 | 0 |
| Total | 174 | 0 | 0 |

- Restore: exitoso.
- Build Release: exitoso, 0 errores y 0 advertencias.
- PostgreSQL real: validado mediante Testcontainers en integración y funcionales.

Los logs `fail: Microsoft.EntityFrameworkCore.Database.Connection` inmediatamente posteriores a `DROP DATABASE ... WITH (FORCE)` se producen durante el reinicio intencional de bases temporales y no indican pruebas fallidas.

## Cobertura limpia

- Parser: `MultiReport (3x Cobertura)`.
- Cobertura global de líneas: 87.3%.
- `Licitaciones.Domain`: 91.4%.
- `Licitaciones.Application`: 83.8%.
- `Licitaciones.Api`: 88.4%.
- `Licitaciones.Infrastructure`: 95.1%.
- `Licitaciones.Web`: 61.6%.
- Branch coverage: 59%.
- Method coverage: 84%.
- Umbrales cumplidos: global >= 70%, Domain >= 80% y Application >= 80%.

Esta es la medición definitiva y utiliza únicamente los tres reportes actuales, sin mezclar ejecuciones históricas.

## Docker

- `docker compose config`: exitoso.
- Construcción de imágenes Web y API: exitosa.
- Arranque de servicios: exitoso.
- PostgreSQL 16: saludable y expuesto en el puerto host 55432.
- API: disponible en el puerto 8081 con health check.
- Web: disponible en el puerto 8080 con health check.
- Persistencia: comprobada después de reiniciar contenedores; proveedores, licitaciones, ofertas y tipos de cambio permanecieron.
- `docker compose down -v`: no ejecutado para conservar el volumen.

## Kubernetes

- `kubectl kustomize k8s`: exitoso.
- Recursos renderizados: Namespace, ConfigMap, Secret de ejemplo, PostgreSQL, PVC, Deployments y Services de API y Web, probes, requests y limits.
- `kubectl apply --dry-run=client --validate=false -k k8s`: no completó la consulta al API server porque Kubernetes local no estaba iniciado en `kubernetes.docker.internal:6443`.
- Despliegue, pods, logs y persistencia real en un clúster activo: Pendiente.

## Evidencia formal pendiente

- Issue de Iteración 4: Pendiente.
- Commits finales: Pendiente.
- Pull Request: Pendiente.
- CI remoto posterior al push: Pendiente.
- Revisión formal final del Navigator: Pendiente.
- Merge a `main`: Pendiente.
- Tag `v1.0.0-rc`: Pendiente.
- GitHub Release: Pendiente.

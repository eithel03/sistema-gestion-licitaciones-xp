# Iteración 4 - Moneda, UX y consolidación técnica

- Objetivo: completar tipos de cambio, visualización CRC/USD, modo claro y oscuro, consolidación de API, pruebas e infraestructura para una versión candidata reproducible.
- Duración: Pendiente de completar por el equipo.
- Fecha: Pendiente de completar por el equipo.
- Driver principal: Eithel.
- Navigator principal: Chavala.
- Rama: `feature/iteracion-04-moneda-ux`.
- Versión candidata prevista: `v1.0.0-rc`.
- Puntos planificados: 32.

## Historias seleccionadas

| Historia | Puntos | Resultado local |
| --- | ---: | --- |
| HU-30 | 5 | CRUD local de tipos de cambio implementado y validado |
| HU-31 | 3 | Activación de un único tipo de cambio implementada y validada |
| HU-32 | 3 | Alternancia visual CRC/USD implementada y validada |
| HU-33 | 3 | Modo claro y oscuro con preferencia persistida implementado y validado |
| HU-34 | 5 | Swagger UI, OpenAPI, versionado, ProblemDetails y correlación implementados y validados |
| HU-35 | 5 | Suite y cobertura consolidadas localmente; workflow de CI preparado y ejecución remota de la Iteración 4 pendiente |
| HU-36 | 5 | Docker validado y manifiestos Kubernetes preparados y renderizados |
| HU-37 | 3 | Documentación y trazabilidad actualizadas localmente; cierre formal pendiente |

## Resultado funcional

- El CRUD de tipos de cambio funciona con datos locales y sin servicios externos.
- `Fecha` es requerida y admite varios registros para una misma fecha.
- `CrcPorUsd` debe ser mayor que cero y permanece en `numeric(18,2)`.
- Solo existe un tipo de cambio activo; al activar otro, el anterior queda inactivo.
- La activación oficial usa `PATCH /api/v1/tipos-cambio/{id}/activar`.
- CRC permanece como fuente de verdad persistida; USD se calcula solo para presentación con `USD = CRC / CrcPorUsd`.
- La interfaz muestra la fecha del tipo de cambio aplicado.
- Se validó la conversión de CRC 1,000,000 a USD 2,000.00 y de CRC 750,000 a USD 1,500.00 con un tipo de cambio de 500.
- El modo claro y oscuro puede alternarse y su preferencia persiste, incluso después de reiniciar los contenedores.

## API consolidada

- Swagger UI interactivo: `/swagger`.
- Documento OpenAPI v1: `/swagger/v1/swagger.json`.
- La documentación OpenAPI publica únicamente los verbos HTTP reales.
- Cambio oficial de estado: `PATCH /api/v1/licitaciones/{id}/estado`.
- Los endpoints `POST /api/v1/licitaciones/{id}/publish` y `POST /api/v1/licitaciones/{id}/close` se conservan por compatibilidad.
- Los errores controlados usan `application/problem+json` e incluyen `title`, `status`, `detail`, `code` y `correlationId`.
- El `correlationId` del cuerpo coincide con el encabezado `X-Correlation-ID`.
- No se exponen trazas, rutas internas, consultas ni secretos.

## Ciclos TDD

1. ROJO: pruebas de dominio y Application no compilaban por ausencia de tipos de cambio. VERDE: entidad, validaciones, servicios, activación y conversión. REFACTOR: contratos y resultados reutilizables en Application.
2. ROJO: pruebas de persistencia fallaban porque el modelo y las migraciones no incluían tipos de cambio. VERDE: configuración EF Core, repositorio, restricciones e índice parcial único. REFACTOR: activación consistente dentro del repositorio.
3. ROJO: pruebas API y MVC devolvían rutas ausentes. VERDE: CRUD, activación, conversión visual, vistas y preferencias. REFACTOR: parcial monetario reutilizable.
4. ROJO: las pruebas de fecha duplicada reproducían la restricción incorrecta. VERDE: migración `20260814014136_AllowDuplicateTipoCambioDates`. REFACTOR: `IX_TiposCambio_Fecha` quedó como índice normal y se conservó `IX_TiposCambio_UnicoActivo`.
5. ROJO: las entradas `500,00` y otros formatos equivalentes fallaban en el formulario MVC. VERDE: enlace decimal flexible y entrada localizada. REFACTOR: normalización compartida para punto y coma.
6. ROJO: la validación cliente rechazaba nombres Unicode válidos de proveedores. VERDE: ajuste mínimo del patrón cliente. REFACTOR: se mantuvo intacta la regla Unicode correcta del dominio.
7. ROJO: pruebas de endurecimiento detectaron Swagger UI ausente, verbos OpenAPI incorrectos y ProblemDetails incompleto. VERDE: UI interactiva, contrato exacto, correlación en cuerpo y media type estándar. REFACTOR: generación y traducción de errores centralizadas.
8. ROJO: pruebas funcionales detectaron que faltaba `PATCH /api/v1/licitaciones/{id}/estado`. VERDE: endpoint y DTO con reutilización de transiciones de dominio. REFACTOR: se mantuvieron `publish` y `close` como rutas compatibles sin duplicar reglas.

## Ajustes y refactorizaciones

- Se aceptan `500`, `500.00`, `500,00`, `520.50` y `520,50` como entradas equivalentes de tipo de cambio; se rechazan cero, negativos y texto.
- Se corrigió una regresión de validación cliente para aceptar nombres como `Tecnología Empresarial CR` y continuar rechazando símbolos no permitidos como `@`.
- Se ocultó la acción redundante de activación cuando el tipo de cambio ya está activo.
- Se corrigió la documentación OpenAPI para no anunciar POST, PUT o DELETE en rutas que solo admiten GET.
- Se centralizó la producción de ProblemDetails y correlación para los módulos API existentes.

## Pruebas y cobertura

- UnitTests: 96/96.
- IntegrationTests: 27/27 con PostgreSQL real mediante Testcontainers.
- FunctionalTests: 51/51 mediante `WebApplicationFactory`.
- Total: 174/174, 0 fallidas y 0 omitidas.
- Build Release: 0 errores y 0 advertencias.
- Cobertura global de líneas: 87.3%.
- Cobertura de líneas: Domain 91.4%, Application 83.8%, API 88.4%, Infrastructure 95.1% y Web 61.6%.
- Branch coverage: 59%.
- Method coverage: 84%.
- Fuente definitiva: `Parser: MultiReport (3x Cobertura)`, sin resultados históricos mezclados.
- Umbrales cumplidos: global >= 70%, Domain >= 80% y Application >= 80%.

Los mensajes de conexión de EF Core inmediatamente posteriores a `DROP DATABASE ... WITH (FORCE)` corresponden al reinicio intencional de bases temporales de Testcontainers y no representan pruebas fallidas.

## Infraestructura

- Docker Compose fue validado mediante configuración, construcción de imágenes, arranque, health checks y acceso a Web, API y PostgreSQL 16.
- Web quedó disponible en el puerto 8080, API en 8081 y PostgreSQL en el puerto host 55432.
- La persistencia del volumen PostgreSQL se comprobó después de reiniciar contenedores; no se ejecutó `docker compose down -v`.
- `kubectl kustomize k8s` renderizó correctamente Namespace, ConfigMap, Secret de ejemplo, PostgreSQL, PVC, Deployments, Services, probes y recursos.
- `kubectl apply --dry-run=client --validate=false -k k8s` no completó la consulta al API server porque Kubernetes local no estaba disponible en `kubernetes.docker.internal:6443`.
- El despliegue y la persistencia reales sobre un clúster Kubernetes activo permanecen pendientes.

## Velocidad técnica

32 puntos implementados y validados localmente. El merge a `main` está evidenciado por `ea9772f`; el cierre formal depende todavía de CI remoto y revisión formal del Navigator.

## Resultado demostrable

El sistema funciona localmente con proveedores, licitaciones, ofertas, evaluación económica, niveles de aprobación, tipos de cambio, presentación CRC/USD, temas visuales, API consolidada, PostgreSQL, pruebas automatizadas, Docker e infraestructura Kubernetes preparada. La versión candidata `v1.0.0-rc` está prevista, pero el tag y la GitHub Release no existen todavía.

## Evidencia Git/GitHub y pendientes formales

- Issue: `#15 - ITER-04: Moneda, UX y consolidación técnica`.
- Pull Request: `#16`, desde `feature/iteracion-04-moneda-ux` hacia `main`.
- Rama publicada mediante `git push -u origin feature/iteracion-04-moneda-ux` y vinculada a `origin/feature/iteracion-04-moneda-ux`.
- Commits registrados:

- `40c4f5d` - `feat(moneda): implementar tipos de cambio y persistencia`.
- `38c5bf5` - `feat(api): consolidar contratos y manejo de errores`.
- `5cba6c2` - `feat(web): agregar moneda visual y preferencias de interfaz`.
- `9b0fa75` - `test(iteracion-04): consolidar pruebas y cobertura`.
- `8103b12` - `chore(deploy): preparar Docker y Kubernetes`.
- `2e710d2` - `docs(xp): documentar cierre tecnico de iteracion 4`.

Antes de esta actualización documental, `git status` indicó `nothing to commit, working tree clean` y `git diff --check` terminó sin errores.

Permanecen pendientes:

- CI remoto del Pull Request: Pendiente de evidencia confirmada.
- Revisión formal final del Navigator: Pendiente.
- Retroalimentación formal de cierre: Pendiente.
- Merge a `main`: realizado mediante `ea9772f`.
- Tag `v1.0.0-rc`: Pendiente.
- GitHub Release: Pendiente.

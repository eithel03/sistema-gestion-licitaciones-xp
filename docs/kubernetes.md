# Kubernetes

Despliegue del Sistema de Gestión de Licitaciones en Kubernetes local (Docker Desktop, Kubernetes v1.34.1).

## Arquitectura

Manifiestos en `k8s/` organizados con Kustomize:

| Manifiesto | Recursos |
| --- | --- |
| `namespace.yaml` | Namespace `licitaciones`. |
| `app-configmap.yaml` | ConfigMap `licitaciones-config`: `ASPNETCORE_ENVIRONMENT`, `ASPNETCORE_URLS`, `HealthChecks__PostgreSQL__Enabled`, `POSTGRES_DB`, `POSTGRES_USER`. |
| `app-secret.example.yaml` | Secret de ejemplo `licitaciones-secret`: `POSTGRES_PASSWORD` y `ConnectionStrings__DefaultConnection`. |
| `app-deployment.yaml` | Deployments `licitaciones-api` y `licitaciones-web` (imágenes `licitaciones-api:v1.0.0-rc` y `licitaciones-web:v1.0.0-rc`), cada uno con initContainer `wait-for-postgres` y probes startup/readiness/liveness. |
| `app-service.yaml` | Service `licitaciones-api` (ClusterIP, 8080) y Service `licitaciones-web` (NodePort `30080`). |
| `postgres-statefulset.yaml` | StatefulSet `postgres` (PostgreSQL 16, 1 réplica, probes startup/readiness/liveness, requests 100m/256Mi y limits 1000m/1Gi). |
| `postgres-service.yaml` | Service `postgres` (ClusterIP, 5432). |
| `postgres-pvc.yaml` | PVC `postgres-data` (1Gi, ReadWriteOnce). |
| `kustomization.yaml` | Composición de todos los recursos. |

La configuración se inyecta mediante `envFrom` (ConfigMap + Secret). La cadena de conexión apunta a `Host=postgres` (nombre del Service), por lo que Web y API resuelven PostgreSQL por nombre dentro del clúster. Ambos workloads ejecutan `dbContext.Database.Migrate()` al iniciar; el initContainer espera a que PostgreSQL acepte conexiones antes de arrancar la aplicación, evitando el reinicio por migración contra una base aún no disponible.

## Despliegue

Requisitos: clúster Kubernetes local activo (Docker Desktop con Kubernetes habilitado) e imágenes locales `licitaciones-api:v1.0.0-rc` y `licitaciones-web:v1.0.0-rc` construidas con Docker Compose (`docker compose build`).

```bash
kubectl kustomize k8s
kubectl apply --dry-run=client -k k8s
kubectl apply -k k8s
kubectl rollout status statefulset/postgres -n licitaciones
kubectl rollout status deployment/licitaciones-api -n licitaciones
kubectl rollout status deployment/licitaciones-web -n licitaciones
```

## Evidencia de despliegue

Fecha de validación: 17 y 18 de agosto de 2026.

### Recursos aplicados

- Namespace `licitaciones`: `Active`.
- ConfigMap `licitaciones-config`: 5 entradas.
- Secret `licitaciones-secret`: 2 entradas (ejemplo, sin secretos reales).
- PVC `postgres-data`: `Bound` (1Gi, RWO).
- StatefulSet `postgres`: `1/1` Ready.
- Deployments `licitaciones-api` y `licitaciones-web`: `1/1` Available.
- Services: `licitaciones-api` ClusterIP `8080`, `licitaciones-web` NodePort `8080:30080`, `postgres` ClusterIP `5432`.

### Pods

- `postgres-0`: Running, `1/1`, 0 reinicios.
- `licitaciones-api-*`: Running, `1/1`, 0 reinicios.
- `licitaciones-web-*`: Running, `1/1`, 0 reinicios.

### Probes

| Workload | Startup | Readiness | Liveness |
| --- | --- | --- | --- |
| `licitaciones-api` | `GET /health:8080` | `GET /health:8080` | `GET /health:8080` |
| `licitaciones-web` | `GET /health:8080` | `GET /health:8080` | `GET /health:8080` |
| `postgres` | `pg_isready` | `pg_isready` | `pg_isready` |

### Health checks

- `http://localhost:30080/health` (Web vía NodePort): `Healthy`.
- `http://localhost:30080/` (landing MVC): HTTP 200.
- API vía `kubectl port-forward svc/licitaciones-api 8082:8080`: `/health` = `Healthy`; `/swagger/v1/swagger.json` = HTTP 200.

### Migraciones

Las seis migraciones quedaron aplicadas en PostgreSQL dentro del clúster (tabla `__EFMigrationsHistory`):

- `20260810092133_CreateProveedores`.
- `20260811234653_MakeProveedorNameUniqueIndexPartial`.
- `20260812002104_CreateLicitaciones`.
- `20260813011055_Iteration03OfertasAprobacion`.
- `20260813205016_Iteration04TiposCambio`.
- `20260814014136_AllowDuplicateTipoCambioDates`.

### Prueba de persistencia

1. Se creó el proveedor "Proveedor Persistencia K8s Fase 7" mediante `POST /api/v1/proveedores` (id `9d51c481-714d-4f45-95aa-b847df69222e`).
2. Se eliminó el pod `postgres-0` (`kubectl delete pod postgres-0 -n licitaciones`).
3. El StatefulSet recreó el pod con la misma identidad estable y quedó Ready en aproximadamente 15 segundos.
4. El dato persistió: `SELECT count(*) FROM "Proveedores"` devolvió `1` y `GET /api/v1/proveedores?search=Persistencia K8s` devolvió el proveedor original.

La persistencia sobrevive al reinicio del pod porque el StatefulSet monta el PVC `postgres-data` en `/var/lib/postgresql/data`. No se ejecutó `kubectl delete pvc postgres-data -n licitaciones`; eliminar el PVC destruiría los datos.

## Nota sobre reinicio forzado

Durante la prueba de persistencia, una petición emitida exactamente mientras el pod de PostgreSQL se terminaba respondió `500` por `57P01: terminating connection due to administrator command`. Es un error transitorio esperado al forzar la terminación de un pod con consultas en vuelo; la siguiente petición respondió correctamente. El sistema de health check registró la caída y la recuperación sin intervención manual.

## Comandos de verificación

```bash
kubectl get namespaces
kubectl get pods -n licitaciones -o wide
kubectl get deployments,statefulsets -n licitaciones
kubectl get services -n licitaciones
kubectl get pvc -n licitaciones
kubectl get configmaps,secrets -n licitaciones
kubectl describe pod <pod> -n licitaciones
kubectl logs <pod> -n licitaciones
kubectl port-forward svc/licitaciones-api 8082:8080 -n licitaciones
kubectl exec -n licitaciones postgres-0 -- psql -U licitaciones_app -d licitaciones
```

## Limpieza

```bash
kubectl delete -k k8s
```

Para eliminar también los datos persistentes: `kubectl delete pvc postgres-data -n licitaciones` (irreversible; requiere confirmación explícita).
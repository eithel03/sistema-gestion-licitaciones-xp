# Kubernetes

## Arquitectura actual

La carpeta `/k8s` contiene diez objetos renderizados mediante Kustomize:

| Archivo | Recursos |
|---|---|
| `namespace.yaml` | Namespace `licitaciones`. |
| `app-configmap.yaml` | ConfigMap de entorno, URLs, health check y datos PostgreSQL no secretos. |
| `app-secret.example.yaml` | Secret de ejemplo con contraseña y cadena de conexión. |
| `app-deployment.yaml` | Deployments `licitaciones-web` y `licitaciones-api`. |
| `app-service.yaml` | Service ClusterIP de API y NodePort 30080 de Web. |
| `postgres-statefulset.yaml` | StatefulSet PostgreSQL 16. |
| `postgres-service.yaml` | Service PostgreSQL 5432. |
| `postgres-pvc.yaml` | PVC `postgres-data`, 1 GiB RWO. |
| `kustomization.yaml` | Agregación de todos los recursos. |

## Aplicaciones Web y API

- Una réplica de cada host.
- Imágenes `licitaciones-web:v1.0.0-rc` y `licitaciones-api:v1.0.0-rc`.
- `imagePullPolicy: IfNotPresent`.
- Puerto 8080.
- Configuración mediante `envFrom` de ConfigMap y Secret.
- Requests 100m/128Mi y limits 500m/512Mi.
- `startupProbe`, `readinessProbe` y `livenessProbe` contra `/health`.
- Init container `postgres:16` que usa `pg_isready` antes de iniciar el host.

El Service Web es NodePort 30080. El Service API es interno ClusterIP.

## PostgreSQL

- StatefulSet de una réplica con `postgres:16`.
- Service estable `postgres`.
- PVC montado en `/var/lib/postgresql/data`.
- Variables obtenidas de ConfigMap/Secret.
- Probes startup/readiness/liveness mediante `pg_isready`.
- Requests 100m/256Mi y limits 1000m/1Gi.

## Migraciones

No existe Job ni init container migrador. Los init containers solo esperan la disponibilidad de PostgreSQL. Web y API ejecutan `Database.Migrate()` durante su propio arranque y pueden intentar migrar simultáneamente.

## Kustomize y CI

`kustomization.yaml` incluye todos los manifiestos, incluido `app-secret.example.yaml`. GitHub Actions descarga Kustomize 5.5.0 y ejecuta únicamente:

```bash
/tmp/kustomize build k8s/ > /dev/null
```

CI comprueba que los archivos se puedan renderizar, pero no valida esquemas con una API Kubernetes, no aplica dry-run de servidor y no despliega un clúster.

## Secret e imágenes

- El Secret contiene valores de ejemplo y debe reemplazarse antes de un entorno no académico.
- Al estar incluido en Kustomize, `kubectl apply -k k8s` crearía el secreto de ejemplo si no se sustituye.
- `IfNotPresent` y las etiquetas `v1.0.0-rc` están orientados al clúster local documentado.
- No existe publicación automática de imágenes ni tag Git oficial `v1.0.0-rc`.

## Evidencia histórica de Fase 7

El equipo registró un despliegue real sobre Docker Desktop Kubernetes v1.34.1:

- Namespace activo y tres pods Ready;
- Deployments y StatefulSet disponibles;
- PVC Bound;
- health checks 200;
- seis migraciones aplicadas;
- persistencia después de recrear `postgres-0`.

Esta es evidencia histórica del 17–18 de agosto de 2026; no se repitió durante Fase 9. El PR `#22` fue integrado posteriormente en `main` mediante `82c8c58`.

## Limitaciones actuales

- No existe migrador independiente.
- Web/API pueden competir por migraciones.
- El Secret de ejemplo forma parte del renderizado predeterminado.
- El health check de Web no incluye PostgreSQL.
- No hay Ingress ni publicación externa de API; Web usa NodePort local.
- CI solo hace `kustomize build`.

## Comandos de referencia

No se ejecutaron en Fase 9:

```bash
kubectl kustomize k8s
kubectl apply --dry-run=client -k k8s
kubectl apply -k k8s
kubectl get all,pvc,configmap,secret -n licitaciones
```

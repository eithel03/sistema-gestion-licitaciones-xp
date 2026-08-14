# Kubernetes

Manifiestos básicos preparados para la Iteración 4 en `k8s/`.

## Recursos

- `namespace.yaml`: Namespace `licitaciones`.
- `configmap.yaml`: ambiente, configuración no secreta y variables necesarias.
- `secret.example.yaml`: ejemplo para `POSTGRES_PASSWORD` y `ConnectionStrings__DefaultConnection`, sin secretos reales.
- `postgres.yaml`: PostgreSQL 16, PVC, Deployment y Service.
- `api.yaml`: Deployment y Service de API con probes, requests y limits.
- `web.yaml`: Deployment y Service de Web con probes, requests y limits.
- `kustomization.yaml`: composición de recursos.

## Uso previsto

Antes de aplicar en un clúster real, reemplazar los valores de `secret.example.yaml` por secretos administrados para el entorno.

```bash
kubectl kustomize k8s
kubectl apply -k k8s
```

## Validación local

- `kubectl kustomize k8s`: exitoso; todos los manifiestos renderizaron correctamente.
- `kubectl apply --dry-run=client --validate=false -k k8s`: ejecutado, pero no completó la consulta al API server porque Kubernetes local de Docker Desktop no estaba iniciado o disponible en `kubernetes.docker.internal:6443`.

La falta del API server no representa un error sintáctico de los manifiestos. El renderizado Kustomize confirma la composición local de Namespace, ConfigMap, Secret de ejemplo, PostgreSQL, PVC, Deployments, Services, probes y recursos.

## Pendiente

- Aplicar los manifiestos sobre un clúster Kubernetes activo.
- Verificar pods y logs reales.
- Validar conectividad entre Web, API y PostgreSQL en el clúster.
- Comprobar persistencia real del PVC.

No se afirma que existan pods ejecutándose ni que la persistencia haya sido validada en Kubernetes.

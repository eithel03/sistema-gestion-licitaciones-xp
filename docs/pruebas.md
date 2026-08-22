# Pruebas

## Estado actual del código

La solución contiene cuatro proyectos de pruebas xUnit:

| Suite | Declaraciones `[Fact]`/`[Theory]` identificadas en Fase 9 | Alcance |
|---|---:|---|
| `Licitaciones.UnitTests` | 90 | Domain, Application y arquitectura básica. |
| `Licitaciones.IntegrationTests` | 37 | PostgreSQL, EF Core, migraciones, restricciones y concurrencia. |
| `Licitaciones.FunctionalTests` | 37 | MVC/API mediante `WebApplicationFactory` y PostgreSQL Testcontainers. |
| `Licitaciones.E2ETests` | 6 | Chromium Playwright, Web real y PostgreSQL Testcontainers. |
| **Total de declaraciones** | **170** | Una teoría puede generar varios casos ejecutados. |

El conteo de 170 corresponde a declaraciones estáticas identificables en `main@36e89ec`; no es un resultado de ejecución ni debe compararse directamente con el número de casos descubiertos por xUnit.

## Pruebas unitarias

Cubren:

- infraestructura común de Domain (`Entity`, `ValueObject`, resultados y reloj);
- normalización, caracteres y borrado lógico de proveedores;
- código, presupuesto, fecha, estados y vencimiento de licitaciones;
- reglas de oferta, mejor oferta, desempate y clasificación;
- rangos y aprobador;
- tipos de cambio, activación y conversión;
- servicios de Application y resultados controlados;
- independencia de Domain respecto de Infrastructure, Web y API.

## Pruebas de integración

Usan `Testcontainers.PostgreSql` 4.13.0 con PostgreSQL 16. Cubren:

- apertura de conexión real;
- aplicación de las seis migraciones;
- filtros, orden y paginación de proveedores;
- índices únicos parciales;
- borrado lógico;
- concurrencia de licitaciones, ofertas, niveles y tipos de cambio;
- FKs y checks de ofertas;
- rango abierto y exclusión de traslapes;
- único tipo de cambio activo;
- activación transaccional, rollback y paginación determinista.

## Pruebas funcionales

Usan `WebApplicationFactory` y bases PostgreSQL temporales. Cubren:

- landing, formularios y CRUD de proveedores;
- API de proveedores y licitaciones;
- transiciones de licitación;
- flujos MVC/API de ofertas y aprobaciones;
- tipo de cambio, conversión, tema y entrada decimal localizada;
- health endpoint;
- rutas OpenAPI, Swagger UI, ProblemDetails y correlación.

Las pruebas OpenAPI comprueban rutas, verbos y presencia de contratos nominales; no demuestran que los esquemas manuales describan todas las propiedades, cuerpos y respuestas.

## Pruebas E2E

La suite usa Playwright con Chromium, un host Web iniciado con Kestrel y PostgreSQL Testcontainers. Sus seis flujos son:

- navegación desde landing a módulos principales;
- alta, edición y duplicidad de proveedor;
- creación, publicación y cierre de licitación;
- registro y validación visible de oferta;
- tema persistido después de recargar;
- conversión visual USD y retorno a CRC.

No existen flujos E2E exclusivos para el CRUD completo de niveles de aprobación o tipos de cambio.

## Evidencia histórica

La Fase 5 y validaciones posteriores registraron históricamente:

| Suite | Casos aprobados reportados |
|---|---:|
| UnitTests | 121/121 |
| IntegrationTests | 37/37 |
| FunctionalTests | 54/54 |
| E2ETests | 6/6 |
| **Total** | **218/218** |

Cobertura de líneas históricamente reportada:

| Assembly | Cobertura |
|---|---:|
| Domain | 91,64 % |
| Application | 88,60 % |
| Infrastructure | 95,43 % |
| API | 90,03 % |
| Web | 66,53 % |
| **Global** | **89,37 %** |

La cobertura global de ramas se registró como 62,78 %. Estos resultados pertenecen a ejecuciones anteriores documentadas por el equipo. No se regeneraron durante Fase 9 y no existe un archivo Cobertura/TRX rastreado que permita presentarlos como ejecución actual.

Los documentos [pruebas-iteracion-04.md](pruebas-iteracion-04.md) e [iteracion-04-evidencia.md](iteraciones/iteracion-04-evidencia.md) conservan la medición histórica intermedia de 174 casos previa a la ampliación de Fase 5.

## Cobertura configurada

- `coverage.runsettings` recopila Cobertura para Unit, Integration y Functional.
- E2E no se instrumenta porque Web se ejecuta en otro proceso.
- ReportGenerator `5.4.17` combina los tres archivos.
- `scripts/verify-coverage.ps1` exige:
  - Domain ≥ 80 %;
  - Application ≥ 80 %;
  - Global ≥ 70 %.

## Integración continua

`.github/workflows/ci.yml` ejecuta en `push` y `pull_request`:

1. restore de la solución;
2. build Release;
3. instalación de Chromium;
4. Unit, Integration, Functional y E2E;
5. combinación y umbrales de cobertura;
6. carga del reporte de cobertura;
7. verificación de formato;
8. revisión de vulnerabilidades NuGet;
9. construcción de imágenes Web/API;
10. `kustomize build k8s/`;
11. job agregador `ci-complete`.

Limitaciones reales del workflow:

- formato usa `continue-on-error: true`, por lo que no bloquea el job;
- vulnerabilidades generan warning pero no provocan fallo;
- el análisis estático se limita a analizadores configurados en el build y formato; no hay Sonar ni CodeQL;
- Kubernetes solo se renderiza con Kustomize; no hay validación contra API de clúster ni despliegue;
- las imágenes se construyen, pero no se publican;
- no existe despliegue automático.

La documentación histórica registra que los checks de Fase 8 aprobaron antes del merge `36e89ec`. Fase 9 no volvió a ejecutar GitHub Actions.

## Casos no cubiertos o débiles

- carrera concurrente de unicidad y `Version` de proveedores;
- migraciones simultáneas iniciadas por Web y API;
- health check de PostgreSQL desde Web;
- navegación MVC entre páginas de ofertas, niveles y tipos de cambio;
- pantalla MVC de mejor oferta/aprobador;
- semántica completa del OpenAPI manual;
- validación Kubernetes con esquema/admission desde CI;
- health checks de Web/API en Docker Compose.

## Comandos reproducibles

Estos comandos describen la configuración; no se ejecutaron en Fase 9:

```powershell
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test tests/Licitaciones.UnitTests/Licitaciones.UnitTests.csproj --configuration Release --no-build
dotnet test tests/Licitaciones.IntegrationTests/Licitaciones.IntegrationTests.csproj --configuration Release --no-build
dotnet test tests/Licitaciones.FunctionalTests/Licitaciones.FunctionalTests.csproj --configuration Release --no-build
dotnet test tests/Licitaciones.E2ETests/Licitaciones.E2ETests.csproj --configuration Release --no-build
```

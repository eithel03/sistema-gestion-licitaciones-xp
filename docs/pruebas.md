# Pruebas

## Estado actual

La Fase 3 preparo la suite unitaria para sostener TDD en las siguientes historias. La base actual incluye pruebas tecnicas de arquitectura y ejemplos minimos de dominio.

## Pruebas unitarias agregadas en Fase 3

- `EntityTests`: igualdad por identidad y rechazo de identificadores por defecto.
- `ValueObjectTests`: igualdad por componentes.
- `ValidationResultTests`: resultados exitosos, errores y proteccion contra fallos sin errores.
- `IClockTests`: reemplazo del reloj por una implementacion fija en pruebas.

## Comandos previstos

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build
```

## Resultado de validación

Durante la sesión original del driver no fue posible ejecutar la suite con .NET 9 debido a que su entorno no disponía del SDK requerido por `global.json`.

Posteriormente, la Fase 3 fue validada por el navigator en un entorno con SDK .NET `9.0.305`.

Se ejecutaron correctamente:

```bash
dotnet restore Licitaciones.sln
dotnet build Licitaciones.sln --configuration Release --no-restore
dotnet test Licitaciones.sln --configuration Release --no-build

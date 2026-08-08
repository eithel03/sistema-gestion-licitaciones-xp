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

## Resultado local

No se pudo ejecutar la suite directamente en esta maquina porque `global.json` requiere el SDK `9.0.305` y el entorno local tiene instalados los SDK `8.0.418` y `10.0.102`.

Se realizo una verificacion alternativa en `C:\tmp` sin `global.json`: la solucion compilo con el SDK `10.0.102` con `0` errores y `0` advertencias, pero la ejecucion de pruebas se aborto porque falta el runtime `Microsoft.NETCore.App 9.0.0`. No se modifica `global.json` porque la configuracion del proyecto y GitHub Actions estan orientadas a .NET 9.

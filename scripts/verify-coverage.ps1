[CmdletBinding()]
param(
    [string]$ReportPath = (Join-Path $PSScriptRoot '..\TestResults\coverage\Cobertura.xml')
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "No existe el reporte combinado: $ReportPath"
}

[xml]$report = Get-Content -LiteralPath $ReportPath
$packages = @{}
foreach ($package in $report.coverage.packages.package) {
    $packages[$package.name] = [double]$package.'line-rate' * 100
}

function Get-Rate([string]$name) {
    if (-not $packages.ContainsKey($name)) {
        throw "El reporte no contiene el ensamblado requerido: $name"
    }
    return $packages[$name]
}

$domain = Get-Rate 'Licitaciones.Domain'
$application = Get-Rate 'Licitaciones.Application'
$global = [double]$report.coverage.'line-rate' * 100

Write-Host ("Domain: {0:N2} %" -f $domain)
Write-Host ("Application: {0:N2} %" -f $application)
Write-Host ("Global: {0:N2} %" -f $global)

$failed = @()
if ($domain -lt 80) { $failed += 'Domain < 80 %' }
if ($application -lt 80) { $failed += 'Application < 80 %' }
if ($global -lt 70) { $failed += 'Global < 70 %' }

if ($failed.Count -gt 0) {
    Write-Error ("Umbrales incumplidos: " + ($failed -join ', '))
    exit 1
}

exit 0

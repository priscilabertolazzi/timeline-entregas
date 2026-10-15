# Testes do backend e do frontend, dentro de containers: não exige o SDK do .NET nem o Node.js instalados (ADR-24).
# Uso: powershell -ExecutionPolicy Bypass -File scripts\test.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

docker compose run --rm -T ferramentas test
$codigoBackend = $LASTEXITCODE
docker compose run --rm -T ferramentas-web test
$codigoFrontend = $LASTEXITCODE

if ($codigoBackend -ne 0) { exit $codigoBackend }
exit $codigoFrontend

# Testes do backend, dentro de um container: não exige o SDK do .NET instalado (ADR-24).
# Uso: powershell -ExecutionPolicy Bypass -File scripts\test.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

docker compose run --rm -T ferramentas test
exit $LASTEXITCODE

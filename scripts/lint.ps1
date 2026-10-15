# Verificação do padrão do código do backend e do frontend, dentro de containers (ADR-24).
# Uso: powershell -ExecutionPolicy Bypass -File scripts\lint.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

docker compose run --rm -T ferramentas lint
$codigoBackend = $LASTEXITCODE
docker compose run --rm -T ferramentas-web lint
$codigoFrontend = $LASTEXITCODE

if ($codigoBackend -ne 0) { exit $codigoBackend }
exit $codigoFrontend

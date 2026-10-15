# Verificação do padrão do código do backend, dentro de um container (ADR-24).
# Uso: powershell -ExecutionPolicy Bypass -File scripts\lint.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

docker compose run --rm -T ferramentas lint
exit $LASTEXITCODE

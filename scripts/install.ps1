# Instalação: cria o .env (se não existir) e baixa as imagens e os pacotes do backend.
# Uso: powershell -ExecutionPolicy Bypass -File scripts\install.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host 'O Docker não foi encontrado. Instale o Docker e abra-o antes de continuar.' -ForegroundColor Red
    exit 1
}

if (-not (Test-Path .env)) {
    Copy-Item .env.example .env
    Write-Host 'Criei o arquivo .env a partir do .env.example. Troque a senha do banco nele, se quiser.'
} else {
    Write-Host 'O arquivo .env já existe e foi mantido.'
}

Write-Host 'Baixando as imagens do banco e das ferramentas...'
docker compose --profile ferramentas pull
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host 'Baixando os pacotes do backend...'
docker compose run --rm -T ferramentas restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host 'Instalação concluída.'

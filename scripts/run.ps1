# Execução: sobe banco e API pelo Docker Compose e informa os endereços.
# Uso: powershell -ExecutionPolicy Bypass -File scripts\run.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

if (-not (Test-Path .env)) {
    Write-Host 'O arquivo .env não existe. Rode primeiro a instalação: powershell -ExecutionPolicy Bypass -File scripts\install.ps1' -ForegroundColor Red
    exit 1
}

docker compose up -d --build --wait db api web
if ($LASTEXITCODE -ne 0) {
    Write-Host 'Não consegui subir o sistema. Se a mensagem acima diz que uma porta não está disponível, outro programa já usa essa porta.' -ForegroundColor Yellow
    Write-Host 'Troque o valor de DB_PORT, de API_PORT ou de WEB_PORT no arquivo .env (por exemplo, para 5433, 8081 ou 4201) e rode este script de novo.' -ForegroundColor Yellow
    exit $LASTEXITCODE
}

# Lê as portas do .env, com os mesmos padrões do docker-compose.yml.
function Ler-Porta($nome, $padrao) {
    $linha = Select-String -Path .env -Pattern "^$nome=(.+)$" | Select-Object -Last 1
    if ($linha) { return $linha.Matches[0].Groups[1].Value.Trim() }
    return $padrao
}
$portaDb = Ler-Porta 'DB_PORT' '5432'
$portaApi = Ler-Porta 'API_PORT' '8080'
$portaWeb = Ler-Porta 'WEB_PORT' '4200'

Write-Host "Banco de dados pronto em localhost:$portaDb."
Write-Host "API pronta em http://localhost:$portaApi (abre a página de documentação)."
Write-Host "Lista de aplicações: http://localhost:$portaApi/api/aplicacoes"
Write-Host "Telas prontas em http://localhost:$portaWeb."
Write-Host 'Para parar: docker compose down (os dados ficam guardados).'

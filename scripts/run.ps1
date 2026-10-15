# Execução: sobe o banco pelo Docker Compose e informa o endereço.
# A API e as telas entram nas tarefas T-08 e T-09.
# Uso: powershell -ExecutionPolicy Bypass -File scripts\run.ps1
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path -Parent $PSScriptRoot)

if (-not (Test-Path .env)) {
    Write-Host 'O arquivo .env não existe. Rode primeiro a instalação: powershell -ExecutionPolicy Bypass -File scripts\install.ps1' -ForegroundColor Red
    exit 1
}

docker compose up -d --wait db
if ($LASTEXITCODE -ne 0) {
    Write-Host 'Não consegui subir o banco. Se a mensagem acima diz que a porta não está disponível, outro programa já usa a porta do banco.' -ForegroundColor Yellow
    Write-Host 'Troque o valor de DB_PORT no arquivo .env (por exemplo, para 5433) e rode este script de novo.' -ForegroundColor Yellow
    exit $LASTEXITCODE
}

# Lê a porta do .env, com o mesmo padrão do docker-compose.yml.
$porta = '5432'
$linha = Select-String -Path .env -Pattern '^DB_PORT=(.+)$' | Select-Object -Last 1
if ($linha) { $porta = $linha.Matches[0].Groups[1].Value.Trim() }

Write-Host "Banco de dados pronto em localhost:$porta."
Write-Host 'Para parar: docker compose down (os dados ficam guardados).'

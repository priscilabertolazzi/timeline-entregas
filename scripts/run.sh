#!/bin/sh
# Execução: sobe banco, API e telas pelo Docker Compose e informa os endereços.
set -eu
cd "$(dirname "$0")/.."

if [ ! -f .env ]; then
  echo "O arquivo .env não existe. Rode primeiro a instalação (scripts/install.sh ou make install)." >&2
  exit 1
fi

if ! docker compose up -d --build --wait db api web; then
  echo "Não consegui subir o sistema. Se a mensagem acima diz que uma porta não está disponível, outro programa já usa essa porta." >&2
  echo "Troque o valor de DB_PORT, de API_PORT ou de WEB_PORT no arquivo .env (por exemplo, para 5433, 8081 ou 4201) e rode este script de novo." >&2
  exit 1
fi

# Lê as portas do .env, com os mesmos padrões do docker-compose.yml.
porta_db="$(sed -n 's/^DB_PORT=//p' .env | tail -n 1)"
porta_api="$(sed -n 's/^API_PORT=//p' .env | tail -n 1)"
porta_web="$(sed -n 's/^WEB_PORT=//p' .env | tail -n 1)"
echo "Banco de dados pronto em localhost:${porta_db:-5432}."
echo "API pronta em http://localhost:${porta_api:-8080} (abre a página de documentação)."
echo "Lista de aplicações: http://localhost:${porta_api:-8080}/api/aplicacoes"
echo "Telas prontas em http://localhost:${porta_web:-4200}."
echo "Para parar: docker compose down (os dados ficam guardados)."

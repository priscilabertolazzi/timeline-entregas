#!/bin/sh
# Execução: sobe o banco pelo Docker Compose e informa o endereço.
# A API e as telas entram nas tarefas T-08 e T-09.
set -eu
cd "$(dirname "$0")/.."

if [ ! -f .env ]; then
  echo "O arquivo .env não existe. Rode primeiro a instalação (scripts/install.sh ou make install)." >&2
  exit 1
fi

if ! docker compose up -d --wait db; then
  echo "Não consegui subir o banco. Se a mensagem acima diz que a porta não está disponível, outro programa já usa a porta do banco." >&2
  echo "Troque o valor de DB_PORT no arquivo .env (por exemplo, para 5433) e rode este script de novo." >&2
  exit 1
fi

# Lê a porta do .env, com o mesmo padrão do docker-compose.yml.
porta="$(sed -n 's/^DB_PORT=//p' .env | tail -n 1)"
echo "Banco de dados pronto em localhost:${porta:-5432}."
echo "Para parar: docker compose down (os dados ficam guardados)."

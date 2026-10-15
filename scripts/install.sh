#!/bin/sh
# Instalação: cria o .env (se não existir) e baixa as imagens e os pacotes do backend.
set -eu
cd "$(dirname "$0")/.."

if ! command -v docker >/dev/null 2>&1; then
  echo "O Docker não foi encontrado. Instale o Docker e abra-o antes de continuar." >&2
  exit 1
fi

if [ ! -f .env ]; then
  cp .env.example .env
  echo "Criei o arquivo .env a partir do .env.example. Troque a senha do banco nele, se quiser."
else
  echo "O arquivo .env já existe e foi mantido."
fi

echo "Baixando as imagens do banco e das ferramentas..."
docker compose --profile ferramentas pull

echo "Baixando os pacotes do backend..."
docker compose run --rm -T ferramentas restore

echo "Instalação concluída."

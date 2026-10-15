#!/bin/sh
# Roda DENTRO do container de ferramentas (docker-compose.yml, serviço "ferramentas").
# Uso: ferramentas-container.sh restore | test | lint
# Trabalha numa cópia do código, sem as pastas bin e obj, para não misturar arquivos de
# compilação do computador com os do container.
set -eu

comando="${1:-}"

mkdir -p /work
tar -C /origem -cf - --exclude=bin --exclude=obj . | tar -C /work -xf -
cd /work/backend

case "$comando" in
  restore) dotnet restore ;;
  test) dotnet test ;;
  lint) dotnet format --verify-no-changes ;;
  *)
    echo "Comando desconhecido: '$comando'. Use restore, test ou lint." >&2
    exit 2
    ;;
esac

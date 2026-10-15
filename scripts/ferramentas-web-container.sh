#!/bin/sh
# Roda DENTRO do container de ferramentas do frontend (docker-compose.yml, serviço "ferramentas-web").
# Uso: ferramentas-web-container.sh restore | test | lint
# Trabalha numa cópia do código, sem node_modules, dist e .angular, para não misturar arquivos
# do computador com os do container.
set -eu

comando="${1:-}"

mkdir -p /work
tar -C /origem/frontend -cf - --exclude=node_modules --exclude=dist --exclude=.angular . | tar -C /work -xf -
cd /work

# Instala exatamente as versões do package-lock.json.
npm ci

case "$comando" in
  restore) ;;
  test) npm test -- --watch=false ;;
  lint) npm run lint ;;
  *)
    echo "Comando desconhecido: '$comando'. Use restore, test ou lint." >&2
    exit 2
    ;;
esac

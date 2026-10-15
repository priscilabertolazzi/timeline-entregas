#!/bin/sh
# Verificação do padrão do código do backend e do frontend, dentro de containers (ADR-24).
set -u
cd "$(dirname "$0")/.."

docker compose run --rm -T ferramentas lint
codigo_backend=$?
docker compose run --rm -T ferramentas-web lint
codigo_frontend=$?

[ "$codigo_backend" -ne 0 ] && exit "$codigo_backend"
exit "$codigo_frontend"

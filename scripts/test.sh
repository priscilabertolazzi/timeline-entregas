#!/bin/sh
# Testes do backend e do frontend, dentro de containers: não exige o SDK do .NET nem o Node.js instalados (ADR-24).
set -u
cd "$(dirname "$0")/.."

docker compose run --rm -T ferramentas test
codigo_backend=$?
docker compose run --rm -T ferramentas-web test
codigo_frontend=$?

[ "$codigo_backend" -ne 0 ] && exit "$codigo_backend"
exit "$codigo_frontend"

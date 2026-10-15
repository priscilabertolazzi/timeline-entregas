#!/bin/sh
# Verificação do padrão do código do backend, dentro de um container (ADR-24).
set -eu
cd "$(dirname "$0")/.."

docker compose run --rm -T ferramentas lint

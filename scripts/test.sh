#!/bin/sh
# Testes do backend, dentro de um container: não exige o SDK do .NET instalado (ADR-24).
set -eu
cd "$(dirname "$0")/.."

docker compose run --rm -T ferramentas test

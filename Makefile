# Atalhos que chamam os scripts de scripts/. No Windows, use os arquivos .ps1.
.PHONY: install run test lint

install:
	sh scripts/install.sh

run:
	sh scripts/run.sh

test:
	sh scripts/test.sh

lint:
	sh scripts/lint.sh

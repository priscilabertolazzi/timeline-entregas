# Timeline de Entregas

> Esqueleto do README. Cada seção abaixo ainda está sem conteúdo e traz uma linha dizendo o que vai conter. O conteúdo é preenchido ao longo das tarefas do `docs/tarefas.md`.

## 1. Título e descrição

*Vai conter: o problema (a manutenção manual do status report semanal) e a solução (cadastro de aplicações e versões, timeline calculada automaticamente, tabela de status e congelamento semanal).*

## 2. O que foi entregue

*Vai conter: a lista do que está funcionando na versão atual, por degrau concluído, e o que ficou de fora.*

## 3. Tabela de conteúdos

- [1. Título e descrição](#1-título-e-descrição)
- [2. O que foi entregue](#2-o-que-foi-entregue)
- [3. Tabela de conteúdos](#3-tabela-de-conteúdos)
- [4. Tecnologias utilizadas](#4-tecnologias-utilizadas)
- [5. Pré-requisitos](#5-pré-requisitos)
- [6. Dependências e ambiente](#6-dependências-e-ambiente)
- [7. Instalação e execução local](#7-instalação-e-execução-local)
- [8. Uso e exemplos](#8-uso-e-exemplos)
- [9. Estrutura do projeto](#9-estrutura-do-projeto)
- [10. Arquitetura da aplicação](#10-arquitetura-da-aplicação)
- [11. Testes](#11-testes)
- [12. Como a IA foi usada no desenvolvimento](#12-como-a-ia-foi-usada-no-desenvolvimento)
- [13. Demonstração visual](#13-demonstração-visual)
- [14. Limitações e próximos passos](#14-limitações-e-próximos-passos)
- [15. Créditos e licença](#15-créditos-e-licença)

## 4. Tecnologias utilizadas

*Vai conter: a linguagem e as ferramentas do backend, do frontend e do banco, o Docker, e o assistente de IA (Claude Code) com os modelos usados em cada fase.*

## 5. Pré-requisitos

Para **usar e avaliar** o projeto, basta instalar:

| Ferramenta | Para quê | Onde obter |
|---|---|---|
| Git | Baixar o projeto | git-scm.com |
| Docker (com Docker Compose) | Rodar o banco e executar os testes e o lint em container | docker.com |

O Docker precisa estar **aberto e funcionando** antes dos comandos abaixo.

> **Limitação conhecida:** o Docker Desktop é gratuito para uso pessoal e em empresas pequenas, mas exige assinatura em empresas de maior porte. Há alternativas gratuitas, como o Docker Engine no WSL 2.

Só para **desenvolver** (alterar o código), instale também o SDK do .NET 10 (dotnet.microsoft.com) e o Node.js em versão LTS (nodejs.org). Os scripts de teste e de lint **não** exigem o SDK nem o Node.js instalados.

## 6. Dependências e ambiente

*Vai conter: os arquivos de dependências (`.csproj`, `global.json`, `package.json`, `package-lock.json` e a versão da imagem do banco no `docker-compose.yml`) e de ambiente (`.env` e `.env.example`), com a equivalência ao `requirements.txt` e ao `pyproject.toml` de projetos Python.*

## 7. Instalação e execução local

**1. Baixar o projeto** (em qualquer sistema; no Windows, escolha uma pasta fora do OneDrive e sem espaços no caminho):

```bash
git clone <endereço do repositório> timeline-entregas
cd timeline-entregas
```

**2. Os quatro comandos do projeto**

| Ação | Windows (PowerShell) | Linux e macOS |
|---|---|---|
| Instalar | `powershell -ExecutionPolicy Bypass -File scripts\install.ps1` | `make install` |
| Executar | `powershell -ExecutionPolicy Bypass -File scripts\run.ps1` | `make run` |
| Testar | `powershell -ExecutionPolicy Bypass -File scripts\test.ps1` | `make test` |
| Verificar o padrão do código | `powershell -ExecutionPolicy Bypass -File scripts\lint.ps1` | `make lint` |

Em Linux e macOS, sem o `make`, use `sh scripts/install.sh`, `sh scripts/run.sh`, `sh scripts/test.sh` e `sh scripts/lint.sh`. No Windows, os arquivos `.sh` também rodam pelo Git Bash.

- **Instalar** cria o arquivo `.env` a partir do `.env.example` (se ele ainda não existir) e baixa as imagens do Docker e os pacotes do backend e do frontend. A primeira vez leva alguns minutos e precisa de internet.
- **Executar** sobe o banco de dados (PostgreSQL), a API e as telas, e informa os endereços. As telas abrem em `http://localhost:4200` (porta `WEB_PORT` do `.env`) e, por enquanto, têm a lista de aplicações (cadastro, edição e exclusão de aplicações, e cadastro de versões) e a tela da versão (rótulo, fase, situação, observação e as sete datas, com um só botão Salvar). A API abre em `http://localhost:8080` (página de documentação) e, com o banco vazio, já vem com as aplicações de exemplo Alfa, Beta, Gama e Delta (veja `http://localhost:8080/api/aplicacoes`). O fuso horário da API vem da variável `TZ` do `.env` (padrão `America/Sao_Paulo`); `SEED_DADOS_EXEMPLO=false` desliga os dados de exemplo. Para parar: `docker compose down` (os dados ficam guardados).
- Se o **Executar** avisar que a porta não está disponível, outro programa (por exemplo, um PostgreSQL já instalado) usa a porta 5432. Troque o valor de `DB_PORT` (ou de `API_PORT` e `WEB_PORT`, se for uma delas) no arquivo `.env` (por exemplo, `DB_PORT` para `5433`) e rode de novo.
- O arquivo `.env` guarda a configuração local, inclusive a senha do banco, e **nunca é versionado**. O `.env.example` traz só valores fictícios.
- **A API e o banco:** ao iniciar, a API cria sozinha as tabelas `aplicacao` e `versao` no banco (schema `timeline`) e atualiza o que faltar. Ela lê a conexão das mesmas variáveis do `.env` (`POSTGRES_*` e `DB_PORT`). Para rodá-la no seu computador, em vez de no Docker, com o banco já no ar (antes, pare a API do Docker com `docker compose stop api`), use o bloco abaixo.

  ```powershell
  Get-Content .env | Where-Object { $_ -match '^\s*[^#\s].*=' } | ForEach-Object { $n, $v = $_ -split '=', 2; Set-Item "env:$n" $v }
  dotnet run --project backend\src\Host
  ```

  A primeira linha copia o `.env` para a sessão do PowerShell; a segunda inicia a API (Ctrl+C para parar). Quando aparecer "Now listening on: http://localhost:8080", abra esse endereço no navegador: ele leva à **página de documentação da API** (seção 8). A porta é a `API_PORT` do `.env`, e a API só aceita chamadas do próprio computador. Para conferir o banco: `docker compose exec db psql -U timeline -d timeline_entregas -c "\d+ timeline.versao"`.

## 8. Uso e exemplos

A API tem uma **página de documentação** (Scalar), aberta no navegador em `http://localhost:8080/scalar` (ou apenas `http://localhost:8080`), com a API em execução (seção 7). Ela lista as operações, mostra os textos de erro possíveis e permite testar cada operação sem instalar nada.

**Exemplo: cadastrar a aplicação Alfa e conferir a recusa de nome repetido** (CA-33)

1. Na coluna da esquerda, em **Aplicações**, clique em **Cadastra uma aplicação**.
2. Clique em **Test Request**. O corpo já vem com `{ "nome": "Alfa" }`.
3. Clique em **Send**. Resultado esperado: `201 Created`, com o `id` e o `nome` da aplicação.
4. Clique em **Send** de novo (ou troque o nome para `alfa`). Resultado esperado: `409 Conflict`, com a mensagem **"Já existe uma aplicação com esse nome."**

As mesmas requisições, na ordem de uso e com o resultado esperado de cada uma, estão em [`http/exemplos.http`](http/exemplos.http) (para ferramentas que executam esse tipo de arquivo, como a extensão REST Client do VS Code, ou para leitura).

**Como os erros são devolvidos:** sempre no mesmo formato, com o código HTTP (400 dado inválido, 404 não encontrado, 409 nome repetido, 422 regra de negócio, 500 erro inesperado), o campo `title` com a primeira mensagem e a lista `mensagens` com todas elas. O texto de cada mensagem está no catálogo do `docs/tecnico/arquitetura.md`, seção 7.6.

*Este texto cresce a cada API entregue (versões, timeline, tabela de status e congelamento).*

## 9. Estrutura do projeto

*Vai conter: a árvore de pastas e o papel de cada uma.*

## 10. Arquitetura da aplicação

*Vai conter: o resumo das camadas do backend, do frontend e do banco, e o link para o `docs/tecnico/arquitetura.md`.*

## 11. Testes

**Como rodar**

| Ação | Windows (PowerShell) | Linux e macOS |
|---|---|---|
| Testes do backend | `powershell -ExecutionPolicy Bypass -File scripts\test.ps1` | `make test` |
| Padrão do código | `powershell -ExecutionPolicy Bypass -File scripts\lint.ps1` | `make lint` |

Os dois rodam dentro de um container, que já traz o SDK do .NET: não é preciso instalar o .NET no computador. O container trabalha numa cópia do código, então os arquivos do seu computador não são alterados.

**O que cada tipo de teste cobre** (será completado ao longo das tarefas)

- **Estrutura (já existe):** confere que o `Domain` não depende de nenhuma outra camada, de pacotes nem de frameworks de infraestrutura.
- **Regras do `Domain` e da `Application`:** um teste por critério de aceite, a partir da T-04. Os testes da `Application` usam um repositório em memória, sem banco.
- **Erros da API:** conferem o código HTTP e o texto de cada erro, e que o erro inesperado não mostra detalhes internos (`Timeline.Api.Tests`).

## 12. Como a IA foi usada no desenvolvimento

*Vai conter: o resumo do processo de trabalho com o assistente de IA, tirado do `docs/diario.md` e do `prompts/prompts-claude-code.md`.*

### Onde a IA acelerou o desenvolvimento

*Vai conter: o tempo real comparado à estimativa sem IA e o resultado de cada sessão.*

### Onde a revisão humana foi decisiva

*Vai conter: o que a revisão pelo comportamento e pelos critérios de aceite capturou.*

### Quais prompts funcionaram melhor

*Vai conter: os pedidos mais eficazes e o que os tornou eficazes.*

### Quais riscos foram mitigados

*Vai conter: os riscos do uso de IA que apareceram ou foram evitados, e como.*

## 13. Demonstração visual

*Vai conter: capturas de tela das telas do sistema, guardadas em `docs/imagens/`.*

## 14. Limitações e próximos passos

*Vai conter: o que ficou fora da versão entregue, os degraus não concluídos e os incrementos previstos.*

## 15. Créditos e licença

*Vai conter: a autoria, os créditos das ferramentas e a licença MIT (arquivo `LICENSE`).*

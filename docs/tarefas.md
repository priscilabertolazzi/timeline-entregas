# Tarefas do Projeto
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão | 0.12 |
| Data | 15/10/2026 |
| Documentos base | `docs/negocio/requisitos-mvp.md` (versão 0.6) e `docs/tecnico/arquitetura.md` (versão 0.5) |

Roteiro de desenvolvimento em tarefas pequenas. Cada tarefa segue o ciclo e a definição de pronto do `docs/tecnico/processo-desenvolvimento.md`.

---

## Como usar

- Ao concluir a tarefa, marque `[x]` e registre a sessão no `docs/diario.md`, com o tempo real, a estimativa sem IA, o modelo usado, o que funcionou e o que a revisão capturou.
- Use os prompts do roteiro (Parte 1 do `prompts/prompts-claude-code.md`) e registre na Parte 2 os pedidos mais relevantes feitos ao Claude Code, com o resultado de cada um.
- As estimativas consideram desenvolvimento assistido por IA.
- Tarefas marcadas como **TDD** começam pelos testes derivados dos critérios de aceite.
- Em cada tarefa, **"Sua verificação"** descreve o que a autora confere para aceitar a tarefa. Os comandos são sempre indicados pelo Claude Code, com explicação em português simples.
- As mensagens de erro citadas (MSG-XX) estão no catálogo do `arquitetura.md`, seção 7.6.

## Entrega em degraus

O sistema é construído em degraus (`arquitetura.md`, ADR-22). Cada degrau termina com algo que funciona e com uma tag no Git, e é um ponto de parada seguro. O primeiro objetivo é chegar à entrega com o trabalho completo e funcionando; cada tela vem depois da API correspondente, e o congelamento fica por último.

| Etapa | Tarefas | Estimativa | Acumulado | Tag ao final |
|---|---|---|---|---|
| 0. Preparação | T-01 a T-03 | 4,0 h | 4,0 h | — |
| 1. APIs de cadastro | T-04 a T-08 | 7,0 h | 11,0 h | `v0.1.0` |
| 2. Telas de cadastro | T-09 e T-10 | 4,0 h | 15,0 h | `v0.2.0` |
| 3. API da timeline | T-11 a T-13 | 5,0 h | 20,0 h | `v0.3.0` |
| 4. Tela da timeline | T-14 | 5,0 h | 25,0 h | `v0.4.0` |
| 5. API da tabela de status das aplicações | T-15 | 1,5 h | 26,5 h | `v0.5.0` |
| 6. Tela da tabela de status das aplicações | T-16 | 1,0 h | 27,5 h | `v0.6.0` |
| 7. Congelamento | T-17 a T-19 | 5,0 h | 32,5 h | `v0.7.0` |
| Entrega (tempo protegido) | T-20 a T-22 | 4,0 h | — | `v1.0.0` |

**Regra do corte**

- O limite é de cerca de 30 horas. A entrega leva 4 horas e **nunca é cortada**. Por isso, os degraus param quando o **tempo real** acumulado, somado no diário, chegar a **26 horas**.
- Antes de começar cada degrau, compare o tempo real acumulado mais a estimativa do degrau com as 26 horas. Se passar, a autora decide entre seguir ou ir direto para a entrega. Pelas estimativas, a decisão aparece no degrau 5 (API da tabela de status das aplicações), que termina em 26,5 horas.
- Os degraus não concluídos entram no README, em "Limitações e próximos passos". Os requisitos deles já estão escritos.

**Revisão independente:** a última tarefa de cada degrau inclui a revisão independente do código, antes da tag (`processo-desenvolvimento.md`, seção 7.3). Ela leva cerca de 20 minutos e consome a margem das estimativas.

---

## Etapa 0 — Preparação

### [x] T-01 — Preparação do ambiente (2 h)

Feita pela autora, com este roteiro. Ao final, todas as verificações devem funcionar.

**1. Verificar o computador**
- Versão do Windows: Configurações → Sistema → Sobre. Precisa ser Windows 10 ou 11, 64 bits.
- Memória: na mesma tela, "RAM instalada". Recomendado: 8 GB ou mais.
- Virtualização: Gerenciador de Tarefas (Ctrl + Shift + Esc) → Desempenho → CPU → "Virtualização: Habilitado". Se estiver desabilitado, pedir ajuda ao Claude antes de seguir.

**2. Instalar as ferramentas, sempre pelos sites oficiais**
- Git: git-scm.com. Durante a instalação, manter as opções padrão (inclui o Git Bash).
- SDK do .NET 10: dotnet.microsoft.com.
- Node.js, versão LTS: nodejs.org. Usado nas telas, a partir da T-09.
- Docker Desktop: docker.com. Aceitar o uso do WSL 2 quando solicitado e reiniciar o computador se pedido.
- Claude Code: usar pela área **Code** do aplicativo Claude para computador (claude.ai/download), que já inclui o Claude Code, sem instalação separada. Conferir se o seu plano da conta Claude inclui o Claude Code (support.claude.com). A versão de terminal (comando `claude`) também funciona, pela documentação oficial (docs.claude.com).
- Conta no GitHub: github.com, se ainda não tiver.

**3. Verificar as instalações**, num terminal (PowerShell):

| Comando | Resultado esperado |
|---|---|
| `git --version` | Mostra a versão do Git |
| `dotnet --version` | Mostra uma versão começando por 10 |
| `node --version` | Mostra a versão do Node.js |
| `docker --version` | Mostra a versão do Docker |
| `docker run hello-world` | Mostra a mensagem "Hello from Docker!" |

**4. Configurar o Git** com seu nome e e-mail (o Claude Code indica os dois comandos).

**5. Montar a pasta do projeto.** Crie a pasta `C:\projetos\timeline-entregas`, fora de pastas sincronizadas pelo OneDrive (como "Documentos" e "Área de Trabalho") e sem espaços nem acentos no caminho. Coloque nela os documentos, já com os nomes e lugares definitivos:

```
timeline-entregas\
├── CLAUDE.md                          (era CLAUDE-raiz.md)
├── backend\CLAUDE.md                  (era CLAUDE-backend.md)
├── frontend\CLAUDE.md                 (era CLAUDE-frontend.md)
├── prompts\prompts-claude-code.md
└── docs\
    ├── negocio\requisitos-mvp.md
    ├── negocio\requisitos-negocio.md
    ├── tecnico\arquitetura.md
    ├── tecnico\processo-desenvolvimento.md
    ├── tarefas.md
    └── diario.md
```

O Claude Code lê o `CLAUDE.md` da pasta escolhida para trabalhar; com esses nomes, ele já conhece as regras do projeto desde a primeira sessão. Documentos de trabalho que não fazem parte do repositório ficam fora desta pasta.

**Sua verificação:** todos os comandos da tabela funcionam, a pasta do projeto está montada como acima e a área Code do aplicativo Claude abre com a pasta `C:\projetos\timeline-entregas` escolhida.

### [x] T-02 — Repositório e documentação inicial (1 h)
- Inicializar o repositório Git.
- Criar, **antes do primeiro commit**, o `.gitignore` (`arquitetura.md`, seção 9), o `.gitattributes` e o `.editorconfig` (`arquitetura.md`, seção 4).
- Criar o `.env.example`, o `LICENSE` (MIT) e o esqueleto do `README.md` com todas as seções previstas, ainda vazias.
- Versionar a pasta `docs/`, os três `CLAUDE.md` e o `prompts/prompts-claude-code.md`, colocados na pasta do projeto na T-01.
- Criar o repositório no GitHub como **privado** e enviar o primeiro commit. Ele só se torna público na T-22, depois do checklist de release.

**Sua verificação:** o repositório aparece no GitHub com os documentos e com os arquivos `.gitignore`, `.gitattributes` e `.editorconfig`, e o arquivo `.env` não aparece.

### [x] T-03 — Esqueleto da solução, banco e scripts (1 h)
- `global.json`, solução e projetos do módulo Timeline (Domain, Application, Infrastructure, Api), Host e projetos de teste, com as referências entre camadas (`arquitetura.md`, seção 4).
- Dependências previstas do backend (`arquitetura.md`, seção 12.3), adicionadas pelas ferramentas oficiais.
- `docker-compose.yml` com o serviço `db` (PostgreSQL), lendo as variáveis do `.env`.
- Scripts `install`, `run`, `test` e `lint` em `.ps1` e `.sh`, e o `Makefile` com os mesmos atalhos. Os scripts `test` e `lint` rodam em container (`arquitetura.md`, seção 12.2, ADR-24).
- Atualizar o README: pré-requisitos, instalação e comandos.

**Sua verificação:** `scripts\install.ps1`, `scripts\test.ps1` e `scripts\lint.ps1` rodam sem erro, e o banco sobe com `scripts\run.ps1`.

---

## Etapa 1 — APIs de cadastro

### [ ] T-04 — Entidades e regras da aplicação e da versão (2 h) — **TDD**
- `Aplicacao`, `Versao` e enums (`arquitetura.md`, seções 5.2 e 5.3), com a exceção de regra de negócio (ADR-26).
- Regras: RN-01, RN-02, RN-05, RN-07 a RN-10, RN-21, RN-22, RN-24, RN-26.
- Critérios de aceite: CA-05, CA-06, CA-13, CA-20, CA-25, CA-32, CA-35, CA-36, CA-39, CA-40, CA-42.
- Mensagens: MSG-11 e MSG-13 a MSG-30.

**Sua verificação:** o Claude Code mostra a lista de testes, um por critério de aceite, todos passando. Você confere se os nomes dos testes correspondem aos cenários e se as mensagens são as do catálogo.

### [ ] T-05 — Banco de dados do cadastro (1,5 h)
- DbContext com o schema `timeline`; tabelas `aplicacao` e `versao`, com as colunas de comparação, as unicidades e as restrições `CHECK` (`arquitetura.md`, seção 6). A tabela do congelado entra na T-18.
- Migration inicial, aplicada automaticamente ao iniciar a API.

**Sua verificação:** você abre o banco (o Claude Code indica como) e confere as duas tabelas, as colunas e as restrições contra a seção 6 do `arquitetura.md`.

### [ ] T-06 — API de aplicações (1 h)
- Listar, cadastrar, renomear e excluir (RF-01, RN-01, RN-04).
- Validação de entrada e erros no padrão Problem Details, com as mensagens MSG-01 a MSG-04, e o tratamento do erro inesperado (MSG-41), válido para toda a API.
- Critérios de aceite: CA-33 e CA-38 (parte do cadastro).
- Página de documentação da API (Scalar), aberta no navegador, já nesta tarefa (ADR-30): é por ela que a autora faz as verificações das APIs.
- Primeiras requisições do `http/exemplos.http`, para o README e para quem avaliar.
- Este cadastro passa a ser o **padrão de referência** para as próximas tarefas.

**Sua verificação:** pela página de documentação da API, no navegador, você cadastra a aplicação Alfa, tenta cadastrar "alfa" e confere a mensagem "Já existe uma aplicação com esse nome.".

### [ ] T-07 — API de versões (1,5 h)
- Cadastrar, detalhar, alterar, mudar a fase, gravar as datas e excluir (RF-03 a RF-05, RF-09, RF-10, RF-14), seguindo o padrão da T-06.
- Critério de aceite: CA-34. Mensagens MSG-05 a MSG-12 e MSG-26.

**Sua verificação:** pela página de documentação da API, você cadastra uma versão, informa as datas, tenta passar para Produção com entrega futura e confere a mensagem do CA-39. Depois, repete o cenário do CA-40 (de Backlog direto para Produção, com datas passadas) e confere que é aceito.

### [ ] T-08 — Docker da API, fuso horário e dados de exemplo (1 h)
- Dockerfile da API e serviço `api` no `docker-compose.yml`; fuso pela variável `TZ`.
- Página de documentação da API publicada também no Docker (ADR-30).
- Carga de dados fictícios de exemplo quando o banco estiver vazio, com datas relativas ao dia da carga (ADR-25).
- Scripts `run` e `test` atualizados; README atualizado.
- Revisão independente (`processo-desenvolvimento.md`, seção 7.3). Tag `v0.1.0`.

**Sua verificação:** `scripts\run.ps1` sobe banco e API; a página da documentação abre no navegador; a lista de aplicações de exemplo aparece na resposta da API.

---

## Etapa 2 — Telas de cadastro

### [ ] T-09 — Projeto Angular (1 h)
- Workspace Angular com Angular Material, locale `pt-BR`, tema com as cores dos grupos visuais, testes e lint.
- Repasse de `/api` para a API em desenvolvimento (`proxy.conf.json`) e serviço `web` no `docker-compose.yml`, com o servidor das telas repassando `/api` (`arquitetura.md`, seções 8 e 12).
- Scripts `test` e `lint` incluindo o frontend; README atualizado.

**Sua verificação:** `scripts\run.ps1` sobe banco, API e telas, e a página inicial abre no navegador.

### [ ] T-10 — Lista de aplicações e tela da versão (3 h)
- Lista de aplicações: cadastro, edição e exclusão, com confirmação antes de excluir (RF-01, RN-04).
- Tela da versão: rótulo, fase, situação, observação e as sete datas num único formulário, salvo de uma vez (RF-03 a RF-05, RF-09, RF-10, RF-14).
- Mensagens do catálogo exibidas como vêm da API.
- Revisão independente. Tag `v0.2.0`.

**Sua verificação:** pela tela, você repete os cenários do CA-25, do CA-32 e do CA-33 e confere as mensagens.

---

## Etapa 3 — API da timeline

### [ ] T-11 — Cálculo da timeline (3,5 h) — **TDD**
- `CalculadoraTimeline` como função pura (`arquitetura.md`, seções 5.4 e 5.5), com a convenção das datas no JSON (ADR-27).
- Regras: RN-03, RN-06, RN-13 a RN-16, RN-18 a RN-20, RN-25.
- Critérios de aceite: CA-02, CA-03, CA-04, CA-07, CA-08, CA-09, CA-15, CA-22, CA-23, CA-27, CA-28, CA-29, CA-37. Mensagens MSG-33 e MSG-34.
- Casos de borda das seções 5.5 e 11 do `arquitetura.md`.

**Sua verificação:** para o CA-02 e o CA-07, o Claude Code mostra o resultado calculado (trechos, cores e tons) ao lado do texto do critério, e você confere se batem.

### [ ] T-12 — API da timeline (0,5 h)
- `GET /api/timeline` (RF-11, RF-12), com o semestre atual como padrão. Mensagens MSG-31 e MSG-32.
- Exemplos no `http/exemplos.http`.

**Sua verificação:** você cadastra os dados do CA-02 pela tela de cadastro e confere a resposta da API, pela página de documentação, contra o texto do critério.

### [ ] T-13 — Refatoração e revisão do backend (1 h)
- Análise de impacto, revisão dos nomes pelo glossário, eliminação de duplicação (`processo-desenvolvimento.md`, seção 9.1).
- Testes existentes passando sem alteração.
- Revisão independente. Tag `v0.3.0`.

**Sua verificação:** o Claude Code explica o que mudou e confirma que nenhum teste foi alterado.

---

## Etapa 4 — Tela da timeline

### [ ] T-14 — Componente de timeline (5 h)
- Tela do status report, começando pela timeline: componente em SVG, com eixo de meses, sub-linhas, trechos, marco, rótulos, linha da data atual, coluna "Sem previsão" e mensagens (`arquitetura.md`, seção 8).
- Seletor do intervalo de meses (RF-12).
- Critério de aceite: CA-31.
- Revisão independente. Tag `v0.4.0`.

**Sua verificação:** com os dados do CA-02, do CA-07 e do CA-31, você confere a timeline desenhada contra o texto dos critérios, inclusive compartilhando a tela numa chamada (RNF-04).

---

## Etapa 5 — API da tabela de status das aplicações

### [ ] T-15 — Tabela de status das aplicações e contadores na API (1,5 h) — **TDD**
- `CalculadoraTabelaStatus` e `GET /api/tabela-status` (RF-13).
- Regras: RN-01, RN-03, RN-13, RN-21 a RN-23.
- Critérios de aceite: CA-14, CA-21, CA-24. Mensagem MSG-35.
- Revisão independente. Tag `v0.5.0`.

**Sua verificação:** você cadastra os dados do CA-21 pela tela de cadastro e confere os contadores na resposta da API, pela página de documentação: 2 aplicações no total, 1 em produção e 1 em homologação.

---

## Etapa 6 — Tela da tabela de status das aplicações

### [ ] T-16 — Tabela de status das aplicações na tela (1 h)
- Na tela do status report, abaixo da timeline: tabela com as colunas Aplicação, Versão, Fase, Situação, Data de entrega e Observação, e os contadores acima dela (RF-13).
- Revisão independente. Tag `v0.6.0`.

**Sua verificação:** com os dados do CA-14 e do CA-21, você confere a linha "Nenhuma versão cadastrada" e os contadores.

---

## Etapa 7 — Congelamento

### [ ] T-17 — Semanas e data de referência (0,5 h) — **TDD**
- Semana ISO, formato `2026-W42` e terça-feira de referência (`arquitetura.md`, seção 5.7; RN-27, RN-28).
- Casos de borda da virada de ano.

**Sua verificação:** o teste de 01/01/2027 resulta na Semana 53/2026, com referência em 29/12/2026.

### [ ] T-18 — Congelamento e Histórico de Status Reports na API (3 h)
- Tabela `status_report_congelado`, com a versão do formato (ADR-28); endpoints de histórico e de congelamento (RF-15 a RF-17; RN-27 a RN-30).
- Critérios de aceite: CA-10, CA-11, CA-12, CA-26, CA-30, CA-38 (parte do congelado), CA-41. Mensagens MSG-36 a MSG-40.

**Sua verificação:** pela página de documentação da API, você congela a semana, altera uma data de entrega e confere que o congelado continua com a data antiga (CA-10).

### [ ] T-19 — Telas do Histórico de Status Reports e do congelado (1,5 h)
- Tela do Histórico de Status Reports, com as ações "Abrir", "Congelar" e "Congelar novamente"; status report congelado somente leitura; confirmação de substituição (RN-28 a RN-30).
- Revisão independente. Tag `v0.7.0`.

**Sua verificação:** pela tela, você repete o cenário do CA-11 e confere o pedido de confirmação.

---

## Entrega (tempo protegido)

### [ ] T-20 — Exemplos de uso (1 h)
- `http/exemplos.http` cobrindo todos os endpoints construídos, na ordem de um fluxo de uso.
- Exemplos `curl` na seção de uso do README.

**Sua verificação:** o Claude Code executa o arquivo de exemplos do começo ao fim e mostra o resultado de cada requisição; você confere que não há erros inesperados.

### [ ] T-21 — README completo e registro do uso de IA (1,5 h)
- Todas as seções do README, incluindo dependências, arquitetura, limitações e próximos passos (os degraus não concluídos).
- Seção sobre o uso de IA, com as quatro subseções (onde a IA acelerou, onde a revisão humana foi decisiva, quais prompts funcionaram melhor, quais riscos foram mitigados), a partir do diário.
- Revisão do `prompts/prompts-claude-code.md`.
- Demonstração visual com dados fictícios, em `docs/imagens/`: GIF da timeline na tela, se a T-14 foi concluída, ou capturas da API e dos testes em execução.

**Sua verificação:** você lê o README inteiro na página do GitHub e confere se alguém de fora entenderia o problema, a solução e como rodar.

### [ ] T-22 — Release 1.0 (1,5 h)
- Revisão independente final.
- Executar o checklist de release do `processo-desenvolvimento.md`, item por item, incluindo a **instalação limpa**: clonar numa pasta nova e seguir só o README.
- Corrigir as pendências encontradas.
- Tornar o repositório público e criar a release `v1.0.0` no GitHub.

**Sua verificação:** você abre o repositório numa janela anônima do navegador e confere cada item do checklist.

---

## Incrementos (se houver tempo)

- [ ] **I-01 — Testes de integração:** endpoints principais com PostgreSQL real.

Incrementos não concluídos são listados em "Limitações e próximos passos" no README.

---

## Histórico do documento

| Versão | Data | Descrição |
|---|---|---|
| 0.1 | 14/10/2026 | Versão inicial, baseada no modelo de datas anterior. |
| 0.2 | 14/10/2026 | Reescrita a partir do `requisitos-mvp.md` versão 0.5 e do `arquitetura.md` versão 0.4: entrega em duas etapas (API na versão 1.0 e telas na versão 1.1); roteiro de preparação do ambiente; congelamento como fase opcional; fase de entrega com tempo protegido; pontos de parada seguros; verificação da autora em cada tarefa. |
| 0.3 | 14/10/2026 | T-02: indicado onde colocar e como renomear os três arquivos `CLAUDE.md`. |
| 0.4 | 15/10/2026 | Reescrita com a entrega em degraus, a partir do `requisitos-mvp.md` versão 0.6 e do `arquitetura.md` versão 0.5: APIs de cadastro, API da timeline, API da tabela de status das aplicações, telas de cadastro, tela da timeline, tela da tabela de status das aplicações e congelamento; regra do corte pelo tempo real; revisão independente em cada degrau; T-01 com Node.js, extensão REST Client e conferência do plano do Claude Code; T-02 com `.gitattributes` e `.editorconfig`; testes e lint em container; mensagens do catálogo e critérios CA-40 a CA-42 nas tarefas. |
| 0.5 | 15/10/2026 | Ordem dos degraus ajustada pela autora, com a entrega como primeiro objetivo: telas de cadastro antes da API da timeline; API e tela da tabela de status das aplicações antes do congelamento; refatoração logo após a API da timeline (T-13). Tarefas renumeradas a partir da T-09. |
| 0.6 | 15/10/2026 | T-02: a verificação da autora inclui os arquivos `.gitignore`, `.gitattributes` e `.editorconfig`. |
| 0.7 | 15/10/2026 | T-01: novo passo 5, montagem da pasta do projeto com os três `CLAUDE.md` já renomeados, para que o Claude Code conheça as regras desde a primeira sessão. T-02: criação do `prompts/prompts-claude-code.md` pelo modelo do processo. |
| 0.8 | 15/10/2026 | O `prompts/prompts-claude-code.md`, com o roteiro de prompts e o registro, passa a ser colocado na pasta do projeto na T-01 e apenas versionado na T-02. |
| 0.9 | 15/10/2026 | T-01: Claude Code pela área Code do aplicativo Claude para computador, sem instalação separada; verificação pela abertura da pasta do projeto, no lugar do comando `claude --version`. |
| 0.10 | 15/10/2026 | A autora não usa o VS Code: retirados o VS Code e a extensão REST Client da T-01; as verificações das APIs passam a ser feitas pela página de documentação da API (Scalar), publicada já na T-06; na T-20, o Claude Code executa o arquivo de exemplos e mostra o resultado. |
| 0.11 | 15/10/2026 | T-01 e T-02 marcadas como concluídas. |
| 0.12 | 15/10/2026 | T-03 marcada como concluída. |

# Diário de Desenvolvimento
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão | 0.17 |
| Data | 15/10/2026 |

Registro de cada sessão de trabalho com o assistente de código. É a fonte da seção do README sobre o uso de IA: onde a IA acelerou o desenvolvimento, onde a revisão humana foi decisiva, quais prompts funcionaram melhor e quais riscos foram mitigados.

**Como usar**

- Uma entrada por sessão, preenchida ao final dela (leva poucos minutos).
- Os pedidos mais relevantes também vão, na íntegra, para `prompts/prompts-claude-code.md`; aqui basta um resumo e a referência.
- Ao concluir cada etapa do `tarefas.md`, atualize o resumo acumulado no fim deste arquivo.

---

## Modelo de entrada

Copie o bloco abaixo para cada nova sessão.

```markdown
### Sessão NN — dd/mm/aaaa

| Campo | Registro |
|---|---|
| Tarefa | T-XX — título |
| Regras e critérios | RN-XX, CA-XX |
| Modelo usado | (ex.: o modelo selecionado no Claude Code) |
| Tempo real | X h |
| Estimativa sem IA | X h |

**O que foi pedido:** resumo do pedido ao Claude Code. Prompt completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-XX.

**Resultado:** o que foi entregue.

**Primeira tentativa:** funcionou / precisou de ajustes.

**Ajustes necessários e por quê:** o que precisou ser corrigido ou pedido de novo.

**O que a revisão humana capturou:** o que a autora percebeu ao testar pelos critérios de aceite ou ao revisar (ex.: uma regra mal interpretada, uma mensagem fora do padrão).

**Riscos mitigados:** qual risco do uso de IA apareceu ou foi evitado (ex.: teste alterado para o código passar, dependência inexistente, dado sensível).

**Aprendizados:** o que vale levar para as próximas sessões, inclusive sobre como pedir.
```

---

## Entradas

### Sessão 00 — 15/10/2026 — Planejamento

| Campo | Registro |
|---|---|
| Tarefa | Planejamento, antes da T-01: adaptação do processo, revisão de arquitetura, estratégia de entrega em etapas, roteiro de prompts e preparação do repositório |
| Regras e critérios | RN-01, RN-02, RN-08, RN-24, RN-25, RN-27, RN-30; CA-02, CA-28, CA-29, CA-33, CA-34, CA-40, CA-41, CA-42 |
| Modelo usado | Claude Opus 5.5, numa conversa no claude.ai (ainda não no Claude Code) |
| Tempo real | Não medido: a anotação do horário de início e de fim passou a ser feita a partir da Sessão 04. |
| Estimativa sem IA | 16 h (proposta do assistente: o mesmo trabalho feito por um arquiteto experiente sem IA, somando a revisão cruzada de nove documentos, a reescrita do processo e das tarefas, o catálogo de 41 mensagens e o roteiro de prompts das 22 tarefas) |

**O que foi pedido:** continuar o planejamento a partir do resumo da sessão anterior; adaptar o processo à autora não técnica; mudar a estratégia de entrega; fazer uma revisão de arquitetura antes do código; preparar o repositório e os prompts de cada tarefa. Pedidos completos na Parte 2 do `prompts/prompts-claude-code.md`, itens P-01 a P-17.

**Resultado:**

| Documento | Versão | O que mudou |
|---|---|---|
| `requisitos-mvp.md` | 0.6 | Avanço direto de fase, nomes sem diferença de maiúsculas, prorrogação corrigível em Produção, Histórico antes do primeiro congelamento; CA-40 a CA-42 |
| `requisitos-negocio.md` | 0.68 | Alinhado ao MVP 0.6 |
| `arquitetura.md` | 0.8 | Correção da convenção de datas do CA-02, catálogo de 41 mensagens, colunas de comparação de nomes, testes e lint em container, dados de exemplo com datas relativas, dependências aprovadas, decisões ADR-22 a ADR-30 |
| `processo-desenvolvimento.md` | 0.6 | Papéis, revisão pelo comportamento, formato do plano, revisão independente, termos técnicos, pedidos no modelo CO-STAR |
| `tarefas.md` | 0.8 | Entrega em etapas, regra do corte pelo tempo real, montagem da pasta na T-01 |
| Os três `CLAUDE.md` | — | Alinhados ao processo, ao catálogo e às etapas |
| `prompts/prompts-claude-code.md` | 0.3 | Roteiro de prompts no modelo CO-STAR e registro dos pedidos |
| Repositório no GitHub | — | Criado como privado e vazio |

**Primeira tentativa:** 15 dos 17 pedidos funcionaram de primeira. No P-10, o assistente não conseguiu conferir sozinho se o repositório estava privado, e a conferência ficou com a autora. No P-11, o roteiro foi criado num documento à parte, depois foi para o arquivo de prompts do projeto (P-12) e, por fim, foi reescrito no modelo CO-STAR explícito (P-17).

**Ajustes necessários e por quê:**

- A ordem das etapas mudou duas vezes. O assistente recomendou fazer a API da timeline logo depois das APIs de cadastro; a autora manteve a sua ordem, com a entrega como primeiro objetivo (P-02 e P-04).
- O assistente encontrou e corrigiu erros próprios nas conferências que fazia ao final de cada alteração: duas menções ao contexto externo do projeto em documentos do repositório; o catálogo de mensagens dado como aprovado antes da revisão da autora; a tabela de histórico do processo fora de ordem; as datas dos históricos registradas como 14/10/2026 em vez de 15/10/2026.

**O que a revisão humana capturou:**

- As decisões de negócio foram da autora: avanço direto de fase para a carga inicial (N-01), nomes iguais sem diferença de maiúsculas (N-02), correção da prorrogação em Produção (N-03), catálogo de mensagens (N-04), Histórico sem congelados (N-05), limites de tamanho (N-06).
- A autora apontou que os prompts do roteiro não mostravam o modelo CO-STAR de forma explícita. O roteiro foi reescrito com os seis elementos em cada prompt (P-17).
- A ordem das etapas foi decisão da autora, contra a recomendação do assistente, com um critério que o assistente não tinha: a entrega vem primeiro e a evolução vem depois.
- As perguntas da autora ("o que você vai fazer aqui?", "o que eu faço?", "falta mais alguma coisa?") levaram a uma proposta concreta de dados de exemplo e à descoberta de uma lacuna. Sem os `CLAUDE.md` com o nome certo, a primeira sessão do Claude Code rodaria sem as regras do projeto.

**Riscos mitigados:**

- Teste derivado de um exemplo errado: a convenção de datas do CA-02 foi corrigida antes de o teste existir.
- Dependências inventadas pela IA: lista prevista, com licenças, aprovada de uma vez.
- Informação indevida no repositório público: duas menções ao contexto externo foram removidas; o repositório nasceu privado; o e-mail dos commits será o endereço privado do GitHub.
- Assistente sem as regras do projeto: os `CLAUDE.md` estarão na pasta antes da primeira sessão.
- Mensagens de erro inventadas: o catálogo define o texto exato que os testes conferem.

**Aprendizados:**

- Revisar a arquitetura com perguntas objetivas antes do código encontrou um erro que viraria um teste errado e protegido.
- Perguntar "o que você vai fazer?" ou "o que eu faço?" sobre um item técnico transforma a recomendação em algo que a autora consegue conferir.
- Decisões apresentadas em botões, com exemplos, foram respondidas mais rápido do que perguntas abertas.
- As sessões anteriores de planejamento (requisitos e primeira versão da arquitetura) ficaram registradas nos históricos de versão dos próprios documentos (`requisitos-negocio.md`, `requisitos-mvp.md` e `arquitetura.md`).

### Sessão 01 — 15/10/2026 — T-01, Preparação do ambiente

| Campo | Registro |
|---|---|
| Tarefa | T-01 — Preparação do ambiente |
| Regras e critérios | Nenhum (tarefa de preparação) |
| Modelo usado | Nenhum modelo de IA na execução: a tarefa foi feita pela autora, com o roteiro do `tarefas.md` |
| Tempo real | Não medido: a anotação do horário de início e de fim passou a ser feita a partir da Sessão 04. |
| Estimativa sem IA | 2 h (proposta do assistente: instalar e verificar Git, .NET, Node.js e Docker, configurar o Git e montar a pasta do projeto) |

**O que foi pedido:** não houve pedido ao Claude Code. A autora seguiu o roteiro da T-01: verificou o computador, instalou as ferramentas, conferiu cada uma pelos comandos da tabela, configurou o Git e montou a pasta do projeto. Esta entrada foi criada na Sessão 02, a pedido da autora.

**Resultado:** ambiente pronto. Pasta `C:\Projetos\timeline-entregas` montada com os documentos, os três `CLAUDE.md` e o `prompts/prompts-claude-code.md`; Git configurado com o e-mail privado do GitHub; área Code do aplicativo Claude aberta na pasta.

**Primeira tentativa:** precisou de ajustes: três problemas no ambiente, descritos abaixo.

**Ajustes necessários e por quê:**

- O comando `docker` não era reconhecido depois da instalação. Só passou a funcionar depois de reiniciar o computador.
- O motor do Docker ficou parado até a instalação do WSL, que o Docker Desktop usa no Windows.
- O e-mail do Git foi configurado por engano com os sinais `<` e `>` em volta do endereço e depois corrigido.

**O que a revisão humana capturou:** a conferência de cada ferramenta pelos comandos da tabela da T-01, antes de seguir para o passo seguinte, mostrou os três problemas acima ainda na preparação, e não no meio de uma tarefa de código.

**Riscos mitigados:** assistente sem as regras do projeto (os `CLAUDE.md` já estavam na pasta, com os nomes certos, na primeira sessão); informação indevida nos commits (e-mail privado do GitHub).

**Aprendizados:** depois de instalar o Docker no Windows, reiniciar o computador e conferir se o WSL está instalado antes de testar o comando. Conferir a configuração do Git (`git config user.email`) logo depois de fazê-la.

---

### Sessão 02 — 15/10/2026 — T-02, Repositório e documentação inicial

| Campo | Registro |
|---|---|
| Tarefa | T-02 — Repositório e documentação inicial |
| Regras e critérios | Nenhuma RN ou CA; `arquitetura.md`, seções 4 e 9, e ADR-19 (licença MIT) |
| Modelo usado | Claude Sonnet 5.5, no Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido: a anotação do horário de início e de fim passou a ser feita a partir da Sessão 04. |
| Estimativa sem IA | 1 h (proposta do assistente: um desenvolvedor experiente escreveria os arquivos de configuração e o esqueleto do README, iniciaria o repositório e conferiria o envio ao GitHub) |

**O que foi pedido:** iniciar o repositório e enviar o primeiro commit, com `.gitignore`, `.gitattributes` e `.editorconfig` criados antes dele, `.env.example`, `LICENSE` (MIT) e o esqueleto do README com as 15 seções. O pedido seguiu o prompt da T-02 do roteiro (Parte 1 do `prompts/prompts-claude-code.md`), sem registro à parte na Parte 2.

**Resultado:** repositório iniciado, com o branch `chore/repositorio-inicial` integrado ao `main`. Arquivos de configuração e proteção criados; README com as 15 seções, cada uma com uma linha do que vai conter; nomes das variáveis do `.env.example` registrados no `arquitetura.md` (versão 0.9). A regra das mensagens de commit foi corrigida no `CLAUDE.md` e no `processo-desenvolvimento.md` (versão 0.8): descrição em português com o verbo no infinitivo. T-01 e T-02 marcadas no `tarefas.md` (versão 0.11).

**Primeira tentativa:** funcionou. O plano foi aprovado com três ajustes da autora, sem retrabalho na implementação.

**Ajustes necessários e por quê:** a mensagem de commit proposta no plano usava o verbo conjugado ("inicia"). A autora pediu o infinitivo ("iniciar"), como nos exemplos dos documentos, e a regra escrita foi corrigida para combinar com os exemplos.

**O que a revisão humana capturou:** a regra de commit dizia "no imperativo", mas os exemplos dos documentos usavam o infinitivo ("calcular"), e o plano do assistente ainda propôs uma terceira forma ("inicia"). A regra foi corrigida no `CLAUDE.md` e no `processo-desenvolvimento.md` antes do primeiro commit.

**Riscos mitigados:** o `.env` ficou no `.gitignore` antes do primeiro commit; o `.env.example` tem só valores fictícios; repositório privado até a T-22.

**Aprendizados:** conferir a regra escrita contra os exemplos dos documentos evita contradições que o assistente repetiria. A regra do verbo nos commits fica só no `CLAUDE.md` da raiz; os `CLAUDE.md` do backend e do frontend não a repetem.

---

### Sessão 03 — 15/10/2026 — T-03, Esqueleto da solução, banco e scripts

| Campo | Registro |
|---|---|
| Tarefa | T-03 — Esqueleto da solução, banco e scripts |
| Regras e critérios | Nenhuma RN ou CA; `arquitetura.md`, seções 4, 9, 12.2 e 12.3, e ADR-24 |
| Modelo usado | Claude Sonnet 5.5, no Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido: a sessão foi interrompida no meio por falta de limite de uso e retomada depois. A verificação da autora levou cerca de 10 min. |
| Estimativa sem IA | 3 h (proposta do assistente: um desenvolvedor experiente montaria 7 projetos, 8 scripts e a composição do Docker, e testaria tudo em container) |

**O que foi pedido:** criar o `global.json`, a solução e os projetos do módulo Timeline com as referências entre camadas, o `docker-compose.yml` com o banco, os scripts `install`, `run`, `test` e `lint` (`.ps1` e `.sh`), o `Makefile` e a atualização do README. Pedidos completos na Parte 2 do `prompts/prompts-claude-code.md`, itens P-18 e P-19.

**Resultado:** branch `chore/esqueleto-solucao`. Backend com as quatro camadas, o Host e dois projetos de teste, ligados como no `arquitetura.md`; dependências previstas adicionadas pelas ferramentas oficiais, com a licença de cada uma conferida; banco PostgreSQL 17 sobe pelo Docker Compose; scripts de teste e lint rodam em container, sem o SDK no computador; um teste de estrutura confere que o `Domain` não depende de nenhuma outra camada. `arquitetura.md` na versão 0.10 (ADR-31 e ADR-32) e `tarefas.md` na versão 0.12.

**Primeira tentativa:** precisou de ajustes: três problemas, descritos abaixo.

**Ajustes necessários e por quê:**

- O lint reprovava numa cópia limpa do projeto no Windows: o Git baixava os arquivos com a quebra de linha do Windows (CRLF), e o `.editorconfig` exige a do Linux (LF). Achado ao simular uma instalação limpa (checkout dos arquivos do Git numa pasta nova). Corrigido no `.gitattributes` (`* text=auto eol=lf`, mantendo CRLF só nos `.ps1`), e o lint e os testes passaram na cópia limpa.
- A porta 5432 já estava ocupada por um PostgreSQL instalado no computador da autora, e o banco do projeto não subiu. A porta passou a ser configurável por `DB_PORT` (no `.env` local, 5433), e o script `run` agora explica o erro em português.
- O primeiro teste de reprovação do teste de estrutura usou uma referência que já existia (o `Application` já depende do `Domain`) e falhou por outro motivo. Foi repetido com uma referência ao framework web, e o teste reprovou como esperado.

**O que a revisão humana capturou:** a autora conferiu o resultado no GitHub e estava tudo certo, sem nada diferente do esperado. Os pontos abaixo são as decisões da autora durante a sessão:

- A autora aprovou o pacote `Microsoft.AspNetCore.OpenApi`, que não estava na lista da seção 12.3 e que o Scalar precisa, e pediu um teste de estrutura sem dependência nova, para a tarefa não terminar com "0 testes".
- A autora pediu a confirmação de que `test` e `lint` não dependem do SDK instalado. Isso foi provado rodando os dois com o `dotnet` fora do caminho do terminal.

**Riscos mitigados:** pacote fora da lista aprovada (pedida a aprovação e registrada no `arquitetura.md`); pacote extra que o modelo de projeto de teste traz (`coverlet`, removido); versões de pacotes escritas pelas ferramentas oficiais, sem versão de memória; `.env` fora do Git (confirmado pelo `.gitignore`); banco aberto só para o próprio computador; senha do banco só no `.env`.

**Aprendizados:** testar o teste: um teste só é confiável depois de ser visto reprovando, com o erro certo. Pedir que o assistente prove a promessa do documento ("sem depender do SDK") com um experimento, e não só afirmar. A sessão pode ser interrompida pelo limite de uso: anotar o horário de início e de fim evita ficar sem o tempo real, e o prompt A16 do roteiro serve para retomar.

---

### Sessão 04 — 15/10/2026 — T-04, Entidades e regras da aplicação e da versão

| Campo | Registro |
|---|---|
| Tarefa | T-04 — Entidades e regras da aplicação e da versão (TDD) |
| Regras e critérios | RN-01, RN-02, RN-05, RN-07 a RN-10, RN-21, RN-22, RN-24, RN-26; CA-05, CA-06, CA-13, CA-20, CA-25, CA-32, CA-35, CA-36, CA-39, CA-40, CA-42 |
| Modelo usado | Claude Sonnet 5.5 no plano; Claude Opus 5.5 na implementação, trocado pela autora depois do plano, que indicava o modelo de maior capacidade. No Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido. A verificação da autora no PowerShell levou cerca de 5 min. |
| Estimativa sem IA | 4 h (proposta do assistente: um desenvolvedor experiente derivaria 37 testes dos critérios e das regras, escreveria as entidades com todas as validações e conferiria 19 mensagens contra o catálogo) |

**O que foi pedido:** integrar o registro da T-03 ao `main` e apresentar o plano da T-04, com um teste por critério de aceite, sem alterar arquivos. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-20.

**Resultado:** branch `feat/entidades-regras`. `Aplicacao`, `Versao`, `DatasVersao`, os cinco enums, a `RegraNegocioException` e as mensagens MSG-11 e MSG-13 a MSG-30 no `Domain`. São 37 testes novos: 13 dos 11 critérios de aceite (o CA-32 e o CA-42 têm duas partes cada) e 24 das regras sem critério próprio. Com os 2 testes de estrutura, são 39, todos passando. Os testes foram vistos falhando antes do código, pelo motivo certo. `arquitetura.md` na versão 0.11 (ADR-33). O registro da T-03 já estava no `main`, integrado junto com o processo enxuto.

**Primeira tentativa:** precisou de um ajuste num teste novo, descrito abaixo.

**Ajustes necessários e por quê:**

- O teste da MSG-15 (prorrogação sem as datas do ciclo) usava uma versão em Backlog. O sistema devolveu a MSG-15 e a MSG-23, as duas corretas (ADR-26). O cenário passou a usar uma versão em Homologação, que isola a MSG-15. A regra testada não mudou.
- Na revisão técnica, o assistente acrescentou uma proteção para um valor de fase inexistente, que a `Application` recusa com a MSG-26 na T-07.

**O que a revisão humana capturou:** a autora perguntou por que não foi avisada antes de que o catálogo de mensagens ainda estava "a revisar". O catálogo deveria passar pela revisão dela antes da T-04 (`arquitetura.md`, seção 14), e o plano não avisou. A autora revisou e aprovou as 19 mensagens usadas na tarefa; as demais ficam para antes da T-06.

**Riscos mitigados:** mensagem fora do catálogo (os testes digitam o texto do catálogo, separado do código, e um erro de digitação reprova o teste); teste ajustado para o código passar (o único ajuste foi no cenário de um teste novo, explicado à autora, e a mensagem conferida continuou a mesma); dependência nova (nenhuma; o `TimeProvider` já vem no .NET); relógio lido diretamente (a data atual é sempre fixada nos testes).

**Aprendizados:** no plano, conferir na seção 14 do `arquitetura.md` se algum ponto ainda não revisado pela autora afeta a tarefa e avisar antes da aprovação. Anotar o horário de início e de fim da sessão, para ter o tempo real.

---

### Sessão 05 — 15/10/2026 — T-05, Banco de dados do cadastro

| Campo | Registro |
|---|---|
| Tarefa | T-05 — Banco de dados do cadastro |
| Regras e critérios | RN-01, RN-02, RN-04, RN-07, RN-09, RN-10, RN-24 (como segunda proteção no banco, ADR-13 e ADR-23); nenhum critério de aceite novo (CA-33 e CA-34 só ficam testáveis de ponta a ponta na T-06) |
| Modelo usado | Claude Sonnet 5.5, no plano e na implementação (tarefa rotineira, modelo rápido). No Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido. A revisão da autora levou cerca de 10 min. A sessão começou às 09:07. |
| Estimativa sem IA | 3 h (proposta do assistente: um desenvolvedor experiente escreveria os mapeamentos, geraria a migration, escreveria o SQL de cada restrição e testaria uma a uma no PostgreSQL) |

**O que foi pedido:** antes do plano, mostrar o modelo de dados da seção 6 em linguagem simples, porque a seção 14 o marcava como "A revisar"; depois, o plano da T-05. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-21.

**Resultado:** branch `feat/banco-cadastro`. `TimelineDbContext` e os mapeamentos de `aplicacao` e `versao` no schema `timeline`, com as colunas `nome_comparacao` e `rotulo_comparacao` geradas pelo banco, as duas unicidades, oito restrições `CHECK` de `versao` mais uma de `aplicacao`, e a exclusão em cascata. Migration `Inicial`, aplicada quando a API inicia, por um serviço de inicialização do `Host`. Build sem avisos, 39 testes passando e lint sem apontamentos, também pelos scripts em container. No PostgreSQL real, 45 cenários (gravações aceitas e recusadas) se comportaram como o esperado, e uma gravação e leitura pelas classes do `Domain` confirmou o mapeamento sem mudar o `Domain`. `arquitetura.md` na versão 0.12 (ADR-34, seção 6 e `DB_HOST` na seção 9); seção 14 marcada como "Aprovado" para o modelo de dados.

**Primeira tentativa:** certa quanto ao desenho. Três ajustes no caminho, descritos abaixo.

**Ajustes necessários e por quê:**

- A primeira versão da migration listava as colunas fora da ordem da seção 6 (as datas no fim, as colunas de comparação no meio). Foi fixada a ordem do modelo, para a autora conferir lado a lado.
- O lint reprovou a migration gerada pela ferramenta (fim de linha, codificação e estilo de namespace). A migration passou a ser tratada como código gerado, fora da verificação de padrão, no `.editorconfig`.
- O serviço de inicialização tinha um passo extra, de contar as migrations pendentes, que consultava a tabela de histórico antes de ela existir e acrescentava uma mensagem de erro ao registro da primeira criação. O passo foi retirado. Restam apenas as mensagens `fail:` do próprio EF Core ao criar o banco do zero, que são normais.

**O que a revisão humana capturou:** a autora aprovou o modelo e o plano, incluindo a proteção extra contra nome e rótulo vazios ou com espaço nas pontas, que a seção 6 não previa e foi proposta pelo assistente. Na conferência do banco, não pediu nenhum ajuste.

**Riscos mitigados:** SQL montado por texto (nenhum; só o EF Core, e o SQL das restrições é fixo); senha no código (a conexão vem das variáveis do `.env`, e o `.env` fica fora do Git); dependência nova (nenhuma; EF Core e Npgsql já estavam na seção 12.3); migration rodando sem querer ao criar uma migration (a aplicação fica num serviço que só roda ao iniciar a API); restrição escrita errada (cada uma testada no banco real, com caso aceito e caso recusado); mudança escondida no `Domain` (nenhuma; os 39 testes seguem iguais).

**Aprendizados:** testar cada restrição `CHECK` num banco real, com um caso que deve passar e outro que deve falhar, pega erros que a leitura do SQL não pega (valores vazios são aceitos por padrão numa comparação). Pedir a ordem das colunas igual à do documento facilita a conferência da autora. Anotar o horário de início e de fim da sessão continua valendo, para ter o tempo real.

---

### Sessão 06 — 15/10/2026 — T-06, API de aplicações

| Campo | Registro |
|---|---|
| Tarefa | T-06 — API de aplicações |
| Regras e critérios | RF-01, RN-01, RN-04; CA-33 e CA-38 (parte do cadastro); MSG-01 a MSG-04, MSG-41 e a nova MSG-42 |
| Modelo usado | Claude Sonnet 5.5, no plano e na implementação. No Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido por inteiro. A sessão começou às 10:50; a verificação da autora levou 20 min. |
| Estimativa sem IA | 4 h (proposta do assistente: um desenvolvedor experiente escreveria os quatro casos de uso, o repositório, o tratamento padronizado de erros, a documentação OpenAPI com a página de consulta e os testes, e conferiria tudo contra o PostgreSQL) |

**O que foi pedido:** os endpoints de listar, cadastrar, renomear e excluir aplicações, com a página de documentação (Scalar) já publicada, porque a autora faz as verificações pelo navegador, e o `http/exemplos.http`. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-22.

**Resultado:** branch `feat/api-aplicacoes`. Quatro casos de uso na `Application` (um por operação), `IAplicacaoRepository` com a implementação no EF Core, `AplicacaoEndpoints` e um único `TratadorDeErros`, que converte qualquer exceção em Problem Details com `title` e a lista `mensagens`. Erro inesperado devolve só a MSG-41, e o detalhe vai para o registro do servidor. Página Scalar em `/scalar` (e `/` leva a ela), sem depender da internet. Novo projeto de testes `Timeline.Api.Tests`. Build sem avisos, 73 testes (39 do `Domain`, 27 da `Application`, 7 da `Api`) e lint sem apontamentos, também pelos scripts em container. No PostgreSQL real, o CA-33 e o CA-38 (cascata das versões) se comportaram como esperado, e o 500 foi conferido com o banco parado. `arquitetura.md` na versão 0.14 (MSG-42, formato do Problem Details e ADR-35).

**Primeira tentativa:** certa quanto ao desenho. Dois ajustes no caminho, descritos abaixo.

**Ajustes necessários e por quê:**

- Um JSON quebrado devolvia 400 com o corpo vazio, e não a mensagem do catálogo: fora do ambiente de desenvolvimento, o .NET responde sozinho nesse caso. Foi ligada a opção que faz o erro passar pelo tratador de erros, e agora devolve a MSG-42.
- O exemplo do corpo na página de documentação aparecia como `"nome": null`. Foi acrescentado o exemplo `"Alfa"`, para quem testa só clicar em enviar.

**O que a revisão humana capturou:** a autora aprovou as duas respostas propostas no plano: a lista em ordem alfabética e a nova MSG-42 para o JSON quebrado, que o catálogo não previa. Na verificação, o 404 ao excluir o id 29 era só o valor de exemplo da página; com o id real da lista, a exclusão deu 204.

**Riscos mitigados:** mensagem fora do catálogo (a MSG-42 entrou antes no catálogo, com aprovação); vazamento de detalhes internos no erro 500 (teste com texto sensível na exceção e conferência com o banco parado); dependência nova (nenhuma; o Scalar e o gerador de OpenAPI já estavam na seção 12.3, e o projeto de testes usa os mesmos pacotes dos outros); duplicidade em cadastros simultâneos (o banco recusa e o repositório converte em MSG-03); SQL por texto (nenhum; só consultas do EF Core).

**Aprendizados:** testar a API de verdade, com requisições reais, achou o JSON quebrado sem mensagem, que os testes de unidade não pegariam. Para quem não usa um editor de código, a página de documentação no navegador substitui bem o arquivo de requisições, desde que o corpo de exemplo seja um valor válido.

---

### Sessão 07 — 15/10/2026 — T-07, API de versões

| Campo | Registro |
|---|---|
| Tarefa | T-07 — API de versões |
| Regras e critérios | RF-03 a RF-05, RF-09, RF-10, RF-14, RN-02, RN-07, RN-08; CA-34, CA-39, CA-40; MSG-05 a MSG-12 e MSG-26 |
| Modelo usado | Claude Sonnet 5.5, no plano e na implementação. No Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido por inteiro; a verificação da autora levou 20 min. |
| Estimativa sem IA | 4 h (proposta do assistente: um desenvolvedor experiente escreveria seis casos de uso, o repositório, os endpoints, a validação de formato, cerca de 70 testes e os exemplos de requisição, e conferiria tudo contra o PostgreSQL) |

**O que foi pedido:** os endpoints de cadastrar, detalhar, alterar, mudar a fase, gravar as datas e excluir versões, no padrão da T-06, e os cenários do CA-39 e do CA-40 no `http/exemplos.http`. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-23.

**Resultado:** branch `feat/api-versoes`. Seis casos de uso na `Application` (`CadastrarVersao`, `DetalharVersao`, `AlterarVersao`, `AlterarFaseVersao`, `AlterarDatasVersao`, `ExcluirVersao`), `IVersaoRepository` com a implementação no EF Core, `VersaoEndpoints` e validação de formato num só lugar. As MSG-05 a MSG-10 e a MSG-12 entraram em `Mensagens.cs`. A lista de aplicações passou a trazer o resumo das versões (id, rótulo, fase e situação). Fase e situação saem como texto (`Homologacao`, `OnTrack`). O `TimeProvider` foi registrado na injeção de dependências. Corpos de exemplo preenchidos na página Scalar. `http/exemplos.http` com os passos 12 a 36. Build sem avisos, 141 testes (39 do `Domain`, 95 da `Application`, 7 da `Api`) e lint sem apontamentos. No PostgreSQL real, CA-34, CA-39 e CA-40 se comportaram como esperado. `arquitetura.md` na versão 0.15 (ADR-36).

**Primeira tentativa:** certa. Nenhum ajuste de código foi necessário depois dos testes; o único tropeço foi do roteiro de conferência do assistente, que usou ids fixos (1 e 2) em vez dos ids reais devolvidos pela API, e foi refeito.

**Ajustes necessários e por quê:** nenhum no código. Ver a lacuna abaixo, que mudou o escopo no plano.

**Lacuna encontrada nos requisitos:** a `arquitetura.md` (seção 7.2) diz que `GET /api/aplicacoes` devolve a lista "com suas versões" (RF-03), mas a T-06 entregou só `id` e `nome`, e nenhuma tarefa previa completar isso. O assistente apontou a lacuna no plano, e a autora decidiu incluir já na T-07 o resumo das versões, em vez de deixar para a T-10 (risco de a pendência se perder). Efeito: a resposta da lista ganhou o campo `versoes`, e a linha que monta `ListarAplicacoes` no teste existente `AplicacaoCasosDeUsoTests` mudou (só o preparo do teste, nenhuma verificação), o que foi informado à autora.

**O que a revisão humana capturou:** a decisão sobre a lista com versões. Na verificação, as perguntas da autora mostraram três pontos que o roteiro não deixava claros: o `"v1"` já vinha preenchido no corpo de exemplo e não ficava óbvio que era editável; não existe uma operação só de "listar versões" (a lista vem dentro de Aplicações); e "em andamento, em homologação" é a fase, que tem operação própria, diferente da situação (On track ou Risco). O CA-34, o CA-39 e o CA-40 foram conferidos pela página e aceitos.

**Riscos mitigados:** teste alterado para o código passar (a única mudança em teste existente foi o preparo, informada e dentro do que a autora aprovou); mensagem fora do catálogo (todas as mensagens vêm do catálogo, com o texto exato conferido nos testes); apagar as datas sem querer (corpo ausente em `/datas` é recusado com a MSG-42); mudança parcial em caso de erro (a regra que pode falhar roda antes de qualquer alteração); dependência nova (nenhuma); SQL por texto (nenhum; só consultas do EF Core); fase informada como número (recusada com a MSG-26).

**Aprendizados:** conferir a seção 7.2 da arquitetura contra o que cada tarefa de API entrega achou a lacuna da lista antes de ela virar retrabalho na T-10. Corpos de exemplo preenchidos ajudam, mas o passo a passo da página precisa dizer que eles são editáveis e que a fase e a situação têm operações diferentes. Em roteiros de conferência, usar os ids devolvidos pela API, e não números fixos.

---

### Sessão 08 — 15/10/2026 — T-08, Docker da API, fuso horário e dados de exemplo

| Campo | Registro |
|---|---|
| Tarefa | T-08 — Docker da API, fuso horário e dados de exemplo |
| Regras e critérios | RN-19, CA-23; ADR-25, ADR-30 e ADR-37 |
| Modelo usado | Claude Sonnet 5.5, no plano e na implementação. No Claude Code (área Code do aplicativo Claude) |
| Tempo real | 30 min (informado pela autora). |
| Estimativa sem IA | 3 h (proposta do assistente: Dockerfile em duas etapas, serviço no Compose, carga de exemplo com datas relativas, teste de um ano inteiro e atualização dos scripts e do README) |

**O que foi pedido:** Dockerfile e serviço `api` com o fuso pela variável `TZ`, a página de documentação também no Docker, a carga dos dados de exemplo com datas relativas ao dia da carga e um teste que simula a carga em cada dia de um ano. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-24.

**Resultado:** branch `feat/docker-api-dados-exemplo`. `backend/Dockerfile` (duas etapas, com `tzdata`), `.dockerignore`, serviço `api` no `docker-compose.yml`, caso de uso `CarregarDadosDeExemplo` na `Application`, serviço de inicialização no `Host`, `run.ps1` e `run.sh` subindo banco e API. 147 testes passando (6 novos), lint limpo, e a API conferida de verdade no Docker (data com fuso -03, quatro aplicações carregadas).

**Primeira tentativa:** certa. O único tropeço foi de ferramenta (um comando de edição com Python, que não existe no computador), refeito sem afetar o código.

**Lacuna encontrada nos requisitos:** a seção 12.1 da `arquitetura.md` cita as aplicações Alfa, Beta, Gama e Delta, mas não lista versões, fases nem datas. O assistente propôs um quadro e a autora aprovou; o quadro foi registrado na seção 12.1 (`arquitetura.md` 0.16).

**O que a revisão humana capturou:** nada.

**Riscos mitigados:** dados de exemplo que envelhecem (teste de um ano inteiro, incluindo 29/02); data errada entre 21h e meia-noite (fuso pela `TZ` e `tzdata` na imagem); carga duplicada (só carrega com o banco vazio); senha em código (tudo pelo `.env`).

**Aprendizados:** quando um documento remete a "um conjunto" de dados, conferir se ele existe antes de planejar. Revisão independente e tag `v0.1.0` ficam para depois do commit.

---

### Sessão 09 — 15/10/2026 — T-09, Projeto Angular

| Campo | Registro |
|---|---|
| Tarefa | T-09 — Projeto Angular |
| Regras e critérios | Nenhuma RN ou CA; `arquitetura.md`, seções 8 e 12, ADR-21, ADR-24 e ADR-38 |
| Modelo usado | Claude Sonnet 5.5, no plano e na implementação. No Claude Code (área Code do aplicativo Claude) |
| Tempo real | Não medido por inteiro; a verificação da autora levou 10 min. |
| Estimativa sem IA | 2 h (proposta do assistente: um desenvolvedor experiente criaria o projeto, configuraria tema, idioma e repasse da API, escreveria o Dockerfile e o servidor web, e adaptaria os quatro scripts para rodar o frontend em container) |

**O que foi pedido:** criar o projeto Angular com Angular Material, idioma pt-BR, tema com as cores dos grupos visuais, testes e lint; incluir o serviço `web` no `docker-compose.yml`; fazer `test` e `lint` incluírem o frontend, em container. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-25.

**Resultado:** branch `feat/projeto-angular`. Projeto em `frontend/` com a lista de aplicações como página inicial, tema das cinco cores (forte e claro) em `src/tema.scss`, `proxy.conf.json` para o desenvolvimento, `Dockerfile` (Node.js e nginx) e serviço `web`; serviço `ferramentas-web` para testes e lint do frontend em container. 3 testes do frontend e 147 do backend passando, lint limpo, `npm audit` sem vulnerabilidades. Conferido de verdade: telas e `/api` pelo nginx no Docker e pelo `npm start`. `arquitetura.md` na versão 0.17 (ADR-38).

**Primeira tentativa:** precisou de ajustes, descritos abaixo.

**Ajustes necessários e por quê:**

- Um dos 3 testes novos falhou na primeira rodada: o teste não abria a página inicial antes de esperar a chamada à API. Corrigido no próprio teste novo.
- O gerador do Angular incluiu o `prettier`, que não está na lista da seção 12.3; foi retirado. Os pacotes que acompanham as ferramentas aprovadas (`eslint`, `typescript-eslint`, o executor de testes e o `@angular/cdk`) ficaram.
- Uma instalação foi feita por engano na raiz do projeto; os arquivos foram apagados antes de qualquer commit.
- A porta 4300 estava ocupada por outro programa do computador da autora; o teste do servidor de desenvolvimento usou outra porta, sem mexer no programa.

**O que a revisão humana capturou:** nada. A autora conferiu a página inicial com Alfa, Beta, Gama e Delta.

**Riscos mitigados:** dependência fora da lista (`prettier` retirado, extras explicados à autora); versões de pacotes escritas pelo `npm`, não de memória; instalação limpa garantida por `npm ci` em container; telas e API na mesma origem, sem CORS; nenhum teste existente alterado.

**Aprendizados:** conferir o que o gerador de projeto traz de dependências além da lista aprovada. Antes de usar uma porta para teste, ver se ela está livre. Rodar comandos sempre a partir da pasta certa evita instalações no lugar errado.

---

### Sessão 10 — 15/10/2026 — T-10, Lista de aplicações e tela da versão

| Campo | Registro |
|---|---|
| Tarefa | T-10 — Lista de aplicações e tela da versão |
| Regras e critérios | RF-01, RF-03 a RF-05, RF-09, RF-10, RF-14; RN-04; verificação pela tela: CA-25, CA-32, CA-33 |
| Modelo usado | Claude Sonnet 5.5, no plano e na implementação. No Claude Code (área Code do aplicativo Claude) |
| Tempo real | 1h30 a 2 h (informado pela autora; contadas 2 h na regra do corte) |
| Estimativa sem IA | 8 h (proposta do assistente: um desenvolvedor experiente criaria a operação única da API com testes, 2 telas, os serviços, as janelinhas e 35 testes das telas, e faria o ajuste de layout) |

**O que foi pedido:** a lista de aplicações (cadastro, edição e exclusão com confirmação) e a tela da versão (rótulo, fase, situação, observação e as sete datas), mais as pendências da T-09 (provar que `test` e `lint` falham de verdade e rodar `install.sh` e `run.sh`). Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, item P-26.

**Resultado:** branch `feat/lista-aplicacoes-tela-versao`. Lista de aplicações e tela da versão com um só botão Salvar; operação nova `PUT /api/versoes/{id}/completo` (ADR-39, tudo ou nada); padrão das telas no ADR-40. 173 testes da API (26 novos) e 38 das telas (35 novos) passando, lint limpo, build sem erros. Conferido de verdade: CA-25 e CA-32 na API com PostgreSQL, e as telas no navegador. Pendências da T-09: `test.ps1` e `lint.ps1` devolveram código 1 com falha provocada (arquivos temporários apagados); `install.sh` e `run.sh` rodaram pelo Git Bash (não conferidos em Linux nem macOS).

**Primeira tentativa:** precisou de ajustes, descritos abaixo.

**Ajustes necessários e por quê:**

- Dois testes novos das telas procuravam a chamada à API cedo demais, antes de a janelinha terminar a animação de fechar; passaram a esperar a chamada.
- O pacote das telas passou do limite de aviso de 500 kB (calendário e janelinhas); o limite subiu para 1 MB, por rodar só no computador local (ADR-40).
- A conferência de um cenário pelo `curl` do Windows deu "Requisição inválida" por causa dos acentos no envio, não por defeito da API; repetida com o corpo em arquivo UTF-8, passou.
- Depois da implementação, uma revisão de usabilidade pedida pela autora trouxe ajustes de layout (abaixo).

**Mudança de plano decidida pela autora durante a aprovação do plano:** o plano propunha três botões na tela da versão (Dados, Fase e Datas), cada um gravando o seu bloco, porque a API gravava essas três coisas em operações separadas e a ordem entre elas importa (sair do Backlog exige as datas antes, MSG-28; remover as datas exige a fase Backlog antes, MSG-24). A autora recusou os três botões: tudo está na mesma tela e deve haver **um só botão Salvar**, também a opção de abas foi descartada. Um botão só, chamando as três operações em sequência, poderia salvar pela metade e contradizer o CA-32 ("nenhuma alteração é salva"), além de colocar uma regra de ordem na tela, o que o `frontend/CLAUDE.md` proíbe. Decisão: criar na API uma operação única (`PUT /api/versoes/{id}/completo`), que recebe rótulo, situação, observação, fase e as sete datas, valida tudo junto e grava tudo ou nada. As três operações existentes continuam. Isso amplia a T-10 além do frontend (cerca de 1 h a mais na estimativa) e será registrado no `arquitetura.md` (seção 7.2 e novo ADR) e no `tarefas.md`.

**O que a revisão humana capturou:**

- O desenho de três botões, aceitável tecnicamente, não servia para o uso: tudo na mesma tela pede um só salvamento.
- Faltavam boas práticas de UX. Revisão de usabilidade pedida pela autora, aplicada e conferida no navegador: barra de título com a cor do tema; conteúdo com largura máxima; fase e situação como etiquetas, com Risco destacado; linha da versão inteira clicável; "Excluir" em cor de alerta e afastado das demais ações; barra de carregamento; barra fixa no pé com o Salvar e as mensagens da API sempre à vista; datas em grade, uma etapa por linha, com "×" dentro do campo.

**Tag `v0.1.0`:** a autora decidiu criá-la agora, no commit que fechou a etapa 1 (T-08), **antes da revisão independente da T-08** (`processo-desenvolvimento.md`, seção 7.3). Essa revisão foi feita na entrega, junto com a revisão independente final, que cobriu todo o código, inclusive o da etapa 1 (ver "Entrega", abaixo).

**Alterações pedidas pela autora depois do commit da T-10** (ao testar as telas):

- **Campo Situação:** estranhava não poder escolher. Passou a mostrar já "Não iniciado" ao escolher Backlog e "Entregue" ao escolher Produção, sem esperar o salvamento; continua só de leitura (RN-21). Um teste novo das telas cobre isso.
- **Observação obrigatória em Risco (nova regra, RN-22):** a autora decidiu que, quando a situação for Risco, a observação é obrigatória. Aprovados por ela: o texto da **MSG-43**, "Informe a observação quando a situação for Risco.", e duas consequências: a Beta v1 dos dados de exemplo ganhou uma observação fictícia, e versões que já estão em Risco sem observação só voltam a ser salvas depois de ganharem uma observação. A regra vale na API (nova `Versao.AlterarDados`, usada por `PUT /api/versoes/{id}` e por `/completo`); texto só com espaços conta como vazio. Documentos: `requisitos-mvp.md` (RN-22) e `arquitetura.md` (catálogo, MSG-43).
- **Testes:** 9 testes novos da regra. Dois testes **que eu criei na T-10** ganharam uma observação, por serem afetados pela regra. Dois testes **antigos** (`RF10_SituacaoEmDesenvolvimento_E_Aceita`, com "Risco" e "risco") passavam a falhar, porque gravam Risco sem observação; **a autora aprovou a alteração** (passaram a informar uma observação). Resultado final pelos scripts oficiais: 182 testes da API (59 regras, 116 casos de uso, 7 API) e 42 das telas passando, lint limpo.
- **Campo Fase:** a autora pediu para não habilitar o campo Fase em Homologação e Produção. Em Produção o campo fica desabilitado (a fase não muda mais, RN-08). Em Homologação ele continua habilitado, mas só oferece Homologação e Produção, porque desabilitá-lo por inteiro impediria avançar para Produção (RN-08, CA-39, CA-40). Conveniência da tela; a API continua validando (MSG-27 e MSG-30). 3 testes novos das telas.

**Correção e mudanças pedidas depois (código no commit `c0f1f60`):**

- **Bug (CRM v0.2):** versão em Backlog, ao escolher Homologação e informar a prorrogação no mesmo salvamento, era recusada com a MSG-23 ("A prorrogação só pode ser registrada para versões em Homologação."). **Causa:** no salvamento único, quando a fase avança, as datas são gravadas antes da fase (sair do Backlog exige as datas, MSG-28), e a regra da prorrogação olhava a fase **atual** (Backlog). **Correção:** nesse caso a prorrogação nova vale para a fase de **destino**. Escolher outra fase que não seja Homologação continua recusando com a MSG-23. 3 testes novos na API reproduzem e cobrem o caso (2 falhavam antes da correção). Conferido na API real. ADR-39 vale com essa precisão.
- **Salvar volta para a lista:** ao salvar com sucesso, a tela da versão volta para a Lista de aplicações, com o aviso "Alterações salvas."; se a API recusar, a autora continua na tela com as mensagens. 2 testes novos das telas.

- **Cores das etiquetas (pedido da autora):** Backlog e Não iniciado cinza claro; Análise, Desenvolvimento e Testes azul claro; Homologação roxo claro; On track verde claro; Produção e Entregue verde escuro; Risco laranja claro (a autora escolheu laranja ou amarelo, para não confundir com o vermelho da prorrogação). A "Prorrogação da homologação" (vermelho claro) é cor da timeline e não aparece como etiqueta nestas telas. As cores vêm do tema (`tema.scss`, nova variável `--cor-risco`), num componente de etiqueta só (`Etiqueta`), usado na lista e na tela da versão. 5 testes novos das telas.

- **Mensagem de exclusão:** a frase "Os status reports já congelados não são afetados." saiu da confirmação de exclusão da aplicação e das descrições da API (página de documentação), porque o status report não faz parte do escopo atual. Também saiu a menção a "timeline de entregas e status report semanal" da descrição geral da API.
- **Escopo reduzido (decisão da autora):** por enquanto, só o cadastro de aplicações e versões; timeline, tabela de status, congelamento e histórico ficam adiados. Depois, a autora encerrou o desenvolvimento ao fim da etapa 2 e reescreveu o README e os documentos para a entrega (T-20 a T-22); as T-11 a T-19 ficam como próximos passos.
- **Requisitos e critérios acompanham o código:** os documentos reescritos pela autora substituíram os anteriores e perderam o que o código já fazia; o conteúdo foi recolocado, **sem mudar os números de versão dos documentos**, a pedido da autora. No `requisitos-mvp.md`: RN-36 (etiquetas), RF-21 a RF-24 e CA-43 a CA-49 (observação em Risco, prorrogação ao passar para Homologação, salvar volta à lista, campos Fase e Situação, etiquetas, título e versão do sistema, data de entrega na lista), e ajustes em RN-04, RN-08, RN-13, RN-21, RN-22 e RN-24. No `arquitetura.md`: MSG-43 no catálogo, ADR-39, ADR-40 e a rota da lista. Os números RN-31 e RF-18 a RF-20 já estavam reservados na tabela de evoluções futuras (filtros de visualização), por isso os itens novos usam RN-36 e RF-21 a RF-24.
- **Versão do sistema na tela:** a barra de título mostra a tag da última entrega (`v0.2.0`), em `frontend/src/app/versao-sistema.ts`, para ser atualizada junto com cada tag. 1 teste novo.

- **Data de entrega na lista de aplicações e título (pedidos da autora):** a lista mostra, para cada versão, a coluna "Data de entrega" em dd/mm/aaaa, ou "TBD" quando não há datas (RN-13); para isso o resumo das versões da API ganhou `dataEntrega` (`arquitetura.md`, rota da lista). O título passou a "Timeline de Entregas (MVP)", na barra e na aba do navegador. `requisitos-mvp.md` (RF-23, RF-24, CA-48, CA-49). 3 testes novos. **Um teste antigo**, `RF03_ListaDeAplicacoes_TrazAsVersoesDeCadaUmaNaOrdemDeCadastro`, passa a falhar porque compara o resumo da versão sem a data; a autora aprovou o ajuste (incluir a data de entrega 24/08/2026 da v1 no resultado esperado).

**Entrega (T-20 a T-22), a pedido da autora ("faça tudo na ordem que julgar melhor", P-27):**

- **Revisão independente do código:** feita por um agente separado, só leitura, que não participou da implementação. Não achou nenhum problema grave; apontou 10 itens. **Corrigidos:** o "tudo ou nada" da versão agora desfaz em qualquer falha, e não só em regra violada (teste novo); a documentação passou a dizer que o bloco de datas ausente dá a MSG-42 e a fase ausente ou desconhecida dá a MSG-26 (ADR-39 e comentário do código); 3 testes novos. **Registrados como evoluções técnicas no README:** métodos públicos da versão que não conferem a regra do Risco, falta de controle de concorrência, tratamento de corpo grande demais e do cancelamento, teste de camadas só do Domain e nome ausente na confirmação de exclusão. **Resolvidos depois, com a aprovação da autora:** os métodos `AlterarSituacao` e `AlterarObservacao` da versão passaram a ser internos (só o projeto de testes do `Domain` os enxerga, então nenhum teste antigo foi alterado) e a carga de exemplo usa `AlterarDados`; `vitest` e `jsdom` (e `@angular/cdk`, `eslint`, `typescript-eslint` e `@eslint/js`, que vêm com as ferramentas aprovadas) foram registrados na arquitetura, seção 12.3, com a licença MIT conferida nos pacotes instalados.
- **C-11 (informação de terceiros):** a autora passou os termos a procurar, apenas na conversa, e o assistente os buscou em todo o histórico do Git (conteúdo, mensagens de commit, autores, e-mails e nomes de arquivo). Nenhum nome real foi encontrado; as únicas ocorrências foram palavras comuns ("FITNESS" na licença MIT e `flex` no CSS). Os termos não foram escritos em nenhum arquivo.
- **Instalação limpa (C-10 e C-05):** o `main` foi clonado numa pasta nova e o README seguido (instalar, executar, testar e lint): tudo com código 0, 239 testes passando.
- **T-20:** os 42 exemplos do `exemplos.http` foram executados do começo ao fim, num banco vazio, todos com o resultado esperado. O arquivo foi completado: recadastro da Beta (excluída no passo 10), data de entrega na lista e exemplos do CA-43 e do CA-44.
- **Ícone da aba e imagens:** ícone próprio no lugar do logotipo do Angular; as imagens do README refeitas com a v1.0.0 na barra.
- **Checklist de release, pré-verificação do assistente:** C-03 (todos os commits de trabalho no padrão Conventional Commits; os merges registram a integração de cada branch, como `Integrar <branch>`), C-04, C-06 (nenhum segredo no histórico inteiro; `.env` nunca versionado), C-07 e C-09, sem pendências.

**Riscos mitigados:** regra de ordem entre fase e datas fora da tela (ficou na API, ADR-39); data digitada errada apagando um valor sem aviso (campos só de leitura, escolha pelo calendário); datas trocando de dia por fuso (conversão pelos componentes locais); teste ajustado para o código passar (os três testes antigos alterados nesta sessão, listados acima, tiveram a aprovação da autora); nenhuma dependência nova.

**Aprendizados:** quando uma decisão de tela (um só botão) conflita com como a API grava, resolver na API e não na tela. Pedir a revisão de UX antes do commit custou pouco e evitou retrabalho nas próximas telas. Conferir o envio de acentos pelo `curl` no Windows antes de suspeitar da API.

### Sessão 11 — 15/10/2026 — Conferência final da entrega

| Campo | Registro |
|---|---|
| Tarefa | Complemento da T-22: conferência dos documentos entre si e contra as diretrizes do curso |
| Regras e critérios | Checklist de release C-01 a C-11 (`processo-desenvolvimento.md`, seção 14) e seções obrigatórias do README |
| Modelo usado | Claude Opus 5.5, numa conversa no claude.ai |
| Tempo real | Não medido |
| Estimativa sem IA | 2 h (proposta do assistente: ler o README, os seis documentos e o histórico do Git atrás de contradições e corrigi-las) |

**O que foi pedido:** conferir o projeto inteiro, com prioridade para as diretrizes do curso, e deixar tudo coerente para a entrega, sem acrescentar trabalho além do necessário.

**Resultado:** branch `docs/consistencia-entrega`. No README: a menção à revisão independente como não realizada saiu (ela foi feita, e os seus achados já estavam na seção 14); a seção 1 passou a se chamar "Descrição"; a ordem da lista de exemplo ficou igual à da tela (alfabética); a legenda da captura da tela da versão passou a descrever o que a imagem mostra; a porta da captura da API foi explicada; o passo de instalação ganhou a orientação para quem recebe o projeto em `.zip`; a pedido da autora, a seção 14 passou a separar o escopo do MVP (decisões de planejamento), os próximos degraus e as evoluções técnicas sugeridas pela revisão independente final. No diário: tempos não medidos das Sessões 00 a 02 registrados como tais, campos em branco da Sessão 01 preenchidos, a revisão da T-08 dada como coberta pela revisão final e o resumo acumulado completo. No `tarefas.md`: T-20 a T-22 marcadas. No `processo-desenvolvimento.md`: a mensagem dos merges (`Integrar <branch>`). No registro de prompts: o pedido da entrega (P-27). No `arquitetura.md`: aprovação da MSG-43 na tabela da seção 14. Nenhum código alterado.

**Primeira tentativa:** funcionou.

**O que a revisão humana capturou:** a autora pediu que a conferência partisse das diretrizes do curso, e não só do código, o que mostrou contradições que só aparecem lendo os documentos lado a lado.

**Riscos mitigados:** documentação contraditória chegando à avaliação.

**Aprendizados:** ao final, conferir os documentos entre si, e as legendas contra as imagens, e não só o código e os testes.

---

## Resumo acumulado

Atualizado ao final de cada fase do `tarefas.md`.

| Etapa | Tempo real | Estimativa sem IA | Sessões | Acertos na primeira tentativa |
|---|---|---|---|---|
| Planejamento | Não medido | 16 h | 1 (Sessão 00) | 15 de 17 pedidos |
| 0. Preparação | Não medido; verificação da T-03: cerca de 10 min | 6 h (T-01: 2 h; T-02: 1 h; T-03: 3 h) | 3 (Sessões 01 a 03) | 1 de 3 (T-02) |
| 1. APIs de cadastro | T-08: 30 min; as demais não foram medidas por inteiro | 18 h (T-04: 4 h; T-05: 3 h; T-06: 4 h; T-07: 4 h; T-08: 3 h) | 5 (Sessões 04 a 08) | 2 de 5 (T-07 e T-08); T-05 e T-06 com o desenho certo e pequenos ajustes |
| 2. Telas de cadastro | T-09 não medido por inteiro (verificação: 10 min); T-10: 2 h | 10 h (T-09: 2 h; T-10: 8 h) | 2 (Sessões 09 e 10) | 0 de 2 |
| Ajustes pedidos depois da T-10 e entrega (T-20 a T-22) | Cerca de 3,5 h de relógio, entre o commit da T-10 (13:32) e o último commit da release (17:01) | Não estimada | Registrados na Sessão 10; conferência final na Sessão 11 | — |
| 3 a 7 (timeline, tabela de status e congelamento) | Não iniciadas: próximos passos, por decisão da autora (README, seção 14) | — | — | — |
| **Total** | **Cerca de 8,5 h de relógio** entre o commit da T-03 (08:32) e o último commit da release (17:01), pelo histórico do Git, com pausas e verificações da autora; mais o planejamento e a preparação, não medidos | **50 h** (planejamento e etapas 0 a 2) | 12 (Sessões 00 a 11) | — |

O tempo de relógio vem dos horários dos commits no Git, porque o tempo de cada sessão nem sempre foi anotado. As estimativas sem IA foram propostas pelo assistente e aceitas pela autora.

**Destaques para o README** (preencher ao longo do projeto)

| Tópico | Exemplos registrados |
|---|---|
| Onde a IA acelerou o desenvolvimento | Sessão 00: revisão cruzada de nove documentos, catálogo de 41 mensagens e roteiro de prompts das 22 tarefas numa única sessão. A revisão de arquitetura encontrou a contradição do CA-02, que viraria um teste errado. |
| Onde a revisão humana foi decisiva | Sessão 00: a ordem das etapas, com a entrega como primeiro objetivo, e as regras N-01 a N-06 foram decisões da autora; a autora apontou que os prompts precisavam mostrar o modelo CO-STAR. Sessão 10: a autora recusou os três botões Salvar e pediu um só, o que levou à operação única da API (ADR-39); a revisão de UX pedida por ela trouxe as boas práticas de layout que faltavam. |
| Quais prompts funcionaram melhor | P-03 (revisão de arquitetura com papel e perguntas objetivas); P-05 e P-07 ("o que você vai fazer?", "o que eu faço?"); P-08 (conferência final antes de começar). |
| Quais riscos foram mitigados | Sessão 00: teste derivado de exemplo errado; dependências inventadas; informação indevida no repositório público; assistente sem as regras do projeto. |

---

## Histórico do documento

| Versão | Data | Descrição |
|---|---|---|
| 0.2 | 14/10/2026 | Modelo refeito: registro do pedido feito ao assistente, da primeira tentativa, do que a revisão humana capturou e dos riscos mitigados; resumo acumulado por fase e destaques para o README. |
| 0.3 | 15/10/2026 | Registrada a Sessão 00 (planejamento); resumo acumulado organizado pelas etapas do `tarefas.md` 0.8; destaques para o README com os primeiros exemplos. |
| 0.4 | 15/10/2026 | Sessão 00 atualizada com os pedidos P-15 a P-17 e o ajuste dos prompts ao modelo CO-STAR. |
| 0.5 | 15/10/2026 | Registradas as Sessões 01 (T-01) e 02 (T-02); resumo acumulado da Etapa 0 iniciado. |
| 0.6 | 15/10/2026 | Registrada a Sessão 03 (T-03), com os pedidos P-18 e P-19; resumo acumulado da Etapa 0 atualizado. |
| 0.7 | 15/10/2026 | Sessão 03: tempo real (não medido), o que a revisão capturou e novo aprendizado registrados. |
| 0.8 | 15/10/2026 | Registrada a Sessão 04 (T-04), com o pedido P-20. |
| 0.9 | 15/10/2026 | Registrada a Sessão 05 (T-05), com o pedido P-21. |
| 0.10 | 15/10/2026 | Registrada a Sessão 06 (T-06), com o pedido P-22. |
| 0.11 | 15/10/2026 | Registrada a Sessão 07 (T-07), com o pedido P-23 e a lacuna da lista de aplicações sem as versões. |
| 0.12 | 15/10/2026 | Registrada a Sessão 08 (T-08), com o pedido P-24 e a lacuna do conjunto de dados de exemplo. |
| 0.13 | 15/10/2026 | Registrada a Sessão 09 (T-09), com o pedido P-25. |
| 0.14 | 15/10/2026 | Registrada a Sessão 10 (T-10), com o pedido P-26, a mudança para um só botão Salvar, a revisão de UX e a tag `v0.1.0` criada antes da revisão independente da T-08. |
| 0.15 | 15/10/2026 | Sessão 10: registradas as alterações pedidas depois do commit (campo Situação, observação obrigatória em Risco e MSG-43). |
| 0.16 | 15/10/2026 | Sessão 10: entrega (T-20 a T-22) e revisão independente final registradas. Sessão 11: conferência final. Tempos não medidos das Sessões 00 a 02 registrados, campos da Sessão 01 completados, revisão da T-08 dada como coberta pela revisão final e resumo acumulado completo. |
| 0.17 | 15/10/2026 | Sessão 11: seção 14 do README reorganizada em escopo do MVP, próximos degraus e evoluções técnicas. |

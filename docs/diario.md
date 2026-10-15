# Diário de Desenvolvimento
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão do modelo | 0.10 |
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
| Tempo real | *(a preencher pela autora)* |
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

**O que a revisão humana capturou:** *(rascunho do assistente; a autora completa com o que ela própria percebeu)*

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
- Sessões anteriores de planejamento (requisitos e primeira versão da arquitetura) não foram registradas neste diário. A autora pode acrescentá-las, se quiser.

### Sessão 01 — 15/10/2026 — T-01, Preparação do ambiente

| Campo | Registro |
|---|---|
| Tarefa | T-01 — Preparação do ambiente |
| Regras e critérios | Nenhum (tarefa de preparação) |
| Modelo usado | Nenhum modelo de IA na execução: a tarefa foi feita pela autora, com o roteiro do `tarefas.md` |
| Tempo real | *(a preencher pela autora)* |
| Estimativa sem IA | 2 h (proposta do assistente: instalar e verificar Git, .NET, Node.js e Docker, configurar o Git e montar a pasta do projeto) |

**O que foi pedido:** não houve pedido ao Claude Code. A autora seguiu o roteiro da T-01: verificou o computador, instalou as ferramentas, conferiu cada uma pelos comandos da tabela, configurou o Git e montou a pasta do projeto. Esta entrada foi criada na Sessão 02, a pedido da autora.

**Resultado:** ambiente pronto. Pasta `C:\Projetos\timeline-entregas` montada com os documentos, os três `CLAUDE.md` e o `prompts/prompts-claude-code.md`; Git configurado com o e-mail privado do GitHub; área Code do aplicativo Claude aberta na pasta.

**Primeira tentativa:** precisou de ajustes: três problemas no ambiente, descritos abaixo.

**Ajustes necessários e por quê:**

- O comando `docker` não era reconhecido depois da instalação. Só passou a funcionar depois de reiniciar o computador.
- O motor do Docker ficou parado até a instalação do WSL, que o Docker Desktop usa no Windows.
- O e-mail do Git foi configurado por engano com os sinais `<` e `>` em volta do endereço e depois corrigido.

**O que a revisão humana capturou:** *(a preencher pela autora)*

**Riscos mitigados:** assistente sem as regras do projeto (os `CLAUDE.md` já estavam na pasta, com os nomes certos, na primeira sessão); informação indevida nos commits (e-mail privado do GitHub).

**Aprendizados:** *(a preencher pela autora)*

---

### Sessão 02 — 15/10/2026 — T-02, Repositório e documentação inicial

| Campo | Registro |
|---|---|
| Tarefa | T-02 — Repositório e documentação inicial |
| Regras e critérios | Nenhuma RN ou CA; `arquitetura.md`, seções 4 e 9, e ADR-19 (licença MIT) |
| Modelo usado | Claude Sonnet 5.5, no Claude Code (área Code do aplicativo Claude) |
| Tempo real | *(a preencher pela autora)* |
| Estimativa sem IA | 1 h (proposta do assistente: um desenvolvedor experiente escreveria os arquivos de configuração e o esqueleto do README, iniciaria o repositório e conferiria o envio ao GitHub) |

**O que foi pedido:** iniciar o repositório e enviar o primeiro commit, com `.gitignore`, `.gitattributes` e `.editorconfig` criados antes dele, `.env.example`, `LICENSE` (MIT) e o esqueleto do README com as 15 seções. Pedido completo na Parte 2 do `prompts/prompts-claude-code.md`, se registrado.

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

## Resumo acumulado

Atualizado ao final de cada fase do `tarefas.md`.

| Etapa | Tempo real | Estimativa sem IA | Sessões | Acertos na primeira tentativa |
|---|---|---|---|---|
| Planejamento | *(a preencher)* | 16 h | 1 | 15 de 17 pedidos |
| 0. Preparação | | 4,0 h (T-01 a T-03) | 3 (T-01, T-02 e T-03) | |
| 1. APIs de cadastro | | | | |
| 2. Telas de cadastro | | | | |
| 3. API da timeline | | | | |
| 4. Tela da timeline | | | | |
| 5. API da tabela de status das aplicações | | | | |
| 6. Tela da tabela de status das aplicações | | | | |
| 7. Congelamento | | | | |
| Entrega | | | | |
| **Total** | | | | |

**Destaques para o README** (preencher ao longo do projeto)

| Tópico | Exemplos registrados |
|---|---|
| Onde a IA acelerou o desenvolvimento | Sessão 00: revisão cruzada de nove documentos, catálogo de 41 mensagens e roteiro de prompts das 22 tarefas numa única sessão. A revisão de arquitetura encontrou a contradição do CA-02, que viraria um teste errado. |
| Onde a revisão humana foi decisiva | Sessão 00: a ordem das etapas, com a entrega como primeiro objetivo, e as regras N-01 a N-06 foram decisões da autora; a autora apontou que os prompts precisavam mostrar o modelo CO-STAR. |
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

# Prompts do Claude Code
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão | 0.5 |
| Data | 15/10/2026 |

Este arquivo tem duas partes:

1. **Roteiro:** os prompts planejados para cada tarefa do `docs/tarefas.md`. É o ponto de partida de cada pedido.
2. **Registro:** os pedidos realmente enviados que se encaixam nos casos da seção 13.2 do `docs/tecnico/processo-desenvolvimento.md`, na íntegra e com o resultado. É a fonte da seção do README sobre o uso de IA, junto com o `docs/diario.md`.

---

# Parte 1 — Roteiro

## Como usar

Todos os prompts seguem o modelo **CO-STAR**, com os seis elementos explícitos:

| Elemento | O que diz ao Claude Code | De onde vem |
|---|---|---|
| **Contexto** | Em que etapa e tarefa estamos, o que já existe e os pontos de atenção | Do prompt, apontando para os documentos do projeto |
| **Objetivo** | O que a tarefa entrega e quais regras, critérios e mensagens atende | Do item da tarefa no `docs/tarefas.md` |
| **Estilo** | As convenções de código e de nomes a seguir | `CLAUDE.md` da raiz, do backend e do frontend |
| **Tom** | Como falar com a autora | Igual em todos os prompts |
| **Audiência** | Para quem é a resposta | Igual em todos os prompts |
| **Resposta** | O formato esperado: plano primeiro, depois implementação, verificação e passo a passo | Seções 4.1 e 5 do processo |

O Contexto e o Estilo são curtos porque apontam para os documentos: assim o Claude Code recebe só o necessário para a tarefa, e o restante ele lê quando precisa.

**Prompts de continuação.** Dentro de uma mesma sessão, o Contexto, o Estilo, o Tom e a Audiência já estão na conversa. Por isso, os prompts que continuam um trabalho em andamento (aprovar um plano, pedir uma explicação, confirmar um commit) trazem só **Objetivo** e **Resposta**.

**Regras de uso**

- **Uma tarefa por sessão.** Abra o Claude Code na pasta do projeto no início de cada tarefa e feche ao terminar. Uma sessão nova começa limpa e relê o `CLAUDE.md`.
- **Sequência de cada tarefa:** A1 (início de sessão) → prompt da tarefa → prompts de continuação conforme a situação (A2 a A10) → A11 (registro) → A12 (commit). No início de cada etapa, use também o A15 (regra do corte).
- **Copie o prompt inteiro** e troque só o que está entre `< >`.
- **Antes de aprovar o plano**, confira os itens "Confira no plano" da tarefa.
- **Registro:** os pedidos que se encaixam na seção 13.2 do processo vão para a Parte 2 deste arquivo, com o texto que você realmente enviou.

| Técnica | Como aparece nos prompts |
|---|---|
| CO-STAR | Contexto, Objetivo, Estilo, Tom, Audiência e Resposta em cada pedido |
| Otimização de contexto | Contexto e Estilo apontam para os documentos, sem repeti-los |
| Raciocínio explícito | A Resposta pede sempre o plano antes de qualquer alteração |
| Exemplos de referência | "Siga o padrão da T-06" (APIs) e "da T-10" (telas) |
| Iteração | Tarefas grandes divididas em partes, com conferência entre elas (T-11 e T-14) |
| Testes como contrato | Lista de testes falhando antes do código, nas tarefas TDD |

**Visão geral das tarefas**

| Tarefa | Modelo sugerido | Antes de começar |
|---|---|---|
| T-02 Repositório | Rápido | Repositório criado no GitHub (feito) |
| T-03 Esqueleto | Rápido | — |
| T-04 Regras da versão | Maior capacidade | Catálogo de mensagens revisado |
| T-05 Banco | Rápido | Modelo de dados revisado |
| T-06 API de aplicações | Maior capacidade (padrão de referência) | — |
| T-07 API de versões | Rápido | — |
| T-08 Docker e dados de exemplo | Rápido | Dados de exemplo aprovados |
| T-09 Projeto Angular | Rápido | — |
| T-10 Telas de cadastro | Maior capacidade (padrão das telas) | — |
| T-11 Cálculo da timeline | Maior capacidade | — |
| T-12 API da timeline | Rápido | — |
| T-13 Refatoração | Maior capacidade | — |
| T-14 Tela da timeline | Maior capacidade | — |
| T-15 API da tabela de status | Rápido | — |
| T-16 Tela da tabela de status | Rápido | — |
| T-17 Semanas | Rápido | — |
| T-18 Congelamento na API | Maior capacidade | — |
| T-19 Telas do Histórico | Rápido | — |
| T-20 Exemplos de uso | Rápido | — |
| T-21 README | Maior capacidade | Diário em dia |
| T-22 Release | Rápido | Revisão independente final |

Na dúvida, use o de maior capacidade: a revisão da autora é pelo comportamento e não detecta problemas internos do código.

## A — Prompts do ciclo

### A1 — Início de sessão

Primeira mensagem de cada sessão. Confirma que o Claude Code leu as regras e sabe onde o projeto está.

```
CONTEXTO: Projeto Timeline de Entregas. Vamos trabalhar na tarefa <T-XX> do docs/tarefas.md. As regras do projeto estão no CLAUDE.md; o andamento, no docs/tarefas.md e no docs/diario.md.
OBJETIVO: Confirmar que você leu as regras e sabe onde o projeto está, antes de começar a tarefa.
ESTILO: Resumo direto, sem detalhes técnicos.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Até cinco linhas: onde estamos, o que a <T-XX> pede e se falta algo antes de começar. Não altere nenhum arquivo.
```

### A2 — Aprovar o plano (continuação)

```
OBJETIVO: Seguir com o plano que você apresentou, que está aprovado.
RESPOSTA: Crie o branch da tarefa e implemente conforme o plano.
```

### A3 — Corrigir o plano (continuação)

```
OBJETIVO: Ajustar o plano antes da aprovação: <o que está faltando, sobrando ou diferente do esperado>.
RESPOSTA: O plano completo de novo, com o que mudou destacado. Não altere nenhum arquivo.
```

### A4 — Pedir explicação (R-01, continuação)

Use sempre que não conseguir explicar com as suas palavras o que foi feito.

```
OBJETIVO: Eu entender <o trecho ou o termo>, que não ficou claro.
RESPOSTA: Uma explicação de outro jeito, sem termos técnicos, com um exemplo usando os dados fictícios dos critérios de aceite.
```

### A5 — Conferir os testes antes do código (tarefas TDD, continuação)

```
OBJETIVO: Conferir os testes antes de você implementar o código.
RESPOSTA: Uma tabela com os testes falhando: nome do teste, critério ou regra coberto e o cenário em uma frase em português. Diga se a lista é igual à do plano aprovado e, se não for, o que mudou.
```

### A6 — Pedir cenários extras (R-02, continuação)

Exemplos de cenários: data igual ao limite entre duas etapas, data atual no último dia de uma etapa, aplicação sem versões.

```
OBJETIVO: Confirmar, antes de eu aceitar a tarefa, estes cenários:
- <cenário 1>
- <cenário 2>
RESPOSTA: Para cada cenário, o teste existente que o cobre ou uma demonstração. Se algum não estiver coberto, proponha o teste e aguarde minha aprovação.
```

### A7 — Relatório de verificação (R-03 a R-05, continuação)

```
OBJETIVO: Saber se a tarefa está tecnicamente pronta.
RESPOSTA: O resumo do build, dos testes e do lint; a lista dos arquivos de teste criados e alterados nesta tarefa; o resultado da revisão técnica (R-03 e R-04 do processo); e o que você não conseguiu verificar.
```

### A8 — Me guiar na "Sua verificação" (continuação)

```
OBJETIVO: Eu fazer a "Sua verificação" da <T-XX>.
RESPOSTA: Um passo de cada vez, com o comando exato para o PowerShell ou o caminho na página de documentação da API (qual operação abrir e o que preencher), e o resultado esperado. Espere eu confirmar cada passo antes do próximo.
```

### A9 — Relatar um problema

```
CONTEXTO: Problema na verificação da <T-XX>.
O que fiz: <comando ou requisição>.
O que esperava: <resultado, citando o critério de aceite>.
O que apareceu: <texto completo copiado do terminal>.
OBJETIVO: Entender a causa e corrigir sem quebrar o que já funciona.
ESTILO: Siga as convenções do CLAUDE.md. Escreva primeiro um teste que reproduza o problema.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro a explicação da causa, em linguagem simples, sem alterar nada. Depois, o plano de correção, para eu aprovar.
```

### A10 — Quando o Claude Code achar um teste ou critério errado (continuação)

```
OBJETIVO: Decidir sobre <o teste ou o critério> que você indicou como errado. Não altere nada ainda.
RESPOSTA: Um exemplo concreto mostrando qual regra conflita com qual, e as opções, com o efeito de cada uma no comportamento do sistema.
```

### A11 — Registro da sessão (continuação)

```
OBJETIVO: Registrar esta sessão.
Tempo real: <X h>.
O que a revisão capturou: <o que eu percebi ao conferir, ou "nada">.
RESPOSTA: O rascunho da entrada do docs/diario.md, no modelo do próprio diário, com a estimativa sem IA e uma frase de justificativa. Se algum pedido desta sessão se encaixar na seção 13.2 do processo, também o rascunho da entrada na Parte 2 do prompts/prompts-claude-code.md. Marque a tarefa no docs/tarefas.md e proponha a mensagem de commit.
```

### A12 — Confirmar o commit (continuação)

```
OBJETIVO: Concluir a tarefa com a mensagem de commit aprovada.
RESPOSTA: Faça o commit, integre o branch ao main e envie ao GitHub. Depois, me diga como conferir no GitHub.
```

### A13 — Revisão independente (nova sessão)

Na última tarefa de cada etapa, antes da tag. Feche a sessão da tarefa, abra uma nova na pasta do projeto e envie:

```
CONTEXTO: Projeto Timeline de Entregas, fim da etapa <N — nome> do docs/tarefas.md. Você não participou da implementação: esta é uma revisão independente.
OBJETIVO: Encontrar problemas no código antes da tag, comparando-o com o CLAUDE.md, o backend/CLAUDE.md, o frontend/CLAUDE.md (se houver telas) e o docs/tecnico/arquitetura.md.
ESTILO: Revisão crítica e objetiva. Não altere nenhum arquivo.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Uma lista dos problemas, do mais grave para o menos grave, cada um com o arquivo, a regra ou convenção não seguida e a correção sugerida.
```

### A14 — Levar os achados da revisão independente (continuação)

De volta à sessão da tarefa:

```
OBJETIVO: Tratar os problemas apontados pela revisão independente desta etapa:
<cole a lista>
RESPOSTA: Para cada problema, uma explicação em linguagem simples e a indicação de corrigir agora ou deixar como melhoria; depois, o plano de correção. Nenhum teste existente pode ser alterado sem minha aprovação.
```

Depois das correções:

```
OBJETIVO: Marcar o fim da etapa.
RESPOSTA: Crie a tag <v0.X.0> e envie ao GitHub.
```

### A15 — Início de etapa (regra do corte)

```
CONTEXTO: Projeto Timeline de Entregas. Vamos começar a etapa <N — nome> do docs/tarefas.md. Tempo real acumulado, pelo diário: <X h>.
OBJETIVO: Decidir se a etapa cabe antes do corte das 26 h (regra do corte do docs/tarefas.md).
ESTILO: Conta simples, mostrando os números.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: O tempo real mais a estimativa da etapa, comparado com 26 h. Se passar, as opções e o que ficaria como próximo passo no README. Não altere nenhum arquivo.
```

### A16 — Retomar uma sessão interrompida

```
CONTEXTO: Projeto Timeline de Entregas. A sessão anterior da <T-XX> foi interrompida.
OBJETIVO: Saber o estado da tarefa antes de continuar.
ESTILO: Siga as convenções do CLAUDE.md.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Pelo branch atual, pelas alterações ainda não registradas no Git e pelo docs/diario.md: o que já foi feito, o que falta e se algo ficou pela metade. Não altere nada antes de eu confirmar o próximo passo.
```

## B — Preparação (T-01 a T-03)

### T-01 — Preparação do ambiente

Feita pela autora, pelo roteiro do `tarefas.md`, sem prompt ao Claude Code. No passo 4 (configurar o Git), use estes dois comandos no PowerShell:

```
git config --global user.name "<Seu Nome>"
git config --global user.email "<seu e-mail do GitHub>"
```

**Atenção ao e-mail:** ele aparece em todos os commits, e o repositório será público. Para não expor o e-mail pessoal, use o endereço privado que o GitHub oferece: em github.com, Settings → Emails, marque "Keep my email addresses private" e copie o endereço que termina em `@users.noreply.github.com`.

### T-02 — Repositório e documentação inicial

Primeira sessão do projeto. Comece confirmando o que ele entendeu:

```
CONTEXTO: Projeto Timeline de Entregas, primeira sessão. As regras estão no CLAUDE.md, que cita os demais documentos do projeto.
OBJETIVO: Confirmar que você entendeu o projeto e o nosso jeito de trabalhar antes da primeira tarefa.
ESTILO: Resumo direto, sem detalhes técnicos.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Até cinco linhas. Não altere nenhum arquivo.
```

Se a resposta fizer sentido:

```
CONTEXTO: Projeto Timeline de Entregas, preparação. Tarefa T-02 do docs/tarefas.md: Repositório e documentação inicial.
O repositório privado já foi criado no GitHub, vazio: https://github.com/priscilabertolazzi/timeline-entregas.git
A pasta do projeto já tem os documentos, os três CLAUDE.md e o prompts/prompts-claude-code.md, colocados na T-01.
OBJETIVO: Iniciar o repositório e enviar o primeiro commit, com o .gitignore, o .gitattributes e o .editorconfig criados antes dele, o .env.example, o LICENSE (MIT) e o esqueleto do README.md.
O esqueleto do README.md deve ter estas seções, em português, cada uma ainda sem conteúdo e com uma linha dizendo o que vai conter:
1. Título e descrição (o problema e a solução)
2. O que foi entregue
3. Tabela de conteúdos
4. Tecnologias utilizadas (inclui o assistente de IA e os modelos usados)
5. Pré-requisitos
6. Dependências e ambiente (arquivos de dependências e de ambiente, com a equivalência ao requirements.txt e ao pyproject.toml de projetos Python)
7. Instalação e execução local
8. Uso e exemplos
9. Estrutura do projeto
10. Arquitetura da aplicação
11. Testes
12. Como a IA foi usada no desenvolvimento, com as subseções: onde a IA acelerou o desenvolvimento; onde a revisão humana foi decisiva; quais prompts funcionaram melhor; quais riscos foram mitigados
13. Demonstração visual
14. Limitações e próximos passos
15. Créditos e licença
ESTILO: Siga as convenções do CLAUDE.md.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

**Confira no plano**

- O `.gitignore`, o `.gitattributes` e o `.editorconfig` são criados **antes** do primeiro commit.
- O `.env` não é criado nem versionado; o `.env.example` traz só valores fictícios.
- O `LICENSE` é MIT, com o seu nome como autora.
- Os documentos que você colocou na pasta na T-01, incluindo este arquivo, são versionados sem alterações.

### T-03 — Esqueleto da solução, banco e scripts

```
CONTEXTO: Projeto Timeline de Entregas, preparação. Tarefa T-03 do docs/tarefas.md: Esqueleto da solução, banco e scripts.
Referências: arquitetura.md, seções 4 (estrutura), 9 (configuração), 12.2 (scripts) e 12.3 (dependências previstas, já aprovadas).
Pontos de atenção:
- Dependências só da lista da seção 12.3, adicionadas pelas ferramentas oficiais, sem escrever versões.
- Os scripts test e lint rodam em container (ADR-24).
- Os scripts .sh precisam funcionar no Linux e no macOS.
OBJETIVO: Criar o global.json, a solução e os projetos do módulo Timeline com as referências entre camadas, o docker-compose.yml com o banco, os scripts install, run, test e lint (.ps1 e .sh) e o Makefile, e atualizar o README com pré-requisitos, instalação e comandos.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Confirme que os scripts test e lint funcionam sem depender do SDK instalado.
```

**Confira no plano**

- Os projetos e as referências entre eles batem com o diagrama da seção 4 do `arquitetura.md`.
- Nenhuma senha no `docker-compose.yml`: tudo vem do `.env`.
- O README ganha pré-requisitos, instalação e comandos.

## Etapa 1 — APIs de cadastro (T-04 a T-08)

### T-04 — Regras da aplicação e da versão (TDD)

Antes de começar: catálogo de mensagens revisado (`arquitetura.md`, seção 7.6).

```
CONTEXTO: Projeto Timeline de Entregas, etapa 1 (APIs de cadastro). Tarefa T-04 do docs/tarefas.md: Entidades e regras da aplicação e da versão (TDD).
Referências: arquitetura.md, seções 5.2, 5.3 e 11.
Pontos de atenção:
- Uma regra violada lança RegraNegocioException com as mensagens de todas as regras violadas (ADR-26).
- Fases: avanço para qualquer fase seguinte, sem passar pelas intermediárias (RN-08, CA-40).
- Prorrogação: nova só em Homologação; a já registrada pode ser alterada ou removida também em Produção (RN-24, CA-42).
- Nomes e rótulos sem os espaços das pontas e comparados sem diferença de maiúsculas (RN-01, RN-02).
- Data atual sempre fixada nos testes.
OBJETIVO: Criar as entidades Aplicacao e Versao, em TDD, garantindo as regras RN-01, RN-02, RN-05, RN-07 a RN-10, RN-21, RN-22, RN-24 e RN-26, os critérios CA-05, CA-06, CA-13, CA-20, CA-25, CA-32, CA-35, CA-36, CA-39, CA-40 e CA-42, e as mensagens MSG-11 e MSG-13 a MSG-30.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
No plano, liste os testes: um por critério de aceite e os casos de borda da seção 11 do arquitetura.md.
```

**Confira no plano:** cada critério de aceite da lista tem um teste; cada teste de erro espera a mensagem do catálogo; nada de banco nem de API nesta tarefa. Depois, use o A5.

### T-05 — Banco de dados do cadastro

Antes de começar: modelo de dados revisado (`arquitetura.md`, seção 6).

```
CONTEXTO: Projeto Timeline de Entregas, etapa 1 (APIs de cadastro). Tarefa T-05 do docs/tarefas.md: Banco de dados do cadastro.
Referência: arquitetura.md, seção 6 (a tabela do congelado fica para a T-18).
Pontos de atenção:
- Colunas nome_comparacao e rotulo_comparacao geradas pelo banco, com as unicidades sobre elas (ADR-23).
- Todas as restrições CHECK da seção 6, inclusive a da prorrogação sem as datas do ciclo.
- Schema timeline, inclusive para o histórico de migrations.
OBJETIVO: Criar o DbContext, as tabelas aplicacao e versao com as restrições, e a migration inicial, aplicada ao iniciar a API.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Ao final, mostre como eu abro o banco e listo tabelas, colunas e restrições, para conferir contra a seção 6.
```

### T-06 — API de aplicações (padrão de referência)

```
CONTEXTO: Projeto Timeline de Entregas, etapa 1 (APIs de cadastro). Tarefa T-06 do docs/tarefas.md: API de aplicações.
Esta tarefa é o padrão de referência das próximas APIs: as tarefas seguintes vão pedir "siga o padrão da T-06".
OBJETIVO: Criar os endpoints de listar, cadastrar, renomear e excluir aplicações (RF-01, RN-01, RN-04), atendendo os critérios CA-33 e CA-38 (parte do cadastro), com as mensagens MSG-01 a MSG-04 e o tratamento do erro inesperado (MSG-41). Publicar já nesta tarefa a página de documentação da API (Scalar, ADR-30), porque eu não uso o VS Code e vou fazer as verificações pelo navegador. Criar o http/exemplos.http com as requisições desta API, na ordem de uso, para o README e para quem avaliar.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Ao final, explique em linguagem simples como a API está organizada (endpoint, caso de uso, validação, repositório, tratamento de erros).
```

**Confira no plano:** nome duplicado devolve 409 com a MSG-03; aplicação inexistente devolve 404; erro inesperado não mostra detalhes internos.

### T-07 — API de versões

```
CONTEXTO: Projeto Timeline de Entregas, etapa 1 (APIs de cadastro). Tarefa T-07 do docs/tarefas.md: API de versões.
Referência de padrão: siga o padrão da T-06 (API de aplicações). As sete datas são gravadas juntas por PUT /api/versoes/{id}/datas (ADR-12).
OBJETIVO: Criar os endpoints de cadastrar, detalhar, alterar, mudar a fase, gravar as datas e excluir versões (RF-03 a RF-05, RF-09, RF-10, RF-14), atendendo o critério CA-34, com as mensagens MSG-05 a MSG-12 e MSG-26. Incluir no http/exemplos.http os cenários do CA-39 e do CA-40.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Se precisar fugir do padrão da T-06 em algum ponto, explique o motivo no plano.
```

### T-08 — Docker da API, fuso horário e dados de exemplo

Antes de começar: dados de exemplo aprovados e registrados na seção 12.1 do `arquitetura.md`.

```
CONTEXTO: Projeto Timeline de Entregas, etapa 1 (APIs de cadastro), última tarefa da etapa. Tarefa T-08 do docs/tarefas.md: Docker da API, fuso horário e dados de exemplo.
Referências: arquitetura.md, seções 5.6 (fuso) e 12.1 (dados de exemplo), ADR-25 e ADR-30. Depois desta tarefa, farei a revisão independente antes da tag v0.1.0.
OBJETIVO: Criar o Dockerfile e o serviço api no docker-compose.yml, com o fuso pela variável TZ; publicar a página de documentação da API também no Docker; carregar os dados de exemplo exatamente como no conjunto da seção 12.1, com as datas calculadas a partir do dia da carga; e criar um teste que simula a carga em cada dia de um ano e confere que todas as versões respeitam as regras e que pelo menos uma versão atravessa o dia da carga.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

Depois: A13, A14 e a tag `v0.1.0`.

## Etapa 2 — Telas de cadastro (T-09 e T-10)

Comece a etapa com o A15.

### T-09 — Projeto Angular

```
CONTEXTO: Projeto Timeline de Entregas, etapa 2 (telas de cadastro). Tarefa T-09 do docs/tarefas.md: Projeto Angular.
Referências: arquitetura.md, seções 8 e 12; frontend/CLAUDE.md.
Pontos de atenção:
- Dependências só as da seção 12.3 (Angular, Angular Material, angular-eslint), pelo npm, sem escrever versões.
- Repasse de /api pelo proxy.conf.json em desenvolvimento e pelo servidor das telas no Docker (ADR-21).
OBJETIVO: Criar o projeto Angular com Angular Material, locale pt-BR, tema com as cores dos grupos visuais, testes e lint; incluir o serviço web no docker-compose.yml; e fazer os scripts test e lint incluírem o frontend, em container.
ESTILO: Siga as convenções do CLAUDE.md e do frontend/CLAUDE.md, com os termos do glossário nas telas e as mensagens exibidas exatamente como vêm da API.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

### T-10 — Lista de aplicações e tela da versão (padrão das telas)

```
CONTEXTO: Projeto Timeline de Entregas, etapa 2 (telas de cadastro), última tarefa da etapa. Tarefa T-10 do docs/tarefas.md: Lista de aplicações e tela da versão.
Esta tarefa é o padrão de referência das próximas telas. Pontos de atenção:
- As sete datas num único formulário, salvo de uma vez.
- Confirmação antes de excluir.
OBJETIVO: Criar a lista de aplicações (cadastro, edição e exclusão) e a tela da versão (rótulo, fase, situação, observação e as sete datas), atendendo RF-01, RF-03 a RF-05, RF-09, RF-10, RF-14 e RN-04.
ESTILO: Siga as convenções do CLAUDE.md e do frontend/CLAUDE.md, com os termos do glossário nas telas e as mensagens exibidas exatamente como vêm da API.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Antes do plano, descreva cada tela em palavras (campos, botões e mensagens) para eu aprovar o layout. Ao final, explique em linguagem simples como as telas estão organizadas (componentes, serviços de acesso à API, exibição de mensagens).
```

**Confira no plano:** a descrição das telas corresponde ao que você usaria no dia a dia; os nomes dos campos são os do glossário (ex.: "Início do andamento", "Fim da prorrogação").

Depois: A13, A14 e a tag `v0.2.0`.

## Etapa 3 — API da timeline (T-11 a T-13)

Comece a etapa com o A15.

### T-11 — Cálculo da timeline (TDD, em duas partes)

É a regra mais complexa do projeto. Por isso, o trabalho é dividido em duas partes, com uma conferência sua entre elas.

```
CONTEXTO: Projeto Timeline de Entregas, etapa 3 (API da timeline). Tarefa T-11 do docs/tarefas.md: Cálculo da timeline (TDD).
Referências: arquitetura.md, seções 5.4, 5.5 e 11.
Pontos de atenção:
- Convenção das datas no JSON (seção 5.5, ADR-27): no CA-02, com data atual 15/10/2026, o vermelho forte vai de 2026-10-13 a 2026-10-16.
- Casos de borda das seções 5.5 e 11, inclusive etapa de um dia e intervalo de duração zero.
- A CalculadoraTimeline é uma função pura: sem banco, sem relógio, sem rede.
OBJETIVO: Criar a CalculadoraTimeline, em TDD, garantindo as regras RN-03, RN-06, RN-13 a RN-16, RN-18 a RN-20 e RN-25, os critérios CA-02, CA-03, CA-04, CA-07, CA-08, CA-09, CA-15, CA-22, CA-23, CA-27, CA-28, CA-29 e CA-37, e as mensagens MSG-33 e MSG-34. Acrescentar ao teste dos dados de exemplo a verificação de que a timeline do semestre atual nunca fica vazia.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Divida o trabalho em duas partes, cada uma com o seu ciclo de testes:
1. Trechos de uma versão: etapas, intervalos, marco, realizado e previsto, Produção e Backlog (CA-02, CA-03, CA-07, CA-08, CA-09, CA-22, CA-37).
2. Timeline completa: ordem, coluna "Sem previsão", intervalo de meses, cortes e mensagens (CA-04, CA-15, CA-23, CA-27, CA-28, CA-29).
Ao final da parte 1, pare e me mostre o resultado do CA-02 e do CA-07 ao lado do texto dos critérios.
```

**Confira na parada da parte 1:** cada trecho do CA-02 (azul forte de 01/09 a 25/09, pontilhado até 01/10, roxo forte até 13/10, vermelho forte até o dia 15/10, inclusive, vermelho claro até 23/10, pontilhado até 28/10, marco claro em 28/10) aparece no resultado. Se bater:

```
OBJETIVO: Seguir para a parte 2; a parte 1 está conferida.
RESPOSTA: Implemente a parte 2 conforme o plano.
```

### T-12 — API da timeline

```
CONTEXTO: Projeto Timeline de Entregas, etapa 3 (API da timeline). Tarefa T-12 do docs/tarefas.md: API da timeline.
Referência de padrão: siga o padrão da T-06.
OBJETIVO: Criar o endpoint GET /api/timeline (RF-11, RF-12), com o semestre atual como padrão quando não houver parâmetros, e as mensagens MSG-31 e MSG-32. Incluir exemplos no http/exemplos.http, inclusive um intervalo inválido.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

### T-13 — Refatoração e revisão do backend

```
CONTEXTO: Projeto Timeline de Entregas, etapa 3 (API da timeline), última tarefa da etapa. Tarefa T-13 do docs/tarefas.md: Refatoração e revisão do backend.
Referências: processo, seção 9.1 (refatoração); arquitetura.md, seção 5.1 (glossário). Nenhum teste pode ser alterado.
OBJETIVO: Melhorar a clareza do backend: nomes alinhados ao glossário, sem duplicação entre as APIs, responsabilidade única, sem mudar comportamento.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
No lugar do plano comum, a análise de impacto: o que propõe mudar e por quê, em quais arquivos e quais testes cobrem cada mudança.
```

Depois: A13, A14 e a tag `v0.3.0`.

## Etapas 4 a 6 — Tela da timeline e tabela de status das aplicações (T-14 a T-16)

Comece cada etapa com o A15.

### T-14 — Tela da timeline (em quatro partes)

É a tarefa mais longa (5 h). As paradas permitem corrigir o desenho cedo, com os dados de exemplo na tela.

```
CONTEXTO: Projeto Timeline de Entregas, etapa 4 (tela da timeline). Tarefa T-14 do docs/tarefas.md: Componente de timeline.
Referências: arquitetura.md, seção 8; frontend/CLAUDE.md. Referência de padrão: telas da T-10.
Pontos de atenção:
- O componente só desenha o que vem da API; nenhuma regra de negócio na tela.
- SVG próprio, sem bibliotecas de gráfico de Gantt.
OBJETIVO: Criar a tela do status report com a timeline desenhada, atendendo as regras de desenho RN-13 a RN-20 e RN-25, o RF-12, o critério CA-31 e o RNF-04 (legível em compartilhamento de tela).
ESTILO: Siga as convenções do CLAUDE.md e do frontend/CLAUDE.md, com os termos do glossário nas telas e as mensagens exibidas exatamente como vêm da API.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Divida o trabalho em quatro partes e pare ao final de cada uma para eu conferir na tela, com os dados de exemplo:
1. Eixo de meses, linhas das aplicações e linha da data atual.
2. Trechos, intervalos pontilhados e marco, com cores e tons.
3. Sub-linhas para versões sobrepostas (CA-31), rótulos dentro das barras e coluna "Sem previsão".
4. Seletor do intervalo de meses e mensagens de timeline vazia.
```

Em cada parada, se estiver certo:

```
OBJETIVO: Seguir para a parte <N+1>; a parte <N> está conferida.
RESPOSTA: Implemente a parte <N+1> conforme o plano.
```

Se algo estiver diferente:

```
OBJETIVO: Corrigir a parte <N>: eu vejo <o que aparece> e esperava <o que deveria aparecer>, pelo <critério> (ex.: "a v2 da Alfa está na mesma sub-linha da v1, mas pelo CA-31 deveria estar abaixo").
RESPOSTA: A causa em linguagem simples e a correção, sem seguir para a próxima parte antes da minha conferência.
```

Depois: A13, A14 e a tag `v0.4.0`.

### T-15 — API da tabela de status das aplicações (TDD)

```
CONTEXTO: Projeto Timeline de Entregas, etapa 5 (API da tabela de status das aplicações). Tarefa T-15 do docs/tarefas.md: Tabela de status das aplicações e contadores na API (TDD).
Referências de padrão: o cálculo segue o modelo da CalculadoraTimeline (T-11); o endpoint segue o padrão da T-06.
Ponto de atenção: o intervalo de meses não afeta a tabela nem os contadores (RN-20).
OBJETIVO: Criar a CalculadoraTabelaStatus e o endpoint GET /api/tabela-status, em TDD, atendendo RN-01, RN-03, RN-13, RN-21 a RN-23 e RF-13, os critérios CA-14, CA-21 e CA-24, e a mensagem MSG-35.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

Depois: A13, A14 e a tag `v0.5.0`.

### T-16 — Tela da tabela de status das aplicações

```
CONTEXTO: Projeto Timeline de Entregas, etapa 6 (tela da tabela de status das aplicações). Tarefa T-16 do docs/tarefas.md: Tabela de status das aplicações na tela.
Referência de padrão: telas da T-10. A tela do status report já tem a timeline (T-14).
OBJETIVO: Acrescentar à tela do status report, abaixo da timeline, a tabela de status das aplicações, com os contadores acima dela (RF-13).
ESTILO: Siga as convenções do CLAUDE.md e do frontend/CLAUDE.md, com os termos do glossário nas telas e as mensagens exibidas exatamente como vêm da API.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

Depois: A13, A14 e a tag `v0.6.0`.

## Etapa 7 — Congelamento (T-17 a T-19)

Comece a etapa com o A15. Pelas estimativas, ela fica depois do corte das 26 h e deve ser feita só se o tempo real permitir.

### T-17 — Semanas e data de referência (TDD)

```
CONTEXTO: Projeto Timeline de Entregas, etapa 7 (congelamento). Tarefa T-17 do docs/tarefas.md: Semanas e data de referência (TDD).
Referência: arquitetura.md, seção 5.7 (classe ISOWeek, formato 2026-W42, terça-feira de referência).
OBJETIVO: Calcular a semana ISO e a data de referência (RN-27, RN-28), em TDD, com os casos de borda obrigatórios: 01/01/2027 pertence à Semana 53/2026, com referência em 29/12/2026; congelamento numa segunda-feira, com referência no dia seguinte.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

### T-18 — Congelamento e Histórico de Status Reports na API

```
CONTEXTO: Projeto Timeline de Entregas, etapa 7 (congelamento). Tarefa T-18 do docs/tarefas.md: Congelamento e Histórico de Status Reports na API.
Referências: arquitetura.md, seções 5.7, 6 (tabela status_report_congelado) e 7.2; ADR-11 e ADR-28. Referência de padrão: T-06.
Pontos de atenção:
- O congelado guarda o resultado calculado e nunca recalcula ao ser consultado.
- Recongelar, a semana atual ou uma anterior, mantém a terça-feira e o intervalo de meses originais.
- Cada semana do histórico informa se pode ser aberta, congelada ou congelada novamente.
OBJETIVO: Criar a tabela do congelado e os endpoints de histórico e de congelamento (RF-15 a RF-17, RN-27 a RN-30), atendendo os critérios CA-10, CA-11, CA-12, CA-26, CA-30, CA-38 (parte do congelado) e CA-41, com as mensagens MSG-36 a MSG-40.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

**Confira no plano:** os testes do CA-10 e do CA-26 usam datas fixas diferentes para o congelamento e para a consulta, como nos critérios (ex.: congelado em 09/10/2026, consultado depois da alteração da data de entrega).

### T-19 — Telas do Histórico de Status Reports e do congelado

```
CONTEXTO: Projeto Timeline de Entregas, etapa 7 (congelamento), última tarefa da etapa. Tarefa T-19 do docs/tarefas.md: Telas do Histórico de Status Reports e do congelado.
Referência de padrão: telas da T-10. O congelado reaproveita a timeline da T-14 e a tabela da T-16, somente leitura.
Ponto de atenção: as ações aparecem conforme a API informa.
OBJETIVO: Criar a tela do Histórico de Status Reports, com as ações "Abrir", "Congelar" e "Congelar novamente", e a visualização somente leitura do congelado, com confirmação antes de substituir (RN-28 a RN-30).
ESTILO: Siga as convenções do CLAUDE.md e do frontend/CLAUDE.md, com os termos do glossário nas telas e as mensagens exibidas exatamente como vêm da API.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

Depois: A13, A14 e a tag `v0.7.0`.

## Entrega (T-20 a T-22) e incremento

A entrega nunca é cortada. Comece por ela quando o tempo real chegar a 26 h, mesmo com etapas pendentes.

### T-20 — Exemplos de uso

```
CONTEXTO: Projeto Timeline de Entregas, entrega. Tarefa T-20 do docs/tarefas.md: Exemplos de uso.
O http/exemplos.http foi crescendo a cada tarefa.
OBJETIVO: Revisar o http/exemplos.http para cobrir todos os endpoints construídos e executá-lo do começo ao fim, mostrando o resultado de cada requisição, na ordem de um fluxo de uso, com comentários em português explicando cada passo, e incluir os exemplos curl correspondentes na seção de uso do README.
ESTILO: Siga as convenções do CLAUDE.md.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

### T-21 — README completo e registro do uso de IA

Antes de começar: diário em dia, com o resumo acumulado atualizado.

```
CONTEXTO: Projeto Timeline de Entregas, entrega. Tarefa T-21 do docs/tarefas.md: README completo e registro do uso de IA.
Fontes: os documentos do projeto, o docs/diario.md e a Parte 2 do prompts/prompts-claude-code.md.
OBJETIVO: Preencher todas as seções do README. A seção sobre o uso de IA traz exemplos reais em cada uma das quatro subseções e o tempo real comparado à estimativa sem IA. Em "Limitações e próximos passos", listar as etapas não concluídas, as evoluções da seção 12 do requisitos-mvp.md e as limitações conhecidas (uso local, sem autenticação, exclusão definitiva, licença do Docker Desktop em empresas de maior porte).
ESTILO: Siga as convenções do CLAUDE.md. Não invente números nem exemplos: use só o que está registrado.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Diga também quais capturas de tela ou GIF preciso gravar, com os dados de exemplo, e onde salvar.
```

**Confira no resultado:** você lê o README inteiro como se fosse alguém de fora. Cada número da seção sobre IA tem que estar no diário.

### T-22 — Release 1.0

Antes de começar: revisão independente final (A13 e A14).

```
CONTEXTO: Projeto Timeline de Entregas, entrega, última tarefa. Tarefa T-22 do docs/tarefas.md: Release 1.0.
O checklist de release está na seção 14 do processo. Para o item C-11, vou informar nesta conversa os termos a procurar no histórico; não escreva esses termos em nenhum arquivo.
OBJETIVO: Verificar o checklist de release item por item, corrigir as pendências e testar a instalação limpa (C-10) seguindo só o README.
ESTILO: Siga as convenções do CLAUDE.md.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
Primeiro, a pré-verificação automática dos itens objetivos, com um relatório item por item: OK, pendente ou depende de mim. Na instalação limpa, me guie sem ajudar além do que o README diz: quero testar o README como alguém de fora.
```

Com o checklist completo:

```
OBJETIVO: Publicar a versão 1.0; o checklist está conferido.
RESPOSTA: Crie a tag v1.0.0 e me guie para tornar o repositório público e criar a release v1.0.0 no GitHub.
```

### I-01 — Testes de integração (se houver tempo)

```
CONTEXTO: Projeto Timeline de Entregas, incremento, se houver tempo. Tarefa I-01 do docs/tarefas.md: Testes de integração.
Ponto de atenção: dependência fora da lista da seção 12.3 do arquitetura.md precisa da minha aprovação, com a explicação de para que serve e se é gratuita.
OBJETIVO: Criar testes dos endpoints principais com o PostgreSQL real, rodando pelo script test.
ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
```

---

# Parte 2 — Registro

Pedidos realmente enviados que se encaixam nos casos da seção 13.2 do processo:

- o primeiro pedido de um padrão que depois serve de referência (ex.: a API de aplicações, na T-06);
- um pedido que não funcionou de primeira, junto com a versão melhorada;
- um pedido sobre uma regra complexa (ex.: o cálculo da timeline);
- um pedido que evitou um risco (ex.: o assistente parou e perguntou diante de uma contradição);
- um pedido de revisão independente.

Modelo de cada entrada:

```markdown
### P-NN — dd/mm/aaaa — T-XX

**Pedido (na íntegra):**
> texto exatamente como foi enviado

**Resultado:** o que o assistente entregou.

**Funcionou de primeira?** Sim ou não. Se não, o que mudou no pedido seguinte (P-NN).

**Por que é relevante:** o que esse pedido ensina sobre como pedir.
```

## Entradas

Pedidos da conversa de planejamento (Sessão 00 do diário), na ordem em que foram feitos. Respostas dadas por botões aparecem no resultado do pedido que as gerou. Mensagens sem pedido não entram.

### P-01 — 15/10/2026 — Planejamento: adaptação do processo

**Pedido (na íntegra):**
> Estou continuando um projeto. Comece lendo o resumo-sessao.md e depois todos os outros documentos anexados. O resumo explica o projeto, como devemos trabalhar juntas (eu não sou técnica: dê contexto e exemplos em toda mensagem, e as decisões técnicas ficam com você), o que já foi decidido e os próximos passos. Quando terminar, confirme que entendeu e siga para o próximo passo da seção 6: adaptar o processo-desenvolvimento.md.

**Resultado:** leitura dos onze documentos e `processo-desenvolvimento.md` versão 0.2: papéis da autora e do assistente; revisão pelo comportamento, com o checklist R-01 a R-05 dividido entre o que o assistente verifica e o que a autora confere; formato do plano; revisão independente; termos técnicos explicados.

**Funcionou de primeira?** Sim, com uma ressalva: a seção 13.3 citava o contexto externo do projeto, o que não pode aparecer no repositório. O próprio assistente encontrou e corrigiu o problema na conferência feita durante o P-03.

**Por que é relevante:** o contexto ficou num documento de passagem, e o pedido só apontou para ele, com o objetivo e o jeito de trabalhar. Assim, o trabalho continuou de outra conversa sem repetir explicações.

### P-02 — 15/10/2026 — Planejamento: nova estratégia de entrega

**Pedido (na íntegra):**
> o tarefas.md eu quero mudar a estrategia
>
> primeiro vamos fazer todas as apis de cadastro
> segundo, fazemos as telas de cadastro se der tempo
> fazemos a api da timeline, se der tempo
> fazemos a tela da timeline, se der tempo
> congelamos a timeline, se der tempo
> fazemos a api da lista de aplicações, se der tempo
> fazemos a tela da lista de aplicações (tabela de status das aplicações), se der tempo
>
> ok?

**Resultado:** análise antes de qualquer mudança, com as estimativas acumuladas. Dois pontos foram levantados: o congelamento guarda a tabela de status das aplicações (RN-27), então não pode vir antes dela; e o atraso de uma etapa recairia sobre a timeline. As decisões ficaram para a autora, em botões.

**Funcionou de primeira?** Sim, como análise. A decisão final veio no P-05: a autora manteve a sua ordem e aceitou só o ajuste da tabela de status.

**Por que é relevante:** perguntar "ok?" em vez de mandar aplicar levou a uma análise antes da mudança. Foi assim que apareceu uma dependência de regra (RN-27) que a ordem proposta quebraria.

### P-03 — 15/10/2026 — Planejamento: revisão de arquitetura

**Pedido (na íntegra):**
> agora eu quero que como um arquiteto de software senior com mais de 10 anos de experiência, você analise todos os documentos
>
> a arquitetura está de acordo com o documento de negócios?
> a arquitetura tem gaps, inconsistências, incoerencias, contradições?
> o processo de desenvolvimento e as tarefas estão aptas para começar o desenvolvimento?
>
> Está faltando alguma coisa ser definida ou já podemos executar com cloude o desenvolvimento?

**Resultado:** revisão com doze pontos técnicos (A-01 a A-12), sete perguntas de negócio (N-01 a N-07) e quatro ajustes nas tarefas. O ponto mais grave foi uma contradição no exemplo do CA-02: o dia atual ficaria fora do realizado (RN-25), e o teste nasceria errado. A autora decidiu por botões: avanço direto de fase (N-01); nomes iguais sem diferença de maiúsculas (N-02); correção da prorrogação em Produção (N-03); catálogo de mensagens (N-04); Histórico sem congelados (N-05); limites de tamanho (N-06). Resultado nos documentos: requisitos 0.6 e 0.68 (CA-40 a CA-42), `arquitetura.md` 0.5 com o catálogo de 41 mensagens, `tarefas.md` 0.4, processo 0.3 e os três `CLAUDE.md` revisados.

**Funcionou de primeira?** Sim.

**Por que é relevante:** dar um papel ao assistente e fazer perguntas objetivas produziu uma revisão completa antes de qualquer código, quando corrigir custa pouco.

### P-04 — 15/10/2026 — Planejamento: ajuste da ordem das etapas

**Pedido (na íntegra):**
> tarefas.md
>
> adeque para isso
>
> * fazer a API da tabela de status antes do congelamento. Ela é pequena, cerca de 1,5 h;
>
> Ponto 2: o risco de atraso cai sobre a timeline.
> nao tem problema, o primeiro objetivo é entregar o trabalho, depois posso evoluir para o que realmente fará diferença no dia a dia

**Resultado:** `tarefas.md` 0.5, com a ordem da autora (APIs de cadastro, telas de cadastro, API e tela da timeline, API e tela da tabela de status, congelamento). Processo 0.4 e `arquitetura.md` 0.6 foram alinhados.

**Funcionou de primeira?** Sim.

**Por que é relevante:** citar trechos da resposta anterior e explicar o motivo da decisão ("o primeiro objetivo é entregar") deixou claro o que mudar e o que manter.

### P-05 — 15/10/2026 — Planejamento: dados de exemplo

**Pedido (na íntegra):**
> A-04 — Dados de exemplo envelhecem. Se os dados de exemplo tiverem datas fixas de 2026 e alguém rodar o sistema em março de 2027, a timeline do semestre atual mostra "Nenhuma versão no período selecionado". A primeira impressão seria uma tela vazia. Correção: as datas de exemplo são calculadas a partir do dia da carga (ex.: a v1 da Alfa começa 30 dias antes de hoje). Antes da carga de exemplo.
>
> o que voce vai fazer aqui?

**Resultado:** proposta concreta do conjunto de dados de exemplo (Alfa, Beta, Gama e Delta, com sete situações diferentes), com as datas calculadas para dois dias diferentes e um teste que simula a carga em cada dia de um ano. A proposta aguarda a aprovação da autora.

**Funcionou de primeira?** Sim.

**Por que é relevante:** perguntar "o que você vai fazer" sobre um item técnico transformou uma correção abstrata numa proposta que a autora consegue conferir.

### P-06 — 15/10/2026 — Planejamento: aprovação das dependências

**Pedido (na íntegra):**
> Proposta: listar essas dependências no `arquitetura.md`, com a finalidade e a licença de cada uma, sem números de versão, para você aprovar de uma vez. Antes do esqueleto.
>
> sim

**Resultado:** dependências previstas marcadas como aprovadas (`arquitetura.md` 0.7). Na conferência, o assistente encontrou um erro próprio e o corrigiu: o catálogo de mensagens aparecia como aprovado antes da revisão da autora.

**Funcionou de primeira?** Sim.

**Por que é relevante:** aprovar a lista de uma vez evita que o desenvolvimento pare a cada pacote e impede versões inventadas pela IA.

### P-07 — 15/10/2026 — Planejamento: arquivos de configuração

**Pedido (na íntegra):**
> A-07 — Faltam dois arquivos de configuração. São dois arquivos, ambos necessários antes do primeiro commit (T-02):
>
> o que eu faço?

**Resultado:** explicação dos dois arquivos com exemplos, e a verificação da T-02 passou a incluí-los (`tarefas.md` 0.6).

**Funcionou de primeira?** Sim.

**Por que é relevante:** a pergunta "o que eu faço?" separou o que é trabalho do assistente do que a autora confere.

### P-08 — 15/10/2026 — Planejamento: conferência antes de começar

**Pedido (na íntegra):**
> falta mais alguma coisa para eu mudar para o mode de codar com o cloude?

**Resultado:** a revisão do caminho até a primeira sessão encontrou uma lacuna. Os arquivos de regras seriam renomeados só na T-02, e o Claude Code faria essa tarefa sem conhecer as regras do projeto. A montagem da pasta, com os nomes definitivos, passou para a T-01 (`tarefas.md` 0.7).

**Funcionou de primeira?** Sim.

**Por que é relevante:** uma pergunta aberta de conferência final revelou um problema que nenhuma revisão por documento tinha mostrado.

### P-09 — 15/10/2026 — Planejamento: criação do repositório no GitHub

**Pedido (na íntegra):**
> me ajude a criar o git

(Com uma captura de tela do formulário de criação de repositório do GitHub.)

**Resultado:** orientação campo a campo. O ponto principal foi trocar a visibilidade de pública para privada e não criar README, `.gitignore` nem licença pelo GitHub, para evitar conflito no primeiro envio.

**Funcionou de primeira?** Sim.

**Por que é relevante:** a captura de tela deu o contexto exato e evitou um risco: o formulário vinha marcado como público.

### P-10 — 15/10/2026 — Planejamento: confirmação do repositório

**Pedido (na íntegra):**
> https://github.com/priscilabertolazzi/timeline-entregas.git

**Resultado:** o assistente não conseguiu confirmar a visibilidade a partir do seu ambiente, disse isso e passou dois testes para a autora fazer (a etiqueta "Private" e a janela anônima).

**Funcionou de primeira?** Parcialmente: a conferência automática não foi possível, e a verificação ficou com a autora.

**Por que é relevante:** mostra o assistente declarando o que não conseguiu verificar, em vez de presumir.

### P-11 — 15/10/2026 — Planejamento: roteiro de prompts

**Pedido (na íntegra, com um trecho omitido):**
> prepare prompts para eu executar as tarefas do projeto da melhor forma com o cloude (seguindo as recomendações/orientações [trecho omitido])

O trecho omitido cita o contexto externo do projeto, que não pode aparecer no repositório.

**Resultado:** roteiro com os prompts do ciclo (A1 a A16) e os de cada tarefa, aplicando contexto estruturado, plano antes do código, exemplos de referência, iteração em partes e testes antes do código.

**Funcionou de primeira?** Parcialmente: o roteiro foi criado num documento à parte, e a autora pediu que ficasse no arquivo de prompts do projeto (P-12). Além disso, não mostrava o modelo CO-STAR de forma explícita, o que a autora apontou depois (P-17).

**Por que é relevante:** o roteiro reduz cada pedido de tarefa a poucas linhas, porque o contexto está nos documentos.

### P-12 — 15/10/2026 — Planejamento: roteiro no arquivo do projeto

**Pedido (na íntegra):**
> tem um doc de prompts né? salva lá

**Resultado:** este arquivo, com o roteiro (Parte 1) e o registro (Parte 2). O `tarefas.md` (0.8), o processo (0.5), o `arquitetura.md` (0.8) e o `CLAUDE.md` da raiz foram alinhados.

**Funcionou de primeira?** Sim.

**Por que é relevante:** manter o roteiro e o registro no mesmo arquivo deixa visível a diferença entre o que foi planejado e o que foi enviado.

### P-13 — 15/10/2026 — Planejamento: o que falta para começar

**Pedido (na íntegra):**
> e volta a pergunta: o que falta para eu começar a codar com o cloude?

**Resultado:** lista objetiva: só a T-01 impede o início, com a tabela das versões dos arquivos para a pasta do projeto. As revisões pendentes vêm com a tarefa antes da qual precisam estar prontas.

**Funcionou de primeira?** Sim.

**Por que é relevante:** repetir a pergunta depois de várias mudanças obrigou uma conferência atualizada.

### P-14 — 15/10/2026 — Planejamento: registros da sessão

**Pedido (na íntegra):**
> atualize o resumo da sessão
>
> faça isso aqui que voce sugeriu também
>
> Uma sugestão para o diário: registre uma "Sessão 00 — Planejamento" com o trabalho feito aqui. Há bons exemplos para a seção do README sobre o uso de IA:
>
> * Onde a IA ajudou: a revisão de arquitetura encontrou a contradição do CA-02, que viraria um teste errado.
> * Onde a decisão humana foi decisiva: a ordem dos degraus e as regras N-01 a N-05 foram suas.
>
>
> e no doc prompts, faça tudo desde o zero

**Resultado:** este registro, desde o primeiro pedido; a Sessão 00 do diário; o resumo da sessão atualizado (documento de passagem, fora do repositório); e a correção das datas dos históricos para 15/10/2026.

**Funcionou de primeira?** Sim.

**Por que é relevante:** registrar o planejamento desde o início dá ao README exemplos reais também da fase de requisitos e arquitetura, não só da programação.

### P-15 — 15/10/2026 — Planejamento: todos os documentos para baixar

**Pedido (na íntegra):**
> no final de tudo, traga TODOS os documentos atualizados aqui no chat para eu baixar

**Resultado:** os doze documentos entregues juntos, com a versão de cada um conferida e uma tabela indicando o destino de cada arquivo na pasta do projeto. Ao comparar com a própria pasta, a autora confirmou a lista e notou o arquivo novo, que não existia antes.

**Funcionou de primeira?** Sim.

**Por que é relevante:** pedir tudo num só lugar, no fim, evita misturar versões antigas e novas na pasta do projeto.

### P-16 — 15/10/2026 — Planejamento: visualizar Markdown no VS Code

**Pedido (na íntegra):**
> qual o comando no vs code para deixar um md bonito?

**Resultado:** atalhos da visualização formatada e a extensão gratuita que desenha os diagramas Mermaid.

**Funcionou de primeira?** Sim.

**Por que é relevante:** mostra a IA apoiando a autora também no uso das ferramentas, não só no projeto.

### P-17 — 15/10/2026 — Planejamento: prompts no modelo CO-STAR

**Pedido (na íntegra):**
> ah, os prompts tem que estar no model CO-STAR

**Resultado:** o roteiro (Parte 1 deste arquivo) foi reescrito com os seis elementos explícitos em cada prompt de tarefa e do ciclo. Os prompts de continuação trazem só Objetivo e Resposta. O modelo de pedido do processo (seção 6) foi alinhado.

**Funcionou de primeira?** Sim.

**Por que é relevante:** a autora identificou que o modelo CO-STAR, usado de forma distribuída nos documentos, não aparecia nos prompts. Com os elementos explícitos, cada pedido mostra o modelo e continua curto, porque os elementos permanentes apontam para os documentos.

### P-18 — 15/10/2026 — T-03

**Pedido (na íntegra):**
> CONTEXTO: Projeto Timeline de Entregas, preparação. Tarefa T-03 do docs/tarefas.md: Esqueleto da solução, banco e scripts.
> Referências: arquitetura.md, seções 4 (estrutura), 9 (configuração), 12.2 (scripts) e 12.3 (dependências previstas, já aprovadas).
> Pontos de atenção:
> - Dependências só da lista da seção 12.3, adicionadas pelas ferramentas oficiais, sem escrever versões.
> - Os scripts test e lint rodam em container (ADR-24).
> - Os scripts .sh precisam funcionar no Linux e no macOS.
> OBJETIVO: Criar o global.json, a solução e os projetos do módulo Timeline com as referências entre camadas, o docker-compose.yml com o banco, os scripts install, run, test e lint (.ps1 e .sh) e o Makefile, e atualizar o README com pré-requisitos, instalação e comandos.
> ESTILO: Siga as convenções do CLAUDE.md e do backend/CLAUDE.md, com os nomes do glossário e as mensagens do catálogo (arquitetura.md, seção 7.6) com o texto exato.
> TOM: Comigo, português simples, sem jargão, com exemplos dos dados fictícios (Alfa, Beta, Gama, Delta).
> AUDIÊNCIA: Eu, analista de requisitos não técnica, que confiro o resultado pelo comportamento e pelos critérios de aceite.
> RESPOSTA: Primeiro o plano, no formato da seção 4.1 do processo, sem alterar nenhum arquivo. Depois da minha aprovação: implementação, relatório de verificação (build, testes e lint) e o passo a passo da "Sua verificação".
> Confirme que os scripts test e lint funcionam sem depender do SDK instalado.

**Resultado:** o assistente apresentou o plano sem alterar arquivos e perguntou sobre o pacote `Microsoft.AspNetCore.OpenApi`, que o Scalar precisa e que não estava na seção 12.3. Depois da aprovação, criou o esqueleto do backend, o banco no Docker Compose e os oito scripts. A frase final do pedido virou um experimento: `test.ps1` e `lint.ps1` rodaram com o `dotnet` fora do caminho do terminal e passaram.

**Funcionou de primeira?** O plano, sim. A implementação precisou de três ajustes: a porta 5432 já ocupada por outro PostgreSQL, a quebra de linha do Windows que reprovava o lint numa cópia limpa e os acentos dos `.ps1` (ver Sessão 03 do diário). Nenhum deles mudou o pedido.

**Por que é relevante:** evitou um risco: o assistente parou e pediu aprovação em vez de adicionar um pacote fora da lista. A última frase ("Confirme que... sem depender do SDK instalado") transformou uma promessa do documento em algo provado por experimento, e não só afirmado.

### P-19 — 15/10/2026 — T-03

**Pedido (na íntegra):**
> OBJETIVO: Responder às perguntas e aprovar o plano.
> 1. Aprovado o pacote Microsoft.AspNetCore.OpenApi. Inclua-o na lista de dependências previstas da seção 12.3 do arquitetura.md.
> 2. Crie o teste de estrutura, sem nenhuma dependência nova, conferindo que o Domain não depende de nenhuma outra camada. Ele deve rodar pelo scripts\test.ps1.
> RESPOSTA: As decisões técnicas estão de acordo; registre-as no arquitetura.md. Pode criar o branch e implementar.

**Resultado:** pacote adicionado e registrado na seção 12.3; dois testes de estrutura no `Domain.Tests`; decisões técnicas registradas no `arquitetura.md` (ADR-31 e ADR-32).

**Funcionou de primeira?** Sim.

**Por que é relevante:** a autora respondeu às perguntas do plano com decisões numeradas e já disse onde registrá-las. Sem o teste de estrutura, a tarefa terminaria com "0 testes", sem prova de que o caminho de testes funciona de ponta a ponta.

---

## Histórico do documento

| Versão | Data | Descrição |
|---|---|---|
| 0.1 | 15/10/2026 | Versão inicial: roteiro de prompts por tarefa (prompts do ciclo A1 a A16 e prompts da T-01 à T-22 e do I-01) e modelo do registro dos pedidos. |
| 0.2 | 15/10/2026 | Registro dos pedidos da conversa de planejamento, desde o primeiro (P-01 a P-14). |
| 0.3 | 15/10/2026 | Roteiro reescrito no modelo CO-STAR explícito; registro dos pedidos P-15 a P-17. |
| 0.4 | 15/10/2026 | Verificações das APIs pela página de documentação no navegador, sem VS Code (T-06, T-08, T-20 e A8). |
| 0.5 | 15/10/2026 | Registro dos pedidos P-18 e P-19 (T-03). |

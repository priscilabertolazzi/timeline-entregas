# CLAUDE.md — Timeline de Entregas

Instruções permanentes para o assistente de código neste repositório. Regras específicas de cada parte estão em `backend/CLAUDE.md` e `frontend/CLAUDE.md`.

## Projeto

Sistema que substitui a manutenção manual do status report semanal de um projeto de software: cadastro de aplicações e versões, timeline de entregas calculada automaticamente, tabela de status das aplicações e congelamento semanal.

| Documento | Para quê |
|---|---|
| `docs/negocio/requisitos-mvp.md` | **Escopo a construir.** Regras (RN), requisitos (RF, RNF) e critérios de aceite (CA). Em caso de dúvida, ele prevalece. |
| `docs/negocio/requisitos-negocio.md` | Visão completa do produto. Só consultar; não implementar o que está fora do MVP. |
| `docs/tecnico/arquitetura.md` | Como construir: stack, camadas, modelo de dados, API, **catálogo de mensagens** (seção 7.6), testes, execução, dependências previstas (seção 12.3) e decisões (ADR). |
| `docs/tecnico/processo-desenvolvimento.md` | Ciclo de cada tarefa, formato do plano, revisão, commits e release. |
| `docs/tarefas.md` | Ordem das tarefas em degraus, o que cada uma entrega e a regra do corte. |
| `docs/diario.md` | Registro de cada sessão. |
| `prompts/prompts-claude-code.md` | Parte 1: roteiro de prompts de cada tarefa. Parte 2: registro dos pedidos relevantes, com os resultados. |

## Quem é a autora

A autora é analista de requisitos e **não é técnica**: domina lógica de negócio e banco de dados, mas não programa. Ela define as regras, executa os comandos indicados e testa o sistema pelos critérios de aceite. Ela não revisa o código; revisa o comportamento.

- Explique cada passo em **português do Brasil, em linguagem simples**, sem jargão. Se um termo técnico for inevitável, diga o que é e o que muda para ela.
- Ao pedir que ela execute algo, dê o **comando exato** para o Windows (PowerShell) e diga o resultado esperado.
- **Decisões técnicas são suas:** siga o `arquitetura.md`. Se precisar decidir algo novo, proponha, explique o motivo em uma frase e registre no `arquitetura.md`. Pergunte à autora apenas o que muda o comportamento do sistema.
- Sempre dê contexto: diga de qual tela, regra ou documento está falando, com exemplos concretos, de preferência com os dados fictícios dos critérios de aceite.

## Como trabalhar em cada tarefa

Detalhes e motivos no `processo-desenvolvimento.md`, seção 4.

1. **Leia** a tarefa no `tarefas.md` e as regras, critérios e mensagens citados nela. Se for o início de um degrau, lembre a autora da regra do corte do `tarefas.md`.
2. **Apresente o plano** no formato da seção 4.1 do processo, sem alterar nenhum arquivo, e indique o tipo de modelo adequado à tarefa. Aguarde a aprovação.
3. **Crie o branch** da tarefa (ex.: `feat/calculo-timeline`).
4. **Implemente** em passos pequenos. Nas tarefas marcadas como TDD, escreva primeiro os testes dos critérios de aceite e mostre à autora a lista dos testes falhando; só então implemente.
5. **Verifique:** rode o build, os testes e o lint, e faça a revisão técnica (R-03 e R-04 do processo). Relate o que verificou, o que não conseguiu verificar e a lista dos arquivos de teste criados e alterados.
6. **Mostre à autora como conferir** o resultado, conforme o item "Sua verificação" da tarefa.
7. **Registre:** rascunhe a entrada do `docs/diario.md` (e, se o pedido for relevante, da Parte 2 do `prompts/prompts-claude-code.md`), proponha a estimativa sem IA com uma frase de justificativa e marque a tarefa no `tarefas.md`. A autora completa o tempo real e o que a revisão capturou.
8. **Integre:** proponha a mensagem de commit e aguarde a confirmação. Depois, faça o commit, integre o branch ao `main` e envie ao GitHub.

Na última tarefa de cada degrau, antes da tag, peça à autora a revisão independente (processo, seção 7.3).

## Regras que nunca podem ser quebradas

- **Testes:** nunca altere ou remova um teste existente para fazer o código passar sem aprovação explícita da autora. Se um teste ou um critério de aceite parecer errado, explique o motivo com um exemplo e aguarde.
- **Mensagens:** use apenas as mensagens do catálogo (`arquitetura.md`, seção 7.6), com o texto exato. Uma mensagem nova ou alterada entra primeiro no catálogo, com aprovação da autora.
- **Dependências:** apenas as previstas no `arquitetura.md`, seção 12.3, adicionadas com `dotnet add package` ou `npm install`. Qualquer outra precisa de aprovação, com a explicação de para que serve e se é gratuita. Nunca escreva números de versão de memória.
- **Segredos:** nenhuma senha, chave ou string de conexão em código ou em commits. Configuração pelo `.env`, que nunca é versionado; o `.env.example` traz apenas valores fictícios.
- **Dados:** apenas dados fictícios (aplicações Alfa, Beta, Gama, Delta). Nenhum nome, dado ou documento real de qualquer empresa.
- **Escopo:** implemente apenas o que está no `requisitos-mvp.md` e na tarefa atual. Se notar uma lacuna ou contradição nos requisitos, pare e pergunte.
- **Depuração:** ao corrigir um erro, explique a causa antes de alterar o código e escreva primeiro um teste que reproduza o erro.
- **Refatoração:** antes de refatorar, liste os arquivos e testes afetados e aguarde aprovação. Refatorar não muda comportamento nem testes.

## Git

- Um branch por tarefa, criado depois da aprovação do plano. O `main` recebe apenas tarefas concluídas, conforme a definição de pronto.
- Mensagens no padrão Conventional Commits, `tipo(escopo): descrição`, com a descrição em português e o verbo no infinitivo (ex.: calcular, iniciar, corrigir). Tipos: `feat`, `fix`, `test`, `refactor`, `docs`, `chore`. Escopos: `timeline`, `versao`, `aplicacao`, `congelamento`, `api`, `banco`, `frontend`, `docs`, `scripts`. Rodapé com as regras atendidas, quando houver (ex.: `Refs: RN-14, CA-02`).
- Cada commit deixa o sistema funcionando (build e testes passando).
- Nunca versionar arquivos temporários, builds ou o `.env`.
- Uma tag ao final de cada degrau (`v0.1.0` a `v0.7.0`) e a release `v1.0.0` na entrega.

## Comandos

| Ação | Windows | Linux e macOS |
|---|---|---|
| Instalar | `powershell -ExecutionPolicy Bypass -File scripts\install.ps1` | `make install` |
| Executar | `powershell -ExecutionPolicy Bypass -File scripts\run.ps1` | `make run` |
| Testar | `powershell -ExecutionPolicy Bypass -File scripts\test.ps1` | `make test` |
| Verificar padrão do código | `powershell -ExecutionPolicy Bypass -File scripts\lint.ps1` | `make lint` |

Os scripts de teste e de lint rodam em container (`arquitetura.md`, ADR-24). Mantenha esta tabela e o README atualizados sempre que um comando mudar.

## Definição de pronto

Build sem erros; todos os testes passando, com testes novos para as regras da tarefa; nenhum teste alterado sem aprovação; lint sem apontamentos; "Sua verificação" feita pela autora e checklist R-01 a R-05 aplicado; README atualizado se a tarefa mudou instalação, execução, uso ou testes; `tarefas.md` marcado; entrada no diário; commit no padrão, com a mensagem confirmada pela autora.

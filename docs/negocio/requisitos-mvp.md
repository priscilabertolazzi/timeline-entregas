# Documento de Requisitos de Negócio — MVP
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão | 0.7 |
| Data | 16/10/2026 |
| Status | Versão de entrega (v1.0.0). Descreve o que foi construído: o cadastro de aplicações e versões, pela API e pelas telas. As próximas etapas estão na seção 12. |
| Documento de origem | `docs/negocio/requisitos-negocio.md` (visão completa do produto, versão 0.68) |

---

## 1. Introdução

Este documento define o que a versão 1.0.0 do **Timeline de Entregas** entrega: o cadastro de aplicações e versões, com fases, datas, situação e observação, pela API e pelas telas. Ele é um recorte do documento de requisitos de negócio completo, que descreve a visão integral do produto.

**Identificadores:** as regras, os requisitos e os critérios de aceite mantêm os números do documento completo, os mesmos citados nos testes e no código. Os números que não aparecem nas seções 3 a 10 pertencem às próximas etapas e estão listados na seção 12.

O contexto do projeto, o processo atual, os problemas e as alternativas avaliadas estão descritos nas seções 1 e 2 do documento completo.

---

## 2. Resumo do contexto

O status report semanal do projeto é mantido manualmente em PowerPoint, com uma timeline de entregas e uma tabela de status das aplicações. A manutenção consome pelo menos 1 hora por semana, a timeline nunca fica alinhada às datas reais, a mesma informação é mantida em mais de um slide e as mudanças de datas são registradas à mão. Esta versão digitaliza a entrada dessas informações: o responsável registra aplicações, versões, fases, datas e observações num cadastro único, com as regras de negócio conferidas a cada salvamento. A partir desse cadastro, as próximas etapas desenham a timeline, montam a tabela de status das aplicações e congelam o status report de cada semana (seção 12).

---

## 3. Objetivos

| ID | Objetivo |
|---|---|
| OBJ-01 | Manter um único cadastro de aplicações e versões, com as regras de negócio conferidas a cada salvamento, como base da timeline e da tabela de status das aplicações (seção 12). |
| OBJ-07 | Entregar uma primeira versão das APIs do sistema pronta para ser evoluída e incorporada ao projeto acompanhado como uma aplicação interna, seguindo os padrões de arquitetura adotados nele. |

---

## 4. Escopo

### 4.1 Dentro do escopo

- Cadastro de aplicações e de suas versões, com exclusão.
- Registro de fases e datas de cada versão, incluindo a prorrogação intencional da homologação, com as regras conferidas a cada salvamento.
- Situação (On track ou Risco) e observação de cada versão.
- Lista de aplicações, com a fase e a situação de cada versão em etiquetas coloridas e a data de entrega.
- Tela da versão, com todos os dados num único formulário e um só botão Salvar, que grava tudo ou nada.

### 4.2 Fora do escopo desta versão

- As próximas etapas (timeline, tabela de status das aplicações e congelamento semanal) e as evoluções descritas na seção 12.
- Todos os itens fora do escopo do documento completo (seção 4.2 dele).

---

## 5. Partes interessadas e perfis

| Perfil | Descrição | Uso do sistema |
|---|---|---|
| Responsável pelo status report | Pessoa que mantém os dados do projeto e conduz a reunião de status. | Cadastra e atualiza os dados. Único usuário do sistema nesta versão. |

---

## 6. Glossário

| Termo | Definição |
|---|---|
| Aplicação | Produto ou sistema acompanhado no projeto. |
| Versão | Entrega identificada de uma aplicação (ex.: v0, v0.1, v1, v1.2). Uma aplicação pode ter nenhuma, uma ou várias versões. |
| Fase | Ponto do ciclo de vida em que uma versão se encontra, informado pelo usuário: Backlog, Análise, Desenvolvimento, Testes, Homologação ou Produção. |
| Etapa | Período da versão delimitado por uma data de início e uma de fim: andamento, homologação e prorrogação. Na tela da versão, cada etapa ocupa uma linha. |
| Grupo visual | Classificação das fases por cor: Não iniciado, Em andamento, Em homologação, Homologação prorrogada e Em produção. Já existe no código, como base da timeline e dos contadores das próximas etapas (seção 12). |
| Situação | Indicador de saúde de uma versão: Não iniciado, On track, Risco ou Entregue. É diferente da fase. |
| Início do andamento | Data em que a versão sai do Backlog e começa a ser trabalhada, seja em análise, desenvolvimento ou testes. |
| Fim do andamento | Data em que terminam a análise, o desenvolvimento e os testes da versão. |
| Início da homologação | Data em que a versão entra em homologação. |
| Fim da homologação | Data de conclusão da homologação. Alterá-la não gera prorrogação. |
| Prorrogação da homologação | Extensão do prazo de homologação registrada intencionalmente pelo usuário, com início e fim próprios. Alterar as datas da homologação não gera prorrogação. |
| Início da prorrogação | Data em que começa a prorrogação da homologação. |
| Fim da prorrogação | Data em que termina a prorrogação da homologação. |
| Data de entrega | Data do deploy em produção, quando a versão fica disponível aos usuários. É a última data da versão e pode haver um intervalo entre ela e o fim da homologação. |
| Replanejamento | Alteração de qualquer data de uma versão. |
| Sem previsão (TBD) | TBD é a sigla de *To Be Defined* (a definir). Indica uma versão sem datas: na lista de aplicações, a coluna de data de entrega mostra "TBD". |
| Observação | Texto livre, escrito pelo usuário, com contexto sobre a versão. |
| Data atual | Data do computador do usuário. É a referência das regras de Produção (RN-07). |
| Lista de aplicações | Tela de cadastro onde o usuário mantém as aplicações e vê as versões de cada uma. |
| Tela da versão | Tela onde o usuário altera rótulo, fase, situação, observação e as sete datas de uma versão, com um único botão Salvar. |

---

## 7. Regras de negócio

Todas as regras desta versão se aplicam ao cadastro: à API e às telas de cadastro (lista de aplicações e tela da versão).

### 7.1 Aplicações e versões

**RN-01: Identificação e cadastro da aplicação.** *Aplica-se a: cadastro.* Toda aplicação possui um nome, que deve ser único no sistema. Na comparação, os espaços no início e no fim são desconsiderados e não há diferença entre maiúsculas e minúsculas: "Alfa", "alfa" e "Alfa " são o mesmo nome. O nome é guardado sem os espaços das pontas. Uma aplicação pode ser cadastrada sem versões. Nesse caso, ela aparece na lista de aplicações com a indicação "Nenhuma versão cadastrada".

**RN-02: Identificação e cadastro da versão.** *Aplica-se a: cadastro.* Toda versão pertence a uma única aplicação e possui um rótulo (ex.: v1.2). Uma aplicação pode ter quantas versões forem necessárias, desde que cada rótulo seja único dentro dela, comparado da mesma forma que o nome da aplicação (RN-01): "v1" e "V1" são o mesmo rótulo. Para cadastrar uma versão, basta o rótulo: ela é criada na fase Backlog e sem datas. Enquanto as datas não forem informadas (RN-09), a versão aparece com "TBD" no lugar da data de entrega (RN-13).

**RN-04: Exclusão.** *Aplica-se a: cadastro.* No MVP, aplicações e versões são excluídas definitivamente, após confirmação do usuário. Excluir uma aplicação exclui também todas as suas versões. Itens excluídos deixam de aparecer na lista de aplicações. O arquivamento, com restauração, é uma evolução futura (seção 12). A confirmação diz o nome da aplicação (ou da versão) e avisa que a exclusão não pode ser desfeita; na aplicação, avisa também que todas as suas versões serão excluídas.

### 7.2 Fases

**RN-05: Fases possíveis.** *Aplica-se a: cadastro.* Uma versão está sempre em exatamente uma das fases: Backlog, Análise, Desenvolvimento, Testes, Homologação ou Produção.

**RN-07: Fase informada pelo usuário.** *Aplica-se a: cadastro.* A fase da versão é sempre informada pelo usuário, na tela de cadastro da versão, respeitando o sentido das fases (RN-08). Uma versão sem datas está sempre em **Backlog** (não iniciada):

- Para passar para Análise ou fases seguintes, as datas da versão precisam estar informadas (RN-09).
- Para passar para Produção, a data de entrega precisa ser igual ou anterior à data atual. Em Produção, ela não pode ser alterada para depois da data atual.
- As datas só podem ser removidas de uma versão em Backlog, e sempre todas de uma vez: a versão volta a ficar sem datas, com "TBD" (RN-13). Para remover as datas de uma versão em Análise, Desenvolvimento ou Testes, o usuário primeiro muda a fase para Backlog (RN-08) e depois remove as datas. É o caso de uma versão que foi despriorizada e voltou para a fila.
- Versões em Homologação ou Produção não voltam para Backlog (RN-08) e, por isso, não podem ter as datas removidas.

Fora essas situações, fase e datas são independentes: mudar uma não altera a outra automaticamente.

**RN-08: Sentido das fases.** *Aplica-se a: cadastro.* As fases seguem a ordem Backlog → Análise → Desenvolvimento → Testes → Homologação → Produção, com as seguintes regras:

- Entre Backlog, Análise, Desenvolvimento e Testes, o usuário pode alterar a fase livremente, inclusive retornando (ex.: de Testes para Desenvolvimento).
- Para a frente, o usuário pode avançar para qualquer fase seguinte, sem passar pelas intermediárias (ex.: de Backlog direto para Produção, na carga inicial de uma versão já entregue), respeitando as condições da RN-07: as datas informadas para sair do Backlog e a data de entrega igual ou anterior à data atual para entrar em Produção.
- Uma vez em Homologação, a versão **não retorna** a fases anteriores. A etapa de homologação já contempla as correções e os desenvolvimentos necessários, que mantêm a versão em Homologação; se o prazo for estendido, o usuário altera o fim da homologação ou registra uma prorrogação (RN-24), conforme o combinado no projeto.
- Novos pacotes de correção liberados para homologação não alteram o início da homologação. O usuário pode mencioná-los na observação (RN-22).
- Uma versão em Produção não muda mais de fase.
- Na tela da versão, o campo Fase fica desabilitado para uma versão em Produção e, para uma versão em Homologação, oferece apenas Homologação e Produção. O sistema continua recusando as mudanças proibidas, com as mensagens da regra.

### 7.3 Datas

**RN-09: Datas de uma versão.** *Aplica-se a: cadastro.* Uma versão possui até sete datas, cada uma com um **único valor**:

| Data | O que marca |
|---|---|
| Início do andamento | Início do andamento. |
| Fim do andamento | Fim do andamento. |
| Início da homologação | Início da homologação. |
| Fim da homologação | Fim da homologação. |
| Início da prorrogação | Início da prorrogação da homologação. Opcional (RN-24). |
| Fim da prorrogação | Fim da prorrogação da homologação. Opcional (RN-24). |
| Data de entrega | Deploy em produção. É sempre a última data da versão. |

Não existe distinção entre data prevista e data realizada: enquanto o evento não acontece, a data representa o planejado; quando necessário, o usuário a altera, inclusive para refletir a data em que o evento de fato ocorreu. Uma versão tem nenhuma data ou as cinco datas do ciclo (início e fim do andamento, início e fim da homologação e data de entrega), informadas em conjunto. Assim, uma versão tem sempre um plano completo, que pode ser replanejado a qualquer momento (RN-26). A prorrogação segue o mesmo princípio: início e fim informados juntos, ou nenhum dos dois (RN-24).

**RN-10: Coerência cronológica.** *Aplica-se a: cadastro.* Quando informadas, as datas devem respeitar a ordem: início do andamento ≤ fim do andamento ≤ início da homologação ≤ fim da homologação ≤ início da prorrogação ≤ fim da prorrogação ≤ data de entrega. As etapas não se sobrepõem, mas pode haver intervalos entre elas. O sistema não deve aceitar datas fora dessa ordem. As datas da versão, incluindo as da prorrogação, são alteradas e salvas em conjunto, num único salvamento: se qualquer uma violar esta ordem, nenhuma alteração é salva.

**RN-13: Versão sem previsão (TBD).** *Aplica-se a: cadastro (lista de aplicações).* TBD significa que a versão ainda não tem datas (RN-09). Na lista de aplicações, a coluna "Data de entrega" de cada versão mostra a data em dd/mm/aaaa ou "TBD" (RF-24).

### 7.4 Situação e observação

**RN-21: Situação.** *Aplica-se a: cadastro.* A situação de uma versão segue estas regras:

| Condição | Situação |
|---|---|
| Versão em Backlog | Não iniciado (automática) |
| Versão em Produção | Entregue (automática) |
| Demais fases | On track ou Risco, informada pelo usuário. Valor inicial: On track, inclusive quando a versão sai novamente do Backlog. |

Na tela da versão, a situação automática aparece já ao escolher a fase, em campo só de leitura: "Não iniciado" ao escolher Backlog e "Entregue" ao escolher Produção. Quando a situação é Risco, a observação é obrigatória (RN-22).

No MVP, a situação On track ou Risco é sempre definida pelo usuário. A sugestão automática de Risco é uma evolução futura (seção 12).

**RN-22: Observação.** *Aplica-se a: cadastro.* Cada versão possui uma observação em texto livre, editável a qualquer momento pelo usuário. **Quando a situação da versão for Risco, a observação é obrigatória:** o sistema não salva uma versão em Risco sem observação (texto só com espaços conta como vazio), e informa "Informe a observação quando a situação for Risco." O histórico de observações é uma evolução futura (seção 12).

### 7.5 Prorrogação e replanejamento

**RN-24: Prorrogação da homologação.** *Aplica-se a: cadastro.* A prorrogação é uma decisão de acompanhamento do projeto, registrada **intencionalmente** pelo usuário: ele informa o início e o fim da prorrogação, a partir do fim da homologação. Alterar as datas da homologação **nunca** gera prorrogação automaticamente; a homologação permanece como está, refletindo os combinados do projeto. Uma nova prorrogação só pode ser registrada para versões em Homologação; quando o usuário passa a fase para Homologação e informa a prorrogação **no mesmo salvamento**, vale a fase de destino e a prorrogação é aceita (CA-44). Uma prorrogação já registrada pode ser alterada ou removida a qualquer momento, inclusive depois que a versão passa para Produção (ex.: para corrigir o fim da prorrogação de uma versão já entregue).

**RN-26: Liberdade de replanejamento.** *Aplica-se a: cadastro.* Todas as datas de uma versão podem ser alteradas a qualquer momento pelo usuário, respeitando a coerência cronológica (RN-10), para acompanhar o monitoramento contínuo e os planos de ação do projeto.

### 7.6 Telas de cadastro

**RN-36: Etiquetas de fase e situação.** *Aplica-se a: cadastro (lista de aplicações e tela da versão).* A fase e a situação de cada versão aparecem em etiquetas coloridas:

| Etiqueta | Cor |
|---|---|
| Backlog, Não iniciado | Cinza claro |
| Análise, Desenvolvimento, Testes | Azul claro |
| Homologação | Roxo claro |
| On track | Verde claro |
| Produção, Entregue | Verde escuro |
| Risco | Laranja claro |

As cores ficam definidas num único lugar do tema.

---

## 8. Requisitos funcionais

| ID | Requisito | Regras relacionadas |
|---|---|---|
| RF-01 | O usuário pode cadastrar, editar e excluir aplicações. | RN-01, RN-04 |
| RF-03 | O usuário pode cadastrar, editar e excluir versões de uma aplicação. | RN-02, RN-04 |
| RF-04 | O usuário pode alterar a fase de uma versão, respeitando o sentido das fases. | RN-05, RN-07, RN-08 |
| RF-05 | O usuário pode informar, alterar e remover as datas da versão (início e fim do andamento, início e fim da homologação e data de entrega). As datas são informadas, alteradas e removidas em conjunto, num único salvamento. | RN-07, RN-09, RN-10, RN-26 |
| RF-09 | O usuário pode registrar e editar a observação de uma versão. | RN-22 |
| RF-10 | O usuário pode definir a situação (On track ou Risco) de versões em andamento ou em homologação. | RN-21 |
| RF-14 | O usuário pode registrar, alterar e remover a prorrogação da homologação de uma versão, informando o início e o fim da prorrogação. | RN-10, RN-24 |
| RF-21 | Ao salvar a versão com sucesso, o sistema volta para a lista de aplicações, com o aviso "Alterações salvas.". Se o salvamento for recusado, o usuário permanece na tela da versão, com as mensagens. | RN-10, CA-45 |
| RF-22 | A lista de aplicações e a tela da versão mostram a fase e a situação em etiquetas coloridas. | RN-36, CA-47 |
| RF-23 | A barra de título mostra o nome "Timeline de Entregas (MVP)" e a versão do sistema (a tag da entrega, ex.: v1.0.0). | CA-48 |
| RF-24 | A lista de aplicações mostra a data de entrega de cada versão em dd/mm/aaaa e, quando a versão não tem datas, "TBD". | RN-13, CA-49 |

---

## 9. Requisitos não funcionais de negócio

| ID | Requisito |
|---|---|
| RNF-01 | O sistema será utilizado localmente, sem hospedagem. |
| RNF-02 | O sistema não deve gerar custos de licença ou de serviços. |
| RNF-03 | Interface e conteúdo em português do Brasil, com datas no formato dd/mm/aaaa. |
| RNF-04 | As telas devem ser legíveis quando apresentadas por compartilhamento de tela em reuniões, ajustando-se ao tamanho da tela, sem limite fixo de aplicações e versões. |

---

## 10. Critérios de aceite

Os cenários abaixo usam dados fictícios. Datas não citadas em um cenário estão informadas e são coerentes com a RN-10.

**CA-05: Coerência de datas**
- **Dado** que a versão v1 da aplicação Alfa tem início da homologação em 01/10/2026
- **Quando** o usuário tenta alterar o fim do andamento para 05/10/2026
- **Então** o sistema não aceita a data e informa que o fim do andamento não pode ser posterior ao início da homologação.

**CA-06: Versão em homologação não retorna de fase**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação
- **Quando** o usuário tenta alterar a fase para Desenvolvimento
- **Então** o sistema não aceita a alteração e informa que uma versão em homologação não retorna a fases anteriores, devendo eventuais extensões de prazo ser tratadas pelo fim da homologação ou por uma prorrogação.

**CA-08: Alteração das datas da homologação não gera prorrogação**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com homologação de 01/10/2026 a 13/10/2026 e nenhuma prorrogação registrada
- **Quando** o usuário altera o fim da homologação para 23/10/2026
- **Então** o sistema salva o novo fim da homologação
- **E** a versão continua sem prorrogação registrada.

**CA-13: Versão sem datas permanece em Backlog**
- **Dado** que a versão v2 da aplicação Alfa está em Backlog e não possui datas
- **Quando** o usuário tenta alterar a fase para Análise
- **Então** o sistema não aceita a alteração e informa que é preciso informar as datas da versão antes de iniciá-la.

**CA-14: Aplicação sem versões**
- **Dado** que a aplicação Delta foi cadastrada e não possui nenhuma versão
- **Quando** o usuário abre a lista de aplicações
- **Então** a aplicação Delta aparece com a indicação "Nenhuma versão cadastrada".

**CA-20: Remoção de datas fora do Backlog**
- **Dado** que a versão v2 da aplicação Alfa está em Desenvolvimento, com as datas informadas
- **Quando** o usuário tenta remover as datas
- **Então** o sistema não aceita e informa que é preciso mudar a fase da versão para Backlog antes de remover as datas.

**CA-24: Alteração da observação**
- **Dado** que a versão v1 da aplicação Alfa tem a observação "Homologação em andamento."
- **Quando** o usuário altera a observação para "Acesso sendo liberado em lotes."
- **Então** a versão passa a ter a observação "Acesso sendo liberado em lotes."

**CA-25: Datas informadas em conjunto**
- **Dado** que a versão v3 da aplicação Alfa está em Backlog e não possui datas
- **Quando** o usuário informa apenas a data de entrega, 30/11/2026
- **Então** o sistema não aceita e informa que as datas de andamento, de homologação e de entrega devem ser informadas em conjunto.

**CA-32: Alteração inválida não salva nada**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com fim da homologação em 13/10/2026 e data de entrega em 20/10/2026
- **Quando** o usuário registra uma prorrogação de 13/10/2026 a 25/10/2026, sem alterar a data de entrega, e salva
- **Então** o sistema não salva nenhuma alteração e informa que o fim da prorrogação não pode ser posterior à data de entrega
- **E**, se no mesmo salvamento o usuário também alterar a data de entrega para 28/10/2026, todas as alterações são salvas.

**CA-33: Nome de aplicação duplicado**
- **Dado** que existe a aplicação Alfa
- **Quando** o usuário tenta cadastrar outra aplicação com o nome Alfa
- **Então** o sistema não aceita e informa que já existe uma aplicação com esse nome
- **E** o mesmo acontece com os nomes "alfa" e "Alfa " (com espaço no fim), considerados iguais a Alfa (RN-01).

**CA-34: Rótulo de versão duplicado**
- **Dado** que a aplicação Alfa tem a versão v1
- **Quando** o usuário tenta cadastrar outra versão v1 na aplicação Alfa
- **Então** o sistema não aceita e informa que já existe uma versão com esse rótulo na aplicação
- **E** o mesmo acontece com o rótulo "V1" (RN-02)
- **E** a aplicação Beta continua podendo ter a sua própria versão v1.

**CA-35: Remoção das datas em Backlog**
- **Dado** que a versão v2 da aplicação Alfa está em Backlog, com as datas informadas
- **Quando** o usuário remove as datas
- **Então** as cinco datas são removidas
- **E** a lista de aplicações mostra "TBD" na data de entrega da v2.

**CA-36: Prorrogação fora da Homologação**
- **Dado** que a versão v1 da aplicação Beta está em Desenvolvimento
- **Quando** o usuário tenta registrar uma prorrogação
- **Então** o sistema não aceita e informa que a prorrogação só pode ser registrada para versões em Homologação.

**CA-38: Exclusão de aplicação**
- **Dado** que a aplicação Delta tem as versões v1 e v2
- **Quando** o usuário exclui a aplicação Delta e confirma
- **Então** a aplicação Delta e suas versões deixam de existir no cadastro e na lista de aplicações.

**CA-39: Produção com data de entrega futura**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com data de entrega em 28/10/2026
- **E** a data atual é 14/10/2026
- **Quando** o usuário tenta alterar a fase para Produção
- **Então** o sistema não aceita e informa que, para passar para Produção, a data de entrega deve ser igual ou anterior à data atual.

**CA-40: Avanço direto para Produção na carga inicial**
- **Dado** que a versão v0 da aplicação Beta está em Backlog, com andamento de 01/08/2026 a 14/08/2026, homologação de 17/08/2026 a 21/08/2026 e data de entrega em 24/08/2026
- **E** a data atual é 14/10/2026
- **Quando** o usuário altera a fase diretamente para Produção
- **Então** o sistema aceita a alteração, sem exigir as fases intermediárias
- **E** a situação da versão passa a ser Entregue.

**CA-42: Correção da prorrogação de uma versão entregue**
- **Dado** que a versão v1 da aplicação Alfa está em Produção, com fim da homologação em 13/10/2026, prorrogação de 13/10/2026 a 23/10/2026 e data de entrega em 28/10/2026
- **E** a data atual é 30/10/2026
- **Quando** o usuário altera o fim da prorrogação para 22/10/2026 e salva
- **Então** o sistema aceita a alteração
- **E**, para a versão v0 da aplicação Beta, em Produção e sem prorrogação, o sistema não aceita o registro de uma nova prorrogação e informa que a prorrogação só pode ser registrada para versões em Homologação.

**CA-43: Observação obrigatória em Risco**
- **Dado** que a versão v1 da aplicação Beta está em Desenvolvimento, sem observação
- **Quando** o usuário altera a situação para Risco, sem escrever a observação (ou com um texto só de espaços), e salva
- **Então** o sistema não aceita e informa "Informe a observação quando a situação for Risco."
- **E**, se o usuário escrever a observação "Dependência de outra equipe." e salvar, o sistema aceita
- **E** uma versão em On track pode ficar sem observação.

**CA-44: Prorrogação ao passar para Homologação**
- **Dado** que a versão v0.2 da aplicação CRM está em Backlog, com as cinco datas informadas
- **Quando** o usuário altera a fase para Homologação, informa a prorrogação de 19/10/2026 a 23/10/2026, com data de entrega em 26/10/2026, e salva uma única vez
- **Então** o sistema aceita: a versão fica em Homologação, com a prorrogação registrada
- **E**, se o usuário alterar a fase para Desenvolvimento no lugar de Homologação, o sistema não aceita e informa que a prorrogação só pode ser registrada para versões em Homologação.

**CA-45: Salvar volta para a lista de aplicações**
- **Dado** que o usuário está na tela da versão v1 da aplicação Alfa
- **Quando** salva alterações aceitas pelo sistema
- **Então** o sistema volta para a lista de aplicações e mostra "Alterações salvas."
- **E**, se o sistema não aceitar o salvamento, o usuário permanece na tela da versão, com as mensagens, sem nenhuma alteração salva.

**CA-46: Campos Fase e Situação na tela da versão**
- **Dado** que a versão v1 da aplicação Alfa está em Produção
- **Quando** o usuário abre a tela da versão
- **Então** o campo Fase aparece desabilitado e a Situação aparece como "Entregue", só para leitura
- **E**, para uma versão em Homologação, o campo Fase oferece apenas Homologação e Produção
- **E**, ao escolher a fase Backlog, a Situação passa a mostrar "Não iniciado" e, ao escolher Produção, "Entregue", sem ser possível editá-la.

**CA-47: Etiquetas de fase e situação**
- **Dado** que a aplicação Alfa tem a versão v1 em Homologação, On track, e a aplicação Beta tem a versão v1 em Desenvolvimento, em Risco
- **Quando** o usuário abre a lista de aplicações
- **Então** a v1 da Alfa mostra "Homologação" em roxo claro e "On track" em verde claro
- **E** a v1 da Beta mostra "Desenvolvimento" em azul claro e "Risco" em laranja claro (cores da RN-36).

**CA-48: Título e versão do sistema na tela**
- **Dado** que a entrega do sistema tem a tag v1.0.0
- **Quando** o usuário abre qualquer tela
- **Então** a barra de título mostra "Timeline de Entregas (MVP)" e "v1.0.0".

**CA-49: Data de entrega na lista de aplicações**
- **Dado** que a versão v1 da aplicação Alfa tem data de entrega em 23/10/2026 e a versão v2 está em Backlog, sem datas
- **Quando** o usuário abre a lista de aplicações
- **Então** a v1 mostra "23/10/2026" na coluna "Data de entrega"
- **E** a v2 mostra "TBD".

---

## 11. Premissas e restrições

- Haverá um único usuário responsável pela manutenção dos dados.
- A carga inicial das aplicações e versões existentes será feita manualmente pelo usuário. Quando a data real de uma versão antiga tiver se perdido, o usuário informa a melhor data conhecida para cada uma das cinco datas (RN-09) e pode registrar isso na observação.

---

## 12. Evolução do projeto

O cadastro desta versão já guarda todas as datas de cada etapa, que são a base das próximas etapas, e o código já tem os tipos que a timeline vai usar (etapa, grupo visual e tom). As regras, os requisitos e os critérios de aceite de cada etapa estão descritos no documento completo (`requisitos-negocio.md`), com os números abaixo.

### 12.1 Próximas etapas

| Etapa | Escopo | Números reservados |
|---|---|---|
| Timeline | Desenho automático da timeline a partir das datas e fases: etapas coloridas, intervalos, marco de entrega, realizado e previsto, linha da data atual, coluna "Sem previsão", versões sobrepostas em sub-linhas, rótulos e escolha do intervalo de meses, com o semestre atual como padrão. | OBJ-02, OBJ-04, OBJ-05; RN-03, RN-06, RN-14 a RN-16, RN-18 a RN-20, RN-25; RF-11, RF-12; CA-02 a CA-04, CA-07, CA-09, CA-15, CA-22, CA-23, CA-27 a CA-29, CA-31, CA-37 |
| Tabela de status das aplicações | Tabela com aplicação, versão, fase, situação, data de entrega e observação de cada versão, e os contadores de aplicações por grupo. | RN-23; RF-13; CA-21 |
| Congelamento semanal | Congelar o status report de cada semana, consultar o Histórico de Status Reports e abrir uma semana congelada, em modo somente leitura. | OBJ-06; RN-27 a RN-30; RF-15 a RF-17; CA-10 a CA-12, CA-26, CA-30, CA-41 |

### 12.2 Evoluções depois das próximas etapas

| Evolução | O que acrescenta | Referência no documento completo |
|---|---|---|
| Ordenação manual | Mover aplicações e versões para a ordem desejada e restaurar a ordem cronológica quando quiser. | RN-03, RF-02, CA-18, CA-19 |
| Filtros de visualização | Escolher quais aplicações aparecem, ocultar versões entregues e filtrar a tabela por fase e situação, com contadores sempre considerando o projeto inteiro e o congelado registrando os filtros aplicados. | RN-31 a RN-35, RF-18 a RF-20, CA-16, CA-17 |
| Detalhes ao passar o mouse | Mostrar todas as datas e a fase da versão ao passar o mouse sobre a barra da timeline. | RN-18, CA-22 |
| Sugestão automática de Risco | Sugerir a situação Risco quando uma prorrogação é registrada ou quando o fim de uma etapa passa sem a versão avançar de fase. | RN-21, CA-02, CA-09 |
| Incorporação como aplicação interna | Integrar o sistema ao ambiente do projeto acompanhado, seguindo seus padrões de arquitetura. | OBJ-07 |
| Calendário de dias úteis | Calcular as datas de início e fim de cada etapa a partir de estimativas de duração em dias úteis, com calendários nacional, estadual, municipal e datas livres. | OBJ-08 |
| Novas visões | Aproveitar as datas de cada etapa para outras visualizações, como uma visão para a equipe de desenvolvimento. | RN-09 |
| Níveis de acesso | Restringir quem pode substituir o status report congelado de semanas anteriores. | Seção 13, RN-28 |
| Dia de referência configurável | Escolher o dia da semana usado como data atual do status report congelado, hoje fixo na terça-feira. | Seção 13, RN-27 |
| Fases, etapas e cores configuráveis | Cadastrar as fases, as etapas e suas cores, e a correspondência (de-para) entre fases e etapas. | Seção 13, RN-05, RN-06 |
| Histórico de alterações | Registrar automaticamente as alterações de datas, com motivo opcional, e de observações, com consulta pelo usuário. | OBJ-03, RN-12, RN-22, RF-08, RF-09, CA-01, CA-24 |
| Arquivamento | Arquivar e restaurar aplicações e versões, no lugar da exclusão definitiva. | RN-04, RF-01, RF-03 |
| Backup e restauração | Copiar e restaurar os dados do sistema. | RNF-05 |

---

## 13. Histórico do documento

| Versão | Data | Descrição |
|---|---|---|
| 0.1 a 0.6 | 14 e 15/10/2026 | Recorte do MVP a partir do documento completo e revisões sucessivas, com timeline, tabela de status das aplicações e congelamento semanal. O detalhe de cada versão está no histórico do Git. |
| 0.7 | 16/10/2026 | O documento passa a descrever só o que foi entregue na v1.0.0, o cadastro de aplicações e versões, com as regras e os critérios de aceite ajustados ao comportamento entregue (CA-08, CA-14, CA-24, CA-35 e CA-38). A timeline, a tabela de status das aplicações e o congelamento semanal passam para a seção 12, com os seus números. |

# Documento de Requisitos de Negócio — MVP
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão | 0.6 (rascunho para refinamento) |
| Data | 15/10/2026 |
| Status | Em elaboração |
| Documento de origem | `docs/negocio/requisitos-negocio.md` (visão completa do produto, versão 0.68) |

---

## 1. Introdução

Este documento define o **MVP** (produto mínimo viável) do **Timeline de Entregas**: a primeira versão do sistema que será desenvolvida e entregue. Ele é um recorte do documento de requisitos de negócio completo, que descreve a visão integral do produto e permanece como referência para as próximas evoluções.

O critério do recorte foi manter tudo o que substitui o status report semanal feito hoje em PowerPoint — cadastro, datas e fases, timeline, tabela de status das aplicações e congelamento semanal — e deixar para evoluções futuras os recursos que facilitam o uso, mas não são indispensáveis para essa substituição.

**Identificadores:** as regras, requisitos e critérios de aceite mantêm os mesmos identificadores do documento completo, para que seja possível rastrear cada item entre os dois documentos. Por isso, a numeração deste documento tem lacunas: elas correspondem a itens que ficaram fora do MVP. Quando uma regra foi reduzida no recorte, o próprio texto da regra informa o que ficou para depois.

O contexto do projeto, o processo atual, os problemas e as alternativas avaliadas estão descritos nas seções 1 e 2 do documento completo.

---

## 2. Resumo do contexto

O status report semanal do projeto é mantido manualmente em PowerPoint, com uma timeline de entregas e uma tabela de status das aplicações. A manutenção consome pelo menos 1 hora por semana, a timeline nunca fica alinhada às datas reais, a mesma informação é mantida em mais de um slide e as mudanças de datas são registradas à mão. O MVP digitaliza a entrada dessas informações: o responsável registra aplicações, versões, fases, datas e observações, e o sistema desenha a timeline, monta a tabela de status das aplicações e congela o status report de cada semana.

---

## 3. Objetivos do MVP

| ID | Objetivo |
|---|---|
| OBJ-01 | Manter um único cadastro de aplicações e versões, do qual a timeline e a tabela de status das aplicações são geradas automaticamente. |
| OBJ-02 | Eliminar o posicionamento manual: a posição e as cores das barras devem ser calculadas a partir das datas e fases. |
| OBJ-04 | Preservar a convenção do projeto: fases detalhadas na tabela de status das aplicações e etapas coloridas por grupo visual na timeline, com a correspondência definida no sistema (RN-06). |
| OBJ-05 | Reduzir o tempo gasto na atualização semanal do status report. |
| OBJ-06 | Manter o histórico semanal do status report, permitindo recuperar exatamente como a timeline e a tabela de status das aplicações estavam em qualquer semana anterior, salvo quando o usuário substituir intencionalmente o status report de uma semana (RN-28). |
| OBJ-07 | Entregar uma primeira versão das APIs do sistema pronta para ser evoluída e incorporada ao projeto acompanhado como uma aplicação interna, seguindo os padrões de arquitetura adotados nele. |

**Indicadores de sucesso**

- Tempo de atualização semanal da timeline e da tabela de status das aplicações, comparado ao processo atual, que consome pelo menos 1 hora por semana.
- Nenhum ajuste manual de posicionamento ou cor necessário após a atualização dos dados.
- Timeline e tabela de status das aplicações sempre consistentes entre si.

---

## 4. Escopo do MVP

### 4.1 Dentro do escopo

- Cadastro de aplicações e de suas versões, com exclusão.
- Registro de fases e datas de cada versão, incluindo a prorrogação intencional da homologação.
- Timeline com etapas, intervalos, marco de entrega, realizado e previsto, linha da data atual e coluna "Sem previsão".
- Escolha do intervalo de meses da timeline, com o semestre atual como padrão.
- Tabela de status das aplicações, com contadores.
- Ordem cronológica automática de aplicações e versões.
- Congelamento semanal do status report e consulta às semanas anteriores pelo Histórico de Status Reports.

### 4.2 Fora do escopo do MVP

- Todos os itens fora do escopo do documento completo (seção 4.2 dele).
- As evoluções descritas na seção 12 deste documento.

---

## 5. Partes interessadas e perfis

| Perfil | Descrição | Uso do sistema |
|---|---|---|
| Responsável pelo status report | Pessoa que mantém os dados do projeto e conduz a reunião de status. | Cadastra e atualiza dados; apresenta a timeline e a tabela de status das aplicações. Único usuário do sistema nesta versão. |
| Participantes da reunião | Gestores, equipe e partes interessadas do projeto. | Apenas visualizam, por meio do compartilhamento de tela. Não acessam o sistema. |

---

## 6. Glossário

| Termo | Definição |
|---|---|
| Aplicação | Produto ou sistema acompanhado no projeto. Na timeline, cada aplicação com versões a mostrar ocupa uma linha, dividida em sub-linhas quando suas versões se sobrepõem (RN-14). |
| Versão | Entrega identificada de uma aplicação (ex.: v0, v0.1, v1, v1.2). Uma aplicação pode ter nenhuma, uma ou várias versões. |
| Fase | Ponto do ciclo de vida em que uma versão se encontra, informado pelo usuário: Backlog, Análise, Desenvolvimento, Testes, Homologação ou Produção. |
| Etapa | Período da versão delimitado por uma data de início e uma de fim: andamento, homologação e prorrogação. Na timeline, cada etapa é um trecho da barra, com a cor do seu grupo visual (RN-06). |
| Grupo visual | Classificação usada para definir as cores da timeline e para agrupar as fases nos contadores da tabela de status das aplicações: Não iniciado, Em andamento, Em homologação, Homologação prorrogada e Em produção. |
| Situação | Indicador de saúde de uma versão: Não iniciado, On track, Risco ou Entregue. É diferente da fase. |
| Início do andamento | Data em que a versão sai do Backlog e começa a ser trabalhada, seja em análise, desenvolvimento ou testes. Marca o início do andamento, exibido em azul na timeline. |
| Fim do andamento | Data em que terminam a análise, o desenvolvimento e os testes da versão. Marca o fim do andamento. |
| Início da homologação | Data em que a versão entra em homologação. Marca o início da homologação, exibida em roxo na timeline. |
| Fim da homologação | Data de conclusão da homologação. Marca o fim da homologação. Alterá-la não gera prorrogação. |
| Prorrogação da homologação | Extensão do prazo de homologação registrada intencionalmente pelo usuário, com início e fim próprios, exibida em vermelho. Alterar as datas da homologação não gera prorrogação. |
| Início da prorrogação | Data em que começa a prorrogação da homologação. Marca o início da prorrogação, exibida em vermelho na timeline. |
| Fim da prorrogação | Data em que termina a prorrogação da homologação. Marca o fim da prorrogação. |
| Data de entrega | Data do deploy em produção, quando a versão fica disponível aos usuários. É a última data da versão e pode haver um intervalo entre ela e o fim da homologação. |
| Intervalo | Período entre duas etapas, ou entre a última etapa e a entrega, sem atividade registrada. Exibido como uma linha pontilhada. |
| Replanejamento | Alteração de qualquer data de uma versão. |
| Realizado | Parte do período de uma versão até a data atual, inclusive. Desenhada em tom forte na timeline. |
| Previsto (forecast) | Parte do período de uma versão posterior à data atual. Desenhada em tom claro na timeline. |
| Sem previsão (TBD) | TBD é a sigla de *To Be Defined* (a definir). Indica uma versão sem datas: na timeline, ela fica na coluna "Sem previsão", com "TBD" no rótulo; na tabela de status das aplicações, a coluna de data de entrega mostra "TBD". |
| Observação | Texto livre, escrito pelo usuário, com contexto sobre a versão. |
| Timeline | Visão gráfica do status report: uma linha por aplicação, com as barras das versões dispostas ao longo dos meses. Corresponde ao slide "Cronograma de Entregas". |
| Tabela de status das aplicações | Visão em tabela do status report: uma linha por versão de cada aplicação, com as colunas Aplicação, Versão, Fase, Situação, Data de entrega e Observação, além dos contadores. Corresponde ao slide "Status das Aplicações". |
| Lista de aplicações | Tela de cadastro onde o usuário mantém as aplicações. Não faz parte do status report. |
| Status report congelado | Cópia somente leitura da timeline e da tabela de status das aplicações de uma semana, com os dados do cadastro no momento do último congelamento e a terça-feira da semana como data atual. |
| Semana de referência | Semana, de segunda-feira a domingo, à qual um status report congelado pertence. É identificada pelo número da semana no padrão ISO 8601 e pelo ano, exibidos em português no formato "Semana 42/2026". |
| Data atual | Data do computador do usuário. Na timeline, é a data que divide realizado e previsto. No status report congelado, a data atual é a data de referência. |
| Data de referência | Terça-feira da semana de referência. É a data atual do status report congelado: nela ficam a linha da data atual e a divisão entre realizado e previsto. |
| Momento do congelamento | Data e hora do último congelamento de uma semana. Quando a semana é congelada novamente, o momento anterior deixa de existir. |
| Histórico de Status Reports | Tela que lista as semanas, da mais recente para a mais antiga, desde a primeira semana congelada até a semana atual. Nela o usuário abre um status report congelado, congela a semana atual ou congela novamente uma semana já congelada (RN-28, RN-30). Não faz parte do status report. |

---

## 7. Regras de negócio do MVP

Cada regra indica onde se aplica: no **cadastro** (dados informados pelo usuário), na **timeline**, na **tabela de status das aplicações**, no **status report congelado** ou em mais de um deles. Todo dado cadastrado alimenta a timeline e a tabela de status das aplicações. Por isso, regras de cadastro são marcadas apenas como "cadastro"; quando uma delas tem um efeito específico em alguma visão, o próprio texto da regra o descreve.

### 7.1 Aplicações e versões

**RN-01: Identificação e cadastro da aplicação.** *Aplica-se a: cadastro.* Toda aplicação possui um nome, que deve ser único no sistema. Na comparação, os espaços no início e no fim são desconsiderados e não há diferença entre maiúsculas e minúsculas: "Alfa", "alfa" e "Alfa " são o mesmo nome. O nome é guardado sem os espaços das pontas. Uma aplicação pode ser cadastrada sem versões. Nesse caso, ela aparece na lista de aplicações e na tabela de status das aplicações, em uma linha única com a indicação "Nenhuma versão cadastrada", mas **não aparece na timeline** até ter pelo menos uma versão cadastrada.

**RN-02: Identificação e cadastro da versão.** *Aplica-se a: cadastro.* Toda versão pertence a uma única aplicação e possui um rótulo (ex.: v1.2). Uma aplicação pode ter quantas versões forem necessárias, desde que cada rótulo seja único dentro dela, comparado da mesma forma que o nome da aplicação (RN-01): "v1" e "V1" são o mesmo rótulo. Para cadastrar uma versão, basta o rótulo: ela é criada na fase Backlog e sem datas. Enquanto as datas não forem informadas (RN-09), a versão aparece com "TBD" no lugar da data de entrega (RN-13).

**RN-03: Ordem das aplicações e das versões.** *Aplica-se a: timeline, tabela de status das aplicações e status report congelado.* As aplicações seguem a ordem cronológica, pelo início do andamento mais antigo entre as suas versões; aplicações sem nenhuma data ficam por último. Em caso de empate, e entre as aplicações sem datas, vale a ordem em que foram cadastradas. A ordem das aplicações define a sequência das linhas da timeline, de cima para baixo, e a sequência das aplicações na tabela de status das aplicações, e é mantida no status report congelado.

Dentro de cada aplicação, as versões seguem a ordem cronológica, pelo início do andamento; versões sem datas ficam por último. Em caso de empate, e entre as versões sem datas, vale a ordem em que foram cadastradas.

A ordem das versões define:

- na **tabela de status das aplicações**, a sequência das linhas de cada aplicação;
- na **timeline**, a sequência das versões na coluna "Sem previsão" (RN-13), da esquerda para a direita, e a sequência das sub-linhas, de cima para baixo, quando versões se sobrepõem (RN-14). Na horizontal, as versões com datas ficam posicionadas pelas próprias datas.

No MVP, a ordem é sempre a cronológica. A ordenação manual é uma evolução futura (seção 12).

**RN-04: Exclusão.** *Aplica-se a: cadastro.* No MVP, aplicações e versões são excluídas definitivamente, após confirmação do usuário. Excluir uma aplicação exclui também todas as suas versões. Itens excluídos deixam de aparecer na lista de aplicações, na timeline, na tabela de status das aplicações e nos contadores, mas continuam nos status reports já congelados (RN-29). O arquivamento, com restauração, previsto no documento completo, é uma evolução futura (seção 12).

### 7.2 Fases

**RN-05: Fases possíveis.** *Aplica-se a: cadastro.* Uma versão está sempre em exatamente uma das fases: Backlog, Análise, Desenvolvimento, Testes, Homologação ou Produção.

**RN-06: Grupos visuais e cores.** *Aplica-se a: timeline e tabela de status das aplicações.* O sistema usa cinco grupos visuais, cada um com uma cor de referência:

| Grupo visual | Cor de referência | Na timeline, define a cor de | Nos contadores da tabela de status das aplicações, reúne as fases |
|---|---|---|---|
| Não iniciado | Cinza | Toda a barra de uma versão em Backlog (RN-16) | Backlog |
| Em andamento | Azul | Trecho do início ao fim do andamento | Análise, Desenvolvimento e Testes |
| Em homologação | Roxo | Trecho do início ao fim da homologação | Homologação |
| Homologação prorrogada | Vermelho | Trecho do início ao fim da prorrogação (RN-24) | Não é contado à parte; conta como Em homologação (RN-23) |
| Em produção | Verde | Marco de entrega e todas as etapas de uma versão em Produção (RN-15) | Produção |

Na tabela de status das aplicações, a coluna de fase mostra a fase detalhada (ex.: Testes). Cada cor tem um tom forte (realizado) e um tom claro (previsto), conforme RN-25.

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

### 7.3 Datas

**RN-09: Datas de uma versão.** *Aplica-se a: cadastro.* Uma versão possui até sete datas, cada uma com um **único valor**:

| Data | O que marca |
|---|---|
| Início do andamento | Início do andamento (desenhado em azul na timeline). |
| Fim do andamento | Fim do andamento. |
| Início da homologação | Início da homologação (desenhada em roxo na timeline). |
| Fim da homologação | Fim da homologação. |
| Início da prorrogação | Início da prorrogação da homologação (desenhada em vermelho na timeline). Opcional (RN-24). |
| Fim da prorrogação | Fim da prorrogação da homologação. Opcional (RN-24). |
| Data de entrega | Deploy em produção. É sempre a última data da versão. |

Não existe distinção entre data prevista e data realizada: enquanto o evento não acontece, a data representa o planejado; quando necessário, o usuário a altera, inclusive para refletir a data em que o evento de fato ocorreu. Uma versão tem nenhuma data ou as cinco datas do ciclo (início e fim do andamento, início e fim da homologação e data de entrega), informadas em conjunto. A timeline é um status executivo: uma versão só é apresentada com um plano completo, que pode ser replanejado a qualquer momento (RN-26). A prorrogação segue o mesmo princípio: início e fim informados juntos, ou nenhum dos dois (RN-24). As datas de cada etapa ficam registradas, permitindo no futuro outras visualizações, como uma visão da equipe de desenvolvimento.

**RN-10: Coerência cronológica.** *Aplica-se a: cadastro.* Quando informadas, as datas devem respeitar a ordem: início do andamento ≤ fim do andamento ≤ início da homologação ≤ fim da homologação ≤ início da prorrogação ≤ fim da prorrogação ≤ data de entrega. As etapas não se sobrepõem, mas pode haver intervalos entre elas. O sistema não deve aceitar datas fora dessa ordem. As datas da versão, incluindo as da prorrogação, são alteradas e salvas em conjunto, num único salvamento: se qualquer uma violar esta ordem, nenhuma alteração é salva.

**RN-13: Versão sem previsão (TBD).** *Aplica-se a: timeline e tabela de status das aplicações.* TBD significa que a versão ainda não tem datas (RN-09). Na **timeline**, ela não tem barra: aparece apenas como um rótulo (ex.: "v1.2 (TBD)") em uma coluna "Sem previsão", localizada à direita do último mês do eixo, na linha da sua aplicação. Na **tabela de status das aplicações**, a coluna de data de entrega mostra "TBD".

### 7.4 Composição da timeline

**RN-14: Composição da barra de uma versão.** *Aplica-se a: timeline.* A barra de uma versão em Análise, Desenvolvimento, Testes ou Homologação é desenhada exatamente a partir das datas informadas (RN-09):

1. **Em andamento (azul):** do início ao fim do andamento.
2. **Em homologação (roxo):** do início ao fim da homologação.
3. **Homologação prorrogada (vermelho):** do início ao fim da prorrogação, somente quando houver prorrogação registrada (RN-24).
4. **Entrega:** um marco (losango verde) na data de entrega.
5. **Intervalos:** os períodos entre etapas, e entre a última etapa e a entrega, são desenhados como uma linha pontilhada, mantendo a leitura de que pertencem à mesma versão.

Cada trecho, e também o marco de entrega, é dividido pela data atual em realizado e previsto (RN-25).

Em qualquer fase, versões da mesma aplicação cujos períodos, do início do andamento à data de entrega, não se sobrepõem ficam lado a lado na linha da aplicação. Quando há sobreposição, as versões são empilhadas em sub-linhas dentro da linha da aplicação: seguindo a ordem das versões (RN-03), cada versão ocupa a primeira sub-linha, de cima para baixo, em que não se sobrepõe a nenhuma outra. Para essa verificação, o período da versão inclui o espaço ocupado pelo seu rótulo (RN-18). Há espaço entre as sub-linhas, para que as barras não fiquem grudadas.

**RN-15: Versão entregue.** *Aplica-se a: timeline.* Quando a versão está em Produção, todas as suas etapas são desenhadas em verde ("Em produção"), em tom forte. Os intervalos continuam desenhados como linha pontilhada, e a barra termina no marco de entrega.

**RN-16: Versão em Backlog.** *Aplica-se a: timeline.* A barra de uma versão em Backlog com datas é desenhada inteiramente em cinza claro (previsto), do início do andamento até a data de entrega, terminando no marco de entrega, também em cinza claro, sem trechos de outras cores nem linhas pontilhadas. Sem datas, aplica-se a RN-13.

**RN-18: Rótulo da barra.** *Aplica-se a: timeline.* Cada versão tem um texto fixo (rótulo) com o nome da versão seguido da data de entrega, no formato dd/mm/aaaa, ou de "TBD" quando a versão não tiver datas (RN-13). Ex.: "v1 (23/10/2026)", "v1.2 (TBD)". Nas versões com barra, o rótulo é escrito dentro dela, alinhado ao início da parte visível; quando parte dela está fora do intervalo de meses (RN-20), ele fica no início da parte que aparece. Se a barra for mais curta que o rótulo, o texto continua à direita, até onde for preciso. As demais datas não aparecem escritas na timeline; elas definem apenas onde cada etapa começa e termina. Todas as datas da versão podem ser consultadas na tela de cadastro da versão. Mostrar os detalhes ao passar o mouse sobre a barra é uma evolução futura (seção 12).

**RN-19: Marcador de data atual.** *Aplica-se a: timeline.* A timeline desenha uma linha vertical tracejada na data atual, atravessando todas as aplicações. A data atual é a data do computador do usuário; no status report congelado, é a data de referência (RN-27).

**RN-20: Período da timeline.** *Aplica-se a: timeline.* O eixo horizontal da timeline é dividido em meses. Por padrão, o intervalo de meses mostrado é o **semestre atual**: de janeiro a junho, no primeiro semestre, ou de julho a dezembro, no segundo. O usuário pode alterar o intervalo (mês inicial e mês final) a qualquer momento. Uma barra que começa ou termina fora desse intervalo é cortada na borda correspondente; uma versão inteiramente fora dele não aparece na timeline. A coluna "Sem previsão" aparece sempre, independentemente do intervalo escolhido. Uma aplicação cujas versões estão todas fora do intervalo, sem nenhuma na coluna "Sem previsão", não aparece na timeline nesse intervalo, mas continua na tabela de status das aplicações e nos contadores. O intervalo de meses vale somente para a timeline: a tabela de status das aplicações e os contadores consideram todas as versões, independentemente do período.

Quando nenhuma versão aparece na timeline, o eixo de meses e a coluna "Sem previsão" continuam desenhados, e uma mensagem aparece centralizada, no lugar das barras:

| Situação | Mensagem |
|---|---|
| Nenhuma versão cadastrada | "Nenhuma versão cadastrada para as aplicações." |
| Existem versões, mas nenhuma no intervalo escolhido | "Nenhuma versão no período selecionado. Altere o intervalo de meses." |

### 7.5 Situação e observação

**RN-21: Situação.** *Aplica-se a: cadastro.* A situação de uma versão segue estas regras:

| Condição | Situação |
|---|---|
| Versão em Backlog | Não iniciado (automática) |
| Versão em Produção | Entregue (automática) |
| Demais fases | On track ou Risco, informada pelo usuário. Valor inicial: On track, inclusive quando a versão sai novamente do Backlog. |

No MVP, a situação On track ou Risco é sempre definida pelo usuário. A sugestão automática de Risco é uma evolução futura (seção 12).

**RN-22: Observação.** *Aplica-se a: cadastro.* Cada versão possui uma observação em texto livre, editável a qualquer momento pelo usuário. A tabela de status das aplicações mostra a observação atual, na coluna "Observação"; ela não aparece na timeline. O histórico de observações é uma evolução futura (seção 12).

### 7.6 Contadores

**RN-23: Contadores da tabela de status das aplicações.** *Aplica-se a: tabela de status das aplicações.* Acima da tabela de status das aplicações aparecem os contadores: o total de aplicações e a quantidade de aplicações em cada grupo: Não iniciadas, Em andamento, Em homologação e Em produção. Cada aplicação é contada uma única vez, no grupo da **sua versão mais avançada**, considerando a ordem Não iniciado → Em andamento → Em homologação → Em produção. Versões com homologação prorrogada contam como Em homologação. Aplicações sem versões contam como Não iniciadas. Assim, o total é sempre a soma dos quatro grupos.

### 7.7 Planejamento e acompanhamento

**RN-24: Prorrogação da homologação.** *Aplica-se a: cadastro.* A prorrogação é uma decisão de acompanhamento do projeto, registrada **intencionalmente** pelo usuário: ele informa o início e o fim da prorrogação, a partir do fim da homologação. Alterar as datas da homologação **nunca** gera prorrogação automaticamente; a homologação permanece roxa, refletindo os combinados do projeto. Uma nova prorrogação só pode ser registrada para versões em Homologação. Uma prorrogação já registrada pode ser alterada ou removida a qualquer momento, inclusive depois que a versão passa para Produção (ex.: para corrigir o fim da prorrogação de uma versão já entregue).

**RN-25: Realizado e previsto.** *Aplica-se a: timeline.* Na timeline, a data atual divide cada trecho da barra: a parte até a data atual, inclusive, é desenhada em tom forte (realizado), e a parte posterior, em tom claro (previsto). Um marco de entrega na própria data atual fica em tom forte. As linhas pontilhadas dos intervalos não são divididas em tons. Cada etapa mantém sua cor nos dois tons; o mesmo vale para o marco de entrega. Há duas exceções: a barra de uma versão em Produção é desenhada inteiramente em tom forte (RN-15), e a de uma versão em Backlog, inteiramente em cinza claro (RN-16).

**RN-26: Liberdade de replanejamento.** *Aplica-se a: cadastro.* Todas as datas de uma versão podem ser alteradas a qualquer momento pelo usuário, respeitando a coerência cronológica (RN-10), para acompanhar o monitoramento contínuo e os planos de ação do projeto.

### 7.8 Histórico semanal

**RN-27: Congelamento do status report.** *Aplica-se a: status report congelado.* Por ação do usuário, o sistema congela o status report da semana: guarda uma cópia do resultado já calculado da timeline (trechos, cores, rótulos, coluna "Sem previsão", intervalo de meses e linha da data atual posicionada na data de referência) e da tabela de status das aplicações (aplicação, versão, fase, situação, data de entrega, observação e contadores). Cada status report congelado registra a semana de referência (ex.: Semana 42/2026), a data de referência, o momento do congelamento e o intervalo de meses selecionado na timeline no momento do congelamento. A data de referência é sempre a terça-feira da semana de referência, mesmo quando o congelamento é feito em outro dia. O congelamento não é automático. Os detalhes exibidos ao passar o mouse sobre a barra, que o documento completo também guarda no congelado, ficam para a evolução correspondente (seção 12).

**RN-28: Um status report congelado por semana.** *Aplica-se a: status report congelado.* As semanas seguem a numeração ISO 8601 (semanas de segunda a domingo; a semana 1 é a que contém a primeira quinta-feira do ano). O ano faz parte do identificador porque, nas viradas de ano, alguns dias pertencem à semana do ano vizinho (ex.: 01/01/2027 pertence à Semana 53/2026). Ao congelar, o sistema usa a semana da data atual (RN-19). Cada semana de referência possui no máximo um status report congelado. O usuário pode congelar novamente a semana atual ou uma semana anterior já congelada, escolhida no Histórico de Status Reports (RN-30): o sistema pede confirmação e substitui o congelado daquela semana pelos dados atuais do cadastro, mantendo a terça-feira daquela semana como data de referência (RN-27) e o intervalo de meses do congelado original. O novo congelamento substitui o anterior por completo: o conteúdo e o momento do congelamento anteriores deixam de existir. Uma semana anterior que não foi congelada não pode ser congelada depois (RN-30). Restringir quem pode substituir congelados de semanas anteriores é uma evolução futura (seção 12).

**RN-29: Preservação do congelado.** *Aplica-se a: status report congelado.* Um status report congelado não é alterado automaticamente por mudanças posteriores no cadastro (datas, fases, observações, exclusões) e não pode ser editado campo a campo nem excluído pelo usuário. A única forma de mudá-lo é substituí-lo por um novo congelamento (RN-28). Aplicações e versões excluídas depois do congelamento continuam aparecendo nas semanas em que existiam, até que a semana seja substituída.

**RN-30: Histórico de Status Reports.** *Aplica-se a: status report congelado.* Na tela Histórico de Status Reports, o usuário consulta a lista de semanas, da mais recente para a mais antiga, desde a primeira semana congelada até a semana atual. Enquanto nenhuma semana tiver sido congelada, a lista mostra apenas a semana atual, que pode ser congelada. Cada semana congelada é identificada pela semana de referência, pelo intervalo de datas da semana, pela data de referência e pelo momento do congelamento (ex.: "Semana 42/2026 — 12/10 a 18/10/2026 — referência 13/10/2026 — congelado em 14/10/2026"). Uma semana que não foi congelada aparece na lista com a indicação "Sem status report congelado", pois seus dados não foram salvos (ex.: "Semana 40/2026 — 28/09 a 04/10/2026 — Sem status report congelado"). O usuário visualiza qualquer semana congelada em modo somente leitura, com indicação clara de que se trata de um status report congelado e de sua data de referência.

---

## 8. Requisitos funcionais do MVP

| ID | Requisito | Regras relacionadas |
|---|---|---|
| RF-01 | O usuário pode cadastrar, editar e excluir aplicações. | RN-01, RN-04 |
| RF-03 | O usuário pode cadastrar, editar e excluir versões de uma aplicação. | RN-02, RN-04 |
| RF-04 | O usuário pode alterar a fase de uma versão, respeitando o sentido das fases. | RN-05, RN-07, RN-08 |
| RF-05 | O usuário pode informar, alterar e remover as datas da versão (início e fim do andamento, início e fim da homologação e data de entrega). As datas são informadas, alteradas e removidas em conjunto, num único salvamento. | RN-07, RN-09, RN-10, RN-26 |
| RF-09 | O usuário pode registrar e editar a observação de uma versão. | RN-22 |
| RF-10 | O usuário pode definir a situação (On track ou Risco) de versões em andamento ou em homologação. | RN-21 |
| RF-11 | O usuário pode visualizar a timeline de entregas. | RN-03, RN-06, RN-13 a RN-20, RN-24, RN-25 |
| RF-12 | O usuário pode escolher o intervalo de meses mostrado na timeline. | RN-20 |
| RF-13 | O usuário pode visualizar a tabela de status das aplicações com os contadores. | RN-01, RN-03, RN-06, RN-13, RN-20 a RN-23 |
| RF-14 | O usuário pode registrar, alterar e remover a prorrogação da homologação de uma versão, informando o início e o fim da prorrogação. | RN-10, RN-24 |
| RF-15 | O usuário pode congelar o status report da semana atual e recongelar o da semana atual ou de semanas anteriores já congeladas. | RN-27, RN-28, RN-29 |
| RF-16 | O usuário pode consultar o Histórico de Status Reports, com a lista de semanas. | RN-30 |
| RF-17 | O usuário pode visualizar o status report congelado de qualquer semana, em modo somente leitura. | RN-29, RN-30 |

---

## 9. Requisitos não funcionais de negócio

| ID | Requisito |
|---|---|
| RNF-01 | O sistema será utilizado localmente, sem hospedagem. |
| RNF-02 | O sistema não deve gerar custos de licença ou de serviços. |
| RNF-03 | Interface e conteúdo em português do Brasil, com datas no formato dd/mm/aaaa e semanas no formato "Semana 42/2026". |
| RNF-04 | A timeline e a tabela de status das aplicações devem ser legíveis quando apresentadas por compartilhamento de tela em reuniões, ajustando-se dinamicamente ao tamanho da tela, sem limite fixo de aplicações e versões. |

---

## 10. Critérios de aceite do MVP

Os cenários abaixo usam dados fictícios. Datas não citadas em um cenário estão informadas e são coerentes com a RN-10.

**CA-02: Homologação prorrogada**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com andamento de 01/09/2026 a 25/09/2026, homologação de 01/10/2026 a 13/10/2026, prorrogação de 13/10/2026 a 23/10/2026 e data de entrega em 28/10/2026
- **E** a data atual é 15/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra azul forte de 01/09 a 25/09, linha pontilhada de 25/09 a 01/10, roxo forte de 01/10 a 13/10, vermelho forte de 13/10 a 15/10, vermelho claro de 15/10 a 23/10, linha pontilhada de 23/10 a 28/10 e o marco de entrega em tom claro em 28/10

**CA-03: Versão entregue**
- **Dado** que a versão v0 da aplicação Beta está em Produção, com andamento de 01/08/2026 a 14/08/2026, homologação de 17/08/2026 a 21/08/2026 e data de entrega em 24/08/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra, em verde forte, o andamento de 01/08 a 14/08, linha pontilhada de 14/08 a 17/08, a homologação de 17/08 a 21/08, linha pontilhada de 21/08 a 24/08 e o marco de entrega em 24/08
- **E** a situação da versão é Entregue.

**CA-04: Versão sem previsão**
- **Dado** que a versão v1.2 da aplicação Gama não possui nenhuma data
- **Quando** o usuário abre a timeline
- **Então** a versão aparece na coluna "Sem previsão" da linha da aplicação, com o rótulo "v1.2 (TBD)".

**CA-05: Coerência de datas**
- **Dado** que a versão v1 da aplicação Alfa tem início da homologação em 01/10/2026
- **Quando** o usuário tenta alterar o fim do andamento para 05/10/2026
- **Então** o sistema não aceita a data e informa que o fim do andamento não pode ser posterior ao início da homologação.

**CA-06: Versão em homologação não retorna de fase**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação
- **Quando** o usuário tenta alterar a fase para Desenvolvimento
- **Então** o sistema não aceita a alteração e informa que uma versão em homologação não retorna a fases anteriores, devendo eventuais extensões de prazo ser tratadas pelo fim da homologação ou por uma prorrogação.

**CA-07: Realizado e previsto em versão em andamento**
- **Dado** que a versão v1 da aplicação Beta está em Desenvolvimento, com andamento de 15/08/2026 a 20/10/2026, homologação de 23/10/2026 a 06/11/2026 e data de entrega em 10/11/2026
- **E** a data atual é 14/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra azul forte de 15/08 a 14/10, azul claro de 14/10 a 20/10, linha pontilhada de 20/10 a 23/10, roxo claro de 23/10 a 06/11, linha pontilhada de 06/11 a 10/11 e o marco de entrega em tom claro em 10/11.

**CA-08: Alteração das datas da homologação não gera prorrogação**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com homologação de 01/10/2026 a 13/10/2026 e nenhuma prorrogação registrada
- **E** a data atual é 15/10/2026
- **Quando** o usuário altera o fim da homologação para 23/10/2026
- **Então** a barra não tem trecho vermelho, mostrando roxo forte de 01/10 a 15/10 e roxo claro de 15/10 a 23/10

**CA-09: Desenho conforme as datas informadas**
- **Dado** que a versão v1 da aplicação Gama está em Desenvolvimento, com andamento de 15/09/2026 a 10/10/2026, homologação de 15/10/2026 a 30/10/2026 e data de entrega em 05/11/2026
- **E** a data atual é 14/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra azul forte de 15/09 a 10/10, linha pontilhada de 10/10 a 15/10, roxo claro de 15/10 a 30/10, linha pontilhada de 30/10 a 05/11 e o marco de entrega em tom claro em 05/11
- **E** o trecho azul não é estendido além de 10/10, mesmo com a versão ainda em Desenvolvimento

**CA-10: Status report congelado não muda**
- **Dado** que em 09/10/2026 o usuário congelou o status report, quando a versão v1 da aplicação Alfa tinha data de entrega 13/10/2026
- **E** depois disso a data de entrega foi alterada para 23/10/2026
- **Quando** o usuário consulta o status report da Semana 41/2026 (05/10/2026 a 11/10/2026)
- **Então** a versão aparece com data de entrega 13/10/2026 e o marcador da data atual em 06/10/2026, terça-feira da Semana 41/2026.

**CA-11: Novo congelamento na mesma semana**
- **Dado** que existe um status report congelado em 13/10/2026
- **Quando** o usuário congela novamente em 15/10/2026, na mesma semana
- **Então** o sistema pede confirmação e, se confirmado, substitui o status report da Semana 42/2026 (12/10/2026 a 18/10/2026) pelos dados atuais do cadastro
- **E** mantém a data de referência 13/10/2026, e o momento do congelamento passa a ser 15/10/2026.

**CA-12: Semanas anteriores preservadas**
- **Dado** que existe um status report congelado na Semana 41/2026 (05/10/2026 a 11/10/2026)
- **Quando** o usuário congela o status report em 14/10/2026
- **Então** é criado o status report da Semana 42/2026 (12/10/2026 a 18/10/2026), com data de referência 13/10/2026 e congelado em 14/10/2026
- **E** o da semana anterior permanece inalterado.

**CA-13: Versão sem datas permanece em Backlog**
- **Dado** que a versão v2 da aplicação Alfa está em Backlog e não possui datas
- **Quando** o usuário tenta alterar a fase para Análise
- **Então** o sistema não aceita a alteração e informa que é preciso informar as datas da versão antes de iniciá-la.

**CA-14: Aplicação sem versões**
- **Dado** que a aplicação Delta foi cadastrada e não possui nenhuma versão
- **Quando** o usuário abre a timeline e a tabela de status das aplicações
- **Então** a aplicação Delta não aparece na timeline
- **E** aparece na tabela de status das aplicações, em uma linha com a indicação "Nenhuma versão cadastrada"
- **E** é contada como Não iniciada nos contadores.

**CA-15: Versão parcialmente fora do período**
- **Dado** que a versão v1 da aplicação Beta tem andamento de 15/08/2026 a 20/10/2026
- **E** o intervalo de meses da timeline é de setembro a dezembro de 2026
- **Quando** o usuário abre a timeline
- **Então** a etapa de andamento aparece a partir de 01/09, cortada na borda inicial do período.

**CA-20: Remoção de datas fora do Backlog**
- **Dado** que a versão v2 da aplicação Alfa está em Desenvolvimento, com as datas informadas
- **Quando** o usuário tenta remover as datas
- **Então** o sistema não aceita e informa que é preciso mudar a fase da versão para Backlog antes de remover as datas.

**CA-21: Contagem da aplicação pela versão mais avançada**
- **Dado** que a aplicação Alfa tem a versão v0 em Produção e a versão v1 em Desenvolvimento
- **E** a aplicação Beta tem apenas a versão v1 em Homologação
- **Quando** o usuário abre a tabela de status das aplicações
- **Então** os contadores mostram 2 aplicações no total, 1 em produção (Alfa) e 1 em homologação (Beta).

**CA-22: Rótulo da barra**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com andamento de 01/09/2026 a 25/09/2026, homologação de 01/10/2026 a 13/10/2026 e data de entrega em 20/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra o rótulo "v1 (20/10/2026)"

**CA-23: Intervalo padrão da timeline**
- **Dado** que a data atual é 14/10/2026
- **Quando** o usuário abre a timeline
- **Então** o eixo mostra os meses de julho a dezembro de 2026
- **E** o usuário pode alterar o intervalo de meses.

**CA-24: Alteração da observação**
- **Dado** que a versão v1 da aplicação Alfa tem a observação "Homologação em andamento."
- **Quando** o usuário altera a observação para "Acesso sendo liberado em lotes."
- **Então** a tabela de status das aplicações mostra "Acesso sendo liberado em lotes."

**CA-25: Datas informadas em conjunto**
- **Dado** que a versão v3 da aplicação Alfa está em Backlog e não possui datas
- **Quando** o usuário informa apenas a data de entrega, 30/11/2026
- **Então** o sistema não aceita e informa que as datas de andamento, de homologação e de entrega devem ser informadas em conjunto.

**CA-26: Recongelamento de semana anterior**
- **Dado** que a Semana 41/2026 (05/10/2026 a 11/10/2026) foi congelada em 09/10/2026, quando a versão v1 da aplicação Alfa tinha data de entrega 13/10/2026
- **E** depois disso a data de entrega foi alterada para 23/10/2026
- **Quando** o usuário recongela a Semana 41/2026 em 20/10/2026 e confirma
- **Então** o status report da Semana 41/2026 passa a mostrar a data de entrega 23/10/2026, com o marcador da data atual em 06/10/2026
- **E** o Histórico de Status Reports indica "congelado em 20/10/2026".

**CA-27: Aplicação sem versões no período**
- **Dado** que a aplicação Alfa tem apenas a versão v0, com data de entrega em 20/03/2026
- **E** o intervalo de meses da timeline é de julho a dezembro de 2026
- **Quando** o usuário abre a timeline e a tabela de status das aplicações
- **Então** a aplicação Alfa não aparece na timeline
- **E** aparece na tabela de status das aplicações e é considerada nos contadores.

**CA-28: Timeline sem versões no período**
- **Dado** que todas as versões cadastradas têm datas entre janeiro e junho de 2026
- **E** o intervalo de meses da timeline é de julho a dezembro de 2026
- **Quando** o usuário abre a timeline
- **Então** nenhuma linha de aplicação aparece
- **E** a mensagem "Nenhuma versão no período selecionado. Altere o intervalo de meses." aparece centralizada, no lugar das barras, com o eixo de meses e a coluna "Sem previsão" desenhados.

**CA-29: Timeline sem versões cadastradas**
- **Dado** que nenhuma aplicação possui versões cadastradas
- **Quando** o usuário abre a timeline
- **Então** a mensagem "Nenhuma versão cadastrada para as aplicações." aparece centralizada, no lugar das barras.

**CA-30: Semana sem status report congelado**
- **Dado** que as Semanas 39/2026 e 41/2026 foram congeladas e a Semana 40/2026 (28/09/2026 a 04/10/2026) não foi
- **Quando** o usuário abre o Histórico de Status Reports
- **Então** a Semana 40/2026 aparece com a indicação "Sem status report congelado"
- **E** ela não pode ser aberta nem congelada.

**CA-31: Versões sobrepostas**
- **Dado** que a aplicação Alfa tem a versão v1, com andamento de 01/09/2026 a 25/09/2026, homologação de 01/10/2026 a 13/10/2026 e data de entrega em 20/10/2026
- **E** a versão v2, com andamento de 15/09/2026 a 30/11/2026, homologação de 01/12/2026 a 15/12/2026 e data de entrega em 18/12/2026
- **Quando** o usuário abre a timeline
- **Então** a v1 aparece na primeira sub-linha da aplicação Alfa e a v2 na sub-linha abaixo, com espaço entre elas.

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
- **E** a versão aparece na coluna "Sem previsão" da timeline, com o rótulo "v2 (TBD)".

**CA-36: Prorrogação fora da Homologação**
- **Dado** que a versão v1 da aplicação Beta está em Desenvolvimento
- **Quando** o usuário tenta registrar uma prorrogação
- **Então** o sistema não aceita e informa que a prorrogação só pode ser registrada para versões em Homologação.

**CA-37: Versão em Backlog**
- **Dado** que a versão v3 da aplicação Gama está em Backlog, com andamento de 01/11/2026 a 20/11/2026, homologação de 23/11/2026 a 04/12/2026 e data de entrega em 10/12/2026
- **Quando** o usuário abre a timeline
- **Então** a barra é desenhada inteiramente em cinza claro, de 01/11 a 10/12, sem linhas pontilhadas, terminando no marco de entrega em cinza claro.

**CA-38: Exclusão de aplicação (somente no MVP)**
- **Dado** que a aplicação Delta tem as versões v1 e v2
- **E** a Semana 41/2026 foi congelada com a aplicação Delta
- **Quando** o usuário exclui a aplicação Delta e confirma
- **Então** a aplicação Delta e suas versões deixam de existir no cadastro, na timeline, na tabela de status das aplicações e nos contadores
- **E** continuam aparecendo no status report congelado da Semana 41/2026.

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

**CA-41: Histórico de Status Reports antes do primeiro congelamento**
- **Dado** que nenhuma semana foi congelada
- **E** a data atual é 14/10/2026
- **Quando** o usuário abre o Histórico de Status Reports
- **Então** a lista mostra apenas a Semana 42/2026 (12/10/2026 a 18/10/2026), com a indicação "Sem status report congelado"
- **E** essa semana pode ser congelada.

**CA-42: Correção da prorrogação de uma versão entregue**
- **Dado** que a versão v1 da aplicação Alfa está em Produção, com fim da homologação em 13/10/2026, prorrogação de 13/10/2026 a 23/10/2026 e data de entrega em 28/10/2026
- **E** a data atual é 30/10/2026
- **Quando** o usuário altera o fim da prorrogação para 22/10/2026 e salva
- **Então** o sistema aceita a alteração
- **E**, para a versão v0 da aplicação Beta, em Produção e sem prorrogação, o sistema não aceita o registro de uma nova prorrogação e informa que a prorrogação só pode ser registrada para versões em Homologação.

---

## 11. Premissas e restrições

- Haverá um único usuário responsável pela manutenção dos dados.
- A carga inicial das aplicações e versões existentes será feita manualmente pelo usuário. Quando a data real de uma versão antiga tiver se perdido, o usuário informa a melhor data conhecida para cada uma das cinco datas (RN-09) e pode registrar isso na observação.

---

## 12. Evoluções futuras

O MVP foi desenhado para crescer sem retrabalho: o cadastro já guarda todas as datas de cada etapa, a ordem padrão já existe para receber a ordenação manual, e a timeline e a tabela de status das aplicações já são montadas pela API, o que permite acrescentar filtros e novas visões sem mudar as regras existentes. As evoluções abaixo estão especificadas ou registradas no documento completo e podem ser implementadas em versões seguintes, em qualquer ordem.

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
| 0.1 | 14/10/2026 | Versão inicial do MVP, recortada da versão 0.61 do documento completo. |
| 0.2 | 14/10/2026 | Revisão da versão 0.1: grupos visuais e cores (glossário, RN-06); datas da versão informadas e removidas em conjunto (RN-02, RN-07, RN-09, RN-13, RN-14, RN-16, RN-18, RF-05, CA-09, CA-13, CA-20, CA-25); histórico ao informar e remover datas (RN-12); data atual do computador do usuário (glossário, RN-19); aplicação sem versões no período e mensagens da timeline sem versões (RN-20, CA-27 a CA-29); recongelamento de semanas e terça-feira como data de referência (OBJ-06, glossário, RN-27 a RN-30, RF-15, CA-10 a CA-12, CA-26); alinhamentos em RN-24, RN-25 e CA-06; novas evoluções futuras (seção 12); separadores duplicados removidos. Alinhada à versão 0.64 do documento completo. |
| 0.3 | 14/10/2026 | Segunda revisão: versões sobrepostas em sub-linhas (glossário, RN-03, RN-14, CA-31); datas alteradas e salvas em conjunto, tudo ou nada (RN-10, RF-05, CA-32); remoção de datas somente em Backlog (RN-07, CA-20, CA-35); situação ao sair novamente do Backlog (RN-21); dia atual como realizado (glossário, RN-25); ordem com desempate pelo cadastro (RN-03); rótulo com ano (RN-18, CA-22); intervalo de meses no congelamento e no recongelamento, substituição completa e semanas sem congelado na lista (glossário, RN-27, RN-28, RN-30, CA-11, CA-26, CA-30); definição de etapa e de fase, objetivo OBJ-04, colunas da tabela de status das aplicações, intervalo de meses só na timeline (glossário, RN-20); RNF-04; nota de datas nos critérios de aceite e CA-05; premissa da carga inicial; referências do RF-11 e do RF-13; novos CA-33, CA-34, CA-36 e CA-37; exclusão definitiva no lugar do arquivamento (escopo, RN-04, RN-20, RN-23, RN-29, RF-01, RF-03, CA-38); históricos de datas e de observações retirados do MVP, junto com o motivo das alterações (introdução, contexto, OBJ-03, escopo, RN-12, RN-22, RN-24, RN-26, RF-05, RF-08, RF-09, RF-14, CA-01, CA-08, CA-24); backup retirado do MVP (RNF-05); nota sobre os detalhes ao passar o mouse (RN-27); novas evoluções na seção 12. Alinhada à versão 0.65 do documento completo. |
| 0.4 | 14/10/2026 | Versão em Produção somente com data de entrega até a data atual (RN-07, CA-39); intervalos pontilhados também em Produção (RN-06, RN-15, CA-03); rótulo dentro da barra (RN-14, RN-18). Alinhada à versão 0.66 do documento completo. |
| 0.5 | 14/10/2026 | Tela de consulta às semanas anteriores renomeada para Histórico de Status Reports e incluída no glossário (escopo, RN-28, RN-30, RF-16, CA-26, CA-30). Alinhada à versão 0.67 do documento completo. |
| 0.6 | 15/10/2026 | Revisão de arquitetura: avanço para qualquer fase seguinte, sem passar pelas intermediárias (RN-08, CA-40); nomes e rótulos comparados sem diferença entre maiúsculas e minúsculas e sem os espaços das pontas (RN-01, RN-02, CA-33, CA-34); prorrogação já registrada pode ser alterada ou removida também em Produção (RN-24, CA-42); Histórico de Status Reports antes do primeiro congelamento (RN-30, CA-41); "versões ativas", termo do arquivamento, substituído por "versões cadastradas" (CA-28, CA-29). Alinhada à versão 0.68 do documento completo. |

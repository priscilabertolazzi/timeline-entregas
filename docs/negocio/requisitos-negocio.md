# Documento de Requisitos de Negócio
## Timeline de Entregas de Projeto

| Item | Valor |
|---|---|
| Versão | 0.68 |
| Data | 15/10/2026 |
| Status | Referência: visão completa do produto. O escopo construído é o do `requisitos-mvp.md`. |
| Escopo do documento | Regras e requisitos de negócio. Decisões técnicas (arquitetura, API, banco de dados) ficam no Documento Técnico. |

---

## 1. Introdução

Este documento define os requisitos de negócio do **Timeline de Entregas**, um sistema que digitaliza a manutenção do status report semanal de um projeto de software composto por várias aplicações. Embora o contexto deste documento seja o acompanhamento de aplicações, o mesmo modelo pode ser usado em outros contextos, acompanhando, por exemplo, milestones ou outros tipos de entregável.

Hoje, o status report é montado manualmente em PowerPoint toda semana. Com o sistema, as informações de cada aplicação passam a ser registradas uma única vez, de forma estruturada, e a partir delas o sistema gera automaticamente:

- a **timeline de entregas**, mostrando a fase de cada versão de cada aplicação ao longo do tempo;
- a **tabela de status das aplicações**, com a fase, a situação, a data estimada de entrega e a observação de cada versão de cada aplicação;
- o **histórico das mudanças de datas**, com seus motivos;
- uma **cópia congelada do status report de cada semana**, para consulta futura.

O documento trata apenas do negócio: problema, objetivos, escopo e regras. As decisões técnicas estão no documento de arquitetura. Os identificadores (RN, RF, RNF, CA, QA) são mantidos estáveis entre versões, para que possam ser referenciados no desenvolvimento e nos testes.

---

## 2. Contexto e problema

### 2.1 O projeto acompanhado

O projeto entrega um conjunto de aplicações. Cada aplicação evolui em **versões** (v0, v1, v1.2...), e cada versão passa pelas seguintes fases até chegar aos usuários:

| Fase no status report | O que inclui |
|---|---|
| Não iniciada | Versão planejada, ainda no backlog. |
| Em andamento | Análise, desenvolvimento e testes. |
| Em homologação | Validação da versão pelos usuários principais, incluindo as correções e os desenvolvimentos necessários. |
| Em produção | Versão entregue e disponível para uso. |

Para cada versão, a informação mais importante é a **data estimada de entrega para produção**, quando a versão é implantada em produção após o fim da homologação. Essas datas mudam ao longo do projeto, conforme o monitoramento contínuo e os planos de ação, e as mudanças relevantes precisam ser registradas com seus motivos.

### 2.2 Como o acompanhamento é feito hoje

Toda semana, o responsável pelo status report atualiza um arquivo PowerPoint que contém, entre outros, dois slides mantidos manualmente:

- **Cronograma de Entregas**: uma timeline com uma linha por aplicação e barras coloridas por versão, indicando a fase de cada uma ao longo do tempo.
- **Status das Aplicações**: uma tabela com versão, fase, data de entrega estimada, situação e observação de cada versão, além de contadores com o total de aplicações e a quantidade de aplicações em cada fase.

O arquivo de cada semana é guardado, formando o histórico usado quando é preciso recuperar a situação do projeto em uma semana anterior.

### 2.3 Problemas

1. **Retrabalho semanal**: cada mudança de data ou fase exige ajustar barras, rótulos e tabela manualmente.
2. **Falta de precisão no desenho**: os critérios de cada trecho são claros, mas as barras e seus trechos são posicionados manualmente no PowerPoint, e a timeline nunca fica 100% alinhada às datas reais.
3. **Informação duplicada**: a mesma informação é mantida em mais de um slide, com risco de inconsistência entre eles.
4. **Histórico manual**: replanejamentos de data precisam ser registrados à mão nas observações (ex.: "Final da homologação alterado de 13/10 para 23/10").

### 2.4 Alternativas avaliadas

O uso da própria ferramenta de gestão do projeto (Jira) para gerar a timeline foi avaliado e descartado, pelos seguintes motivos:

- **Não atende às particularidades do projeto**: a visualização de timeline da ferramenta não representa os detalhes que o status report precisa mostrar, como as fases agrupadas, a separação entre realizado e previsto e a prorrogação da homologação.
- **Não cabe em um slide**: a visualização fica grande demais para uma apresentação executiva.
- **Linguagem técnica**: as convenções adotadas na ferramenta são técnicas e precisariam ser traduzidas para a linguagem de um status report executivo.
- **Sem extensões adequadas**: não há, no ambiente disponível, extensões ou painéis que ofereçam a visualização desejada para o status report executivo.

### 2.5 Solução proposta

Digitalizar a entrada de informações: o responsável registra aplicações, versões, fases, datas e motivos de mudança, e o sistema desenha a timeline, monta a tabela de status das aplicações, mantém o histórico de alterações e congela o status report de cada semana. A informação é registrada uma vez e todas as visões são geradas a partir dela.

---

## 3. Objetivos

| ID | Objetivo |
|---|---|
| OBJ-01 | Manter um único cadastro de aplicações e versões, do qual a timeline e a tabela de status das aplicações são geradas automaticamente. |
| OBJ-02 | Eliminar o posicionamento manual: a posição e as cores das barras devem ser calculadas a partir das datas e fases. |
| OBJ-03 | Registrar automaticamente o histórico de alterações de datas de cada versão, com seus motivos. |
| OBJ-04 | Preservar a convenção do projeto: fases detalhadas na tabela de status das aplicações e etapas coloridas por grupo visual na timeline, com a correspondência definida no sistema (RN-06). |
| OBJ-05 | Reduzir o tempo gasto na atualização semanal do status report. |
| OBJ-06 | Manter o histórico semanal do status report, permitindo recuperar exatamente como a timeline e a tabela de status das aplicações estavam em qualquer semana anterior, salvo quando o usuário substituir intencionalmente o status report de uma semana (RN-28). |
| OBJ-07 | Entregar uma primeira versão das APIs do sistema pronta para ser evoluída e incorporada ao projeto acompanhado como uma aplicação interna, seguindo os padrões de arquitetura adotados nele. |
| OBJ-08 | **Evolução futura:** calcular as datas de início e fim de cada etapa a partir de estimativas de duração em dias, considerando um calendário de dias úteis e não úteis em nível nacional, estadual e municipal, além de datas livres definidas pelo usuário. |

**Indicadores de sucesso**

- Tempo de atualização semanal da timeline e da tabela de status das aplicações, comparado ao processo atual, que consome pelo menos 1 hora por semana.
- Esforço para iniciar o acompanhamento de um novo projeto: a criação do modelo atual do status report levou entre 12 e 16 horas; com o sistema, basta cadastrar as aplicações e versões, sem construir um novo modelo.
- Nenhum ajuste manual de posicionamento ou cor necessário após a atualização dos dados.
- Timeline e tabela de status das aplicações sempre consistentes entre si.

---

## 4. Escopo

### 4.1 Dentro do escopo

- Cadastro de aplicações e de suas versões.
- Registro de fases e datas de cada versão.
- Histórico automático de alterações de datas, com motivos, e de observações.
- Registro intencional de prorrogação da homologação.
- Observação livre por versão.
- Visualização da timeline.
- Visualização da tabela de status das aplicações, com contadores.
- Congelamento semanal do status report e consulta às semanas anteriores pelo Histórico de Status Reports.
- Filtros de visualização: período, aplicações, versões entregues, fase e situação.

### 4.2 Fora do escopo (nesta versão)

- Conteúdo textual do status report: destaques, riscos, planos de ação e próximos passos.
- Múltiplos usuários, autenticação e controle de acesso.
- Hospedagem: o sistema será usado localmente.
- Exportação para PowerPoint, PDF ou imagem. O histórico semanal é mantido e consultado dentro do próprio sistema (seção 7.8).
- Integração com ferramentas de gestão (ex.: Jira, Azure DevOps).
- Incorporação ao ambiente do projeto como aplicação interna. Esta versão é preparada para isso (OBJ-07), mas a incorporação em si é uma evolução futura.
- Notificações e alertas.
- Calendário de dias úteis e cálculo automático de datas a partir de estimativas de duração (evolução futura, OBJ-08).

Itens fora do escopo podem ser reavaliados em versões futuras. As evoluções já identificadas estão na seção 13.

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
| Lista de aplicações | Tela de cadastro onde o usuário mantém as aplicações e define a ordem de exibição. Não faz parte do status report. |
| Status report congelado | Cópia somente leitura da timeline e da tabela de status das aplicações de uma semana, com os dados do cadastro no momento do último congelamento e a terça-feira da semana como data atual. |
| Semana de referência | Semana, de segunda-feira a domingo, à qual um status report congelado pertence. É identificada pelo número da semana no padrão ISO 8601 e pelo ano, exibidos em português no formato "Semana 42/2026". |
| Data atual | Data do computador do usuário. Na timeline, é a data que divide realizado e previsto. No status report congelado, a data atual é a data de referência. |
| Data de referência | Terça-feira da semana de referência. É a data atual do status report congelado: nela ficam a linha da data atual e a divisão entre realizado e previsto. |
| Momento do congelamento | Data e hora do último congelamento de uma semana. Quando a semana é congelada novamente, o momento anterior deixa de existir. |
| Histórico de Status Reports | Tela que lista as semanas, da mais recente para a mais antiga, desde a primeira semana congelada até a semana atual. Nela o usuário abre um status report congelado, congela a semana atual ou congela novamente uma semana já congelada (RN-28, RN-30). Não faz parte do status report. |

---

## 7. Regras de negócio

Cada regra indica onde se aplica: no **cadastro** (dados informados pelo usuário), na **timeline**, na **tabela de status das aplicações**, no **status report congelado** ou em mais de um deles. Todo dado cadastrado alimenta a timeline e a tabela de status das aplicações. Por isso, regras de cadastro são marcadas apenas como "cadastro"; quando uma delas tem um efeito específico em alguma visão, o próprio texto da regra o descreve.

### 7.1 Aplicações e versões

**RN-01: Identificação e cadastro da aplicação.** *Aplica-se a: cadastro.* Toda aplicação possui um nome, que deve ser único no sistema. Na comparação, os espaços no início e no fim são desconsiderados e não há diferença entre maiúsculas e minúsculas: "Alfa", "alfa" e "Alfa " são o mesmo nome. O nome é guardado sem os espaços das pontas. Uma aplicação pode ser cadastrada sem versões. Nesse caso, ela aparece na lista de aplicações e na tabela de status das aplicações, em uma linha única com a indicação "Nenhuma versão cadastrada", mas **não aparece na timeline** até ter pelo menos uma versão cadastrada.

**RN-02: Identificação e cadastro da versão.** *Aplica-se a: cadastro.* Toda versão pertence a uma única aplicação e possui um rótulo (ex.: v1.2). Uma aplicação pode ter quantas versões forem necessárias, desde que cada rótulo seja único dentro dela, comparado da mesma forma que o nome da aplicação (RN-01): "v1" e "V1" são o mesmo rótulo. Para cadastrar uma versão, basta o rótulo: ela é criada na fase Backlog e sem datas. Enquanto as datas não forem informadas (RN-09), a versão aparece com "TBD" no lugar da data de entrega (RN-13).

**RN-03: Ordem das aplicações e das versões.** *Aplica-se a: timeline, tabela de status das aplicações e status report congelado.* As aplicações seguem, **por padrão**, a ordem cronológica, pelo início do andamento mais antigo entre as suas versões; aplicações sem nenhuma data ficam por último. Em caso de empate, e entre as aplicações sem datas, vale a ordem em que foram cadastradas. O usuário pode **alterar manualmente** a ordem das aplicações a qualquer momento. A partir daí, a ordem manual prevalece, mesmo que as datas mudem; uma aplicação nova entra no final, e o usuário pode restaurar a ordem padrão quando quiser.

A ordem das aplicações define a sequência das linhas da timeline, de cima para baixo, e a sequência das aplicações na tabela de status das aplicações, e é mantida no status report congelado.

Dentro de cada aplicação, as versões seguem, **por padrão**, a ordem cronológica, pelo início do andamento; versões sem datas ficam por último. Em caso de empate, e entre as versões sem datas, vale a ordem em que foram cadastradas. O usuário pode **alterar manualmente** a ordem das versões de uma aplicação a qualquer momento. A partir daí, a ordem manual prevalece, mesmo que as datas mudem; uma versão nova entra no final da aplicação, e o usuário pode restaurar a ordem padrão quando quiser.

A ordem das versões define:

- na **tabela de status das aplicações**, a sequência das linhas de cada aplicação;
- na **timeline**, a sequência das versões na coluna "Sem previsão" (RN-13), da esquerda para a direita, e a sequência das sub-linhas, de cima para baixo, quando versões se sobrepõem (RN-14). Na horizontal, as versões com datas ficam posicionadas pelas próprias datas.

**RN-04: Arquivamento.** *Aplica-se a: cadastro.* Aplicações e versões não são apagadas; são arquivadas. Itens arquivados não aparecem na timeline nem na tabela de status das aplicações e não são considerados nos contadores, mas seus dados e histórico são preservados e podem ser restaurados.

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

**RN-11:** *Removida na versão 0.27. Cada data da versão é um valor único, sem distinção entre data confirmada e aproximada.*

**RN-12: Histórico de alterações de datas.** *Aplica-se a: cadastro.* Toda alteração de qualquer uma das datas da versão (RN-09), incluindo o registro e a remoção da prorrogação, gera automaticamente um registro contendo qual data foi alterada, o valor anterior, o novo valor e o momento da alteração. Informar as datas pela primeira vez e removê-las também são alterações: geram um registro para cada data, com valor anterior vazio, no primeiro caso, ou novo valor vazio, no segundo. O usuário pode, opcionalmente, informar um motivo. O histórico não pode ser editado nem apagado pelo usuário.

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

**RN-17:** *Removida na versão 0.27, junto com a RN-11.*

**RN-18: Rótulo e detalhes da barra.** *Aplica-se a: timeline.* Cada versão tem um texto fixo (rótulo) com o nome da versão seguido da data de entrega, no formato dd/mm/aaaa, ou de "TBD" quando a versão não tiver datas (RN-13). Ex.: "v1 (23/10/2026)", "v1.2 (TBD)". Nas versões com barra, o rótulo é escrito dentro dela, alinhado ao início da parte visível; quando parte dela está fora do intervalo de meses (RN-20), ele fica no início da parte que aparece. Se a barra for mais curta que o rótulo, o texto continua à direita, até onde for preciso. As demais datas não aparecem escritas na timeline; elas definem apenas onde cada etapa começa e termina.

Ao passar o mouse sobre a barra, o sistema mostra os detalhes da versão: todas as datas informadas (RN-09), no formato dd/mm/aaaa, e a fase atual.

**RN-19: Marcador de data atual.** *Aplica-se a: timeline.* A timeline desenha uma linha vertical tracejada na data atual, atravessando todas as aplicações. A data atual é a data do computador do usuário; no status report congelado, é a data de referência (RN-27).

**RN-20: Período da timeline.** *Aplica-se a: timeline.* O eixo horizontal da timeline é dividido em meses. Por padrão, o intervalo de meses mostrado é o **semestre atual**: de janeiro a junho, no primeiro semestre, ou de julho a dezembro, no segundo. O usuário pode alterar o intervalo (mês inicial e mês final) a qualquer momento. Uma barra que começa ou termina fora desse intervalo é cortada na borda correspondente; uma versão inteiramente fora dele não aparece na timeline. A coluna "Sem previsão" aparece sempre, independentemente do intervalo escolhido. Uma aplicação cujas versões estão todas fora do intervalo, sem nenhuma na coluna "Sem previsão", não aparece na timeline nesse intervalo, mas continua na tabela de status das aplicações e nos contadores. O intervalo de meses vale somente para a timeline: a tabela de status das aplicações e os contadores consideram todas as versões, independentemente do período.

Quando nenhuma versão aparece na timeline, o eixo de meses e a coluna "Sem previsão" continuam desenhados, e uma mensagem aparece centralizada, no lugar das barras:

| Situação | Mensagem |
|---|---|
| Nenhuma versão cadastrada, ou todas arquivadas | "Nenhuma versão cadastrada para as aplicações." |
| Existem versões, mas nenhuma no intervalo escolhido | "Nenhuma versão no período selecionado. Altere o intervalo de meses." |

### 7.5 Situação e observação

**RN-21: Situação.** *Aplica-se a: cadastro.* A situação de uma versão segue estas regras:

| Condição | Situação |
|---|---|
| Versão em Backlog | Não iniciado (automática) |
| Versão em Produção | Entregue (automática) |
| Demais fases | On track ou Risco, informada pelo usuário. Valor inicial: On track, inclusive quando a versão sai novamente do Backlog. |

O sistema deve **sugerir** a situação Risco, cabendo ao usuário confirmar, quando: uma prorrogação for registrada (RN-24); ou a data atual ultrapassar o fim de uma etapa sem que a versão tenha avançado de fase (ex.: fim do andamento ultrapassado com a versão ainda em Desenvolvimento, ou fim da homologação ultrapassado com a versão ainda em Homologação). A sugestão é um lembrete para o usuário atualizar as datas ou a fase.

**RN-22: Observação e seu histórico.** *Aplica-se a: cadastro.* Cada versão possui uma observação em texto livre, editável a qualquer momento pelo usuário. A tabela de status das aplicações mostra sempre a observação atual, na coluna "Observação"; ela não aparece na timeline. Toda alteração da observação gera automaticamente um registro no histórico da versão, com o texto anterior, o novo texto e o momento da alteração, permitindo consultar todas as observações anteriores. Assim como o histórico de datas (RN-12), o histórico de observações não pode ser editado nem apagado pelo usuário.

### 7.6 Contadores

**RN-23: Contadores da tabela de status das aplicações.** *Aplica-se a: tabela de status das aplicações.* Acima da tabela de status das aplicações aparecem os contadores: o total de aplicações e a quantidade de aplicações em cada grupo: Não iniciadas, Em andamento, Em homologação e Em produção. Cada aplicação é contada uma única vez, no grupo da **sua versão mais avançada**, considerando a ordem Não iniciado → Em andamento → Em homologação → Em produção. Versões com homologação prorrogada contam como Em homologação. Versões arquivadas não são consideradas. Aplicações sem versões contam como Não iniciadas. Assim, o total é sempre a soma dos quatro grupos.

### 7.7 Planejamento e acompanhamento

**RN-24: Prorrogação da homologação.** *Aplica-se a: cadastro.* A prorrogação é uma decisão de acompanhamento do projeto, registrada **intencionalmente** pelo usuário: ele informa o início e o fim da prorrogação, a partir do fim da homologação, e um motivo opcional. Alterar as datas da homologação **nunca** gera prorrogação automaticamente; a homologação permanece roxa, refletindo os combinados do projeto. Uma nova prorrogação só pode ser registrada para versões em Homologação. Uma prorrogação já registrada pode ser alterada ou removida a qualquer momento, inclusive depois que a versão passa para Produção (ex.: para corrigir o fim da prorrogação de uma versão já entregue).

**RN-25: Realizado e previsto.** *Aplica-se a: timeline.* Na timeline, a data atual divide cada trecho da barra: a parte até a data atual, inclusive, é desenhada em tom forte (realizado), e a parte posterior, em tom claro (previsto). Um marco de entrega na própria data atual fica em tom forte. As linhas pontilhadas dos intervalos não são divididas em tons. Cada etapa mantém sua cor nos dois tons; o mesmo vale para o marco de entrega. Há duas exceções: a barra de uma versão em Produção é desenhada inteiramente em tom forte (RN-15), e a de uma versão em Backlog, inteiramente em cinza claro (RN-16).

**RN-26: Liberdade de replanejamento.** *Aplica-se a: cadastro.* Todas as datas de uma versão podem ser alteradas a qualquer momento pelo usuário, respeitando a coerência cronológica (RN-10), para acompanhar o monitoramento contínuo e os planos de ação do projeto. As alterações são registradas automaticamente (RN-12).

### 7.8 Histórico semanal

**RN-27: Congelamento do status report.** *Aplica-se a: status report congelado.* Por ação do usuário, o sistema congela o status report da semana: guarda uma cópia do resultado já calculado da timeline (trechos, cores, rótulos, detalhes exibidos ao passar o mouse, coluna "Sem previsão", intervalo de meses e linha da data atual posicionada na data de referência) e da tabela de status das aplicações (aplicação, versão, fase, situação, data de entrega, observação e contadores). Cada status report congelado registra a semana de referência (ex.: Semana 42/2026), a data de referência, o momento do congelamento e o intervalo de meses selecionado na timeline no momento do congelamento. A data de referência é sempre a terça-feira da semana de referência, mesmo quando o congelamento é feito em outro dia. O congelamento não é automático.

**RN-28: Um status report congelado por semana.** *Aplica-se a: status report congelado.* As semanas seguem a numeração ISO 8601 (semanas de segunda a domingo; a semana 1 é a que contém a primeira quinta-feira do ano). O ano faz parte do identificador porque, nas viradas de ano, alguns dias pertencem à semana do ano vizinho (ex.: 01/01/2027 pertence à Semana 53/2026). Ao congelar, o sistema usa a semana da data atual (RN-19). Cada semana de referência possui no máximo um status report congelado. O usuário pode congelar novamente a semana atual ou uma semana anterior já congelada, escolhida no Histórico de Status Reports (RN-30): o sistema pede confirmação e substitui o congelado daquela semana pelos dados atuais do cadastro, mantendo a terça-feira daquela semana como data de referência (RN-27) e o intervalo de meses do congelado original. O novo congelamento substitui o anterior por completo: o conteúdo e o momento do congelamento anteriores deixam de existir. Uma semana anterior que não foi congelada não pode ser congelada depois (RN-30). Restringir quem pode substituir congelados de semanas anteriores é uma evolução futura (seção 13).

**RN-29: Preservação do congelado.** *Aplica-se a: status report congelado.* Um status report congelado não é alterado automaticamente por mudanças posteriores no cadastro (datas, fases, observações, arquivamentos) e não pode ser editado campo a campo nem excluído pelo usuário. A única forma de mudá-lo é substituí-lo por um novo congelamento (RN-28). Aplicações e versões arquivadas depois do congelamento continuam aparecendo nas semanas em que existiam, até que a semana seja substituída.

**RN-30: Histórico de Status Reports.** *Aplica-se a: status report congelado.* Na tela Histórico de Status Reports, o usuário consulta a lista de semanas, da mais recente para a mais antiga, desde a primeira semana congelada até a semana atual. Enquanto nenhuma semana tiver sido congelada, a lista mostra apenas a semana atual, que pode ser congelada. Cada semana congelada é identificada pela semana de referência, pelo intervalo de datas da semana, pela data de referência e pelo momento do congelamento (ex.: "Semana 42/2026 — 12/10 a 18/10/2026 — referência 13/10/2026 — congelado em 14/10/2026"). Uma semana que não foi congelada aparece na lista com a indicação "Sem status report congelado", pois seus dados não foram salvos (ex.: "Semana 40/2026 — 28/09 a 04/10/2026 — Sem status report congelado"). O usuário visualiza qualquer semana congelada em modo somente leitura, com indicação clara de que se trata de um status report congelado e de sua data de referência.

### 7.9 Filtros

**RN-31: Filtro de aplicações.** *Aplica-se a: timeline e tabela de status das aplicações.* O usuário pode escolher quais aplicações aparecem na timeline e na tabela de status das aplicações. Por padrão, todas as aplicações ativas aparecem nas duas visões. A ordem definida na RN-03 é mantida.

**RN-32: Ocultar versões entregues.** *Aplica-se a: timeline e tabela de status das aplicações.* O usuário pode ocultar as versões em Produção na timeline e na tabela de status das aplicações. Por padrão, elas aparecem nas duas visões.

**RN-33: Filtro da tabela de status das aplicações por fase e situação.** *Aplica-se a: tabela de status das aplicações.* O usuário pode filtrar as linhas da tabela de status das aplicações por uma ou mais fases e por uma ou mais situações. Por padrão, todas as fases e situações aparecem. Este filtro não afeta a timeline.

**RN-34: Filtros e contadores.** *Aplica-se a: tabela de status das aplicações.* Os contadores da tabela de status das aplicações (RN-23) sempre consideram o projeto inteiro, independentemente dos filtros aplicados, para que os números não mudem conforme os filtros escolhidos.

**RN-35: Filtros e congelamento.** *Aplica-se a: status report congelado.* Filtros mudam apenas o que aparece na timeline e na tabela de status das aplicações; nunca alteram os dados. O status report congelado guarda todas as aplicações e versões ativas, independentemente dos filtros, e também registra os filtros que estavam aplicados no momento do congelamento. Ao consultar uma semana congelada, ela é aberta com esses filtros, e o usuário pode alterá-los para consultar, sem modificar o congelado.

---

## 8. Requisitos funcionais

| ID | Requisito | Regras relacionadas |
|---|---|---|
| RF-01 | O usuário pode cadastrar, editar, arquivar e restaurar aplicações. | RN-01, RN-04 |
| RF-02 | O usuário pode alterar manualmente a ordem das aplicações e, dentro de cada aplicação, a ordem das versões, além de restaurar a ordem padrão de cada uma. | RN-03 |
| RF-03 | O usuário pode cadastrar, editar, arquivar e restaurar versões de uma aplicação. | RN-02, RN-04 |
| RF-04 | O usuário pode alterar a fase de uma versão, respeitando o sentido das fases. | RN-05, RN-07, RN-08 |
| RF-05 | O usuário pode informar, alterar e remover as datas da versão (início e fim do andamento, início e fim da homologação e data de entrega), com motivo opcional. As datas são informadas, alteradas e removidas em conjunto, num único salvamento. | RN-07, RN-09, RN-10, RN-12, RN-26 |
| RF-08 | O usuário pode consultar o histórico de alterações de datas de uma versão. | RN-12 |
| RF-09 | O usuário pode registrar e editar a observação de uma versão e consultar o histórico de observações anteriores. | RN-22 |
| RF-10 | O usuário pode definir a situação (On track ou Risco) de versões em andamento ou em homologação. | RN-21 |
| RF-11 | O usuário pode visualizar a timeline de entregas. | RN-03, RN-06, RN-13 a RN-20, RN-24, RN-25 |
| RF-12 | O usuário pode escolher o intervalo de meses mostrado na timeline. | RN-20 |
| RF-13 | O usuário pode visualizar a tabela de status das aplicações com os contadores. | RN-01, RN-03, RN-06, RN-13, RN-20 a RN-23 |
| RF-14 | O usuário pode registrar, alterar e remover a prorrogação da homologação de uma versão, informando o início e o fim da prorrogação e um motivo opcional. | RN-12, RN-24 |
| RF-15 | O usuário pode congelar o status report da semana atual e recongelar o da semana atual ou de semanas anteriores já congeladas. | RN-27, RN-28, RN-29 |
| RF-16 | O usuário pode consultar o Histórico de Status Reports, com a lista de semanas. | RN-30 |
| RF-17 | O usuário pode visualizar o status report congelado de qualquer semana, em modo somente leitura. | RN-29, RN-30, RN-35 |
| RF-18 | O usuário pode escolher quais aplicações aparecem na timeline e na tabela de status das aplicações. | RN-31 |
| RF-19 | O usuário pode ocultar as versões entregues na timeline e na tabela de status das aplicações. | RN-32 |
| RF-20 | O usuário pode filtrar a tabela de status das aplicações por fase e por situação. | RN-33, RN-34 |

---

## 9. Requisitos não funcionais de negócio

| ID | Requisito |
|---|---|
| RNF-01 | O sistema será utilizado localmente, sem hospedagem. |
| RNF-02 | O sistema não deve gerar custos de licença ou de serviços. |
| RNF-03 | Interface e conteúdo em português do Brasil, com datas no formato dd/mm/aaaa e semanas no formato "Semana 42/2026". |
| RNF-04 | A timeline e a tabela de status das aplicações devem ser legíveis quando apresentadas por compartilhamento de tela em reuniões, ajustando-se dinamicamente ao tamanho da tela, sem limite fixo de aplicações e versões. |
| RNF-05 | Os dados devem poder ser copiados (backup) e restaurados, evitando a perda do histórico do projeto. |

---

## 10. Critérios de aceite (cenários principais)

Os cenários abaixo usam dados fictícios. Datas não citadas em um cenário estão informadas e são coerentes com a RN-10.

**CA-01: Histórico automático de alterações de datas**
- **Dado** que a versão v1 da aplicação Alfa tem data de entrega 13/10/2026
- **Quando** o usuário altera a data de entrega para 23/10/2026
- **Então** o sistema registra no histórico: data alterada "data de entrega", valor anterior 13/10/2026, novo valor 23/10/2026 e o momento da alteração.

**CA-02: Homologação prorrogada**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com andamento de 01/09/2026 a 25/09/2026, homologação de 01/10/2026 a 13/10/2026, prorrogação de 13/10/2026 a 23/10/2026 e data de entrega em 28/10/2026
- **E** a data atual é 15/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra azul forte de 01/09 a 25/09, linha pontilhada de 25/09 a 01/10, roxo forte de 01/10 a 13/10, vermelho forte de 13/10 a 15/10, vermelho claro de 15/10 a 23/10, linha pontilhada de 23/10 a 28/10 e o marco de entrega em tom claro em 28/10
- **E** o sistema sugere a situação Risco para a versão.

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
- **E** a alteração é registrada no histórico de alterações de datas.

**CA-09: Fim de etapa ultrapassado**
- **Dado** que a versão v1 da aplicação Gama está em Desenvolvimento, com andamento de 15/09/2026 a 10/10/2026, homologação de 15/10/2026 a 30/10/2026 e data de entrega em 05/11/2026
- **E** a data atual é 14/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra azul forte de 15/09 a 10/10, linha pontilhada de 10/10 a 15/10, roxo claro de 15/10 a 30/10, linha pontilhada de 30/10 a 05/11 e o marco de entrega em tom claro em 05/11
- **E** o trecho azul não é estendido além de 10/10, mesmo com a versão ainda em Desenvolvimento
- **E** o sistema sugere a situação Risco para a versão, como lembrete para atualizar as datas ou a fase.

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

**CA-16: Ocultar versões entregues**
- **Dado** que a aplicação Beta tem a versão v0 em Produção e a versão v1 em Desenvolvimento
- **Quando** o usuário oculta as versões entregues
- **Então** a timeline e a tabela de status das aplicações mostram apenas a versão v1 da aplicação Beta
- **E** os contadores continuam considerando todas as aplicações do projeto.

**CA-17: Congelamento com filtros aplicados**
- **Dado** que o usuário está com as versões entregues ocultas
- **Quando** ele congela o status report da semana
- **Então** o status report congelado guarda todas as versões ativas, inclusive as entregues
- **E** ao ser consultado, abre com as versões entregues ocultas, podendo o usuário exibi-las.

**CA-18: Ordem manual das versões**
- **Dado** que a aplicação Alfa tem as versões v1 (andamento a partir de 01/08/2026) e v2 (andamento a partir de 01/09/2026), na ordem padrão
- **Quando** o usuário move a versão v2 para antes da v1
- **Então** a tabela de status das aplicações mostra a v2 antes da v1
- **E** essa ordem é mantida mesmo que as datas das versões sejam alteradas, até que o usuário restaure a ordem padrão.

**CA-19: Ordem manual das aplicações**
- **Dado** que as aplicações Alfa (versão mais antiga a partir de 01/08/2026) e Beta (versão mais antiga a partir de 01/09/2026) estão na ordem padrão
- **Quando** o usuário move a aplicação Beta para antes da Alfa
- **Então** a timeline e a tabela de status das aplicações mostram a Beta antes da Alfa
- **E** uma aplicação cadastrada depois disso entra no final da ordem.

**CA-20: Remoção de datas fora do Backlog**
- **Dado** que a versão v2 da aplicação Alfa está em Desenvolvimento, com as datas informadas
- **Quando** o usuário tenta remover as datas
- **Então** o sistema não aceita e informa que é preciso mudar a fase da versão para Backlog antes de remover as datas.

**CA-21: Contagem da aplicação pela versão mais avançada**
- **Dado** que a aplicação Alfa tem a versão v0 em Produção e a versão v1 em Desenvolvimento
- **E** a aplicação Beta tem apenas a versão v1 em Homologação
- **Quando** o usuário abre a tabela de status das aplicações
- **Então** os contadores mostram 2 aplicações no total, 1 em produção (Alfa) e 1 em homologação (Beta).

**CA-22: Rótulo e detalhes da barra**
- **Dado** que a versão v1 da aplicação Alfa está em Homologação, com andamento de 01/09/2026 a 25/09/2026, homologação de 01/10/2026 a 13/10/2026 e data de entrega em 20/10/2026
- **Quando** o usuário abre a timeline
- **Então** a barra mostra o rótulo "v1 (20/10/2026)"
- **E**, ao passar o mouse sobre a barra, aparecem a fase Homologação e as datas 01/09/2026, 25/09/2026, 01/10/2026, 13/10/2026 e 20/10/2026, cada uma identificada pelo seu nome.

**CA-23: Intervalo padrão da timeline**
- **Dado** que a data atual é 14/10/2026
- **Quando** o usuário abre a timeline
- **Então** o eixo mostra os meses de julho a dezembro de 2026
- **E** o usuário pode alterar o intervalo de meses.

**CA-24: Histórico de observações**
- **Dado** que a versão v1 da aplicação Alfa tem a observação "Homologação em andamento."
- **Quando** o usuário altera a observação para "Acesso sendo liberado em lotes."
- **Então** a tabela de status das aplicações mostra "Acesso sendo liberado em lotes."
- **E** o histórico da versão registra o texto anterior, o novo texto e o momento da alteração.

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
- **Dado** que todas as versões ativas têm datas entre janeiro e junho de 2026
- **E** o intervalo de meses da timeline é de julho a dezembro de 2026
- **Quando** o usuário abre a timeline
- **Então** nenhuma linha de aplicação aparece
- **E** a mensagem "Nenhuma versão no período selecionado. Altere o intervalo de meses." aparece centralizada, no lugar das barras, com o eixo de meses e a coluna "Sem previsão" desenhados.

**CA-29: Timeline sem versões cadastradas**
- **Dado** que nenhuma aplicação possui versões ativas
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

## 12. Questões em aberto

| ID | Questão | Recomendação do analista |
|---|---|---|
| QA-01 | ~~Uma versão pode voltar de fase?~~ | **Resolvida na v0.2:** a versão não retorna após entrar em Homologação; correções ocorrem dentro da homologação. Ver RN-08 e CA-06. |
| QA-02 | ~~Como exibir o futuro planejado de uma versão em andamento?~~ | **Resolvida na v0.3:** a barra cobre todo o período da versão, com datas planejadas por etapa e divisão entre realizado (tom forte) e previsto (tom claro). Incluídas a prorrogação intencional (v0.4) e a liberdade de replanejamento. Ver RN-09, RN-14, RN-24 a RN-26 e CA-07 a CA-09. |
| QA-03 | ~~Qual o comprimento da barra cinza de uma versão em Backlog?~~ | **Resolvida na v0.3:** do início do andamento até a data de entrega, em tom claro; marcador quando houver apenas a data de entrega. Ver RN-16. |
| QA-04 | ~~Datas aproximadas devem ser diferenciadas visualmente na timeline?~~ | **Encerrada na v0.27:** a distinção entre datas confirmadas e aproximadas foi removida (RN-11 e RN-17). |
| QA-05 | ~~Como contar uma aplicação nos contadores, já que ela tem várias versões?~~ | **Resolvida na v0.57:** a aplicação é contada no grupo da sua versão mais avançada. Ver RN-23 e CA-21. |
| QA-06 | ~~A observação deve ser um texto único ou ter histórico?~~ | **Resolvida na v0.61:** a tabela mostra a observação atual, e toda alteração fica registrada no histórico da versão. Ver RN-22 e CA-24. |
| QA-07 | ~~Uma versão que ultrapassa o fim de uma etapa sem avançar de fase deve ser destacada?~~ | **Resolvida na v0.36:** o sistema sugere a situação Risco como lembrete (RN-21). |
| QA-08 | ~~O intervalo de meses da timeline deve ter um padrão?~~ | **Resolvida na v0.60:** por padrão, o semestre atual, ajustável pelo usuário. Ver RN-20 e CA-23. |

---

## 13. Evoluções futuras

Recursos com necessidade de negócio já identificada, deixados para versões posteriores.

| Evolução | O que acrescenta | Referência |
|---|---|---|
| Incorporação como aplicação interna | Integrar o sistema ao ambiente do projeto acompanhado, seguindo seus padrões de arquitetura. | OBJ-07 |
| Calendário de dias úteis | Calcular as datas de início e fim de cada etapa a partir de estimativas de duração em dias úteis, com calendários nacional, estadual, municipal e datas livres definidas pelo usuário. | OBJ-08 |
| Níveis de acesso | Definir perfis de usuário e restringir quem pode substituir o status report congelado de semanas anteriores. Depende do suporte a múltiplos usuários (seção 4.2). | RN-28 |
| Dia de referência configurável | Permitir ao usuário escolher o dia da semana usado como data atual do status report congelado, definido inicialmente como terça-feira. | RN-27, RN-28 |
| Fases, etapas e cores configuráveis | Cadastrar as fases, as etapas e suas cores, e a correspondência (de-para) entre fases e etapas. | RN-05, RN-06 |

---

## 14. Histórico do documento

| Versão | Data | Descrição |
|---|---|---|
| 0.1 | 14/10/2026 | Versão inicial, elaborada a partir do status report atual e das definições levantadas com o usuário. |
| 0.2 | 14/10/2026 | QA-01 resolvida: definida a RN-08 (sentido das fases) e incluído o CA-06. |
| 0.3 | 14/10/2026 | QA-02 e QA-03 resolvidas: datas planejadas por etapa, barra cobrindo todo o período, realizado e previsto, linha de base e liberdade de replanejamento (RN-09, RN-10, RN-12, RN-14, RN-16, RN-21, RN-24 a RN-26, RF-07, RF-08, RF-14, CA-01, CA-02, CA-07 a CA-09). |
| 0.4 | 14/10/2026 | Prorrogação da homologação passa a ser registrada intencionalmente pelo usuário, com data final própria; removida a linha de base automática. Alterados glossário, RN-06, RN-08, RN-18, RN-09, RN-10, RN-12, RN-14, RN-21, RN-24, RF-14, CA-02, CA-06 e CA-08. |
| 0.5 | 14/10/2026 | Incluído o histórico semanal: OBJ-06, escopo, glossário, seção 7.8 (RN-27 a RN-30), RF-15 a RF-17 e CA-10 a CA-12. |
| 0.6 | 14/10/2026 | Semana de referência identificada pelo padrão ISO 8601 (ex.: `2026-W42`), registrada junto com a data de referência. Alterados glossário, RN-27, RN-28, RN-30 e CA-10 a CA-12. |
| 0.7 | 14/10/2026 | Ajustada, no contexto, a descrição dos contadores de aplicações por fase. |
| 0.8 | 14/10/2026 | Contexto: reescrito o problema 4 e removido o problema de vocabulário, pois os nomes diferentes são uma convenção do projeto. OBJ-04 ajustado. |
| 0.9 | 14/10/2026 | Contexto: problema das cores incorporado ao problema 2, pois os critérios são claros e a dificuldade é a precisão do desenho. |
| 0.10 | 14/10/2026 | Introdução e contexto reescritos (projeto acompanhado, processo atual, problemas e solução proposta). Incluída a fase Análise no grupo Em andamento (glossário, RN-05 a RN-08, RF-04). |
| 0.11 | 14/10/2026 | Incluído o OBJ-07 (primeira versão pronta para evolução e incorporação como aplicação interna) e a incorporação em si listada como fora do escopo desta versão. |
| 0.12 | 14/10/2026 | Introdução e glossário: os itens acompanhados podem ser aplicações, milestones ou outros entregáveis. |
| 0.13 | 14/10/2026 | Mantido o contexto de aplicações; a introdução apenas indica que o modelo pode ser usado em outros contextos. Glossário restaurado. |
| 0.14 | 14/10/2026 | Introdução: ajuste de redação na descrição da timeline. |
| 0.15 | 14/10/2026 | Introdução: ajuste de redação na descrição da tabela de status. |
| 0.16 | 14/10/2026 | Introdução: removida a frase sobre o público do documento. |
| 0.17 | 14/10/2026 | Contexto: homologação descrita como validação pelos usuários principais. |
| 0.18 | 14/10/2026 | Removidos os exemplos de monitoramento no contexto e na RN-26. |
| 0.19 | 14/10/2026 | Contexto e RN-26: menção ao monitoramento contínuo e aos planos de ação. |
| 0.20 | 14/10/2026 | Contexto: ajuste de redação no problema 2. |
| 0.21 | 14/10/2026 | OBJ-03 e escopo alinhados ao histórico de alterações de todas as datas e à prorrogação intencional. |
| 0.22 | 14/10/2026 | Datas de versões históricas passam a se chamar "aproximadas", evitando conflito com "data estimada de entrega" (glossário, RN-11, RN-17, RF-05, RF-06, premissas, QA-04). Removida a premissa sobre o rodapé do status report. |
| 0.23 | 14/10/2026 | Indicadores: registrado o tempo atual de atualização (pelo menos 1 hora por semana). |
| 0.24 | 14/10/2026 | Indicadores: incluído o esforço de criação do modelo atual do status report (12 a 16 horas). |
| 0.25 | 14/10/2026 | Escopo: ajuste de redação no item da tabela de status. |
| 0.26 | 14/10/2026 | Contexto: incluída a seção 2.4, com a avaliação do Jira como alternativa e os motivos do descarte. |
| 0.27 | 14/10/2026 | Removida a distinção entre datas confirmadas e aproximadas: cada data é um valor único (glossário, RN-11, RN-17, RF-05, RF-06, premissas; QA-04 encerrada). |
| 0.28 | 14/10/2026 | Datas simplificadas: quatro datas de valor único (início do desenvolvimento, início da homologação, data de entrega e fim da prorrogação), sem distinção entre previsto e realizado e sem registro de deploy. Fase sempre informada pelo usuário. Alterados escopo, glossário, RN-07 a RN-10, RN-12 a RN-16, RN-18, RN-21, RN-24, RF-04 a RF-07, RF-14, CA-01 a CA-05 e CA-07 a CA-10. |
| 0.29 | 14/10/2026 | Versão sem início do desenvolvimento permanece em Backlog (RN-07) e incluído o CA-13. |
| 0.30 | 14/10/2026 | Explicitado que o cadastro da versão exige apenas o rótulo, cria a versão em Backlog e não exige datas (RN-02, RN-09). |
| 0.31 | 14/10/2026 | "Início do desenvolvimento" renomeado para "início do andamento", pois a versão pode começar pela análise; incluído no glossário. |
| 0.32 | 14/10/2026 | Glossário: início do andamento associado à etapa azul. |
| 0.33 | 14/10/2026 | RN-09: explicitado que o trecho prorrogado começa na data de entrega. |
| 0.34 | 14/10/2026 | Ordem das datas corrigida: azul, roxo, vermelho (opcional) e entrega. A data de entrega é sempre a última; a prorrogação passa a ser registrada pelo seu início (glossário, RN-09, RN-10, RN-12, RN-14 a RN-16, RN-18, RN-24, RN-27, RF-14, CA-02). |
| 0.35 | 14/10/2026 | Incluído o OBJ-08 (evolução futura: calendário de dias úteis e cálculo de datas por estimativa de duração) e o item correspondente fora do escopo. |
| 0.36 | 14/10/2026 | Modelo de sete datas: início e fim de cada etapa (andamento, homologação e prorrogação) e data de entrega como deploy em produção, com intervalos entre etapas e entrega como marco. Alterados contexto 2.1, glossário, RN-08 a RN-10, RN-12 a RN-16, RN-18, RN-21, RN-24, RN-25, RF-05, RF-14, CA-01 a CA-05, CA-07 a CA-09; QA-07 resolvida. |
| 0.37 | 14/10/2026 | Explicitado que a homologação contempla correções e desenvolvimentos necessários (contexto 2.1 e RN-08). |
| 0.38 | 14/10/2026 | Glossário: datas em ordem cronológica, incluídos início e fim da prorrogação e ajustadas as definições de fim da homologação e TBD. |
| 0.39 | 14/10/2026 | Glossário e RN-09: datas descritas pelo significado de negócio, com a cor apenas como forma de exibição. |
| 0.40 | 14/10/2026 | Glossário: incluído o significado da sigla TBD. |
| 0.41 | 14/10/2026 | Semana de referência exibida em português, no formato "Semana 42/2026" (glossário, RN-27, RN-28, RN-30, RNF-03, CA-10 a CA-12). |
| 0.42 | 14/10/2026 | RN-02: explicitado que uma aplicação pode ter N versões, com rótulos únicos. |
| 0.43 | 14/10/2026 | Aplicação pode ser cadastrada sem versões e, nesse caso, não aparece na timeline (glossário, RN-01, CA-14). |
| 0.44 | 14/10/2026 | RN-03: aplicação nova entra no final da ordem, a ordem vale também para o congelado e as versões passam a ser ordenadas por data, não pelo rótulo. |
| 0.45 | 14/10/2026 | RN-02: versão recém-cadastrada é exibida como TBD. |
| 0.46 | 14/10/2026 | Versões sem data exibidas em uma coluna "Sem previsão" no fim da timeline (RN-13 e demais referências). |
| 0.47 | 14/10/2026 | Incluídos filtros: comportamento do período (RN-20), seção 7.9 (RN-31 a RN-35), RF-18 a RF-20, CA-15 a CA-17 e item de escopo. |
| 0.48 | 14/10/2026 | Cada regra passa a indicar onde se aplica (cadastro, timeline, tabela de status ou status report congelado); incluídos no glossário os termos timeline, tabela de status e lista de aplicações; ajustes de redação em RN-02 a RN-04, RN-13 e RN-23. |
| 0.49 | 14/10/2026 | Seção 7: termos genéricos de exibição substituídos por descrições concretas do que aparece na timeline e na tabela de status (RN-02 a RN-04, RN-06, RN-09, RN-13 a RN-16, RN-18 a RN-20, RN-22, RN-23, RN-25, RN-27, RN-31 a RN-35, RF-02, RF-12). |
| 0.50 | 14/10/2026 | RN-01 e CA-14: aplicação sem versões também não aparece na tabela de status. Explicado, no início da seção 7, o significado da marcação "cadastro". |
| 0.51 | 14/10/2026 | Regras de cadastro marcadas apenas como "cadastro". |
| 0.52 | 14/10/2026 | "Tabela de status" renomeada para "tabela de status das aplicações" e termos padronizados ("timeline" no lugar de "cronograma"); os nomes dos slides atuais foram mantidos onde o texto se refere a eles. |
| 0.53 | 14/10/2026 | RN-03: ordem das versões passa a ter padrão cronológico com possibilidade de ordenação manual e restauração do padrão; RF-02 ampliado e incluído o CA-18. |
| 0.54 | 14/10/2026 | RN-03: aplicações também passam a ter ordem padrão cronológica, com ordenação manual e restauração do padrão; RF-02 ajustado e incluído o CA-19. |
| 0.55 | 14/10/2026 | RN-07: início do andamento pode ser removido de versões em andamento, retornando a versão para Backlog com confirmação; incluído o CA-20. |
| 0.56 | 14/10/2026 | Removidas da tabela de requisitos funcionais as linhas dos requisitos RF-06 e RF-07, eliminados na versão 0.28. |
| 0.57 | 14/10/2026 | QA-05 resolvida: aplicação contada no grupo da sua versão mais avançada (RN-23, CA-21). |
| 0.58 | 14/10/2026 | Aplicação sem versões passa a aparecer na tabela de status das aplicações e a contar como Não iniciada (RN-01, RN-23, CA-14). |
| 0.59 | 14/10/2026 | RN-18: rótulo fixo com a data de entrega e detalhes de todas as datas ao passar o mouse; RN-27 inclui os detalhes no congelado; incluído o CA-22. |
| 0.60 | 14/10/2026 | QA-08 resolvida: intervalo padrão da timeline é o semestre atual (RN-20, CA-23). |
| 0.61 | 14/10/2026 | QA-06 resolvida: observação com histórico de alterações (RN-22, RF-09, escopo, CA-24). |
| 0.62 | 14/10/2026 | Incluída a seção 13, com as evoluções futuras: incorporação como aplicação interna, calendário de dias úteis, níveis de acesso e dia de referência configurável. Escopo (4.2) passa a apontar para ela. |
| 0.63 | 14/10/2026 | Seção 13: incluída a evolução de fases, etapas e cores configuráveis. |
| 0.64 | 14/10/2026 | Alinhamento à revisão da versão 0.1 do MVP: grupos visuais e cores (glossário, RN-06); datas da versão informadas e removidas em conjunto (RN-02, RN-07, RN-09, RN-13, RN-14, RN-16, RN-18, RF-05, CA-09, CA-13, CA-20, CA-25); histórico ao informar e remover datas (RN-12); data atual do computador do usuário (glossário, RN-19); aplicação sem versões no período e mensagens da timeline sem versões (RN-20, CA-27 a CA-29); recongelamento de semanas e terça-feira como data de referência (OBJ-06, glossário, RN-27 a RN-30, RF-15, CA-10 a CA-12, CA-26); alinhamentos em RN-24, RN-25 e CA-06. |
| 0.65 | 14/10/2026 | Alinhamento à segunda revisão do MVP (versão 0.3): versões sobrepostas em sub-linhas (glossário, RN-03, RN-14, CA-31); datas alteradas e salvas em conjunto, tudo ou nada (RN-10, RF-05, CA-32); remoção de datas somente em Backlog (RN-07, CA-20, CA-35); situação ao sair novamente do Backlog (RN-21); dia atual como realizado (glossário, RN-25); ordem com desempate pelo cadastro (RN-03); rótulo com ano (RN-18, CA-22); intervalo de meses no congelamento e no recongelamento, substituição completa e semanas sem congelado na lista (glossário, RN-27, RN-28, RN-30, CA-11, CA-26, CA-30); definição de etapa e de fase, objetivo OBJ-04, colunas da tabela de status das aplicações, intervalo de meses só na timeline (glossário, RN-20); RNF-04; nota de datas nos critérios de aceite e CA-05; premissa da carga inicial; referências do RF-11 e do RF-13; novos CA-33, CA-34, CA-36 e CA-37. |
| 0.66 | 14/10/2026 | Alinhamento à versão 0.4 do MVP: versão em Produção somente com data de entrega até a data atual (RN-07, CA-39); intervalos pontilhados também em Produção (RN-06, RN-15, CA-03); rótulo dentro da barra (RN-14, RN-18). |
| 0.67 | 14/10/2026 | Tela de consulta às semanas anteriores renomeada para Histórico de Status Reports e incluída no glossário (escopo, RN-28, RN-30, RF-16, CA-26, CA-30). |
| 0.68 | 15/10/2026 | Alinhamento à versão 0.6 do MVP: avanço para qualquer fase seguinte, sem passar pelas intermediárias (RN-08, CA-40); nomes e rótulos comparados sem diferença entre maiúsculas e minúsculas e sem os espaços das pontas (RN-01, RN-02, CA-33, CA-34); prorrogação já registrada pode ser alterada ou removida também em Produção (RN-24, CA-42); Histórico de Status Reports antes do primeiro congelamento (RN-30, CA-41). |

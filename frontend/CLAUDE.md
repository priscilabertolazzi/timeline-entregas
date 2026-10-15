# CLAUDE.md — Frontend

Convenções das telas em Angular. As telas são construídas em degraus, depois das APIs correspondentes (`docs/tarefas.md`; `docs/tecnico/arquitetura.md`, ADR-22). Detalhes na seção 8 do `arquitetura.md`.

## Princípio

O frontend **não decide regras de negócio.** Trechos, cores, tons, cortes, ordem, rótulos, mensagens, situação, contadores e as ações possíveis no Histórico de Status Reports vêm prontos da API. O frontend cuida apenas do que depende da tela: converter datas em posições, distribuir as versões em sub-linhas (RN-14) e posicionar os rótulos dentro das barras (RN-18).

## Estrutura

- Angular com componentes standalone e Angular Material para formulários, tabelas e diálogos.
- Comunicação com a API por serviços dedicados, sempre pelo caminho relativo `/api`. Em desenvolvimento, o repasse é feito pelo `proxy.conf.json`; no Docker, pelo servidor das telas.
- Telas do MVP: lista de aplicações, tela da versão, status report (timeline e tabela de status das aplicações), Histórico de Status Reports e status report congelado (`arquitetura.md`, seção 8).

## Componente de timeline

- Desenhado em SVG, próprio, sem bibliotecas de gráfico de Gantt.
- Cada data do JSON é um ponto no início do dia; a linha da data atual fica no fim do dia atual (`arquitetura.md`, seção 5.5, ADR-27).
- Etapas de duração zero (início igual ao fim) são desenhadas com uma largura mínima visível.
- Sub-linhas: seguindo a ordem recebida da API, cada versão ocupa a primeira sub-linha em que não se sobrepõe a nenhuma outra, considerando também a largura do rótulo; há espaço entre as sub-linhas (RN-14).
- Ajusta-se à largura da tela, sem limite fixo de aplicações, e deve ser legível em compartilhamento de tela (RNF-04).

## Aparência e textos

- Locale `pt-BR`; datas dd/mm/aaaa; semanas "Semana 42/2026" (RNF-03).
- Cores dos grupos visuais, com tom forte e tom claro, definidas num único lugar do tema (RN-06, RN-25).
- Termos das telas iguais aos do glossário: "timeline", "tabela de status das aplicações", "lista de aplicações", "Histórico de Status Reports".
- Mensagens de erro exibidas exatamente como vêm da API (catálogo do `arquitetura.md`, seção 7.6). Não crie textos de erro próprios.
- Confirmação antes de excluir (RN-04) e antes de substituir um congelado (RN-28).

## Segurança

- Use a proteção nativa do Angular contra código malicioso. **Proibido** contornar a sanitização ou inserir HTML a partir de dados.

## Documentação no código

- Comentários no padrão TSDoc em serviços e componentes, em português; comentários explicativos só onde a lógica não é óbvia, citando a regra.

## Comandos

| Ação | Comando |
|---|---|
| Executar em desenvolvimento | `npm start` |
| Testes | `npm test` |
| Padrão do código | `npm run lint` |
| Build | `npm run build` |
| Dependências vulneráveis | `npm audit` |

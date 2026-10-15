# CLAUDE.md — Backend

Convenções da API em .NET. Detalhes e motivos no `docs/tecnico/arquitetura.md`.

## Estrutura e camadas

- Módulo `Timeline`, com as camadas `Timeline.Domain`, `Timeline.Application`, `Timeline.Infrastructure` e `Timeline.Api`, mais o `Host` (`arquitetura.md`, seção 4).
- `Domain` não depende de nenhuma outra camada nem de framework de infraestrutura. `Application` conhece só o `Domain`. `Api` conhece só a `Application`. Só o `Host` conhece todas.
- Responsabilidade única: regras da entidade, cálculos da timeline e da tabela, casos de uso e acesso a dados ficam em classes separadas.

## Nomes

- Termos de domínio em português, sem acentos, iguais ao glossário do `requisitos-mvp.md` (`Aplicacao`, `Versao`, `Fase`, `Etapa`, `GrupoVisual`, `Situacao`, `StatusReportCongelado`). Termos técnicos em inglês (`AplicacaoRepository`, `VersaoEndpoints`).
- Use a tabela de nomes do `arquitetura.md`, seção 5.1. Não invente sinônimos.

## Domínio

- A `Versao` só muda por métodos que validam as regras antes de alterar qualquer dado (`arquitetura.md`, seção 5.3). Se uma regra falhar, nada muda.
- Regra violada: lance `RegraNegocioException` com as mensagens do catálogo de **todas** as regras violadas na operação (ADR-26).
- As datas são gravadas pela operação única `AlterarDatas`, que recebe as sete datas e valida tudo antes (RN-09, RN-10).
- Nomes de aplicação e rótulos de versão são guardados sem os espaços das pontas e comparados sem diferença entre maiúsculas e minúsculas (RN-01, RN-02).
- Datas de negócio são `DateOnly`; o momento do congelamento é `DateTimeOffset`.
- **Nunca leia o relógio diretamente.** A data atual vem do `TimeProvider` injetado (`arquitetura.md`, seção 5.6).
- O cálculo da timeline e da tabela de status das aplicações são funções puras, sem acesso a banco, relógio ou rede (`arquitetura.md`, seções 5.4 e 5.5).
- Datas no eixo são pontos no início do dia; a divisão entre realizado e previsto é escrita como o dia seguinte à data atual. Siga os casos de borda da seção 5.5 (ADR-27).
- Semanas pela classe `ISOWeek`, no formato `2026-W42` (`arquitetura.md`, seção 5.7).

## API

- Minimal APIs, com os endpoints da seção 7.2 do `arquitetura.md`, agrupados em classes como `AplicacaoEndpoints`; rotas sob `/api` (ADR-29).
- Entrada e saída por DTOs (records) por operação; nunca exponha entidades.
- Erros no padrão Problem Details, com as mensagens do **catálogo** (`arquitetura.md`, seção 7.6), com o texto exato. Não crie mensagens fora do catálogo.
- Status: 400 entrada inválida, 404 inexistente, 409 duplicidade (inclusive a detectada pelo banco) ou substituição sem confirmação, 422 regra de negócio, 500 sem detalhes internos.
- Validação de formato na `Application`, com recursos nativos do .NET; regras de negócio no `Domain`.
- Documentação OpenAPI publicada em todos os ambientes, com a página de visualização (ADR-30).

## Banco de dados

- EF Core com Npgsql; schema `timeline`, inclusive a tabela de histórico de migrations.
- Tabelas e colunas em snake_case, em português, sem acentos, conforme a seção 6 do `arquitetura.md`, com as colunas de comparação, as unicidades e as restrições `CHECK` previstas.
- Migrations versionadas e aplicadas ao iniciar a API.
- **Proibido** montar SQL por concatenação de texto; use apenas consultas do EF Core (parametrizadas).

## Testes

- xUnit, com as asserções do próprio framework. Sem bibliotecas de licença comercial.
- TDD nas tarefas marcadas: o teste nasce do texto do critério e a mensagem esperada é a do catálogo.
- Nome do teste cita o critério ou a regra e descreve cenário e resultado (ex.: `CA02_HomologacaoProrrogada_DivideVermelhoNaDataAtual`).
- A data atual é sempre fixada no teste, com o relógio falso da Microsoft.
- Repositórios em memória são classes simples escritas no projeto de teste; não use o provedor em memória do EF Core.
- Todo erro corrigido ganha um teste que o reproduz.
- Cubra os casos de borda das seções 5.5 e 11 do `arquitetura.md`.

## Documentação no código

- Comentários de documentação XML (`///`) em classes e métodos públicos, em português.
- Comentários explicativos só onde a lógica não é óbvia, explicando o porquê e citando a regra (ex.: `// RN-25: o dia atual conta como realizado`).

## Comandos

No computador da autora, o SDK do .NET está instalado e estes comandos rodam diretamente. Os scripts `test` e `lint` da raiz rodam os mesmos comandos em container.

| Ação | Comando |
|---|---|
| Build | `dotnet build` |
| Testes | `dotnet test` |
| Padrão do código | `dotnet format --verify-no-changes` |
| Nova migration | `dotnet ef migrations add NomeDaMigration` (no projeto de infraestrutura) |
| Pacotes vulneráveis | `dotnet list package --vulnerable` |

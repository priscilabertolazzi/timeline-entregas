using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Timeline.Application.Versoes;

namespace Timeline.Api;

/// <summary>
/// Endpoints de versões (arquitetura.md, seção 7.2), no mesmo padrão de <see cref="AplicacaoEndpoints"/>:
/// cada rota só repassa a chamada a um caso de uso; erros são convertidos pelo <see cref="TratadorDeErros"/>.
/// </summary>
public static class VersaoEndpoints
{
    /// <summary>Registra o cadastro em <c>/api/aplicacoes/{id}/versoes</c> e as demais rotas de <c>/api/versoes</c>.</summary>
    public static IEndpointRouteBuilder MapVersoes(this IEndpointRouteBuilder rotas)
    {
        rotas.MapPost("/api/aplicacoes/{aplicacaoId:long}/versoes", async (long aplicacaoId, RotuloVersaoEntrada? entrada, CadastrarVersao caso, CancellationToken ct) =>
                Results.Json(await caso.ExecutarAsync(aplicacaoId, entrada, ct), statusCode: StatusCodes.Status201Created))
            .WithTags("Versões")
            .WithName("CadastrarVersao")
            .WithSummary("Cadastra uma versão")
            .WithDescription("Só o rótulo é informado. A versão nasce em Backlog, sem datas. O rótulo é obrigatório, tem até 20 caracteres e não pode repetir o de outra versão da mesma aplicação, mesmo com maiúsculas e minúsculas diferentes (RN-02).")
            .Produces<VersaoResposta>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        var grupo = rotas.MapGroup("/api/versoes").WithTags("Versões");

        grupo.MapGet("/{id:long}", async (long id, DetalharVersao caso, CancellationToken ct) =>
                TypedResults.Ok(await caso.ExecutarAsync(id, ct)))
            .WithName("DetalharVersao")
            .WithSummary("Detalha uma versão")
            .WithDescription("Devolve o rótulo, a fase, a situação, a observação e as sete datas.")
            .Produces<VersaoResposta>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapPut("/{id:long}", async (long id, AlterarVersaoEntrada? entrada, AlterarVersao caso, CancellationToken ct) =>
                TypedResults.Ok(await caso.ExecutarAsync(id, entrada, ct)))
            .WithName("AlterarVersao")
            .WithSummary("Altera rótulo, situação e observação")
            .WithDescription("A situação (OnTrack ou Risco) só pode mudar em versões em Análise, Desenvolvimento, Testes ou Homologação (RN-21); se vier vazia, a situação atual é mantida. A observação vazia remove a observação (RN-22).")
            .Produces<VersaoResposta>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapPut("/{id:long}/fase", async (long id, FaseEntrada? entrada, AlterarFaseVersao caso, CancellationToken ct) =>
                TypedResults.Ok(await caso.ExecutarAsync(id, entrada, ct)))
            .WithName("AlterarFaseVersao")
            .WithSummary("Altera a fase")
            .WithDescription("Fases: Backlog, Analise, Desenvolvimento, Testes, Homologacao e Producao. Pode avançar para qualquer fase seguinte, sem passar pelas intermediárias; de Homologação não se retorna, e de Produção não se muda (RN-08). Sair do Backlog exige as datas, e entrar em Produção exige data de entrega até a data atual (RN-07).")
            .Produces<VersaoResposta>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapPut("/{id:long}/datas", async (long id, DatasVersaoEntrada? entrada, AlterarDatasVersao caso, CancellationToken ct) =>
                TypedResults.Ok(await caso.ExecutarAsync(id, entrada, ct)))
            .WithName("AlterarDatasVersao")
            .WithSummary("Grava as sete datas de uma vez")
            .WithDescription("Datas no formato AAAA-MM-DD. As cinco datas do ciclo (início e fim do andamento, início e fim da homologação e entrega) vêm juntas; a prorrogação, só em Homologação (RN-24). Todas vazias removem as datas, o que só é possível em Backlog (RN-07, RN-09, RN-10).")
            .Produces<VersaoResposta>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapPut("/{id:long}/completo", async (long id, AlterarVersaoCompletaEntrada? entrada, AlterarVersaoCompleta caso, CancellationToken ct) =>
                TypedResults.Ok(await caso.ExecutarAsync(id, entrada, ct)))
            .WithName("AlterarVersaoCompleta")
            .WithSummary("Grava rótulo, situação, observação, fase e as sete datas de uma vez")
            .WithDescription("É o salvamento do formulário da tela da versão: tudo ou nada. Se qualquer regra falhar, nada é salvo (RN-10). Fase e datas são aplicadas na ordem que a RN-07 permite: avançando de fase, as datas vêm antes; nos demais casos, a fase vem antes. Situação vazia mantém a atual; observação vazia remove a observação; fase e datas são obrigatórias no corpo, e datas todas vazias removem as datas (só em Backlog).")
            .Produces<VersaoResposta>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapDelete("/{id:long}", async (long id, ExcluirVersao caso, CancellationToken ct) =>
            {
                await caso.ExecutarAsync(id, ct);
                return Results.NoContent();
            })
            .WithName("ExcluirVersao")
            .WithSummary("Exclui uma versão")
            .WithDescription("Exclui definitivamente a versão (RN-04).")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return rotas;
    }
}

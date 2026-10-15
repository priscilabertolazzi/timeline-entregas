using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Timeline.Application.Aplicacoes;

namespace Timeline.Api;

/// <summary>
/// Endpoints de aplicações (arquitetura.md, seção 7.2). Este é o padrão das próximas APIs: cada rota
/// só repassa a chamada a um caso de uso; erros são convertidos pelo <see cref="TratadorDeErros"/>.
/// </summary>
public static class AplicacaoEndpoints
{
    /// <summary>Registra as rotas de <c>/api/aplicacoes</c>.</summary>
    public static IEndpointRouteBuilder MapAplicacoes(this IEndpointRouteBuilder rotas)
    {
        var grupo = rotas.MapGroup("/api/aplicacoes").WithTags("Aplicações");

        grupo.MapGet("/", async (ListarAplicacoes caso, CancellationToken ct) => TypedResults.Ok(await caso.ExecutarAsync(ct)))
            .WithName("ListarAplicacoes")
            .WithSummary("Lista as aplicações")
            .WithDescription("Devolve as aplicações em ordem alfabética, sem diferença entre maiúsculas e minúsculas, cada uma com o resumo das suas versões (id, rótulo, fase e situação).")
            .Produces<IReadOnlyList<AplicacaoComVersoesResposta>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapPost("/", async (NomeAplicacaoEntrada? entrada, CadastrarAplicacao caso, CancellationToken ct) =>
                Results.Json(await caso.ExecutarAsync(entrada, ct), statusCode: StatusCodes.Status201Created))
            .WithName("CadastrarAplicacao")
            .WithSummary("Cadastra uma aplicação")
            .WithDescription("O nome é obrigatório, tem até 100 caracteres e não pode repetir o de outra aplicação, mesmo com maiúsculas e minúsculas diferentes (RN-01).")
            .Produces<AplicacaoResposta>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapPut("/{id:long}", async (long id, NomeAplicacaoEntrada? entrada, RenomearAplicacao caso, CancellationToken ct) =>
                TypedResults.Ok(await caso.ExecutarAsync(id, entrada, ct)))
            .WithName("RenomearAplicacao")
            .WithSummary("Renomeia uma aplicação")
            .WithDescription("As mesmas regras do cadastro valem para o novo nome.")
            .Produces<AplicacaoResposta>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        grupo.MapDelete("/{id:long}", async (long id, ExcluirAplicacao caso, CancellationToken ct) =>
            {
                await caso.ExecutarAsync(id, ct);
                return Results.NoContent();
            })
            .WithName("ExcluirAplicacao")
            .WithSummary("Exclui uma aplicação")
            .WithDescription("Exclui definitivamente a aplicação e todas as suas versões (RN-04). Os status reports já congelados não são afetados.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return rotas;
    }
}

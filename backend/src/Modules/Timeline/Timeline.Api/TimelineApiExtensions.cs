using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Timeline.Application.Aplicacoes;

namespace Timeline.Api;

/// <summary>Registro da camada de API: casos de uso, tratamento de erros e documentação (ADR-30).</summary>
public static class TimelineApiExtensions
{
    /// <summary>Caminho da página de documentação da API.</summary>
    public const string CaminhoDocumentacao = "/scalar";

    /// <summary>Registra os casos de uso, o tratador de erros e a geração do documento OpenAPI.</summary>
    public static IServiceCollection AddTimelineApi(this IServiceCollection services)
    {
        services.AddScoped<ListarAplicacoes>();
        services.AddScoped<CadastrarAplicacao>();
        services.AddScoped<RenomearAplicacao>();
        services.AddScoped<ExcluirAplicacao>();

        services.AddExceptionHandler<TratadorDeErros>();

        // Sem isto, o .NET responde 400 com o corpo vazio quando o JSON vem quebrado (fora do ambiente de
        // desenvolvimento). Assim o erro passa pelo TratadorDeErros e devolve a MSG-42.
        services.Configure<RouteHandlerOptions>(opcoes => opcoes.ThrowOnBadRequest = true);

        services.AddOpenApi(opcoes =>
        {
            opcoes.AddDocumentTransformer((documento, _, _) =>
            {
                documento.Info.Title = "Timeline de Entregas";
                documento.Info.Description = "API de cadastro de aplicações e versões, timeline de entregas e status report semanal.";
                return Task.CompletedTask;
            });

            // Valor de exemplo no corpo da requisição, para a página de documentação não mostrar "null".
            opcoes.AddSchemaTransformer((esquema, contexto, _) =>
            {
                if (contexto.JsonTypeInfo.Type == typeof(NomeAplicacaoEntrada)
                    && esquema.Properties?.TryGetValue("nome", out var propriedade) == true
                    && propriedade is OpenApiSchema nome)
                {
                    nome.Examples = [JsonValue.Create("Alfa")];
                }

                return Task.CompletedTask;
            });
        });
        return services;
    }

    /// <summary>
    /// Liga o tratamento de erros, publica o documento OpenAPI e a página de documentação (Scalar)
    /// em todos os ambientes, pois o sistema é local (ADR-30), e mapeia as rotas da API.
    /// </summary>
    public static WebApplication UseTimelineApi(this WebApplication app)
    {
        app.UseExceptionHandler(_ => { });

        app.MapOpenApi();
        app.MapScalarApiReference(CaminhoDocumentacao, opcoes => opcoes.WithTitle("Timeline de Entregas — API"));
        app.MapGet("/", () => Results.Redirect(CaminhoDocumentacao)).ExcludeFromDescription();

        app.MapAplicacoes();
        return app;
    }
}

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Timeline.Application.Aplicacoes;
using Timeline.Application.Versoes;

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
        services.AddScoped<CadastrarVersao>();
        services.AddScoped<DetalharVersao>();
        services.AddScoped<AlterarVersao>();
        services.AddScoped<AlterarFaseVersao>();
        services.AddScoped<AlterarDatasVersao>();
        services.AddScoped<ExcluirVersao>();

        // Data atual vinda do relógio injetado, nunca lida direto (arquitetura.md, seção 5.6). O fuso é o
        // do ambiente (variável TZ do .env, ADR-03).
        services.TryAddSingleton(TimeProvider.System);

        // Fase e situação saem como texto (Homologacao, OnTrack), e não como número.
        services.Configure<JsonOptions>(opcoes => opcoes.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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

            // Valores de exemplo nos corpos das requisições, para a página de documentação não mostrar "null".
            opcoes.AddSchemaTransformer((esquema, contexto, _) =>
            {
                if (esquema.Properties is not null)
                {
                    foreach (var (propriedade, exemplo) in ExemplosDeCorpo(contexto.JsonTypeInfo.Type))
                    {
                        if (esquema.Properties.TryGetValue(propriedade, out var campo) && campo is OpenApiSchema alvo)
                        {
                            alvo.Examples = [JsonValue.Create(exemplo)];
                        }
                    }
                }

                return Task.CompletedTask;
            });
        });
        return services;
    }

    private static IEnumerable<(string Propriedade, string Exemplo)> ExemplosDeCorpo(Type tipo)
    {
        if (tipo == typeof(NomeAplicacaoEntrada))
        {
            yield return ("nome", "Alfa");
        }
        else if (tipo == typeof(RotuloVersaoEntrada))
        {
            yield return ("rotulo", "v1");
        }
        else if (tipo == typeof(AlterarVersaoEntrada))
        {
            yield return ("rotulo", "v1");
            yield return ("situacao", "OnTrack");
            yield return ("observacao", "Aguardando a homologação do cliente.");
        }
        else if (tipo == typeof(FaseEntrada))
        {
            yield return ("fase", "Desenvolvimento");
        }
        else if (tipo == typeof(DatasVersaoEntrada))
        {
            // Datas da v1 da Alfa nos critérios de aceite; a prorrogação fica vazia.
            yield return ("inicioAndamento", "2026-09-01");
            yield return ("fimAndamento", "2026-09-25");
            yield return ("inicioHomologacao", "2026-10-01");
            yield return ("fimHomologacao", "2026-10-13");
            yield return ("dataEntrega", "2026-10-20");
        }
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
        app.MapVersoes();
        return app;
    }
}

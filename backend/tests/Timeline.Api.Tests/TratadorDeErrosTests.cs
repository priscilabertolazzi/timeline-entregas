using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Timeline.Api;
using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Api.Tests;

/// <summary>
/// Conversão de erros em Problem Details (arquitetura.md, seção 7.4): status, mensagens do catálogo
/// com o texto exato e nenhum detalhe interno no erro inesperado (MSG-41).
/// </summary>
public sealed class TratadorDeErrosTests
{
    private sealed record Resposta(int Status, string? ContentType, string Corpo, JsonElement Json)
    {
        public string Title => Json.GetProperty("title").GetString()!;

        public string[] Mensagens => Json.GetProperty("mensagens").EnumerateArray().Select(m => m.GetString()!).ToArray();
    }

    private static async Task<Resposta> Tratar(Exception erro)
    {
        var contexto = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
        };
        contexto.Request.Method = "POST";
        contexto.Request.Path = "/api/aplicacoes";
        contexto.Response.Body = new MemoryStream();

        var tratou = await new TratadorDeErros(NullLogger<TratadorDeErros>.Instance)
            .TryHandleAsync(contexto, erro, CancellationToken.None);

        Assert.True(tratou);
        contexto.Response.Body.Position = 0;
        var corpo = await new StreamReader(contexto.Response.Body).ReadToEndAsync();
        return new Resposta(contexto.Response.StatusCode, contexto.Response.ContentType, corpo, JsonDocument.Parse(corpo).RootElement.Clone());
    }

    [Fact]
    public async Task EntradaInvalida_Devolve400ComAMensagemDoCatalogo()
    {
        var resposta = await Tratar(new EntradaInvalidaException(Mensagens.Msg01));

        Assert.Equal(400, resposta.Status);
        Assert.Equal("Informe o nome da aplicação.", resposta.Title);
        Assert.Equal(["Informe o nome da aplicação."], resposta.Mensagens);
    }

    [Fact]
    public async Task NaoEncontrado_Devolve404ComAMsg04()
    {
        var resposta = await Tratar(new NaoEncontradoException(Mensagens.Msg04));

        Assert.Equal(404, resposta.Status);
        Assert.Equal("Aplicação não encontrada.", resposta.Title);
    }

    [Fact]
    public async Task CA33_Conflito_Devolve409ComAMsg03()
    {
        var resposta = await Tratar(new ConflitoException(Mensagens.Msg03));

        Assert.Equal(409, resposta.Status);
        Assert.Equal("Já existe uma aplicação com esse nome.", resposta.Title);
    }

    [Fact]
    public async Task ADR26_RegraDeNegocioViolada_Devolve422ComTodasAsMensagens()
    {
        var resposta = await Tratar(new RegraNegocioException([Mensagens.Msg16, Mensagens.Msg17]));

        Assert.Equal(422, resposta.Status);
        Assert.Equal("O início do andamento não pode ser posterior ao fim do andamento.", resposta.Title);
        Assert.Equal(
            [
                "O início do andamento não pode ser posterior ao fim do andamento.",
                "O fim do andamento não pode ser posterior ao início da homologação.",
            ],
            resposta.Mensagens);
    }

    [Fact]
    public async Task Msg42_CorpoIlegivel_Devolve400ComAMsg42SemOTextoDoFramework()
    {
        var resposta = await Tratar(new BadHttpRequestException("Failed to read parameter \"NomeAplicacaoEntrada entrada\" from the request body as JSON."));

        Assert.Equal(400, resposta.Status);
        Assert.Equal(["Requisição inválida. Confira os dados enviados."], resposta.Mensagens);
        Assert.DoesNotContain("Failed to read", resposta.Corpo);
    }

    [Fact]
    public async Task Msg41_ErroInesperado_Devolve500SemDetalhesInternos()
    {
        var resposta = await Tratar(new InvalidOperationException("Falha ao conectar com Password=senha-secreta no servidor interno"));

        Assert.Equal(500, resposta.Status);
        Assert.Equal("Ocorreu um erro inesperado. Tente novamente.", resposta.Title);
        Assert.Equal(["Ocorreu um erro inesperado. Tente novamente."], resposta.Mensagens);
        Assert.DoesNotContain("senha-secreta", resposta.Corpo);
        Assert.DoesNotContain("InvalidOperationException", resposta.Corpo);
        Assert.DoesNotContain("TratadorDeErrosTests", resposta.Corpo);
    }

    [Fact]
    public async Task ProblemDetails_TodasAsRespostasUsamOTipoDeConteudoDoPadrao()
    {
        var resposta = await Tratar(new ConflitoException(Mensagens.Msg03));

        Assert.StartsWith("application/problem+json", resposta.ContentType);
    }
}

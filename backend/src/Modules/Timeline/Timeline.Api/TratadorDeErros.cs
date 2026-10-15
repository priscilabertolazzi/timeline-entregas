using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Api;

/// <summary>
/// Converte toda exceção da API em erro no padrão Problem Details (RFC 9457), com as mensagens do
/// catálogo (arquitetura.md, seções 7.4 e 7.6). O <c>title</c> traz a primeira mensagem, e a lista
/// <c>mensagens</c> traz todas (ADR-26). Erro inesperado devolve a MSG-41, sem detalhes internos.
/// </summary>
public sealed class TratadorDeErros(ILogger<TratadorDeErros> log) : IExceptionHandler
{
    /// <summary>Nome da extensão do Problem Details com a lista de mensagens.</summary>
    public const string CampoMensagens = "mensagens";

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, mensagens) = exception switch
        {
            EntradaInvalidaException e => (StatusCodes.Status400BadRequest, e.Mensagens),
            NaoEncontradoException e => (StatusCodes.Status404NotFound, e.Mensagens),
            ConflitoException e => (StatusCodes.Status409Conflict, e.Mensagens),
            RegraNegocioException e => (StatusCodes.Status422UnprocessableEntity, e.Mensagens),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, [Mensagens.Msg42]),
            _ => (StatusCodes.Status500InternalServerError, [Mensagens.Msg41]),
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            // O detalhe fica só no registro (log) do servidor; a resposta nunca o mostra (seção 10).
            log.LogError(exception, "Erro inesperado ao tratar {Metodo} {Caminho}.", httpContext.Request.Method, httpContext.Request.Path);
        }

        var problema = new ProblemDetails { Status = status, Title = mensagens[0] };
        problema.Extensions[CampoMensagens] = mensagens;

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problema, options: null, contentType: "application/problem+json", cancellationToken);
        return true;
    }
}

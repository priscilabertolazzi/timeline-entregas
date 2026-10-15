using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Aplicacoes;

/// <summary>Caso de uso: renomear uma aplicação (RF-01, RN-01).</summary>
public sealed class RenomearAplicacao(IAplicacaoRepository repositorio)
{
    /// <summary>
    /// Confere se a aplicação existe (MSG-04), valida o nome (MSG-01, MSG-02) e recusa nome de outra
    /// aplicação (MSG-03). Trocar só maiúsculas e minúsculas do próprio nome é permitido.
    /// </summary>
    public async Task<AplicacaoResposta> ExecutarAsync(long id, NomeAplicacaoEntrada? entrada, CancellationToken cancellationToken)
    {
        var aplicacao = await repositorio.ObterAsync(id, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg04);

        var nome = ValidadorNomeAplicacao.Validar(entrada?.Nome);

        if (await repositorio.ExisteNomeAsync(nome, id, cancellationToken))
        {
            throw new ConflitoException(Mensagens.Msg03);
        }

        aplicacao.Renomear(nome);
        await repositorio.SalvarAsync(aplicacao, cancellationToken);
        return AplicacaoResposta.De(aplicacao);
    }
}

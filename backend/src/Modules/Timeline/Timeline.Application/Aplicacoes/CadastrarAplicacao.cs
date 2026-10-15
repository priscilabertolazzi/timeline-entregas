using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Aplicacoes;

/// <summary>Caso de uso: cadastrar uma aplicação (RF-01, RN-01).</summary>
public sealed class CadastrarAplicacao(IAplicacaoRepository repositorio)
{
    /// <summary>
    /// Valida o nome (MSG-01, MSG-02), recusa nome já existente (MSG-03, CA-33) e grava a aplicação.
    /// </summary>
    public async Task<AplicacaoResposta> ExecutarAsync(NomeAplicacaoEntrada? entrada, CancellationToken cancellationToken)
    {
        var nome = ValidadorNomeAplicacao.Validar(entrada?.Nome);

        if (await repositorio.ExisteNomeAsync(nome, null, cancellationToken))
        {
            throw new ConflitoException(Mensagens.Msg03);
        }

        var aplicacao = new Aplicacao(nome);
        await repositorio.AdicionarAsync(aplicacao, cancellationToken);
        return AplicacaoResposta.De(aplicacao);
    }
}

using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Caso de uso: detalhar uma versão, com todas as datas (RF-03).</summary>
public sealed class DetalharVersao(IVersaoRepository versoes)
{
    /// <summary>Devolve a versão, ou informa que ela não existe (MSG-08).</summary>
    public async Task<VersaoResposta> ExecutarAsync(long id, CancellationToken cancellationToken)
    {
        var versao = await versoes.ObterAsync(id, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg08);
        return VersaoResposta.De(versao);
    }
}

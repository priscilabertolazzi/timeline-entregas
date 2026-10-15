using Timeline.Application.Aplicacoes;
using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Caso de uso: cadastrar uma versão numa aplicação (RF-03, RN-02).</summary>
public sealed class CadastrarVersao(IAplicacaoRepository aplicacoes, IVersaoRepository versoes)
{
    /// <summary>
    /// Confere se a aplicação existe (MSG-04), valida o rótulo (MSG-05, MSG-06), recusa rótulo já
    /// existente na aplicação (MSG-07, CA-34) e grava a versão em Backlog, sem datas.
    /// </summary>
    public async Task<VersaoResposta> ExecutarAsync(long aplicacaoId, RotuloVersaoEntrada? entrada, CancellationToken cancellationToken)
    {
        _ = await aplicacoes.ObterAsync(aplicacaoId, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg04);

        var rotulo = ValidadorEntradaVersao.ValidarRotulo(entrada?.Rotulo);

        if (await versoes.ExisteRotuloAsync(aplicacaoId, rotulo, null, cancellationToken))
        {
            throw new ConflitoException(Mensagens.Msg07);
        }

        var versao = new Versao(aplicacaoId, rotulo);
        await versoes.AdicionarAsync(versao, cancellationToken);
        return VersaoResposta.De(versao);
    }
}

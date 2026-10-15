using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Caso de uso: alterar rótulo, situação e observação de uma versão (RF-03, RF-09, RF-10).</summary>
public sealed class AlterarVersao(IVersaoRepository versoes)
{
    /// <summary>
    /// Confere se a versão existe (MSG-08) e valida o formato (MSG-05, 06, 09, 10). Recusa rótulo de outra
    /// versão da mesma aplicação (MSG-07) e situação fora de Análise a Homologação (MSG-11). A situação só é
    /// tratada como alteração quando difere da atual; a observação vazia remove a observação (RN-22).
    /// </summary>
    public async Task<VersaoResposta> ExecutarAsync(long id, AlterarVersaoEntrada? entrada, CancellationToken cancellationToken)
    {
        var versao = await versoes.ObterAsync(id, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg08);

        var rotulo = ValidadorEntradaVersao.ValidarRotulo(entrada?.Rotulo);
        var observacao = ValidadorEntradaVersao.ValidarObservacao(entrada?.Observacao);
        Situacao? situacao = string.IsNullOrWhiteSpace(entrada?.Situacao)
            ? null
            : ValidadorEntradaVersao.ValidarSituacao(entrada.Situacao);

        if (await versoes.ExisteRotuloAsync(versao.AplicacaoId, rotulo, id, cancellationToken))
        {
            throw new ConflitoException(Mensagens.Msg07);
        }

        // A regra que pode falhar vem primeiro, para que nada mude se ela falhar (arquitetura.md, 5.3).
        if (situacao is not null && situacao != versao.SituacaoInformada)
        {
            versao.AlterarSituacao(situacao.Value);
        }

        versao.Renomear(rotulo);
        versao.AlterarObservacao(observacao);
        await versoes.SalvarAsync(versao, cancellationToken);
        return VersaoResposta.De(versao);
    }
}

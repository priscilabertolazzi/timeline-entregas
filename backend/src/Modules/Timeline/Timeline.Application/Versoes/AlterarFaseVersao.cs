using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Caso de uso: mudar a fase de uma versão (RF-04).</summary>
public sealed class AlterarFaseVersao(IVersaoRepository versoes, TimeProvider relogio)
{
    /// <summary>
    /// Confere se a versão existe (MSG-08), valida a fase (MSG-26) e deixa o domínio aplicar o sentido das
    /// fases e as condições da RN-07 e da RN-08 (MSG-27 a MSG-30, CA-39, CA-40).
    /// </summary>
    public async Task<VersaoResposta> ExecutarAsync(long id, FaseEntrada? entrada, CancellationToken cancellationToken)
    {
        var versao = await versoes.ObterAsync(id, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg08);

        var fase = ValidadorEntradaVersao.ValidarFase(entrada?.Fase);

        versao.AlterarFase(fase, relogio);
        await versoes.SalvarAsync(versao, cancellationToken);
        return VersaoResposta.De(versao);
    }
}

using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>
/// Caso de uso: gravar de uma vez rótulo, situação, observação, fase e as sete datas de uma versão,
/// como o formulário da tela da versão (RF-03 a RF-05, RF-09, RF-10, RF-14, ADR-39).
/// </summary>
public sealed class AlterarVersaoCompleta(IVersaoRepository versoes, TimeProvider relogio)
{
    /// <summary>
    /// Confere se a versão existe (MSG-08) e valida o formato (MSG-05, 06, 09, 10, 12 e 26). Recusa rótulo de
    /// outra versão da mesma aplicação (MSG-07). As regras de fase, datas e situação (MSG-11 a MSG-30) são do
    /// domínio, que grava tudo ou nada. Corpo sem fase ou sem datas é requisição ilegível (MSG-42): o
    /// formulário sempre as envia, e uma omissão não pode apagar as datas sem querer.
    /// </summary>
    public async Task<VersaoResposta> ExecutarAsync(long id, AlterarVersaoCompletaEntrada? entrada, CancellationToken cancellationToken)
    {
        var versao = await versoes.ObterAsync(id, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg08);

        if (entrada?.Datas is null)
        {
            throw new EntradaInvalidaException(Mensagens.Msg42);
        }

        var rotulo = ValidadorEntradaVersao.ValidarRotulo(entrada.Rotulo);
        var observacao = ValidadorEntradaVersao.ValidarObservacao(entrada.Observacao);
        Situacao? situacao = string.IsNullOrWhiteSpace(entrada.Situacao)
            ? null
            : ValidadorEntradaVersao.ValidarSituacao(entrada.Situacao);
        var fase = ValidadorEntradaVersao.ValidarFase(entrada.Fase);
        var datas = ValidadorEntradaVersao.ValidarDatas(entrada.Datas);

        if (await versoes.ExisteRotuloAsync(versao.AplicacaoId, rotulo, id, cancellationToken))
        {
            throw new ConflitoException(Mensagens.Msg07);
        }

        versao.Alterar(rotulo, situacao, observacao, fase, datas, relogio);
        await versoes.SalvarAsync(versao, cancellationToken);
        return VersaoResposta.De(versao);
    }
}

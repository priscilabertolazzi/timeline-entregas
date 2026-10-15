using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Caso de uso: gravar as sete datas de uma versão de uma vez (RF-05, RF-14, ADR-12).</summary>
public sealed class AlterarDatasVersao(IVersaoRepository versoes, TimeProvider relogio)
{
    /// <summary>
    /// Confere se a versão existe (MSG-08) e se as datas têm o formato AAAA-MM-DD (MSG-12). As regras de
    /// preenchimento, de ordem e de fase (MSG-13 a MSG-25) são do domínio. Todas vazias removem as datas.
    /// </summary>
    public async Task<VersaoResposta> ExecutarAsync(long id, DatasVersaoEntrada? entrada, CancellationToken cancellationToken)
    {
        var versao = await versoes.ObterAsync(id, cancellationToken)
            ?? throw new NaoEncontradoException(Mensagens.Msg08);

        // Corpo ausente não pode apagar as datas sem querer: é tratado como requisição ilegível.
        if (entrada is null)
        {
            throw new EntradaInvalidaException(Mensagens.Msg42);
        }

        var datas = ValidadorEntradaVersao.ValidarDatas(entrada);

        versao.AlterarDatas(datas, relogio);
        await versoes.SalvarAsync(versao, cancellationToken);
        return VersaoResposta.De(versao);
    }
}

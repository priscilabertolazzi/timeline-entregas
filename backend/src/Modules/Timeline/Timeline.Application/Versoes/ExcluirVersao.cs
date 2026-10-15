using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Caso de uso: excluir uma versão (RF-03).</summary>
public sealed class ExcluirVersao(IVersaoRepository versoes)
{
    /// <summary>
    /// Exclui a versão definitivamente ou informa que ela não existe (MSG-08). Os status reports já
    /// congelados não são afetados (RN-04). A confirmação é pedida pela tela.
    /// </summary>
    public async Task ExecutarAsync(long id, CancellationToken cancellationToken)
    {
        if (!await versoes.ExcluirAsync(id, cancellationToken))
        {
            throw new NaoEncontradoException(Mensagens.Msg08);
        }
    }
}

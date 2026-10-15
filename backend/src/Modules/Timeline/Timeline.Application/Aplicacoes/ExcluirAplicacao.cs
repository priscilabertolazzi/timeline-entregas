using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Aplicacoes;

/// <summary>Caso de uso: excluir uma aplicação e suas versões (RF-01, RN-04).</summary>
public sealed class ExcluirAplicacao(IAplicacaoRepository repositorio)
{
    /// <summary>
    /// Exclui a aplicação definitivamente, com as versões (RN-04), ou informa que ela não existe (MSG-04).
    /// A confirmação do usuário é pedida pela tela, antes de chamar este caso de uso.
    /// </summary>
    public async Task ExecutarAsync(long id, CancellationToken cancellationToken)
    {
        if (!await repositorio.ExcluirAsync(id, cancellationToken))
        {
            throw new NaoEncontradoException(Mensagens.Msg04);
        }
    }
}

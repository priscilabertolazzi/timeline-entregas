namespace Timeline.Application.Aplicacoes;

/// <summary>Caso de uso: listar as aplicações (RF-01).</summary>
public sealed class ListarAplicacoes(IAplicacaoRepository repositorio)
{
    /// <summary>
    /// Devolve as aplicações em ordem alfabética, sem diferença entre maiúsculas e minúsculas.
    /// A ordem é feita aqui, e não no banco, para não depender da configuração de idioma do banco.
    /// </summary>
    public async Task<IReadOnlyList<AplicacaoResposta>> ExecutarAsync(CancellationToken cancellationToken)
    {
        var aplicacoes = await repositorio.ListarAsync(cancellationToken);
        return aplicacoes
            .OrderBy(a => a.Nome, StringComparer.InvariantCultureIgnoreCase)
            .ThenBy(a => a.Id)
            .Select(AplicacaoResposta.De)
            .ToList();
    }
}

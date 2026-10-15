using Timeline.Application.Versoes;

namespace Timeline.Application.Aplicacoes;

/// <summary>Caso de uso: listar as aplicações, com o resumo das suas versões (RF-01, RF-03).</summary>
public sealed class ListarAplicacoes(IAplicacaoRepository repositorio, IVersaoRepository versoes)
{
    /// <summary>
    /// Devolve as aplicações em ordem alfabética, sem diferença entre maiúsculas e minúsculas, cada uma com as
    /// suas versões na ordem de cadastro. A ordem é feita aqui, e não no banco, para não depender da
    /// configuração de idioma do banco.
    /// </summary>
    public async Task<IReadOnlyList<AplicacaoComVersoesResposta>> ExecutarAsync(CancellationToken cancellationToken)
    {
        var aplicacoes = await repositorio.ListarAsync(cancellationToken);
        var versoesPorAplicacao = (await versoes.ListarAsync(cancellationToken))
            .OrderBy(v => v.Id)
            .ToLookup(v => v.AplicacaoId);

        return aplicacoes
            .OrderBy(a => a.Nome, StringComparer.InvariantCultureIgnoreCase)
            .ThenBy(a => a.Id)
            .Select(a => new AplicacaoComVersoesResposta(
                a.Id,
                a.Nome,
                versoesPorAplicacao[a.Id].Select(VersaoResumoResposta.De).ToList()))
            .ToList();
    }
}

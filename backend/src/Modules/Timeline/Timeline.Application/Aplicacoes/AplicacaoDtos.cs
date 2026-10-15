using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Application.Aplicacoes;

/// <summary>Entrada de cadastrar e de renomear aplicação.</summary>
/// <param name="Nome">Nome da aplicação (RN-01). Os espaços das pontas são retirados.</param>
public sealed record NomeAplicacaoEntrada(string? Nome);

/// <summary>Aplicação devolvida pela API.</summary>
/// <param name="Id">Identificador da aplicação.</param>
/// <param name="Nome">Nome da aplicação, sem os espaços das pontas.</param>
public sealed record AplicacaoResposta(long Id, string Nome)
{
    /// <summary>Converte a entidade no DTO de saída; a entidade nunca é exposta.</summary>
    public static AplicacaoResposta De(Aplicacao aplicacao) => new(aplicacao.Id, aplicacao.Nome);
}

/// <summary>Aplicação da lista, com o resumo das suas versões (RF-03).</summary>
/// <param name="Id">Identificador da aplicação.</param>
/// <param name="Nome">Nome da aplicação, sem os espaços das pontas.</param>
/// <param name="Versoes">Versões da aplicação, na ordem de cadastro.</param>
public sealed record AplicacaoComVersoesResposta(long Id, string Nome, IReadOnlyList<VersaoResumoResposta> Versoes);

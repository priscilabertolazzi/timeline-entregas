using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Entrada de cadastrar versão: só o rótulo (RF-03, RN-02).</summary>
/// <param name="Rotulo">Rótulo da versão, como v1.2. Os espaços das pontas são retirados.</param>
public sealed record RotuloVersaoEntrada(string? Rotulo);

/// <summary>Entrada de alterar rótulo, situação e observação (RF-03, RF-09, RF-10).</summary>
/// <param name="Rotulo">Rótulo da versão (RN-02).</param>
/// <param name="Situacao">On track ou Risco (RN-21). Vazia, mantém a situação atual.</param>
/// <param name="Observacao">Texto livre (RN-22). Vazia, remove a observação.</param>
public sealed record AlterarVersaoEntrada(string? Rotulo, string? Situacao, string? Observacao);

/// <summary>Entrada de alterar a fase (RF-04).</summary>
/// <param name="Fase">Backlog, Analise, Desenvolvimento, Testes, Homologacao ou Producao (RN-05).</param>
public sealed record FaseEntrada(string? Fase);

/// <summary>
/// Entrada de gravar as sete datas de uma vez (RF-05, RF-14, ADR-12). Cada data é um texto
/// AAAA-MM-DD; data vazia é data removida, e todas vazias removem as datas da versão.
/// </summary>
public sealed record DatasVersaoEntrada(
    string? InicioAndamento,
    string? FimAndamento,
    string? InicioHomologacao,
    string? FimHomologacao,
    string? InicioProrrogacao,
    string? FimProrrogacao,
    string? DataEntrega);

/// <summary>As sete datas devolvidas pela API.</summary>
public sealed record DatasVersaoResposta(
    DateOnly? InicioAndamento,
    DateOnly? FimAndamento,
    DateOnly? InicioHomologacao,
    DateOnly? FimHomologacao,
    DateOnly? InicioProrrogacao,
    DateOnly? FimProrrogacao,
    DateOnly? DataEntrega)
{
    /// <summary>Converte as datas do domínio no DTO de saída.</summary>
    public static DatasVersaoResposta De(DatasVersao datas) => new(
        datas.InicioAndamento, datas.FimAndamento, datas.InicioHomologacao, datas.FimHomologacao,
        datas.InicioProrrogacao, datas.FimProrrogacao, datas.DataEntrega);
}

/// <summary>Versão devolvida pela API, com todas as datas.</summary>
/// <param name="Id">Identificador da versão.</param>
/// <param name="AplicacaoId">Aplicação a que a versão pertence.</param>
/// <param name="Rotulo">Rótulo, sem os espaços das pontas.</param>
/// <param name="Fase">Fase atual (RN-05).</param>
/// <param name="Situacao">Situação exibida: NaoIniciado, OnTrack, Risco ou Entregue (RN-21).</param>
/// <param name="Observacao">Observação, ou nulo.</param>
/// <param name="Datas">As sete datas (RN-09).</param>
public sealed record VersaoResposta(
    long Id,
    long AplicacaoId,
    string Rotulo,
    Fase Fase,
    Situacao Situacao,
    string? Observacao,
    DatasVersaoResposta Datas)
{
    /// <summary>Converte a entidade no DTO de saída; a entidade nunca é exposta.</summary>
    public static VersaoResposta De(Versao versao) => new(
        versao.Id, versao.AplicacaoId, versao.Rotulo, versao.Fase, versao.Situacao,
        versao.Observacao, DatasVersaoResposta.De(versao.Datas));
}

/// <summary>Versão resumida, usada dentro da lista de aplicações (RF-03).</summary>
/// <param name="Id">Identificador da versão.</param>
/// <param name="Rotulo">Rótulo da versão.</param>
/// <param name="Fase">Fase atual.</param>
/// <param name="Situacao">Situação exibida (RN-21).</param>
public sealed record VersaoResumoResposta(long Id, string Rotulo, Fase Fase, Situacao Situacao)
{
    /// <summary>Converte a entidade no resumo.</summary>
    public static VersaoResumoResposta De(Versao versao) => new(versao.Id, versao.Rotulo, versao.Fase, versao.Situacao);
}

namespace Timeline.Domain;

/// <summary>
/// As sete datas de uma versão (RN-09), sempre tratadas juntas: são informadas, alteradas e
/// removidas num único salvamento (RN-10, RF-05).
/// </summary>
public sealed record DatasVersao(
    DateOnly? InicioAndamento,
    DateOnly? FimAndamento,
    DateOnly? InicioHomologacao,
    DateOnly? FimHomologacao,
    DateOnly? InicioProrrogacao,
    DateOnly? FimProrrogacao,
    DateOnly? DataEntrega)
{
    /// <summary>Versão sem datas (TBD, RN-13).</summary>
    public static DatasVersao Vazias { get; } = new(null, null, null, null, null, null, null);

    /// <summary>As cinco datas do ciclo estão preenchidas (RN-09).</summary>
    public bool CicloCompleto =>
        InicioAndamento is not null && FimAndamento is not null && InicioHomologacao is not null
        && FimHomologacao is not null && DataEntrega is not null;

    /// <summary>Há pelo menos uma data da prorrogação preenchida (RN-24).</summary>
    public bool TemProrrogacao => InicioProrrogacao is not null || FimProrrogacao is not null;

    /// <summary>Nenhuma das sete datas está preenchida.</summary>
    public bool EstaVazia => this == Vazias;
}

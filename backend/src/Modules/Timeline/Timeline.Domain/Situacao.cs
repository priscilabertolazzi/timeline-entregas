namespace Timeline.Domain;

/// <summary>
/// Situação da versão (RN-21). Não iniciado e Entregue são calculadas pela fase; On track e
/// Risco são informadas pelo usuário.
/// </summary>
public enum Situacao
{
    NaoIniciado,
    OnTrack,
    Risco,
    Entregue,
}

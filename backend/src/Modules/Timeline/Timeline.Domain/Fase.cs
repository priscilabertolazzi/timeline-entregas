namespace Timeline.Domain;

/// <summary>
/// Fase em que a versão está, sempre informada pelo usuário (RN-05, RN-07). A ordem dos
/// valores é a ordem das fases (RN-08).
/// </summary>
public enum Fase
{
    Backlog,
    Analise,
    Desenvolvimento,
    Testes,
    Homologacao,
    Producao,
}

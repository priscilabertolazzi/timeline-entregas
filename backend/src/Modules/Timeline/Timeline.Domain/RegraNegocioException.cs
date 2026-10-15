namespace Timeline.Domain;

/// <summary>
/// Informa que uma operação violou regras de negócio, com as mensagens do catálogo de todas as
/// regras violadas (ADR-26). Quando ela é lançada, nada foi alterado. A API a converte em erro 422.
/// </summary>
public sealed class RegraNegocioException : Exception
{
    /// <summary>
    /// Cria a exceção com as mensagens das regras violadas, na ordem em que foram verificadas.
    /// </summary>
    public RegraNegocioException(IEnumerable<string> mensagens)
        : this(mensagens.ToList())
    {
    }

    private RegraNegocioException(List<string> mensagens)
        : base(string.Join(" ", mensagens))
    {
        Mensagens = mensagens.AsReadOnly();
    }

    /// <summary>
    /// Mensagens do catálogo (arquitetura.md, seção 7.6) de todas as regras violadas.
    /// </summary>
    public IReadOnlyList<string> Mensagens { get; }
}

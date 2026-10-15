namespace Timeline.Application.Erros;

/// <summary>
/// Erro esperado de um caso de uso, com as mensagens do catálogo (arquitetura.md, seção 7.6).
/// A API converte cada tipo derivado no status HTTP correspondente (seção 7.4).
/// </summary>
public abstract class ErroDeAplicacaoException : Exception
{
    /// <summary>Cria o erro com as mensagens do catálogo.</summary>
    protected ErroDeAplicacaoException(IReadOnlyList<string> mensagens)
        : base(string.Join(" ", mensagens))
    {
        Mensagens = mensagens;
    }

    /// <summary>Mensagens do catálogo, com o texto exato.</summary>
    public IReadOnlyList<string> Mensagens { get; }
}

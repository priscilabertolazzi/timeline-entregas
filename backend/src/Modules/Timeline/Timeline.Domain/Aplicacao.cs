namespace Timeline.Domain;

/// <summary>
/// Aplicação do projeto, identificada por um nome único (RN-01). A unicidade é verificada na
/// Application e garantida no banco.
/// </summary>
public sealed class Aplicacao
{
    /// <summary>Cria a aplicação com o nome informado.</summary>
    public Aplicacao(string nome)
    {
        Renomear(nome);
    }

    /// <summary>Identificador gerado pelo banco.</summary>
    public long Id { get; private set; }

    /// <summary>Nome da aplicação, sem os espaços das pontas.</summary>
    public string Nome { get; private set; } = string.Empty;

    /// <summary>Altera o nome da aplicação, guardado sem os espaços das pontas (RN-01).</summary>
    public void Renomear(string nome)
    {
        // Nome vazio é recusado antes, na Application, com a MSG-01; aqui é só uma proteção.
        ArgumentException.ThrowIfNullOrWhiteSpace(nome);
        Nome = nome.Trim();
    }
}

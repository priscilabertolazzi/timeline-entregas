using Npgsql;

namespace TimelineEntregas.Host;

/// <summary>
/// Monta a conexão com o PostgreSQL a partir das variáveis do <c>.env</c> (arquitetura.md, seção 9),
/// sem nenhuma senha escrita no código.
/// </summary>
internal static class ConexaoBanco
{
    /// <summary>
    /// Usa POSTGRES_USER, POSTGRES_PASSWORD, POSTGRES_DB, DB_HOST (padrão localhost) e DB_PORT (padrão 5432).
    /// </summary>
    public static string Montar(IConfiguration configuracao)
    {
        var construtor = new NpgsqlConnectionStringBuilder
        {
            Host = configuracao["DB_HOST"] ?? "localhost",
            Port = int.TryParse(configuracao["DB_PORT"], out var porta) ? porta : 5432,
            Database = configuracao["POSTGRES_DB"] ?? "timeline_entregas",
            Username = configuracao["POSTGRES_USER"] ?? "timeline",
            Password = configuracao["POSTGRES_PASSWORD"] ?? string.Empty,
        };
        return construtor.ConnectionString;
    }
}

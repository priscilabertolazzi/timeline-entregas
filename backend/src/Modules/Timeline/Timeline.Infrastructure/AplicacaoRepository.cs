using Microsoft.EntityFrameworkCore;
using Npgsql;
using Timeline.Application.Aplicacoes;
using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Infrastructure;

/// <summary>Acesso às aplicações no PostgreSQL, apenas por consultas do EF Core (parametrizadas).</summary>
internal sealed class AplicacaoRepository(TimelineDbContext contexto) : IAplicacaoRepository
{
    public async Task<IReadOnlyList<Aplicacao>> ListarAsync(CancellationToken cancellationToken)
    {
        return await contexto.Aplicacoes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<Aplicacao?> ObterAsync(long id, CancellationToken cancellationToken)
    {
        return contexto.Aplicacoes.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public Task<bool> ExisteNomeAsync(string nome, long? ignorarId, CancellationToken cancellationToken)
    {
        // RN-01: a comparação não diferencia maiúsculas de minúsculas, como a coluna nome_comparacao.
        var comparacao = nome.ToLowerInvariant();
        return contexto.Aplicacoes.AnyAsync(
            a => a.Nome.ToLower() == comparacao && (ignorarId == null || a.Id != ignorarId),
            cancellationToken);
    }

    public async Task AdicionarAsync(Aplicacao aplicacao, CancellationToken cancellationToken)
    {
        contexto.Aplicacoes.Add(aplicacao);
        await GravarAsync(cancellationToken);
    }

    public Task SalvarAsync(Aplicacao aplicacao, CancellationToken cancellationToken)
    {
        return GravarAsync(cancellationToken);
    }

    public async Task<bool> ExcluirAsync(long id, CancellationToken cancellationToken)
    {
        // RN-04: o banco apaga as versões da aplicação em cascata (ON DELETE CASCADE).
        var apagadas = await contexto.Aplicacoes.Where(a => a.Id == id).ExecuteDeleteAsync(cancellationToken);
        return apagadas > 0;
    }

    private async Task GravarAsync(CancellationToken cancellationToken)
    {
        try
        {
            await contexto.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            // Segunda proteção (ADR-23): dois cadastros simultâneos com o mesmo nome.
            throw new ConflitoException(Mensagens.Msg03);
        }
    }
}

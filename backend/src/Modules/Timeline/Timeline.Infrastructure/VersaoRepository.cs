using Microsoft.EntityFrameworkCore;
using Npgsql;
using Timeline.Application.Erros;
using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Infrastructure;

/// <summary>Acesso às versões no PostgreSQL, apenas por consultas do EF Core (parametrizadas).</summary>
internal sealed class VersaoRepository(TimelineDbContext contexto) : IVersaoRepository
{
    public async Task<IReadOnlyList<Versao>> ListarAsync(CancellationToken cancellationToken)
    {
        return await contexto.Versoes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<Versao?> ObterAsync(long id, CancellationToken cancellationToken)
    {
        return contexto.Versoes.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public Task<bool> ExisteRotuloAsync(long aplicacaoId, string rotulo, long? ignorarId, CancellationToken cancellationToken)
    {
        // RN-02: a comparação não diferencia maiúsculas de minúsculas, como a coluna rotulo_comparacao.
        var comparacao = rotulo.ToLowerInvariant();
        return contexto.Versoes.AnyAsync(
            v => v.AplicacaoId == aplicacaoId && v.Rotulo.ToLower() == comparacao && (ignorarId == null || v.Id != ignorarId),
            cancellationToken);
    }

    public async Task AdicionarAsync(Versao versao, CancellationToken cancellationToken)
    {
        contexto.Versoes.Add(versao);
        await GravarAsync(cancellationToken);
    }

    public Task SalvarAsync(Versao versao, CancellationToken cancellationToken)
    {
        return GravarAsync(cancellationToken);
    }

    public async Task<bool> ExcluirAsync(long id, CancellationToken cancellationToken)
    {
        var apagadas = await contexto.Versoes.Where(v => v.Id == id).ExecuteDeleteAsync(cancellationToken);
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
            // Segunda proteção (ADR-23): dois cadastros simultâneos com o mesmo rótulo na aplicação.
            throw new ConflitoException(Mensagens.Msg07);
        }
    }
}

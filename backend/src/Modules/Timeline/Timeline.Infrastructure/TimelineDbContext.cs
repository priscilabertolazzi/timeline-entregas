using Microsoft.EntityFrameworkCore;
using Timeline.Domain;

namespace Timeline.Infrastructure;

/// <summary>
/// Acesso ao banco do módulo Timeline: tabelas <c>aplicacao</c> e <c>versao</c> no schema
/// <c>timeline</c> (arquitetura.md, seção 6).
/// </summary>
public sealed class TimelineDbContext(DbContextOptions<TimelineDbContext> options) : DbContext(options)
{
    /// <summary>Nome do schema do módulo, também usado pela tabela de histórico de migrations.</summary>
    public const string Schema = "timeline";

    /// <summary>Aplicações cadastradas.</summary>
    public DbSet<Aplicacao> Aplicacoes => Set<Aplicacao>();

    /// <summary>Versões das aplicações.</summary>
    public DbSet<Versao> Versoes => Set<Versao>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimelineDbContext).Assembly);
    }
}

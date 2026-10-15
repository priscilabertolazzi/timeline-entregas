using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timeline.Domain;

namespace Timeline.Infrastructure;

/// <summary>Mapeamento da tabela <c>aplicacao</c> (arquitetura.md, seção 6).</summary>
internal sealed class AplicacaoConfiguration : IEntityTypeConfiguration<Aplicacao>
{
    public void Configure(EntityTypeBuilder<Aplicacao> builder)
    {
        builder.ToTable("aplicacao", table =>
        {
            // Segunda proteção (ADR-13): a Application já tira os espaços das pontas e recusa o vazio.
            table.HasCheckConstraint("ck_aplicacao_nome_sem_espacos_nas_pontas", "nome <> '' AND nome = btrim(nome)");
        });

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").HasColumnOrder(1).UseIdentityAlwaysColumn();
        builder.Property(a => a.Nome).HasColumnName("nome").HasColumnOrder(2).HasMaxLength(100).IsRequired();

        // ADR-23: coluna gerada pelo banco, só para comparar nomes sem diferença de maiúsculas (RN-01).
        builder.Property<string>("NomeComparacao")
            .HasColumnName("nome_comparacao").HasColumnOrder(3)
            .HasComputedColumnSql("lower(nome)", stored: true);
        builder.HasIndex("NomeComparacao").IsUnique().HasDatabaseName("ux_aplicacao_nome_comparacao");

        builder.Property<DateTimeOffset>("CriadoEm")
            .HasColumnName("criado_em").HasColumnOrder(4)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timeline.Domain;

namespace Timeline.Infrastructure;

/// <summary>
/// Mapeamento da tabela <c>versao</c> (arquitetura.md, seção 6). As restrições CHECK repetem no banco
/// as regras de datas do domínio, como segunda proteção (ADR-13).
/// </summary>
internal sealed class VersaoConfiguration : IEntityTypeConfiguration<Versao>
{
    private const string CicloCompleto =
        "inicio_andamento IS NOT NULL AND fim_andamento IS NOT NULL AND inicio_homologacao IS NOT NULL " +
        "AND fim_homologacao IS NOT NULL AND data_entrega IS NOT NULL";

    private const string CicloVazio =
        "inicio_andamento IS NULL AND fim_andamento IS NULL AND inicio_homologacao IS NULL " +
        "AND fim_homologacao IS NULL AND data_entrega IS NULL";

    public void Configure(EntityTypeBuilder<Versao> builder)
    {
        builder.ToTable("versao", table =>
        {
            table.HasCheckConstraint("ck_versao_fase",
                "fase IN ('Backlog', 'Analise', 'Desenvolvimento', 'Testes', 'Homologacao', 'Producao')");

            // RN-21: Não iniciado e Entregue são calculados pela fase e nunca gravados.
            table.HasCheckConstraint("ck_versao_situacao_informada", "situacao_informada IN ('OnTrack', 'Risco')");

            // RN-09: as cinco datas do ciclo vêm todas juntas ou nenhuma.
            table.HasCheckConstraint("ck_versao_ciclo_completo_ou_vazio", $"({CicloCompleto}) OR ({CicloVazio})");

            // RN-09, RN-24: início e fim da prorrogação vêm juntos.
            table.HasCheckConstraint("ck_versao_prorrogacao_completa_ou_vazia",
                "(inicio_prorrogacao IS NULL) = (fim_prorrogacao IS NULL)");

            // RN-09: a prorrogação só existe com as cinco datas do ciclo.
            table.HasCheckConstraint("ck_versao_prorrogacao_exige_ciclo",
                $"(inicio_prorrogacao IS NULL AND fim_prorrogacao IS NULL) OR ({CicloCompleto})");

            // RN-10: ordem cronológica entre as datas preenchidas (comparação com data vazia não reprova).
            table.HasCheckConstraint("ck_versao_ordem_das_datas",
                "inicio_andamento <= fim_andamento AND fim_andamento <= inicio_homologacao " +
                "AND inicio_homologacao <= fim_homologacao " +
                "AND (inicio_prorrogacao IS NULL OR (fim_homologacao <= inicio_prorrogacao AND inicio_prorrogacao <= fim_prorrogacao)) " +
                "AND CASE WHEN inicio_prorrogacao IS NULL THEN fim_homologacao <= data_entrega ELSE fim_prorrogacao <= data_entrega END");

            // RN-07: versão sem datas está em Backlog.
            table.HasCheckConstraint("ck_versao_sem_datas_so_em_backlog", $"NOT ({CicloVazio}) OR fase = 'Backlog'");

            // Segunda proteção: a Application já tira os espaços das pontas e recusa o vazio.
            table.HasCheckConstraint("ck_versao_rotulo_sem_espacos_nas_pontas", "rotulo <> '' AND rotulo = btrim(rotulo)");
        });

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id").HasColumnOrder(1).UseIdentityAlwaysColumn();
        builder.Property(v => v.AplicacaoId).HasColumnName("aplicacao_id").HasColumnOrder(2);
        builder.Property(v => v.Rotulo).HasColumnName("rotulo").HasColumnOrder(3).HasMaxLength(20).IsRequired();
        builder.Property(v => v.Fase).HasColumnName("fase").HasColumnOrder(5).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(v => v.SituacaoInformada).HasColumnName("situacao_informada").HasColumnOrder(6)
            .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(v => v.Observacao).HasColumnName("observacao").HasColumnOrder(7).HasMaxLength(1000);

        // ADR-23: coluna gerada pelo banco, só para comparar rótulos sem diferença de maiúsculas (RN-02).
        builder.Property<string>("RotuloComparacao")
            .HasColumnName("rotulo_comparacao").HasColumnOrder(4)
            .HasComputedColumnSql("lower(rotulo)", stored: true);
        builder.HasIndex(nameof(Versao.AplicacaoId), "RotuloComparacao")
            .IsUnique()
            .HasDatabaseName("ux_versao_aplicacao_rotulo_comparacao");

        builder.Property<DateTimeOffset>("CriadoEm")
            .HasColumnName("criado_em").HasColumnOrder(15)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        // As sete datas viajam juntas no DatasVersao (ADR-12) e ocupam sete colunas da própria tabela.
        builder.ComplexProperty(v => v.Datas, datas =>
        {
            datas.Property(d => d.InicioAndamento).HasColumnName("inicio_andamento").HasColumnOrder(8);
            datas.Property(d => d.FimAndamento).HasColumnName("fim_andamento").HasColumnOrder(9);
            datas.Property(d => d.InicioHomologacao).HasColumnName("inicio_homologacao").HasColumnOrder(10);
            datas.Property(d => d.FimHomologacao).HasColumnName("fim_homologacao").HasColumnOrder(11);
            datas.Property(d => d.InicioProrrogacao).HasColumnName("inicio_prorrogacao").HasColumnOrder(12);
            datas.Property(d => d.FimProrrogacao).HasColumnName("fim_prorrogacao").HasColumnOrder(13);
            datas.Property(d => d.DataEntrega).HasColumnName("data_entrega").HasColumnOrder(14);
        });

        // RN-04: apagar a aplicação apaga as versões. O índice único acima já cobre aplicacao_id.
        builder.HasOne<Aplicacao>()
            .WithMany()
            .HasForeignKey(v => v.AplicacaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Timeline.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "timeline");

            migrationBuilder.CreateTable(
                name: "aplicacao",
                schema: "timeline",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nome_comparacao = table.Column<string>(type: "text", nullable: true, computedColumnSql: "lower(nome)", stored: true),
                    criado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacao", x => x.id);
                    table.CheckConstraint("ck_aplicacao_nome_sem_espacos_nas_pontas", "nome <> '' AND nome = btrim(nome)");
                });

            migrationBuilder.CreateTable(
                name: "versao",
                schema: "timeline",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    aplicacao_id = table.Column<long>(type: "bigint", nullable: false),
                    rotulo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rotulo_comparacao = table.Column<string>(type: "text", nullable: true, computedColumnSql: "lower(rotulo)", stored: true),
                    fase = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    situacao_informada = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    observacao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    inicio_andamento = table.Column<DateOnly>(type: "date", nullable: true),
                    fim_andamento = table.Column<DateOnly>(type: "date", nullable: true),
                    inicio_homologacao = table.Column<DateOnly>(type: "date", nullable: true),
                    fim_homologacao = table.Column<DateOnly>(type: "date", nullable: true),
                    inicio_prorrogacao = table.Column<DateOnly>(type: "date", nullable: true),
                    fim_prorrogacao = table.Column<DateOnly>(type: "date", nullable: true),
                    data_entrega = table.Column<DateOnly>(type: "date", nullable: true),
                    criado_em = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_versao", x => x.id);
                    table.CheckConstraint("ck_versao_ciclo_completo_ou_vazio", "(inicio_andamento IS NOT NULL AND fim_andamento IS NOT NULL AND inicio_homologacao IS NOT NULL AND fim_homologacao IS NOT NULL AND data_entrega IS NOT NULL) OR (inicio_andamento IS NULL AND fim_andamento IS NULL AND inicio_homologacao IS NULL AND fim_homologacao IS NULL AND data_entrega IS NULL)");
                    table.CheckConstraint("ck_versao_fase", "fase IN ('Backlog', 'Analise', 'Desenvolvimento', 'Testes', 'Homologacao', 'Producao')");
                    table.CheckConstraint("ck_versao_ordem_das_datas", "inicio_andamento <= fim_andamento AND fim_andamento <= inicio_homologacao AND inicio_homologacao <= fim_homologacao AND (inicio_prorrogacao IS NULL OR (fim_homologacao <= inicio_prorrogacao AND inicio_prorrogacao <= fim_prorrogacao)) AND CASE WHEN inicio_prorrogacao IS NULL THEN fim_homologacao <= data_entrega ELSE fim_prorrogacao <= data_entrega END");
                    table.CheckConstraint("ck_versao_prorrogacao_completa_ou_vazia", "(inicio_prorrogacao IS NULL) = (fim_prorrogacao IS NULL)");
                    table.CheckConstraint("ck_versao_prorrogacao_exige_ciclo", "(inicio_prorrogacao IS NULL AND fim_prorrogacao IS NULL) OR (inicio_andamento IS NOT NULL AND fim_andamento IS NOT NULL AND inicio_homologacao IS NOT NULL AND fim_homologacao IS NOT NULL AND data_entrega IS NOT NULL)");
                    table.CheckConstraint("ck_versao_rotulo_sem_espacos_nas_pontas", "rotulo <> '' AND rotulo = btrim(rotulo)");
                    table.CheckConstraint("ck_versao_sem_datas_so_em_backlog", "NOT (inicio_andamento IS NULL AND fim_andamento IS NULL AND inicio_homologacao IS NULL AND fim_homologacao IS NULL AND data_entrega IS NULL) OR fase = 'Backlog'");
                    table.CheckConstraint("ck_versao_situacao_informada", "situacao_informada IN ('OnTrack', 'Risco')");
                    table.ForeignKey(
                        name: "FK_versao_aplicacao_aplicacao_id",
                        column: x => x.aplicacao_id,
                        principalSchema: "timeline",
                        principalTable: "aplicacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_aplicacao_nome_comparacao",
                schema: "timeline",
                table: "aplicacao",
                column: "nome_comparacao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_versao_aplicacao_rotulo_comparacao",
                schema: "timeline",
                table: "versao",
                columns: new[] { "aplicacao_id", "rotulo_comparacao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "versao",
                schema: "timeline");

            migrationBuilder.DropTable(
                name: "aplicacao",
                schema: "timeline");
        }
    }
}

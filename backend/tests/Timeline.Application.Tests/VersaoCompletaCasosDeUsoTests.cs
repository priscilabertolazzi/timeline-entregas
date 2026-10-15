using Microsoft.Extensions.Time.Testing;
using Timeline.Application.Aplicacoes;
using Timeline.Application.Erros;
using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Application.Tests;

/// <summary>
/// Caso de uso de gravar tudo de uma vez (ADR-39), com os textos exatos do catálogo e a data atual
/// fixada em 14/10/2026.
/// </summary>
public sealed class VersaoCompletaCasosDeUsoTests
{
    private readonly AplicacaoRepositoryEmMemoria _aplicacoes = new();
    private readonly VersaoRepositoryEmMemoria _versoes = new();
    private readonly FakeTimeProvider _relogio = new(new DateTimeOffset(2026, 10, 14, 12, 0, 0, TimeSpan.Zero));

    private static readonly DatasVersaoEntrada DatasAlfaV1 =
        new("2026-09-01", "2026-09-25", "2026-10-01", "2026-10-13", null, null, "2026-10-20");

    private static readonly DatasVersaoEntrada SemDatas = new(null, null, null, null, null, null, null);

    private async Task<VersaoResposta> CriarV1NaAlfaEmHomologacao()
    {
        var alfa = (await new CadastrarAplicacao(_aplicacoes).ExecutarAsync(new NomeAplicacaoEntrada("Alfa"), CancellationToken.None)).Id;
        var v1 = await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(alfa, new RotuloVersaoEntrada("v1"), CancellationToken.None);
        await new AlterarDatasVersao(_versoes, _relogio).ExecutarAsync(v1.Id, DatasAlfaV1, CancellationToken.None);
        return await new AlterarFaseVersao(_versoes, _relogio).ExecutarAsync(v1.Id, new FaseEntrada("Homologacao"), CancellationToken.None);
    }

    private Task<VersaoResposta> Gravar(long id, AlterarVersaoCompletaEntrada? entrada) =>
        new AlterarVersaoCompleta(_versoes, _relogio).ExecutarAsync(id, entrada, CancellationToken.None);

    private Task<VersaoResposta> Detalhar(long id) =>
        new DetalharVersao(_versoes).ExecutarAsync(id, CancellationToken.None);

    [Fact]
    public async Task RF05_GravarTudoDeUmaVez_DevolveAVersaoComTudoAlterado()
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();
        var datas = DatasAlfaV1 with { InicioProrrogacao = "2026-10-13", FimProrrogacao = "2026-10-25", DataEntrega = "2026-10-28" };

        var gravada = await Gravar(v1.Id, new AlterarVersaoCompletaEntrada("  v1.1  ", "Risco", "Acesso sendo liberado em lotes.", "Homologacao", datas));

        Assert.Equal("v1.1", gravada.Rotulo);
        Assert.Equal(Situacao.Risco, gravada.Situacao);
        Assert.Equal("Acesso sendo liberado em lotes.", gravada.Observacao);
        Assert.Equal(Fase.Homologacao, gravada.Fase);
        Assert.Equal(new DateOnly(2026, 10, 25), gravada.Datas.FimProrrogacao);
        Assert.Equal(new DateOnly(2026, 10, 28), gravada.Datas.DataEntrega);
        Assert.Equal(gravada, await Detalhar(v1.Id));
    }

    [Fact]
    public async Task CA32_ProrrogacaoAte25SemMudarAEntrega_RecusaComMsg21ENaoSalvaNada()
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();
        var datas = DatasAlfaV1 with { InicioProrrogacao = "2026-10-13", FimProrrogacao = "2026-10-25" };

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => Gravar(v1.Id, new AlterarVersaoCompletaEntrada("outro", null, "Texto novo", "Homologacao", datas)));

        Assert.Equal(["O fim da prorrogação não pode ser posterior à data de entrega."], erro.Mensagens);
        var depois = await Detalhar(v1.Id);
        Assert.Equal("v1", depois.Rotulo);
        Assert.Null(depois.Observacao);
        Assert.Null(depois.Datas.InicioProrrogacao);
    }

    [Fact]
    public async Task CA25_SoADataDeEntregaEmBacklog_RecusaComMsg13()
    {
        var alfa = (await new CadastrarAplicacao(_aplicacoes).ExecutarAsync(new NomeAplicacaoEntrada("Alfa"), CancellationToken.None)).Id;
        var v3 = await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(alfa, new RotuloVersaoEntrada("v3"), CancellationToken.None);
        var soEntrega = new DatasVersaoEntrada(null, null, null, null, null, null, "2026-11-30");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => Gravar(v3.Id, new AlterarVersaoCompletaEntrada("v3", null, null, "Backlog", soEntrega)));

        Assert.Equal(["As datas de andamento, de homologação e de entrega devem ser informadas em conjunto."], erro.Mensagens);
    }

    [Fact]
    public async Task RN07_BacklogComDatasEFaseJuntas_Aceita()
    {
        var alfa = (await new CadastrarAplicacao(_aplicacoes).ExecutarAsync(new NomeAplicacaoEntrada("Alfa"), CancellationToken.None)).Id;
        var v3 = await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(alfa, new RotuloVersaoEntrada("v3"), CancellationToken.None);

        var gravada = await Gravar(v3.Id, new AlterarVersaoCompletaEntrada("v3", null, null, "Desenvolvimento", DatasAlfaV1));

        Assert.Equal(Fase.Desenvolvimento, gravada.Fase);
        Assert.Equal(new DateOnly(2026, 9, 1), gravada.Datas.InicioAndamento);
    }

    [Fact]
    public async Task RN02_RotuloIgualAoDeOutraVersaoDaAplicacao_RecusaComMsg07()
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();
        var v2 = await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(v1.AplicacaoId, new RotuloVersaoEntrada("v2"), CancellationToken.None);

        var erro = await Assert.ThrowsAsync<ConflitoException>(
            () => Gravar(v2.Id, new AlterarVersaoCompletaEntrada("V1", null, null, "Backlog", SemDatas)));

        Assert.Equal(["Já existe uma versão com esse rótulo na aplicação."], erro.Mensagens);
    }

    [Fact]
    public async Task RF03_VersaoInexistente_RecusaComMsg08()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(
            () => Gravar(999, new AlterarVersaoCompletaEntrada("v1", null, null, "Backlog", SemDatas)));

        Assert.Equal(["Versão não encontrada."], erro.Mensagens);
    }

    [Fact]
    public async Task RF05_SemCorpoOuSemDatas_RecusaComMsg42ENaoApagaAsDatas()
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();

        var semCorpo = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Gravar(v1.Id, null));
        var semDatas = await Assert.ThrowsAsync<EntradaInvalidaException>(
            () => Gravar(v1.Id, new AlterarVersaoCompletaEntrada("v1", null, null, "Homologacao", null)));

        Assert.Equal(["Requisição inválida. Confira os dados enviados."], semCorpo.Mensagens);
        Assert.Equal(["Requisição inválida. Confira os dados enviados."], semDatas.Mensagens);
        Assert.Equal(new DateOnly(2026, 10, 20), (await Detalhar(v1.Id)).Datas.DataEntrega);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Fantasia")]
    public async Task RN05_FaseAusenteOuDesconhecida_RecusaComMsg26(string? fase)
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(
            () => Gravar(v1.Id, new AlterarVersaoCompletaEntrada("v1", null, null, fase, DatasAlfaV1)));

        Assert.Equal(["Fase inválida."], erro.Mensagens);
    }

    [Fact]
    public async Task RF05_DataEmFormatoInvalido_RecusaComMsg12()
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();
        var datas = DatasAlfaV1 with { DataEntrega = "20/10/2026" };

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(
            () => Gravar(v1.Id, new AlterarVersaoCompletaEntrada("v1", null, null, "Homologacao", datas)));

        Assert.Equal(["Data inválida. Use o formato AAAA-MM-DD."], erro.Mensagens);
    }

    [Fact]
    public async Task RN21_SituacaoDesconhecida_RecusaComMsg10()
    {
        var v1 = await CriarV1NaAlfaEmHomologacao();

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(
            () => Gravar(v1.Id, new AlterarVersaoCompletaEntrada("v1", "Atrasada", null, "Homologacao", DatasAlfaV1)));

        Assert.Equal(["Situação inválida. Use On track ou Risco."], erro.Mensagens);
    }
}

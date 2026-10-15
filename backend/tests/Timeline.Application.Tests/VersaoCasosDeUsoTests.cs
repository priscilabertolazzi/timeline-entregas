using Microsoft.Extensions.Time.Testing;
using Timeline.Application.Aplicacoes;
using Timeline.Application.Erros;
using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Application.Tests;

/// <summary>
/// Casos de uso de versões (RF-03 a RF-05, RF-09, RF-10, RF-14, RN-02), com os textos exatos do
/// catálogo e a data atual fixada em 14/10/2026.
/// </summary>
public sealed class VersaoCasosDeUsoTests
{
    private readonly AplicacaoRepositoryEmMemoria _aplicacoes = new();
    private readonly VersaoRepositoryEmMemoria _versoes = new();
    private readonly FakeTimeProvider _relogio = new(new DateTimeOffset(2026, 10, 14, 12, 0, 0, TimeSpan.Zero));

    private async Task<long> CadastrarAplicacao(string nome) =>
        (await new CadastrarAplicacao(_aplicacoes).ExecutarAsync(new NomeAplicacaoEntrada(nome), CancellationToken.None)).Id;

    private Task<VersaoResposta> Cadastrar(long aplicacaoId, string? rotulo) =>
        new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(aplicacaoId, new RotuloVersaoEntrada(rotulo), CancellationToken.None);

    private Task<VersaoResposta> Detalhar(long id) =>
        new DetalharVersao(_versoes).ExecutarAsync(id, CancellationToken.None);

    private Task<VersaoResposta> Alterar(long id, string? rotulo, string? situacao = null, string? observacao = null) =>
        new AlterarVersao(_versoes).ExecutarAsync(id, new AlterarVersaoEntrada(rotulo, situacao, observacao), CancellationToken.None);

    private Task<VersaoResposta> MudarFase(long id, string? fase) =>
        new AlterarFaseVersao(_versoes, _relogio).ExecutarAsync(id, new FaseEntrada(fase), CancellationToken.None);

    private Task<VersaoResposta> GravarDatas(long id, DatasVersaoEntrada? datas) =>
        new AlterarDatasVersao(_versoes, _relogio).ExecutarAsync(id, datas, CancellationToken.None);

    private Task Excluir(long id) =>
        new ExcluirVersao(_versoes).ExecutarAsync(id, CancellationToken.None);

    // Datas da v1 da Alfa (homologação até 13/10) e da v0 da Beta no CA-40.
    private static DatasVersaoEntrada DatasComEntrega(string entrega) =>
        new("2026-09-01", "2026-09-25", "2026-10-01", "2026-10-13", null, null, entrega);

    private static DatasVersaoEntrada DatasBetaV0 =>
        new("2026-08-01", "2026-08-14", "2026-08-17", "2026-08-21", null, null, "2026-08-24");

    private static readonly DatasVersaoEntrada SemDatas = new(null, null, null, null, null, null, null);

    // ---------- Cadastrar (RF-03, RN-02, CA-34) ----------

    [Fact]
    public async Task RF03_CadastrarV1NaAlfa_NasceEmBacklogSemDatasENaoIniciada()
    {
        var alfa = await CadastrarAplicacao("Alfa");

        var versao = await Cadastrar(alfa, "v1");

        Assert.Equal("v1", versao.Rotulo);
        Assert.Equal(alfa, versao.AplicacaoId);
        Assert.True(versao.Id > 0);
        Assert.Equal(Fase.Backlog, versao.Fase);
        Assert.Equal(Situacao.NaoIniciado, versao.Situacao);
        Assert.Null(versao.Observacao);
        Assert.Equal(new DatasVersaoResposta(null, null, null, null, null, null, null), versao.Datas);
    }

    [Fact]
    public async Task RN02_CadastrarComEspacosNasPontas_GuardaSemOsEspacos()
    {
        var alfa = await CadastrarAplicacao("Alfa");

        var versao = await Cadastrar(alfa, "  v1.2  ");

        Assert.Equal("v1.2", versao.Rotulo);
    }

    [Theory]
    [InlineData("v1")]
    [InlineData("V1")]
    [InlineData("  v1  ")]
    public async Task CA34_CadastrarRotuloIgualAoDaV1DaAlfa_RecusaComMsg07(string rotuloDuplicado)
    {
        var alfa = await CadastrarAplicacao("Alfa");
        await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<ConflitoException>(() => Cadastrar(alfa, rotuloDuplicado));

        Assert.Equal(["Já existe uma versão com esse rótulo na aplicação."], erro.Mensagens);
        Assert.Single(_versoes.Gravadas);
    }

    [Fact]
    public async Task CA34_AplicacaoBetaPodeTerASuaPropriaV1()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var beta = await CadastrarAplicacao("Beta");
        await Cadastrar(alfa, "v1");

        var versaoDaBeta = await Cadastrar(beta, "v1");

        Assert.Equal(beta, versaoDaBeta.AplicacaoId);
        Assert.Equal(2, _versoes.Gravadas.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RN02_CadastrarSemRotulo_RecusaComMsg05(string? rotulo)
    {
        var alfa = await CadastrarAplicacao("Alfa");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Cadastrar(alfa, rotulo));

        Assert.Equal(["Informe o rótulo da versão."], erro.Mensagens);
    }

    [Fact]
    public async Task RN02_CadastrarRotuloComMaisDe20Caracteres_RecusaComMsg06()
    {
        var alfa = await CadastrarAplicacao("Alfa");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Cadastrar(alfa, new string('v', 21)));

        Assert.Equal(["O rótulo da versão deve ter no máximo 20 caracteres."], erro.Mensagens);
    }

    [Fact]
    public async Task RN02_CadastrarRotuloComExatamente20Caracteres_Aceita()
    {
        var alfa = await CadastrarAplicacao("Alfa");

        var versao = await Cadastrar(alfa, new string('v', 20));

        Assert.Equal(20, versao.Rotulo.Length);
    }

    [Fact]
    public async Task RF03_CadastrarEmAplicacaoInexistente_RecusaComMsg04()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => Cadastrar(999, "v1"));

        Assert.Equal(["Aplicação não encontrada."], erro.Mensagens);
    }

    // ---------- Detalhar ----------

    [Fact]
    public async Task RF03_DetalharVersaoCadastrada_DevolveAVersao()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var cadastrada = await Cadastrar(alfa, "v1");

        var detalhe = await Detalhar(cadastrada.Id);

        Assert.Equal(cadastrada, detalhe);
    }

    [Fact]
    public async Task RF03_DetalharVersaoInexistente_RecusaComMsg08()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => Detalhar(999));

        Assert.Equal(["Versão não encontrada."], erro.Mensagens);
    }

    // ---------- Alterar rótulo, situação e observação (RF-03, RF-09, RF-10) ----------

    [Fact]
    public async Task RF03_RenomearVersaoParaV2_GuardaOnovoRotulo()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var alterada = await Alterar(v1.Id, "  v2  ");

        Assert.Equal("v2", alterada.Rotulo);
    }

    [Fact]
    public async Task RN02_TrocarSoMaiusculasDoProprioRotulo_E_Permitido()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var alterada = await Alterar(v1.Id, "V1");

        Assert.Equal("V1", alterada.Rotulo);
    }

    [Fact]
    public async Task CA34_RenomearParaRotuloDeOutraVersaoDaMesmaAplicacao_RecusaComMsg07()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        await Cadastrar(alfa, "v1");
        var v2 = await Cadastrar(alfa, "v2");

        var erro = await Assert.ThrowsAsync<ConflitoException>(() => Alterar(v2.Id, "V1"));

        Assert.Equal(["Já existe uma versão com esse rótulo na aplicação."], erro.Mensagens);
        Assert.Equal("v2", (await Detalhar(v2.Id)).Rotulo);
    }

    [Fact]
    public async Task RN02_AlterarSemRotulo_RecusaComMsg05()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Alterar(v1.Id, " "));

        Assert.Equal(["Informe o rótulo da versão."], erro.Mensagens);
    }

    [Fact]
    public async Task RN02_AlterarParaRotuloComMaisDe20Caracteres_RecusaComMsg06()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Alterar(v1.Id, new string('v', 21)));

        Assert.Equal(["O rótulo da versão deve ter no máximo 20 caracteres."], erro.Mensagens);
    }

    [Fact]
    public async Task RN22_RegistrarObservacao_GuardaOTexto()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var alterada = await Alterar(v1.Id, "v1", observacao: "Aguardando o cliente.");

        Assert.Equal("Aguardando o cliente.", alterada.Observacao);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RN22_ObservacaoVazia_RemoveAObservacao(string? vazia)
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await Alterar(v1.Id, "v1", observacao: "Aguardando o cliente.");

        var alterada = await Alterar(v1.Id, "v1", observacao: vazia);

        Assert.Null(alterada.Observacao);
    }

    [Fact]
    public async Task RN22_ObservacaoComMaisDe1000Caracteres_RecusaComMsg09()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Alterar(v1.Id, "v1", observacao: new string('x', 1001)));

        Assert.Equal(["A observação deve ter no máximo 1000 caracteres."], erro.Mensagens);
    }

    [Fact]
    public async Task RN22_ObservacaoComExatamente1000Caracteres_Aceita()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var alterada = await Alterar(v1.Id, "v1", observacao: new string('x', 1000));

        Assert.Equal(1000, alterada.Observacao!.Length);
    }

    [Theory]
    [InlineData("Risco", Situacao.Risco)]
    [InlineData("risco", Situacao.Risco)]
    [InlineData("OnTrack", Situacao.OnTrack)]
    [InlineData("On track", Situacao.OnTrack)]
    public async Task RF10_SituacaoEmDesenvolvimento_E_Aceita(string texto, Situacao esperada)
    {
        var versao = await VersaoEmDesenvolvimento();

        var alterada = await Alterar(versao.Id, "v1", situacao: texto);

        Assert.Equal(esperada, alterada.Situacao);
    }

    [Theory]
    [InlineData("Entregue")]
    [InlineData("NaoIniciado")]
    [InlineData("Atrasado")]
    [InlineData("1")]
    public async Task RN21_SituacaoDiferenteDeOnTrackOuRisco_RecusaComMsg10(string texto)
    {
        var versao = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Alterar(versao.Id, "v1", situacao: texto));

        Assert.Equal(["Situação inválida. Use On track ou Risco."], erro.Mensagens);
    }

    [Fact]
    public async Task RN21_RiscoEmBacklog_RecusaComMsg11ESemMudarNada()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => Alterar(v1.Id, "v2", situacao: "Risco", observacao: "Nota"));

        Assert.Equal(["A situação só pode ser alterada em versões em Análise, Desenvolvimento, Testes ou Homologação."], erro.Mensagens);
        var depois = await Detalhar(v1.Id);
        Assert.Equal("v1", depois.Rotulo);
        Assert.Null(depois.Observacao);
    }

    [Fact]
    public async Task RN21_OnTrackEmBacklog_NaoE_Alteracao_E_E_Aceito()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var alterada = await Alterar(v1.Id, "v1", situacao: "OnTrack");

        Assert.Equal(Situacao.NaoIniciado, alterada.Situacao);
    }

    [Fact]
    public async Task RN21_RiscoEmProducao_RecusaComMsg11()
    {
        var beta = await CadastrarAplicacao("Beta");
        var v0 = await Cadastrar(beta, "v0");
        await GravarDatas(v0.Id, DatasBetaV0);
        await MudarFase(v0.Id, "Producao");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => Alterar(v0.Id, "v0", situacao: "Risco"));

        Assert.Equal(["A situação só pode ser alterada em versões em Análise, Desenvolvimento, Testes ou Homologação."], erro.Mensagens);
    }

    [Fact]
    public async Task RF03_AlterarVersaoInexistente_RecusaComMsg08()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => Alterar(999, "v1"));

        Assert.Equal(["Versão não encontrada."], erro.Mensagens);
    }

    // ---------- Gravar as datas (RF-05, RF-14) ----------

    [Fact]
    public async Task RF05_InformarAsDatasDaV1DaAlfa_GravaAsSeteDatas()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var alterada = await GravarDatas(v1.Id, DatasComEntrega("2026-10-20"));

        Assert.Equal(
            new DatasVersaoResposta(
                new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 25), new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 13),
                null, null, new DateOnly(2026, 10, 20)),
            alterada.Datas);
        Assert.Equal(alterada.Datas, (await Detalhar(v1.Id)).Datas);
    }

    [Fact]
    public async Task RF05_TodasAsDatasVaziasEmBacklog_RemovemAsDatas()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await GravarDatas(v1.Id, DatasComEntrega("2026-10-20"));

        var alterada = await GravarDatas(v1.Id, SemDatas);

        Assert.Equal(new DatasVersaoResposta(null, null, null, null, null, null, null), alterada.Datas);
    }

    [Fact]
    public async Task CA20_RemoverDatasForaDoBacklog_RecusaComMsg24()
    {
        var versao = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => GravarDatas(versao.Id, SemDatas));

        Assert.Equal(["Para remover as datas, mude antes a fase da versão para Backlog."], erro.Mensagens);
        Assert.NotNull((await Detalhar(versao.Id)).Datas.InicioAndamento);
    }

    [Fact]
    public async Task CA25_DatasDoCicloEmParte_RecusaComMsg13()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => GravarDatas(v1.Id, SemDatas with { InicioAndamento = "2026-09-01", FimAndamento = "2026-09-25" }));

        Assert.Equal(["As datas de andamento, de homologação e de entrega devem ser informadas em conjunto."], erro.Mensagens);
    }

    [Fact]
    public async Task CA05_FimDoAndamentoDepoisDoInicioDaHomologacao_RecusaComMsg17()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => GravarDatas(v1.Id, DatasComEntrega("2026-10-20") with { FimAndamento = "2026-10-05" }));

        Assert.Equal(["O fim do andamento não pode ser posterior ao início da homologação."], erro.Mensagens);
    }

    [Theory]
    [InlineData("2026-9-1")]
    [InlineData("01/09/2026")]
    [InlineData("2026-02-30")]
    [InlineData("amanhã")]
    public async Task RN26_DataForaDoFormatoAAAAMMDD_RecusaComMsg12(string dataTorta)
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(
            () => GravarDatas(v1.Id, DatasComEntrega("2026-10-20") with { InicioAndamento = dataTorta }));

        Assert.Equal(["Data inválida. Use o formato AAAA-MM-DD."], erro.Mensagens);
    }

    [Fact]
    public async Task RF05_CorpoAusente_NaoApagaAsDatas()
    {
        var versao = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => GravarDatas(versao.Id, null));

        Assert.Equal(["Requisição inválida. Confira os dados enviados."], erro.Mensagens);
        Assert.NotNull((await Detalhar(versao.Id)).Datas.InicioAndamento);
    }

    [Fact]
    public async Task RF05_GravarDatasDeVersaoInexistente_RecusaComMsg08()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => GravarDatas(999, SemDatas));

        Assert.Equal(["Versão não encontrada."], erro.Mensagens);
    }

    [Fact]
    public async Task CA36_ProrrogacaoNovaEmDesenvolvimento_RecusaComMsg23()
    {
        var versao = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => GravarDatas(versao.Id, DatasComEntrega("2026-10-28") with { InicioProrrogacao = "2026-10-14", FimProrrogacao = "2026-10-20" }));

        Assert.Equal(["A prorrogação só pode ser registrada para versões em Homologação."], erro.Mensagens);
    }

    [Fact]
    public async Task RF14_ProrrogacaoEmHomologacao_E_Gravada()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await GravarDatas(v1.Id, DatasComEntrega("2026-10-20"));
        await MudarFase(v1.Id, "Homologacao");

        var alterada = await GravarDatas(
            v1.Id,
            DatasComEntrega("2026-10-28") with { InicioProrrogacao = "2026-10-13", FimProrrogacao = "2026-10-23" });

        Assert.Equal(new DateOnly(2026, 10, 13), alterada.Datas.InicioProrrogacao);
        Assert.Equal(new DateOnly(2026, 10, 23), alterada.Datas.FimProrrogacao);
        Assert.Equal(new DateOnly(2026, 10, 28), alterada.Datas.DataEntrega);
    }

    // ---------- Mudar a fase (RF-04) ----------

    [Fact]
    public async Task CA13_SairDoBacklogSemDatas_RecusaComMsg28()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => MudarFase(v1.Id, "Analise"));

        Assert.Equal(["Informe as datas da versão antes de iniciá-la."], erro.Mensagens);
        Assert.Equal(Fase.Backlog, (await Detalhar(v1.Id)).Fase);
    }

    [Theory]
    [InlineData("Desenvolvimento")]
    [InlineData("desenvolvimento")]
    [InlineData("Análise")]
    [InlineData("Homologação")]
    public async Task RF04_AvancarComDatas_AceitaONomeDaFaseComOuSemAcentoEMaiusculas(string fase)
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await GravarDatas(v1.Id, DatasComEntrega("2026-10-20"));

        var alterada = await MudarFase(v1.Id, fase);

        Assert.NotEqual(Fase.Backlog, alterada.Fase);
        Assert.Equal(Situacao.OnTrack, alterada.Situacao);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Pronto")]
    [InlineData("3")]
    public async Task RN05_FaseDesconhecida_RecusaComMsg26(string? fase)
    {
        var versao = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => MudarFase(versao.Id, fase));

        Assert.Equal(["Fase inválida."], erro.Mensagens);
        Assert.Equal(Fase.Desenvolvimento, (await Detalhar(versao.Id)).Fase);
    }

    [Fact]
    public async Task CA06_VoltarDaHomologacaoParaDesenvolvimento_RecusaComMsg27()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await GravarDatas(v1.Id, DatasComEntrega("2026-10-20"));
        await MudarFase(v1.Id, "Homologacao");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => MudarFase(v1.Id, "Desenvolvimento"));

        Assert.Equal(
            ["Uma versão em Homologação não retorna a fases anteriores. Para estender o prazo, altere o fim da homologação ou registre uma prorrogação."],
            erro.Mensagens);
    }

    [Fact]
    public async Task CA39_HomologacaoComEntregaEm28DeOutubroParaProducao_RecusaComMsg29()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await GravarDatas(v1.Id, DatasComEntrega("2026-10-28"));
        await MudarFase(v1.Id, "Homologacao");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => MudarFase(v1.Id, "Producao"));

        Assert.Equal(["Para passar para Produção, a data de entrega deve ser igual ou anterior à data atual."], erro.Mensagens);
        Assert.Equal(Fase.Homologacao, (await Detalhar(v1.Id)).Fase);
    }

    [Fact]
    public async Task CA40_BacklogDiretoParaProducaoComDatasPassadas_AceitaESituacaoViraEntregue()
    {
        var beta = await CadastrarAplicacao("Beta");
        var v0 = await Cadastrar(beta, "v0");
        await GravarDatas(v0.Id, DatasBetaV0);

        var alterada = await MudarFase(v0.Id, "Producao");

        Assert.Equal(Fase.Producao, alterada.Fase);
        Assert.Equal(Situacao.Entregue, alterada.Situacao);
    }

    [Fact]
    public async Task CA40_ProducaoComEntregaNoProprioDiaDaDataAtual_E_Aceita()
    {
        var beta = await CadastrarAplicacao("Beta");
        var v0 = await Cadastrar(beta, "v0");
        await GravarDatas(v0.Id, DatasBetaV0 with { DataEntrega = "2026-10-14" });

        var alterada = await MudarFase(v0.Id, "Producao");

        Assert.Equal(Fase.Producao, alterada.Fase);
    }

    [Fact]
    public async Task RN08_MudarDeFaseEmProducao_RecusaComMsg30()
    {
        var beta = await CadastrarAplicacao("Beta");
        var v0 = await Cadastrar(beta, "v0");
        await GravarDatas(v0.Id, DatasBetaV0);
        await MudarFase(v0.Id, "Producao");

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(() => MudarFase(v0.Id, "Testes"));

        Assert.Equal(["Uma versão em Produção não muda mais de fase."], erro.Mensagens);
    }

    [Fact]
    public async Task RF04_MudarFaseDeVersaoInexistente_RecusaComMsg08()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => MudarFase(999, "Analise"));

        Assert.Equal(["Versão não encontrada."], erro.Mensagens);
    }

    // ---------- Excluir (RF-03) ----------

    [Fact]
    public async Task RF03_ExcluirVersao_RemoveAVersaoEAsOutrasContinuam()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        var v2 = await Cadastrar(alfa, "v2");

        await Excluir(v1.Id);

        await Assert.ThrowsAsync<NaoEncontradoException>(() => Detalhar(v1.Id));
        Assert.Equal("v2", (await Detalhar(v2.Id)).Rotulo);
    }

    [Fact]
    public async Task RF03_ExcluirVersaoInexistente_RecusaComMsg08()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => Excluir(999));

        Assert.Equal(["Versão não encontrada."], erro.Mensagens);
    }

    // ---------- Lista de aplicações com versões (RF-01, RF-03) ----------

    [Fact]
    public async Task RF03_ListaDeAplicacoes_TrazAsVersoesDeCadaUmaNaOrdemDeCadastro()
    {
        var beta = await CadastrarAplicacao("Beta");
        var alfa = await CadastrarAplicacao("Alfa");
        await CadastrarAplicacao("Gama");
        var v1 = await Cadastrar(alfa, "v1");
        await Cadastrar(beta, "v0");
        var v2 = await Cadastrar(alfa, "v2");
        await GravarDatas(v1.Id, DatasBetaV0);
        await MudarFase(v1.Id, "Producao");

        var lista = await new ListarAplicacoes(_aplicacoes, _versoes).ExecutarAsync(CancellationToken.None);

        Assert.Equal(["Alfa", "Beta", "Gama"], lista.Select(a => a.Nome));
        Assert.Equal(
            [new VersaoResumoResposta(v1.Id, "v1", Fase.Producao, Situacao.Entregue), new VersaoResumoResposta(v2.Id, "v2", Fase.Backlog, Situacao.NaoIniciado)],
            lista[0].Versoes);
        Assert.Equal(["v0"], lista[1].Versoes.Select(v => v.Rotulo));
        Assert.Empty(lista[2].Versoes);
    }

    // ---------- Apoio ----------

    // v1 da Alfa em Desenvolvimento, com as datas da Alfa (entrega em 20/10/2026).
    private async Task<VersaoResposta> VersaoEmDesenvolvimento()
    {
        var alfa = await CadastrarAplicacao("Alfa");
        var v1 = await Cadastrar(alfa, "v1");
        await GravarDatas(v1.Id, DatasComEntrega("2026-10-20"));
        return await MudarFase(v1.Id, "Desenvolvimento");
    }
}

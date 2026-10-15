using static Timeline.Domain.Tests.VersaoTeste;

namespace Timeline.Domain.Tests;

/// <summary>
/// Operação Alterar, que grava rótulo, situação, observação, fase e as sete datas de uma vez
/// (ADR-39): tudo ou nada (CA-32) e a ordem entre fase e datas (RN-07).
/// As mensagens esperadas são digitadas aqui com o texto do catálogo (seção 7.6).
/// </summary>
public class VersaoAlterarTests
{
    // Critérios de aceite

    [Fact]
    public void CA25_BacklogSoComDataDeEntrega_RecusaComMsg13ENaoMudaNada()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v3");
        var soEntrega = new DatasVersao(null, null, null, null, null, null, Dia(30, 11));

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v3", null, "Nova observação", Fase.Backlog, soEntrega, relogio));

        Assert.Equal(["As datas de andamento, de homologação e de entrega devem ser informadas em conjunto."], erro.Mensagens);
        Assert.Equal("v3", versao.Rotulo);
        Assert.Null(versao.Observacao);
        Assert.True(versao.Datas.EstaVazia);
    }

    [Fact]
    public void CA32_ProrrogacaoAteDia25SemMudarAEntrega_RecusaComMsg21ENaoSalvaNada()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1 with { FimHomologacao = Dia(13, 10), DataEntrega = Dia(20, 10) }, relogio);
        var comProrrogacao = DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(25, 10) };

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v1 revisada", null, "Texto novo", Fase.Homologacao, comProrrogacao, relogio));

        Assert.Equal(["O fim da prorrogação não pode ser posterior à data de entrega."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
        Assert.Equal("v1", versao.Rotulo);
        Assert.Null(versao.Observacao);
    }

    [Fact]
    public void CA32_ProrrogacaoJuntoComEntregaEm28_SalvaTudo()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);
        var novas = DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(25, 10), DataEntrega = Dia(28, 10) };

        versao.Alterar("v1", null, null, Fase.Homologacao, novas, relogio);

        Assert.Equal(novas, versao.Datas);
    }

    // Fase e datas no mesmo salvamento (RN-07)

    [Fact]
    public void RN07_BacklogSemDatas_DatasEFaseDesenvolvimentoJuntas_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v3");

        versao.Alterar("v3", null, null, Fase.Desenvolvimento, DatasAlfaV1, relogio);

        Assert.Equal(Fase.Desenvolvimento, versao.Fase);
        Assert.Equal(DatasAlfaV1, versao.Datas);
        Assert.Equal(Situacao.OnTrack, versao.Situacao);
    }

    [Fact]
    public void RN24_BacklogSemDatas_HomologacaoComProrrogacaoNoMesmoSalvamento_Aceita()
    {
        // Bug da CRM v0.2: a prorrogação era recusada com a MSG-23 porque a fase ainda era Backlog.
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v0.2");
        var comProrrogacao = DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(18, 10), DataEntrega = Dia(20, 10) };

        versao.Alterar("v0.2", null, null, Fase.Homologacao, comProrrogacao, relogio);

        Assert.Equal(Fase.Homologacao, versao.Fase);
        Assert.Equal(comProrrogacao, versao.Datas);
    }

    [Fact]
    public void RN24_TestesParaHomologacao_ComProrrogacaoNoMesmoSalvamento_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Testes, DatasAlfaV1, relogio);
        var comProrrogacao = DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(18, 10), DataEntrega = Dia(20, 10) };

        versao.Alterar("v1", null, null, Fase.Homologacao, comProrrogacao, relogio);

        Assert.Equal(Fase.Homologacao, versao.Fase);
        Assert.Equal(Dia(18, 10), versao.Datas.FimProrrogacao);
    }

    [Fact]
    public void RN24_BacklogSemDatas_DesenvolvimentoComProrrogacao_RecusaComMsg23()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v0.2");
        var comProrrogacao = DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(18, 10), DataEntrega = Dia(20, 10) };

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v0.2", null, null, Fase.Desenvolvimento, comProrrogacao, relogio));

        Assert.Equal(["A prorrogação só pode ser registrada para versões em Homologação."], erro.Mensagens);
        Assert.Equal(Fase.Backlog, versao.Fase);
        Assert.True(versao.Datas.EstaVazia);
    }

    [Fact]
    public void RN07_BacklogSemDatas_SoAFaseDesenvolvimento_RecusaComMsg28()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v3");

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v3", null, null, Fase.Desenvolvimento, DatasVersao.Vazias, relogio));

        Assert.Equal(["Informe as datas da versão antes de iniciá-la."], erro.Mensagens);
        Assert.Equal(Fase.Backlog, versao.Fase);
    }

    [Fact]
    public void RN07_HomologacaoParaProducao_CorrigindoAEntregaParaHoje_Aceita()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1 with { DataEntrega = Dia(28, 10) }, relogio);

        versao.Alterar("v1", null, null, Fase.Producao, DatasAlfaV1 with { DataEntrega = Dia(14, 10) }, relogio);

        Assert.Equal(Fase.Producao, versao.Fase);
        Assert.Equal(Dia(14, 10), versao.Datas.DataEntrega);
        Assert.Equal(Situacao.Entregue, versao.Situacao);
    }

    [Fact]
    public void RN07_TestesParaBacklog_RemovendoAsDatas_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v2", Fase.Testes, DatasAlfaV1, relogio);

        versao.Alterar("v2", null, null, Fase.Backlog, DatasVersao.Vazias, relogio);

        Assert.Equal(Fase.Backlog, versao.Fase);
        Assert.True(versao.Datas.EstaVazia);
        Assert.Equal(Situacao.NaoIniciado, versao.Situacao);
    }

    [Fact]
    public void RN07_TestesNaMesmaFase_RemovendoAsDatas_RecusaComMsg24()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v2", Fase.Testes, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v2", null, null, Fase.Testes, DatasVersao.Vazias, relogio));

        Assert.Equal(["Para remover as datas, mude antes a fase da versão para Backlog."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    // Todos os campos juntos

    [Fact]
    public void RF03_RotuloSituacaoEObservacao_SaoGravadosJuntoComAsDatas()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        versao.Alterar("v1.1", Situacao.Risco, "Acesso sendo liberado em lotes.", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        Assert.Equal("v1.1", versao.Rotulo);
        Assert.Equal(Situacao.Risco, versao.Situacao);
        Assert.Equal("Acesso sendo liberado em lotes.", versao.Observacao);
    }

    [Fact]
    public void RN21_SaindoDoBacklogComRisco_FicaEmRisco()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v3");

        versao.Alterar("v3", Situacao.Risco, "Dependência de outra equipe.", Fase.Analise, DatasAlfaV1, relogio);

        Assert.Equal(Situacao.Risco, versao.Situacao);
    }

    // RN-22: versão em Risco exige observação

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RN22_RiscoSemObservacao_RecusaComMsg43ENaoMudaNada(string? observacao)
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v1 nova", Situacao.Risco, observacao, Fase.Desenvolvimento, DatasAlfaV1, relogio));

        Assert.Equal(["Informe a observação quando a situação for Risco."], erro.Mensagens);
        Assert.Equal(Situacao.OnTrack, versao.Situacao);
        Assert.Equal("v1", versao.Rotulo);
    }

    [Fact]
    public void RN22_OnTrackSemObservacao_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        versao.Alterar("v1", Situacao.OnTrack, null, Fase.Desenvolvimento, DatasAlfaV1, relogio);

        Assert.Equal(Situacao.OnTrack, versao.Situacao);
    }

    [Fact]
    public void RN22_VersaoEmRiscoApagandoAObservacao_RecusaComMsg43()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);
        versao.AlterarDados("v1", Situacao.Risco, "Dependência de outra equipe.");

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarDados("v1", null, ""));

        Assert.Equal(["Informe a observação quando a situação for Risco."], erro.Mensagens);
        Assert.Equal("Dependência de outra equipe.", versao.Observacao);
    }

    [Fact]
    public void RN22_AlterarDadosDeRiscoComObservacao_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        versao.AlterarDados("v1.1", Situacao.Risco, "Dependência de outra equipe.");

        Assert.Equal(Situacao.Risco, versao.Situacao);
        Assert.Equal("v1.1", versao.Rotulo);
    }

    [Fact]
    public void RN21_SituacaoNula_MantemASituacaoAtual()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);
        versao.AlterarSituacao(Situacao.Risco);

        versao.Alterar("v1", null, "Dependência de outra equipe.", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        Assert.Equal(Situacao.Risco, versao.Situacao);
    }

    // Tudo ou nada

    [Fact]
    public void CA32_FaseRecusadaDepoisDasDatas_DesfazAsDatasETudoMais()
    {
        var relogio = Relogio(14, 10);
        var datasOriginais = DatasAlfaV1 with { DataEntrega = Dia(28, 10) };
        var versao = Criar("v1", Fase.Homologacao, datasOriginais, relogio);

        // Entrega ainda no futuro em Produção: as datas passam, a fase falha com a MSG-29.
        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("outro", null, "Texto novo", Fase.Producao, datasOriginais with { DataEntrega = Dia(30, 10) }, relogio));

        Assert.Equal(["Para passar para Produção, a data de entrega deve ser igual ou anterior à data atual."], erro.Mensagens);
        Assert.Equal(Fase.Homologacao, versao.Fase);
        Assert.Equal(datasOriginais, versao.Datas);
        Assert.Equal("v1", versao.Rotulo);
        Assert.Null(versao.Observacao);
    }

    [Fact]
    public void ADR39_RotuloEmBrancoDepoisDeFaseEDatas_DesfazTudoTambemNesseTipoDeFalha()
    {
        // Revisão independente: o rótulo é o último passo, e a falha dele não é uma RegraNegocioException.
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v3");

        Assert.ThrowsAny<ArgumentException>(
            () => versao.Alterar("   ", null, null, Fase.Desenvolvimento, DatasAlfaV1, relogio));

        Assert.Equal(Fase.Backlog, versao.Fase);
        Assert.True(versao.Datas.EstaVazia);
        Assert.Equal("v3", versao.Rotulo);
    }

    [Fact]
    public void CA32_FaseRecusada_NaoMudaSituacaoObservacaoNemRotulo()
    {
        var relogio = Relogio(14, 10);
        var datasOriginais = DatasAlfaV1 with { DataEntrega = Dia(28, 10) };
        var versao = Criar("v1", Fase.Homologacao, datasOriginais, relogio);

        Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("outro", Situacao.Risco, "Dependência de outra equipe.", Fase.Producao, datasOriginais, relogio));

        Assert.Equal(Fase.Homologacao, versao.Fase);
        Assert.Equal(Situacao.OnTrack, versao.Situacao);
        Assert.Null(versao.Observacao);
        Assert.Equal("v1", versao.Rotulo);
        Assert.Equal(datasOriginais, versao.Datas);
    }

    [Fact]
    public void RN07_ProducaoComEntregaNoProprioDia_AlterarDatasAceita()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v0", Fase.Producao, DatasBetaV0, relogio);

        versao.AlterarDatas(DatasBetaV0 with { DataEntrega = Dia(14, 10) }, relogio);

        Assert.Equal(Dia(14, 10), versao.Datas.DataEntrega);
    }

    [Fact]
    public void RN21_SituacaoRecusadaNoFim_DesfazFaseEDatas()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        // Entrega até hoje em Produção, mas situação Risco em Produção é recusada (MSG-11).
        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v1", Situacao.Risco, null, Fase.Producao, DatasAlfaV1 with { DataEntrega = Dia(14, 10) }, relogio));

        Assert.Equal(["A situação só pode ser alterada em versões em Análise, Desenvolvimento, Testes ou Homologação."], erro.Mensagens);
        Assert.Equal(Fase.Homologacao, versao.Fase);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void CA06_HomologacaoParaDesenvolvimento_RecusaComMsg27ENaoMudaOResto()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.Alterar("v1 nova", null, "Texto novo", Fase.Desenvolvimento, DatasAlfaV1, relogio));

        Assert.Equal(
            ["Uma versão em Homologação não retorna a fases anteriores. Para estender o prazo, altere o fim da homologação ou registre uma prorrogação."],
            erro.Mensagens);
        Assert.Equal("v1", versao.Rotulo);
        Assert.Null(versao.Observacao);
    }
}

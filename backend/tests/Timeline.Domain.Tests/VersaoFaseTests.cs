using static Timeline.Domain.Tests.VersaoTeste;

namespace Timeline.Domain.Tests;

/// <summary>
/// Operação AlterarFase (arquitetura.md, seção 5.3): RN-02, RN-05, RN-07 e RN-08.
/// As mensagens esperadas são digitadas aqui com o texto do catálogo (seção 7.6).
/// </summary>
public class VersaoFaseTests
{
    // Critérios de aceite

    [Fact]
    public void CA06_VersaoEmHomologacao_VoltarParaDesenvolvimento_RecusaComMsg27()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarFase(Fase.Desenvolvimento, relogio));

        Assert.Equal(
            ["Uma versão em Homologação não retorna a fases anteriores. Para estender o prazo, altere o fim da homologação ou registre uma prorrogação."],
            erro.Mensagens);
        Assert.Equal(Fase.Homologacao, versao.Fase);
    }

    [Fact]
    public void CA13_BacklogSemDatas_IrParaAnalise_RecusaComMsg28()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v2");

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarFase(Fase.Analise, relogio));

        Assert.Equal(["Informe as datas da versão antes de iniciá-la."], erro.Mensagens);
        Assert.Equal(Fase.Backlog, versao.Fase);
    }

    [Fact]
    public void CA39_ProducaoComEntregaFutura_RecusaComMsg29()
    {
        var relogio = Relogio(14, 10);
        var datas = DatasAlfaV1 with { DataEntrega = Dia(28, 10) };
        var versao = Criar("v1", Fase.Homologacao, datas, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarFase(Fase.Producao, relogio));

        Assert.Equal(["Para passar para Produção, a data de entrega deve ser igual ou anterior à data atual."], erro.Mensagens);
        Assert.Equal(Fase.Homologacao, versao.Fase);
    }

    [Fact]
    public void CA40_BacklogDiretoParaProducao_AceitaESituacaoEntregue()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v0", Fase.Backlog, DatasBetaV0, relogio);

        versao.AlterarFase(Fase.Producao, relogio);

        Assert.Equal(Fase.Producao, versao.Fase);
        Assert.Equal(Situacao.Entregue, versao.Situacao);
    }

    // Regras sem critério de aceite próprio

    [Fact]
    public void RN02_VersaoNova_NasceEmBacklogSemDatasENaoIniciada()
    {
        var versao = new Versao(1, "  v1.2 ");

        Assert.Equal("v1.2", versao.Rotulo);
        Assert.Equal(Fase.Backlog, versao.Fase);
        Assert.Equal(DatasVersao.Vazias, versao.Datas);
        Assert.Equal(Situacao.NaoIniciado, versao.Situacao);
        Assert.Equal(Situacao.OnTrack, versao.SituacaoInformada);
    }

    [Fact]
    public void RN02_Renomear_GuardaSemEspacosNasPontas()
    {
        var versao = new Versao(1, "v1");

        versao.Renomear(" v1.1  ");

        Assert.Equal("v1.1", versao.Rotulo);
    }

    [Fact]
    public void RN08_EntreBacklogETestes_PodeVoltar()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Testes, DatasAlfaV1, relogio);

        versao.AlterarFase(Fase.Desenvolvimento, relogio);
        versao.AlterarFase(Fase.Backlog, relogio);

        Assert.Equal(Fase.Backlog, versao.Fase);
    }

    [Fact]
    public void RN08_AvancarPulandoFases_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Backlog, DatasAlfaV1, relogio);

        versao.AlterarFase(Fase.Homologacao, relogio);

        Assert.Equal(Fase.Homologacao, versao.Fase);
    }

    [Fact]
    public void RN08_VersaoEmProducao_NaoMudaDeFase_RecusaComMsg30()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v0", Fase.Producao, DatasBetaV0, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarFase(Fase.Homologacao, relogio));

        Assert.Equal(["Uma versão em Produção não muda mais de fase."], erro.Mensagens);
        Assert.Equal(Fase.Producao, versao.Fase);
    }

    [Fact]
    public void RN07_BacklogSemDatasDiretoParaProducao_RecusaComMsg28()
    {
        var relogio = Relogio(14, 10);
        var versao = new Versao(1, "v0");

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarFase(Fase.Producao, relogio));

        Assert.Equal(["Informe as datas da versão antes de iniciá-la."], erro.Mensagens);
        Assert.Equal(Fase.Backlog, versao.Fase);
    }
}

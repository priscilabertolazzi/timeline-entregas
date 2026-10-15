using static Timeline.Domain.Tests.VersaoTeste;

namespace Timeline.Domain.Tests;

/// <summary>
/// Operação AlterarDatas (arquitetura.md, seção 5.3): RN-07, RN-09, RN-10, RN-24 e RN-26.
/// As mensagens esperadas são digitadas aqui com o texto do catálogo (seção 7.6).
/// </summary>
public class VersaoDatasTests
{
    // Critérios de aceite

    [Fact]
    public void CA05_FimDoAndamentoDepoisDoInicioDaHomologacao_RecusaComMsg17()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.AlterarDatas(DatasAlfaV1 with { FimAndamento = Dia(5, 10) }, relogio));

        Assert.Equal(["O fim do andamento não pode ser posterior ao início da homologação."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void CA20_VersaoEmDesenvolvimento_RemoverDatas_RecusaComMsg24()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v2", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.AlterarDatas(DatasVersao.Vazias, relogio));

        Assert.Equal(["Para remover as datas, mude antes a fase da versão para Backlog."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void CA25_SoDataDeEntregaInformada_RecusaComMsg13()
    {
        var relogio = Relogio(15, 10);
        var versao = new Versao(1, "v3");

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.AlterarDatas(DatasVersao.Vazias with { DataEntrega = Dia(30, 11) }, relogio));

        Assert.Equal(["As datas de andamento, de homologação e de entrega devem ser informadas em conjunto."], erro.Mensagens);
        Assert.Equal(DatasVersao.Vazias, versao.Datas);
    }

    [Fact]
    public void CA32_ProrrogacaoPassaDaEntrega_RecusaComMsg21_NadaMuda()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarDatas(
            DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(25, 10) }, relogio));

        Assert.Equal(["O fim da prorrogação não pode ser posterior à data de entrega."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void CA32_MesmoSalvamentoComEntregaEm28_SalvaTudo()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);
        var novas = DatasAlfaV1 with
        {
            InicioProrrogacao = Dia(13, 10),
            FimProrrogacao = Dia(25, 10),
            DataEntrega = Dia(28, 10),
        };

        versao.AlterarDatas(novas, relogio);

        Assert.Equal(novas, versao.Datas);
    }

    [Fact]
    public void CA35_BacklogComDatas_RemoverDatas_RemoveAsCinco()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v2", Fase.Backlog, DatasAlfaV1, relogio);

        versao.AlterarDatas(DatasVersao.Vazias, relogio);

        Assert.Equal(DatasVersao.Vazias, versao.Datas);
        Assert.Equal(Fase.Backlog, versao.Fase);
    }

    [Fact]
    public void CA36_BetaEmDesenvolvimento_RegistrarProrrogacao_RecusaComMsg23()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarDatas(
            DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(16, 10) }, relogio));

        Assert.Equal(["A prorrogação só pode ser registrada para versões em Homologação."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void CA42_ProducaoCorrigeFimDaProrrogacao_Aceita()
    {
        var relogio = Relogio(30, 10);
        var datas = Ciclo(Dia(1, 9), Dia(25, 9), Dia(1, 10), Dia(13, 10), Dia(28, 10)) with
        {
            InicioProrrogacao = Dia(13, 10),
            FimProrrogacao = Dia(23, 10),
        };
        var versao = Criar("v1", Fase.Producao, datas, relogio);

        versao.AlterarDatas(datas with { FimProrrogacao = Dia(22, 10) }, relogio);

        Assert.Equal(Dia(22, 10), versao.Datas.FimProrrogacao);
    }

    [Fact]
    public void CA42_ProducaoSemProrrogacao_NovaProrrogacao_RecusaComMsg23()
    {
        var relogio = Relogio(30, 10);
        var versao = Criar("v0", Fase.Producao, DatasBetaV0, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarDatas(
            DatasBetaV0 with { InicioProrrogacao = Dia(21, 8), FimProrrogacao = Dia(23, 8) }, relogio));

        Assert.Equal(["A prorrogação só pode ser registrada para versões em Homologação."], erro.Mensagens);
        Assert.Equal(DatasBetaV0, versao.Datas);
    }

    // Regras sem critério de aceite próprio

    [Fact]
    public void RN24_SoUmaDataDaProrrogacao_RecusaComMsg14()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.AlterarDatas(DatasAlfaV1 with { InicioProrrogacao = Dia(13, 10) }, relogio));

        Assert.Equal(["O início e o fim da prorrogação devem ser informados em conjunto."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void RN09_ProrrogacaoSemDatasDoCiclo_RecusaComMsg15()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarDatas(
            DatasVersao.Vazias with { InicioProrrogacao = Dia(13, 10), FimProrrogacao = Dia(16, 10) }, relogio));

        Assert.Equal(["Informe as datas de andamento, de homologação e de entrega antes de registrar a prorrogação."], erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }

    [Fact]
    public void RN10_InicioDoAndamentoDepoisDoFim_RecusaComMsg16()
    {
        AssertRecusaDatasDaAlfaV1(
            DatasAlfaV1 with { InicioAndamento = Dia(26, 9) },
            "O início do andamento não pode ser posterior ao fim do andamento.");
    }

    [Fact]
    public void RN10_InicioDaHomologacaoDepoisDoFim_RecusaComMsg18()
    {
        AssertRecusaDatasDaAlfaV1(
            DatasAlfaV1 with { InicioHomologacao = Dia(14, 10) },
            "O início da homologação não pode ser posterior ao fim da homologação.");
    }

    [Fact]
    public void RN10_FimDaHomologacaoDepoisDoInicioDaProrrogacao_RecusaComMsg19()
    {
        AssertRecusaDatasDaAlfaV1(
            DatasAlfaV1 with { InicioProrrogacao = Dia(12, 10), FimProrrogacao = Dia(16, 10) },
            "O fim da homologação não pode ser posterior ao início da prorrogação.");
    }

    [Fact]
    public void RN10_InicioDaProrrogacaoDepoisDoFim_RecusaComMsg20()
    {
        AssertRecusaDatasDaAlfaV1(
            DatasAlfaV1 with { InicioProrrogacao = Dia(17, 10), FimProrrogacao = Dia(16, 10) },
            "O início da prorrogação não pode ser posterior ao fim da prorrogação.");
    }

    [Fact]
    public void RN10_SemProrrogacao_FimDaHomologacaoDepoisDaEntrega_RecusaComMsg22()
    {
        AssertRecusaDatasDaAlfaV1(
            DatasAlfaV1 with { FimHomologacao = Dia(21, 10) },
            "O fim da homologação não pode ser posterior à data de entrega.");
    }

    [Fact]
    public void RN10_DuasDatasForaDeOrdem_DevolveAsDuasMensagens()
    {
        AssertRecusaDatasDaAlfaV1(
            DatasAlfaV1 with { InicioAndamento = Dia(26, 9), InicioHomologacao = Dia(14, 10) },
            "O início do andamento não pode ser posterior ao fim do andamento.",
            "O início da homologação não pode ser posterior ao fim da homologação.");
    }

    [Fact]
    public void RN07_EmProducao_EntregaDepoisDaDataAtual_RecusaComMsg25()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v0", Fase.Producao, DatasBetaV0, relogio);

        var erro = Assert.Throws<RegraNegocioException>(
            () => versao.AlterarDatas(DatasBetaV0 with { DataEntrega = Dia(15, 10) }, relogio));

        Assert.Equal(["Em Produção, a data de entrega não pode ser posterior à data atual."], erro.Mensagens);
        Assert.Equal(DatasBetaV0, versao.Datas);
    }

    [Fact]
    public void RN24_EmProducao_RemoverProrrogacao_Aceita()
    {
        var relogio = Relogio(30, 10);
        var datas = Ciclo(Dia(1, 9), Dia(25, 9), Dia(1, 10), Dia(13, 10), Dia(28, 10)) with
        {
            InicioProrrogacao = Dia(13, 10),
            FimProrrogacao = Dia(23, 10),
        };
        var versao = Criar("v1", Fase.Producao, datas, relogio);

        versao.AlterarDatas(datas with { InicioProrrogacao = null, FimProrrogacao = null }, relogio);

        Assert.False(versao.Datas.TemProrrogacao);
    }

    [Fact]
    public void RN26_EmDesenvolvimento_ReplanejarTodasAsDatas_Aceita()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);
        var novas = Ciclo(Dia(5, 9), Dia(30, 9), Dia(5, 10), Dia(20, 10), Dia(27, 10));

        versao.AlterarDatas(novas, relogio);

        Assert.Equal(novas, versao.Datas);
    }

    private static void AssertRecusaDatasDaAlfaV1(DatasVersao novas, params string[] mensagens)
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Homologacao, DatasAlfaV1, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarDatas(novas, relogio));

        Assert.Equal(mensagens, erro.Mensagens);
        Assert.Equal(DatasAlfaV1, versao.Datas);
    }
}

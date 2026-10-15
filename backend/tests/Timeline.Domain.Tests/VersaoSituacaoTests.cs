using static Timeline.Domain.Tests.VersaoTeste;

namespace Timeline.Domain.Tests;

/// <summary>
/// Operações AlterarSituacao e AlterarObservacao (arquitetura.md, seção 5.3): RN-21 e RN-22.
/// As mensagens esperadas são digitadas aqui com o texto do catálogo (seção 7.6).
/// </summary>
public class VersaoSituacaoTests
{
    [Fact]
    public void RN21_SituacaoEmBacklog_RecusaComMsg11()
    {
        var versao = new Versao(1, "v1");

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarSituacao(Situacao.Risco));

        Assert.Equal(["A situação só pode ser alterada em versões em Análise, Desenvolvimento, Testes ou Homologação."], erro.Mensagens);
        Assert.Equal(Situacao.OnTrack, versao.SituacaoInformada);
    }

    [Fact]
    public void RN21_SituacaoEmProducao_RecusaComMsg11()
    {
        var relogio = Relogio(14, 10);
        var versao = Criar("v0", Fase.Producao, DatasBetaV0, relogio);

        var erro = Assert.Throws<RegraNegocioException>(() => versao.AlterarSituacao(Situacao.Risco));

        Assert.Equal(["A situação só pode ser alterada em versões em Análise, Desenvolvimento, Testes ou Homologação."], erro.Mensagens);
        Assert.Equal(Situacao.Entregue, versao.Situacao);
    }

    [Fact]
    public void RN21_SituacaoEmDesenvolvimento_AceitaRisco()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);

        versao.AlterarSituacao(Situacao.Risco);

        Assert.Equal(Situacao.Risco, versao.Situacao);
    }

    [Fact]
    public void RN21_SairDoBacklogDeNovo_VoltaParaOnTrack()
    {
        var relogio = Relogio(15, 10);
        var versao = Criar("v1", Fase.Desenvolvimento, DatasAlfaV1, relogio);
        versao.AlterarSituacao(Situacao.Risco);
        versao.AlterarFase(Fase.Backlog, relogio);

        versao.AlterarFase(Fase.Analise, relogio);

        Assert.Equal(Situacao.OnTrack, versao.Situacao);
    }

    [Fact]
    public void RN22_Observacao_SubstituiOTexto()
    {
        var versao = new Versao(1, "v1");
        versao.AlterarObservacao("Homologação em andamento.");

        versao.AlterarObservacao("Acesso sendo liberado em lotes.");

        Assert.Equal("Acesso sendo liberado em lotes.", versao.Observacao);
    }
}

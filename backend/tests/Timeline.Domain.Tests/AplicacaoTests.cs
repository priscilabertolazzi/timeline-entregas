namespace Timeline.Domain.Tests;

/// <summary>
/// Regras da aplicação (RN-01). A unicidade do nome é verificada na Application (T-06).
/// </summary>
public class AplicacaoTests
{
    [Fact]
    public void RN01_NomeComEspacosNasPontas_GuardadoSemEles()
    {
        var aplicacao = new Aplicacao("  Alfa ");

        Assert.Equal("Alfa", aplicacao.Nome);
    }

    [Fact]
    public void RN01_Renomear_GuardaSemEspacosNasPontas()
    {
        var aplicacao = new Aplicacao("Alfa");

        aplicacao.Renomear(" Beta  ");

        Assert.Equal("Beta", aplicacao.Nome);
    }
}

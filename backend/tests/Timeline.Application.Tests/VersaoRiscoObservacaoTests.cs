using Microsoft.Extensions.Time.Testing;
using Timeline.Application.Aplicacoes;
using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Application.Tests;

/// <summary>RN-22: versão em Risco exige observação (MSG-43), pelas duas operações de gravação.</summary>
public sealed class VersaoRiscoObservacaoTests
{
    private readonly AplicacaoRepositoryEmMemoria _aplicacoes = new();
    private readonly VersaoRepositoryEmMemoria _versoes = new();
    private readonly FakeTimeProvider _relogio = new(new DateTimeOffset(2026, 10, 14, 12, 0, 0, TimeSpan.Zero));

    private static readonly DatasVersaoEntrada DatasAlfaV1 =
        new("2026-09-01", "2026-09-25", "2026-10-01", "2026-10-13", null, null, "2026-10-20");

    private async Task<VersaoResposta> VersaoEmDesenvolvimento()
    {
        var alfa = (await new CadastrarAplicacao(_aplicacoes).ExecutarAsync(new NomeAplicacaoEntrada("Alfa"), CancellationToken.None)).Id;
        var v1 = await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(alfa, new RotuloVersaoEntrada("v1"), CancellationToken.None);
        await new AlterarDatasVersao(_versoes, _relogio).ExecutarAsync(v1.Id, DatasAlfaV1, CancellationToken.None);
        return await new AlterarFaseVersao(_versoes, _relogio).ExecutarAsync(v1.Id, new FaseEntrada("Desenvolvimento"), CancellationToken.None);
    }

    [Fact]
    public async Task RN22_AlterarVersaoParaRiscoSemObservacao_RecusaComMsg43ENaoMudaNada()
    {
        var v1 = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => new AlterarVersao(_versoes).ExecutarAsync(v1.Id, new AlterarVersaoEntrada("v1.1", "Risco", null), CancellationToken.None));

        Assert.Equal(["Informe a observação quando a situação for Risco."], erro.Mensagens);
        var depois = await new DetalharVersao(_versoes).ExecutarAsync(v1.Id, CancellationToken.None);
        Assert.Equal("v1", depois.Rotulo);
        Assert.Equal(Situacao.OnTrack, depois.Situacao);
    }

    [Fact]
    public async Task RN22_AlterarVersaoParaRiscoComObservacao_Aceita()
    {
        var v1 = await VersaoEmDesenvolvimento();

        var alterada = await new AlterarVersao(_versoes).ExecutarAsync(
            v1.Id, new AlterarVersaoEntrada("v1", "Risco", "Dependência de outra equipe."), CancellationToken.None);

        Assert.Equal(Situacao.Risco, alterada.Situacao);
        Assert.Equal("Dependência de outra equipe.", alterada.Observacao);
    }

    [Fact]
    public async Task RN22_GravarTudoEmRiscoSemObservacao_RecusaComMsg43()
    {
        var v1 = await VersaoEmDesenvolvimento();

        var erro = await Assert.ThrowsAsync<RegraNegocioException>(
            () => new AlterarVersaoCompleta(_versoes, _relogio).ExecutarAsync(
                v1.Id, new AlterarVersaoCompletaEntrada("v1", "Risco", "   ", "Desenvolvimento", DatasAlfaV1), CancellationToken.None));

        Assert.Equal(["Informe a observação quando a situação for Risco."], erro.Mensagens);
        var depois = await new DetalharVersao(_versoes).ExecutarAsync(v1.Id, CancellationToken.None);
        Assert.Equal(Situacao.OnTrack, depois.Situacao);
        Assert.Null(depois.Observacao);
    }
}

using Microsoft.Extensions.Time.Testing;
using Timeline.Application.Exemplo;
using Timeline.Domain;

namespace Timeline.Application.Tests;

/// <summary>
/// Carga dos dados de exemplo (arquitetura.md, seção 12.1, ADR-25): aplicações Alfa, Beta, Gama e Delta,
/// com datas calculadas a partir do dia da carga.
/// </summary>
public sealed class CarregarDadosDeExemploTests
{
    private readonly AplicacaoRepositoryEmMemoria _aplicacoes = new();
    private readonly VersaoRepositoryEmMemoria _versoes = new();

    private async Task<bool> Carregar(DateOnly dia)
    {
        var relogio = new FakeTimeProvider(new DateTimeOffset(dia.Year, dia.Month, dia.Day, 12, 0, 0, TimeSpan.Zero));
        return await new CarregarDadosDeExemplo(_aplicacoes, _versoes, relogio).ExecutarAsync(CancellationToken.None);
    }

    [Fact]
    public async Task ADR25_BancoVazio_CriaAlfaBetaGamaEDelta_ComDeltaSemVersoes()
    {
        var carregou = await Carregar(new DateOnly(2026, 10, 15));

        Assert.True(carregou);
        Assert.Equal(["Alfa", "Beta", "Gama", "Delta"], _aplicacoes.Gravadas.Select(a => a.Nome));
        var delta = _aplicacoes.Gravadas.Single(a => a.Nome == "Delta");
        Assert.DoesNotContain(_versoes.Gravadas, v => v.AplicacaoId == delta.Id);
        Assert.Equal(4, _versoes.Gravadas.Count);
    }

    [Fact]
    public async Task ADR25_DiaDaCarga15Out2026_DatasRelativasAoDia()
    {
        await Carregar(new DateOnly(2026, 10, 15));

        var alfa = _aplicacoes.Gravadas.Single(a => a.Nome == "Alfa");
        var v1 = _versoes.Gravadas.Single(v => v.AplicacaoId == alfa.Id && v.Rotulo == "v1");
        Assert.Equal(Fase.Homologacao, v1.Fase);
        Assert.Equal(new DateOnly(2026, 9, 5), v1.Datas.InicioAndamento);
        Assert.Equal(new DateOnly(2026, 10, 23), v1.Datas.FimHomologacao);
        Assert.Equal(new DateOnly(2026, 10, 25), v1.Datas.DataEntrega);
    }

    [Fact]
    public async Task ADR25_BancoJaTemAplicacoes_NaoCarregaNada()
    {
        await _aplicacoes.AdicionarAsync(new Aplicacao("Omega"), CancellationToken.None);

        var carregou = await Carregar(new DateOnly(2026, 10, 15));

        Assert.False(carregou);
        Assert.Single(_aplicacoes.Gravadas);
        Assert.Empty(_versoes.Gravadas);
    }

    [Fact]
    public async Task ADR25_CargaRepetida_NaoDuplica()
    {
        await Carregar(new DateOnly(2026, 10, 15));
        await Carregar(new DateOnly(2026, 10, 16));

        Assert.Equal(4, _aplicacoes.Gravadas.Count);
        Assert.Equal(4, _versoes.Gravadas.Count);
    }

    [Fact]
    public async Task ADR25_BetaEmRisco_EV0DaAlfaEntregue()
    {
        await Carregar(new DateOnly(2026, 10, 15));

        var beta = _aplicacoes.Gravadas.Single(a => a.Nome == "Beta");
        var alfa = _aplicacoes.Gravadas.Single(a => a.Nome == "Alfa");
        Assert.Equal(Situacao.Risco, _versoes.Gravadas.Single(v => v.AplicacaoId == beta.Id).Situacao);
        Assert.Equal(Situacao.Entregue, _versoes.Gravadas.Single(v => v.AplicacaoId == alfa.Id && v.Rotulo == "v0").Situacao);
    }

    /// <summary>
    /// Simula a carga em cada dia de um ano bissexto (2028, para incluir 29/02): todas as versões
    /// precisam ser aceitas pelas regras do domínio (RN-07, RN-09, RN-10) e pelo menos uma precisa
    /// atravessar o dia da carga, para a timeline nunca aparecer vazia.
    /// </summary>
    [Fact]
    public async Task ADR25_CargaEmCadaDiaDeUmAno_RespeitaAsRegras_EUmaVersaoAtravessaOHoje()
    {
        for (var dia = new DateOnly(2028, 1, 1); dia.Year == 2028; dia = dia.AddDays(1))
        {
            _aplicacoes.Limpar();
            _versoes.Limpar();

            var carregou = await Carregar(dia);

            Assert.True(carregou, $"A carga falhou em {dia:dd/MM/yyyy}.");
            Assert.Equal(4, _versoes.Gravadas.Count);
            Assert.Contains(_versoes.Gravadas, v =>
                v.Fase != Fase.Producao && v.Datas.InicioAndamento <= dia && dia <= v.Datas.DataEntrega);
        }
    }
}

using Microsoft.Extensions.Time.Testing;
using Timeline.Application.Aplicacoes;
using Timeline.Application.Versoes;

namespace Timeline.Application.Tests;

/// <summary>A lista de aplicações traz a data de entrega de cada versão, ou nada quando não há datas (RN-13).</summary>
public sealed class ListaAplicacoesDataEntregaTests
{
    private readonly AplicacaoRepositoryEmMemoria _aplicacoes = new();
    private readonly VersaoRepositoryEmMemoria _versoes = new();
    private readonly FakeTimeProvider _relogio = new(new DateTimeOffset(2026, 10, 14, 12, 0, 0, TimeSpan.Zero));

    [Fact]
    public async Task RN13_ListaDeAplicacoes_TrazADataDeEntregaESemDatasVemNula()
    {
        var alfa = (await new CadastrarAplicacao(_aplicacoes).ExecutarAsync(new NomeAplicacaoEntrada("Alfa"), CancellationToken.None)).Id;
        var v1 = await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(alfa, new RotuloVersaoEntrada("v1"), CancellationToken.None);
        await new CadastrarVersao(_aplicacoes, _versoes).ExecutarAsync(alfa, new RotuloVersaoEntrada("v2"), CancellationToken.None);
        await new AlterarDatasVersao(_versoes, _relogio).ExecutarAsync(
            v1.Id, new DatasVersaoEntrada("2026-09-01", "2026-09-25", "2026-10-01", "2026-10-13", null, null, "2026-10-20"), CancellationToken.None);

        var lista = await new ListarAplicacoes(_aplicacoes, _versoes).ExecutarAsync(CancellationToken.None);

        Assert.Equal(new DateOnly(2026, 10, 20), lista[0].Versoes[0].DataEntrega);
        Assert.Null(lista[0].Versoes[1].DataEntrega);
    }
}

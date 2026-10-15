using Timeline.Application.Exemplo;

namespace TimelineEntregas.Host;

/// <summary>
/// Depois das migrations, carrega os dados fictícios de exemplo se o banco está vazio e a variável
/// <c>SEED_DADOS_EXEMPLO</c> do <c>.env</c> está ligada (arquitetura.md, seção 12.1, ADR-25).
/// </summary>
internal sealed class CargaDeExemploNaInicializacao(
    IServiceProvider servicos, IConfiguration configuracao, ILogger<CargaDeExemploNaInicializacao> log) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.Equals(configuracao["SEED_DADOS_EXEMPLO"], "true", StringComparison.OrdinalIgnoreCase))
        {
            log.LogInformation("Carga de dados de exemplo desligada.");
            return;
        }

        await using var escopo = servicos.CreateAsyncScope();
        var carga = escopo.ServiceProvider.GetRequiredService<CarregarDadosDeExemplo>();
        var carregou = await carga.ExecutarAsync(cancellationToken);
        log.LogInformation(carregou ? "Dados de exemplo carregados." : "O banco já tem dados; nada foi carregado.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

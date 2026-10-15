using Microsoft.EntityFrameworkCore;
using Timeline.Infrastructure;

namespace TimelineEntregas.Host;

/// <summary>
/// Aplica as migrations pendentes quando a API inicia, antes de ela aceitar chamadas. Fica num serviço
/// de inicialização, e não no <c>Program.cs</c>, para a ferramenta de migrations não mexer no banco
/// ao criar uma migration (ADR-34).
/// </summary>
internal sealed class MigracoesNaInicializacao(IServiceProvider servicos, ILogger<MigracoesNaInicializacao> log) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var escopo = servicos.CreateAsyncScope();
        var contexto = escopo.ServiceProvider.GetRequiredService<TimelineDbContext>();

        log.LogInformation("Verificando se o banco precisa ser atualizado.");
        await contexto.Database.MigrateAsync(cancellationToken);
        log.LogInformation("Banco atualizado.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

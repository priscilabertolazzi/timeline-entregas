using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timeline.Application.Aplicacoes;

namespace Timeline.Infrastructure;

/// <summary>Registro da camada de infraestrutura na injeção de dependências.</summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registra o <see cref="TimelineDbContext"/> no PostgreSQL e os repositórios. A tabela de histórico
    /// de migrations também fica no schema <c>timeline</c>.
    /// </summary>
    public static IServiceCollection AddTimelineInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TimelineDbContext>(options => options.UseNpgsql(
            connectionString,
            npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", TimelineDbContext.Schema)));
        services.AddScoped<IAplicacaoRepository, AplicacaoRepository>();
        return services;
    }
}

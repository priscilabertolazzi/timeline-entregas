using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Timeline.Infrastructure;

/// <summary>Registro da camada de infraestrutura na injeção de dependências.</summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registra o <see cref="TimelineDbContext"/> no PostgreSQL. A tabela de histórico de migrations
    /// também fica no schema <c>timeline</c>.
    /// </summary>
    public static IServiceCollection AddTimelineInfrastructure(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<TimelineDbContext>(options => options.UseNpgsql(
            connectionString,
            npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", TimelineDbContext.Schema)));
    }
}

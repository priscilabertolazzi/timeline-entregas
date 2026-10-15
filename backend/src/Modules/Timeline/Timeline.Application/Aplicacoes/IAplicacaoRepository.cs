using Timeline.Domain;

namespace Timeline.Application.Aplicacoes;

/// <summary>Acesso aos dados das aplicações. A implementação fica na Infrastructure.</summary>
public interface IAplicacaoRepository
{
    /// <summary>Devolve todas as aplicações, em qualquer ordem.</summary>
    Task<IReadOnlyList<Aplicacao>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>Devolve a aplicação, ou nulo se ela não existir.</summary>
    Task<Aplicacao?> ObterAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Informa se já existe aplicação com o nome, sem diferença entre maiúsculas e minúsculas (RN-01).
    /// <paramref name="ignorarId"/> exclui da busca a própria aplicação, no caso de renomear.
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, long? ignorarId, CancellationToken cancellationToken);

    /// <summary>
    /// Grava a aplicação nova. Se o banco acusar nome duplicado (dois cadastros simultâneos),
    /// lança <see cref="Erros.ConflitoException"/>.
    /// </summary>
    Task AdicionarAsync(Aplicacao aplicacao, CancellationToken cancellationToken);

    /// <summary>
    /// Grava o novo nome de uma aplicação obtida por <see cref="ObterAsync"/>. Se o banco acusar
    /// nome duplicado, lança <see cref="Erros.ConflitoException"/>.
    /// </summary>
    Task SalvarAsync(Aplicacao aplicacao, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui a aplicação e, em cascata, suas versões (RN-04). Devolve falso se ela não existir.
    /// </summary>
    Task<bool> ExcluirAsync(long id, CancellationToken cancellationToken);
}

using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Acesso aos dados das versões. A implementação fica na Infrastructure.</summary>
public interface IVersaoRepository
{
    /// <summary>Devolve as versões de todas as aplicações, em qualquer ordem.</summary>
    Task<IReadOnlyList<Versao>> ListarAsync(CancellationToken cancellationToken);

    /// <summary>Devolve a versão, ou nulo se ela não existir.</summary>
    Task<Versao?> ObterAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Informa se a aplicação já tem versão com o rótulo, sem diferença entre maiúsculas e minúsculas
    /// (RN-02). <paramref name="ignorarId"/> exclui da busca a própria versão, no caso de alterar.
    /// </summary>
    Task<bool> ExisteRotuloAsync(long aplicacaoId, string rotulo, long? ignorarId, CancellationToken cancellationToken);

    /// <summary>
    /// Grava a versão nova. Se o banco acusar rótulo duplicado (dois cadastros simultâneos),
    /// lança <see cref="Erros.ConflitoException"/>.
    /// </summary>
    Task AdicionarAsync(Versao versao, CancellationToken cancellationToken);

    /// <summary>
    /// Grava as alterações de uma versão obtida por <see cref="ObterAsync"/>. Se o banco acusar
    /// rótulo duplicado, lança <see cref="Erros.ConflitoException"/>.
    /// </summary>
    Task SalvarAsync(Versao versao, CancellationToken cancellationToken);

    /// <summary>Exclui a versão. Devolve falso se ela não existir.</summary>
    Task<bool> ExcluirAsync(long id, CancellationToken cancellationToken);
}

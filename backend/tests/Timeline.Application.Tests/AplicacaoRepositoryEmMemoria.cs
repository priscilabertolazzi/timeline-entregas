using Timeline.Application.Aplicacoes;
using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Tests;

/// <summary>
/// Repositório simples em memória, para testar os casos de uso sem banco. Repete a unicidade sem
/// diferença entre maiúsculas e minúsculas que o banco garante (ADR-23).
/// </summary>
internal sealed class AplicacaoRepositoryEmMemoria : IAplicacaoRepository
{
    private readonly List<Aplicacao> _aplicacoes = [];
    private long _ultimoId;

    /// <summary>Quando ligado, simula dois cadastros simultâneos: a checagem prévia não vê o nome, mas o banco recusa.</summary>
    public bool BancoRecusaDuplicidadeNaGravacao { get; set; }

    /// <summary>Aplicações gravadas, na ordem de cadastro.</summary>
    public IReadOnlyList<Aplicacao> Gravadas => _aplicacoes;

    public Task<IReadOnlyList<Aplicacao>> ListarAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<Aplicacao>>(_aplicacoes.ToList());
    }

    public Task<Aplicacao?> ObterAsync(long id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_aplicacoes.FirstOrDefault(a => a.Id == id));
    }

    public Task<bool> ExisteNomeAsync(string nome, long? ignorarId, CancellationToken cancellationToken)
    {
        if (BancoRecusaDuplicidadeNaGravacao)
        {
            return Task.FromResult(false);
        }

        var existe = _aplicacoes.Any(a => a.Id != ignorarId && string.Equals(a.Nome, nome, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existe);
    }

    public Task AdicionarAsync(Aplicacao aplicacao, CancellationToken cancellationToken)
    {
        RecusarDuplicidade(aplicacao);
        typeof(Aplicacao).GetProperty(nameof(Aplicacao.Id))!.SetValue(aplicacao, ++_ultimoId);
        _aplicacoes.Add(aplicacao);
        return Task.CompletedTask;
    }

    public Task SalvarAsync(Aplicacao aplicacao, CancellationToken cancellationToken)
    {
        RecusarDuplicidade(aplicacao);
        return Task.CompletedTask;
    }

    public Task<bool> ExcluirAsync(long id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_aplicacoes.RemoveAll(a => a.Id == id) > 0);
    }

    private void RecusarDuplicidade(Aplicacao aplicacao)
    {
        var duplicada = _aplicacoes.Any(a => !ReferenceEquals(a, aplicacao) && string.Equals(a.Nome, aplicacao.Nome, StringComparison.OrdinalIgnoreCase));
        if (duplicada)
        {
            throw new ConflitoException(Mensagens.Msg03);
        }
    }
}

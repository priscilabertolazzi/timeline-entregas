using Timeline.Application.Erros;
using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Application.Tests;

/// <summary>
/// Repositório simples em memória, para testar os casos de uso de versões sem banco. Repete a
/// unicidade do rótulo por aplicação, sem diferença entre maiúsculas e minúsculas (ADR-23).
/// </summary>
internal sealed class VersaoRepositoryEmMemoria : IVersaoRepository
{
    private readonly List<Versao> _versoes = [];
    private long _ultimoId;

    /// <summary>Versões gravadas, na ordem de cadastro.</summary>
    public IReadOnlyList<Versao> Gravadas => _versoes;

    public Task<IReadOnlyList<Versao>> ListarAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<Versao>>(_versoes.ToList());
    }

    public Task<Versao?> ObterAsync(long id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_versoes.FirstOrDefault(v => v.Id == id));
    }

    public Task<bool> ExisteRotuloAsync(long aplicacaoId, string rotulo, long? ignorarId, CancellationToken cancellationToken)
    {
        var existe = _versoes.Any(v => v.AplicacaoId == aplicacaoId && v.Id != ignorarId
            && string.Equals(v.Rotulo, rotulo, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existe);
    }

    public Task AdicionarAsync(Versao versao, CancellationToken cancellationToken)
    {
        RecusarDuplicidade(versao);
        typeof(Versao).GetProperty(nameof(Versao.Id))!.SetValue(versao, ++_ultimoId);
        _versoes.Add(versao);
        return Task.CompletedTask;
    }

    public Task SalvarAsync(Versao versao, CancellationToken cancellationToken)
    {
        RecusarDuplicidade(versao);
        return Task.CompletedTask;
    }

    public Task<bool> ExcluirAsync(long id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_versoes.RemoveAll(v => v.Id == id) > 0);
    }

    private void RecusarDuplicidade(Versao versao)
    {
        var duplicada = _versoes.Any(v => !ReferenceEquals(v, versao) && v.AplicacaoId == versao.AplicacaoId
            && string.Equals(v.Rotulo, versao.Rotulo, StringComparison.OrdinalIgnoreCase));
        if (duplicada)
        {
            throw new ConflitoException(Mensagens.Msg07);
        }
    }
}

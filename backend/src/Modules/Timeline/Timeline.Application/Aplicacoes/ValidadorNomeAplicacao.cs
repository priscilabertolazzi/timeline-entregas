using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Aplicacoes;

/// <summary>Validação de formato do nome da aplicação (arquitetura.md, seção 7.5).</summary>
internal static class ValidadorNomeAplicacao
{
    /// <summary>Limite aprovado na seção 6 do arquitetura.md.</summary>
    public const int TamanhoMaximo = 100;

    /// <summary>
    /// Devolve o nome sem os espaços das pontas (RN-01). Nome vazio dá a MSG-01, e nome com mais de
    /// 100 caracteres, a MSG-02. O tamanho conta caracteres, como o banco, e não unidades de texto.
    /// </summary>
    public static string Validar(string? nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new EntradaInvalidaException(Mensagens.Msg01);
        }

        var aparado = nome.Trim();
        if (aparado.EnumerateRunes().Count() > TamanhoMaximo)
        {
            throw new EntradaInvalidaException(Mensagens.Msg02);
        }

        return aparado;
    }
}

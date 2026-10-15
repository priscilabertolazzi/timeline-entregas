using System.Globalization;
using System.Text;
using Timeline.Application.Erros;
using Timeline.Domain;

namespace Timeline.Application.Versoes;

/// <summary>Validação de formato da entrada de versões (arquitetura.md, seção 7.5).</summary>
internal static class ValidadorEntradaVersao
{
    /// <summary>Limite do rótulo, aprovado na seção 6 do arquitetura.md.</summary>
    public const int TamanhoMaximoRotulo = 20;

    /// <summary>Limite da observação (RN-22).</summary>
    public const int TamanhoMaximoObservacao = 1000;

    /// <summary>
    /// Devolve o rótulo sem os espaços das pontas (RN-02). Vazio dá a MSG-05, e mais de 20
    /// caracteres, a MSG-06. O tamanho conta caracteres, como o banco.
    /// </summary>
    public static string ValidarRotulo(string? rotulo)
    {
        if (string.IsNullOrWhiteSpace(rotulo))
        {
            throw new EntradaInvalidaException(Mensagens.Msg05);
        }

        var aparado = rotulo.Trim();
        if (aparado.EnumerateRunes().Count() > TamanhoMaximoRotulo)
        {
            throw new EntradaInvalidaException(Mensagens.Msg06);
        }

        return aparado;
    }

    /// <summary>Observação vazia vira nulo (sem observação); com mais de 1000 caracteres, MSG-09 (RN-22).</summary>
    public static string? ValidarObservacao(string? observacao)
    {
        if (string.IsNullOrWhiteSpace(observacao))
        {
            return null;
        }

        if (observacao.EnumerateRunes().Count() > TamanhoMaximoObservacao)
        {
            throw new EntradaInvalidaException(Mensagens.Msg09);
        }

        return observacao;
    }

    /// <summary>
    /// Converte o texto em On track ou Risco (RN-21), sem diferença entre maiúsculas e minúsculas e
    /// aceitando "On track" ou "OnTrack". Qualquer outro valor dá a MSG-10.
    /// </summary>
    public static Situacao ValidarSituacao(string situacao)
    {
        return Normalizar(situacao) switch
        {
            "ontrack" => Situacao.OnTrack,
            "risco" => Situacao.Risco,
            _ => throw new EntradaInvalidaException(Mensagens.Msg10),
        };
    }

    /// <summary>
    /// Converte o texto numa fase (RN-05), pelo nome, sem diferença entre maiúsculas e minúsculas nem
    /// de acentos. Número ou nome desconhecido dá a MSG-26.
    /// </summary>
    public static Fase ValidarFase(string? fase)
    {
        var procurada = Normalizar(fase);
        foreach (var candidata in Enum.GetValues<Fase>())
        {
            if (Normalizar(candidata.ToString()) == procurada)
            {
                return candidata;
            }
        }

        throw new EntradaInvalidaException(Mensagens.Msg26);
    }

    /// <summary>Converte as sete datas AAAA-MM-DD; qualquer data torta dá a MSG-12. Data vazia é data removida.</summary>
    public static DatasVersao ValidarDatas(DatasVersaoEntrada entrada)
    {
        return new DatasVersao(
            ValidarData(entrada.InicioAndamento),
            ValidarData(entrada.FimAndamento),
            ValidarData(entrada.InicioHomologacao),
            ValidarData(entrada.FimHomologacao),
            ValidarData(entrada.InicioProrrogacao),
            ValidarData(entrada.FimProrrogacao),
            ValidarData(entrada.DataEntrega));
    }

    private static DateOnly? ValidarData(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return null;
        }

        if (DateOnly.TryParseExact(texto.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
        {
            return data;
        }

        throw new EntradaInvalidaException(Mensagens.Msg12);
    }

    // Minúsculas, sem acentos e sem espaços: "On track" = "OnTrack", "Análise" = "analise".
    private static string Normalizar(string? texto)
    {
        if (texto is null)
        {
            return string.Empty;
        }

        var semAcentos = texto.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
        return new string(semAcentos.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLowerInvariant();
    }
}

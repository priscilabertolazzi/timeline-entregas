using Microsoft.Extensions.Time.Testing;

namespace Timeline.Domain.Tests;

/// <summary>
/// Apoio dos testes: monta versões já numa fase, com datas, usando só as operações públicas da
/// entidade, e fixa a data atual com o relógio falso.
/// </summary>
internal static class VersaoTeste
{
    /// <summary>Data de 2026 (todos os critérios de aceite usam esse ano).</summary>
    public static DateOnly Dia(int dia, int mes) => new(2026, mes, dia);

    /// <summary>Relógio parado ao meio-dia da data informada.</summary>
    public static FakeTimeProvider Relogio(int dia, int mes) =>
        new(new DateTimeOffset(2026, mes, dia, 12, 0, 0, TimeSpan.Zero));

    /// <summary>Cinco datas do ciclo, sem prorrogação.</summary>
    public static DatasVersao Ciclo(DateOnly inicioAndamento, DateOnly fimAndamento,
        DateOnly inicioHomologacao, DateOnly fimHomologacao, DateOnly dataEntrega) =>
        new(inicioAndamento, fimAndamento, inicioHomologacao, fimHomologacao, null, null, dataEntrega);

    /// <summary>
    /// Datas da v1 da Alfa nos critérios de aceite: andamento de 01/09 a 25/09, homologação de
    /// 01/10 a 13/10 e entrega em 20/10.
    /// </summary>
    public static DatasVersao DatasAlfaV1 { get; } =
        Ciclo(Dia(1, 9), Dia(25, 9), Dia(1, 10), Dia(13, 10), Dia(20, 10));

    /// <summary>
    /// Datas da v0 da Beta no CA-40: andamento de 01/08 a 14/08, homologação de 17/08 a 21/08 e
    /// entrega em 24/08.
    /// </summary>
    public static DatasVersao DatasBetaV0 { get; } =
        Ciclo(Dia(1, 8), Dia(14, 8), Dia(17, 8), Dia(21, 8), Dia(24, 8));

    /// <summary>
    /// Monta uma versão na fase pedida. A prorrogação, se houver, é registrada em Homologação,
    /// antes de seguir para a fase final, como faria o usuário.
    /// </summary>
    public static Versao Criar(string rotulo, Fase fase, DatasVersao datas, TimeProvider relogio)
    {
        var versao = new Versao(1, rotulo);
        if (datas.EstaVazia)
        {
            return versao;
        }

        versao.AlterarDatas(datas with { InicioProrrogacao = null, FimProrrogacao = null }, relogio);
        if (datas.TemProrrogacao)
        {
            versao.AlterarFase(Fase.Homologacao, relogio);
            versao.AlterarDatas(datas, relogio);
        }

        versao.AlterarFase(fase, relogio);
        return versao;
    }
}

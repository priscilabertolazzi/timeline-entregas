using Timeline.Application.Aplicacoes;
using Timeline.Application.Versoes;
using Timeline.Domain;

namespace Timeline.Application.Exemplo;

/// <summary>
/// Caso de uso: cria as aplicações fictícias Alfa, Beta, Gama e Delta, com versões em várias fases,
/// quando o banco está vazio (arquitetura.md, seção 12.1, ADR-25). As datas são calculadas a partir do
/// dia da carga, para que a timeline do semestre atual nunca apareça vazia.
/// </summary>
public sealed class CarregarDadosDeExemplo(IAplicacaoRepository aplicacoes, IVersaoRepository versoes, TimeProvider relogio)
{
    // Cada versão: aplicação, rótulo, fase, situação informada e os deslocamentos, em dias a partir do
    // dia da carga, de início e fim do andamento, início e fim da homologação e entrega.
    private static readonly ModeloVersao[] Modelos =
    [
        new("Alfa", "v0", Fase.Producao, Situacao.OnTrack, -110, -60, -59, -30, -20),
        new("Alfa", "v1", Fase.Homologacao, Situacao.OnTrack, -40, -10, -9, 8, 10),
        new("Beta", "v1", Fase.Desenvolvimento, Situacao.Risco, -15, 25, 26, 40, 45),
        new("Gama", "v1", Fase.Testes, Situacao.OnTrack, -30, 5, 6, 20, 25),
    ];

    private static readonly string[] NomesDasAplicacoes = ["Alfa", "Beta", "Gama", "Delta"];

    /// <summary>
    /// Se já existe alguma aplicação, não faz nada e devolve falso. Senão, grava os dados de exemplo e
    /// devolve verdadeiro. A Delta fica sem versões, como no CA-23.
    /// </summary>
    public async Task<bool> ExecutarAsync(CancellationToken cancellationToken)
    {
        if ((await aplicacoes.ListarAsync(cancellationToken)).Count > 0)
        {
            return false;
        }

        var hoje = DateOnly.FromDateTime(relogio.GetLocalNow().DateTime);
        var ids = new Dictionary<string, long>();
        foreach (var nome in NomesDasAplicacoes)
        {
            var aplicacao = new Aplicacao(nome);
            await aplicacoes.AdicionarAsync(aplicacao, cancellationToken);
            ids[nome] = aplicacao.Id;
        }

        foreach (var modelo in Modelos)
        {
            var versao = new Versao(ids[modelo.Aplicacao], modelo.Rotulo);
            versao.AlterarDatas(modelo.DatasEm(hoje), relogio);
            versao.AlterarFase(modelo.Fase, relogio);
            if (modelo.Situacao != Situacao.OnTrack)
            {
                versao.AlterarSituacao(modelo.Situacao);
            }

            await versoes.AdicionarAsync(versao, cancellationToken);
        }

        return true;
    }

    private sealed record ModeloVersao(
        string Aplicacao, string Rotulo, Fase Fase, Situacao Situacao,
        int InicioAndamento, int FimAndamento, int InicioHomologacao, int FimHomologacao, int Entrega)
    {
        public DatasVersao DatasEm(DateOnly hoje) => new(
            hoje.AddDays(InicioAndamento), hoje.AddDays(FimAndamento),
            hoje.AddDays(InicioHomologacao), hoje.AddDays(FimHomologacao),
            null, null, hoje.AddDays(Entrega));
    }
}

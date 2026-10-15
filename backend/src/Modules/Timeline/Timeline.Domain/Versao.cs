namespace Timeline.Domain;

/// <summary>
/// Versão de uma aplicação. Só muda pelos métodos abaixo, que validam as regras antes de alterar
/// qualquer dado (arquitetura.md, seção 5.3). Se uma regra falhar, lançam
/// <see cref="RegraNegocioException"/> com as mensagens de todas as regras violadas e nada muda.
/// </summary>
public sealed class Versao
{
    /// <summary>Cria a versão na fase Backlog, sem datas e com a situação informada On track (RN-02, RN-21).</summary>
    public Versao(long aplicacaoId, string rotulo)
    {
        AplicacaoId = aplicacaoId;
        Fase = Fase.Backlog;
        SituacaoInformada = Situacao.OnTrack;
        Renomear(rotulo);
    }

    /// <summary>Identificador gerado pelo banco.</summary>
    public long Id { get; private set; }

    /// <summary>Aplicação a que a versão pertence.</summary>
    public long AplicacaoId { get; private set; }

    /// <summary>Rótulo da versão (ex.: v1.2), sem os espaços das pontas.</summary>
    public string Rotulo { get; private set; } = string.Empty;

    /// <summary>Fase atual (RN-05).</summary>
    public Fase Fase { get; private set; }

    /// <summary>Situação informada pelo usuário: On track ou Risco (RN-21).</summary>
    public Situacao SituacaoInformada { get; private set; }

    /// <summary>
    /// Situação exibida (RN-21): Não iniciado em Backlog, Entregue em Produção e, nas demais
    /// fases, a situação informada.
    /// </summary>
    public Situacao Situacao => Fase switch
    {
        Fase.Backlog => Situacao.NaoIniciado,
        Fase.Producao => Situacao.Entregue,
        _ => SituacaoInformada,
    };

    /// <summary>Observação em texto livre (RN-22).</summary>
    public string? Observacao { get; private set; }

    /// <summary>As sete datas da versão (RN-09).</summary>
    public DatasVersao Datas { get; private set; } = DatasVersao.Vazias;

    /// <summary>
    /// Grava as sete datas de uma vez (RF-05, RF-14), depois de verificar RN-07, RN-09, RN-10 e
    /// RN-24. A data atual vem do relógio informado.
    /// </summary>
    public void AlterarDatas(DatasVersao novas, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(novas);
        ArgumentNullException.ThrowIfNull(relogio);

        var violacoes = new List<string>();

        if (novas.EstaVazia)
        {
            // RN-07: as datas só podem ser removidas em Backlog.
            if (!Datas.EstaVazia && Fase != Fase.Backlog)
            {
                violacoes.Add(Mensagens.Msg24);
            }
        }
        else
        {
            VerificarPreenchimento(novas, violacoes);
            if (novas.CicloCompleto)
            {
                VerificarOrdem(novas, violacoes);
            }

            // RN-24: só a prorrogação nova exige Homologação; a já registrada pode ser
            // corrigida ou removida também em Produção.
            if (novas.TemProrrogacao && !Datas.TemProrrogacao && Fase != Fase.Homologacao)
            {
                violacoes.Add(Mensagens.Msg23);
            }

            // RN-07: em Produção, a entrega não passa da data atual.
            if (Fase == Fase.Producao && novas.DataEntrega > Hoje(relogio))
            {
                violacoes.Add(Mensagens.Msg25);
            }
        }

        LancarSeHouver(violacoes);
        Datas = novas;
    }

    /// <summary>Muda a fase, respeitando o sentido das fases (RN-08) e as condições da RN-07.</summary>
    public void AlterarFase(Fase nova, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(relogio);

        // Fase desconhecida é recusada antes, na Application, com a MSG-26; aqui é só uma proteção.
        if (!Enum.IsDefined(nova))
        {
            throw new ArgumentOutOfRangeException(nameof(nova), nova, "Fase desconhecida.");
        }

        if (nova == Fase)
        {
            return;
        }

        var violacoes = new List<string>();

        if (Fase == Fase.Producao)
        {
            violacoes.Add(Mensagens.Msg30);
        }
        else if (Fase == Fase.Homologacao && nova < Fase.Homologacao)
        {
            violacoes.Add(Mensagens.Msg27);
        }
        else if (Fase == Fase.Backlog && Datas.EstaVazia)
        {
            violacoes.Add(Mensagens.Msg28);
        }
        else if (nova == Fase.Producao && Datas.DataEntrega > Hoje(relogio))
        {
            violacoes.Add(Mensagens.Msg29);
        }

        LancarSeHouver(violacoes);

        // RN-21: ao sair do Backlog, a situação informada recomeça em On track.
        if (Fase == Fase.Backlog)
        {
            SituacaoInformada = Situacao.OnTrack;
        }

        Fase = nova;
    }

    /// <summary>
    /// Define a situação informada, On track ou Risco, só fora de Backlog e de Produção
    /// (RN-21, RF-10).
    /// </summary>
    public void AlterarSituacao(Situacao nova)
    {
        // Valor diferente de On track ou Risco é recusado antes, na Application, com a MSG-10.
        if (nova is not (Situacao.OnTrack or Situacao.Risco))
        {
            throw new ArgumentOutOfRangeException(nameof(nova), nova, "A situação informada deve ser On track ou Risco.");
        }

        if (Fase is Fase.Backlog or Fase.Producao)
        {
            throw new RegraNegocioException([Mensagens.Msg11]);
        }

        SituacaoInformada = nova;
    }

    /// <summary>
    /// Substitui a observação (RN-22). O limite de tamanho é verificado na Application, com a MSG-09.
    /// </summary>
    public void AlterarObservacao(string? texto)
    {
        Observacao = texto;
    }

    /// <summary>
    /// Altera o rótulo, guardado sem os espaços das pontas (RN-02). A unicidade na aplicação é
    /// verificada na Application e garantida no banco.
    /// </summary>
    public void Renomear(string rotulo)
    {
        // Rótulo vazio é recusado antes, na Application, com a MSG-05; aqui é só uma proteção.
        ArgumentException.ThrowIfNullOrWhiteSpace(rotulo);
        Rotulo = rotulo.Trim();
    }

    private static void VerificarPreenchimento(DatasVersao novas, List<string> violacoes)
    {
        // RN-09: as cinco datas do ciclo vêm juntas.
        var cicloVazio = novas.InicioAndamento is null && novas.FimAndamento is null
            && novas.InicioHomologacao is null && novas.FimHomologacao is null && novas.DataEntrega is null;
        if (!cicloVazio && !novas.CicloCompleto)
        {
            violacoes.Add(Mensagens.Msg13);
        }

        // RN-24: início e fim da prorrogação vêm juntos.
        if ((novas.InicioProrrogacao is null) != (novas.FimProrrogacao is null))
        {
            violacoes.Add(Mensagens.Msg14);
        }

        // RN-09: a prorrogação só existe com as cinco datas do ciclo.
        if (novas.TemProrrogacao && !novas.CicloCompleto)
        {
            violacoes.Add(Mensagens.Msg15);
        }
    }

    private static void VerificarOrdem(DatasVersao novas, List<string> violacoes)
    {
        // RN-10: cada data é igual ou posterior à anterior, comparando par a par.
        AdicionarSeDepois(novas.InicioAndamento, novas.FimAndamento, Mensagens.Msg16, violacoes);
        AdicionarSeDepois(novas.FimAndamento, novas.InicioHomologacao, Mensagens.Msg17, violacoes);
        AdicionarSeDepois(novas.InicioHomologacao, novas.FimHomologacao, Mensagens.Msg18, violacoes);

        if (novas.InicioProrrogacao is not null && novas.FimProrrogacao is not null)
        {
            AdicionarSeDepois(novas.FimHomologacao, novas.InicioProrrogacao, Mensagens.Msg19, violacoes);
            AdicionarSeDepois(novas.InicioProrrogacao, novas.FimProrrogacao, Mensagens.Msg20, violacoes);
            AdicionarSeDepois(novas.FimProrrogacao, novas.DataEntrega, Mensagens.Msg21, violacoes);
        }
        else
        {
            AdicionarSeDepois(novas.FimHomologacao, novas.DataEntrega, Mensagens.Msg22, violacoes);
        }
    }

    private static void AdicionarSeDepois(DateOnly? anterior, DateOnly? seguinte, string mensagem, List<string> violacoes)
    {
        if (anterior > seguinte)
        {
            violacoes.Add(mensagem);
        }
    }

    private static void LancarSeHouver(List<string> violacoes)
    {
        if (violacoes.Count > 0)
        {
            throw new RegraNegocioException(violacoes);
        }
    }

    // A data atual é o dia local do relógio, nunca o relógio do sistema (arquitetura.md, seção 5.6).
    private static DateOnly Hoje(TimeProvider relogio) => DateOnly.FromDateTime(relogio.GetLocalNow().DateTime);
}

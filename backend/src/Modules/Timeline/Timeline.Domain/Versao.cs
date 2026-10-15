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
        AlterarDatas(novas, relogio, Fase);
    }

    // faseDaProrrogacao: fase em que a versão estará depois do salvamento. No salvamento único, quando a
    // fase avança, as datas entram antes da fase; a prorrogação nova vale para a fase de destino (RN-24).
    private void AlterarDatas(DatasVersao novas, TimeProvider relogio, Fase faseDaProrrogacao)
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
            if (novas.TemProrrogacao && !Datas.TemProrrogacao && faseDaProrrogacao != Fase.Homologacao)
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

    /// <summary>
    /// Grava de uma vez rótulo, situação, observação, fase e as sete datas (ADR-39), como o formulário
    /// da tela da versão. Tudo ou nada: se qualquer regra falhar, nada muda (CA-32) e a exceção traz as
    /// mensagens do primeiro passo que falhou. Fase e datas dependem uma da outra (RN-07), por isso a
    /// ordem é: avançando de fase, as datas vêm antes (sair do Backlog exige datas, e Produção exige a
    /// entrega até a data atual), e uma prorrogação nova vale para a fase de destino; nos demais casos, a fase vem antes (voltar ao Backlog libera a
    /// remoção das datas). A situação vem depois da fase, pois ao sair do Backlog ela recomeça em On track.
    /// </summary>
    /// <param name="situacao">Nulo mantém a situação atual.</param>
    public void Alterar(string rotulo, Situacao? situacao, string? observacao, Fase fase, DatasVersao datas, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(datas);
        ArgumentNullException.ThrowIfNull(relogio);

        TudoOuNada(() =>
        {
            if (fase > Fase)
            {
                AlterarDatas(datas, relogio, fase);
                AlterarFase(fase, relogio);
            }
            else
            {
                AlterarFase(fase, relogio);
                AlterarDatas(datas, relogio);
            }

            AplicarDados(rotulo, situacao, observacao);
        });
    }

    /// <summary>
    /// Altera rótulo, situação e observação de uma vez (RF-03, RF-09, RF-10), tudo ou nada. Em Risco, a
    /// observação é obrigatória (RN-22, MSG-43). Situação nula mantém a atual.
    /// </summary>
    public void AlterarDados(string rotulo, Situacao? situacao, string? observacao)
    {
        TudoOuNada(() => AplicarDados(rotulo, situacao, observacao));
    }

    // Situação, rótulo e observação, nesta ordem, e depois a regra do Risco, que olha o resultado final.
    private void AplicarDados(string rotulo, Situacao? situacao, string? observacao)
    {
        if (situacao is not null && situacao != SituacaoInformada)
        {
            AlterarSituacao(situacao.Value);
        }

        Renomear(rotulo);
        AlterarObservacao(observacao);

        // RN-22: versão em Risco exige observação (texto só com espaços conta como vazio).
        if (Situacao == Situacao.Risco && string.IsNullOrWhiteSpace(Observacao))
        {
            throw new RegraNegocioException([Mensagens.Msg43]);
        }
    }

    // Se qualquer passo violar uma regra, desfaz o que os passos anteriores já tinham mudado.
    private void TudoOuNada(Action passos)
    {
        var faseAnterior = Fase;
        var situacaoAnterior = SituacaoInformada;
        var datasAnteriores = Datas;
        var rotuloAnterior = Rotulo;
        var observacaoAnterior = Observacao;

        try
        {
            passos();
        }
        catch (RegraNegocioException)
        {
            Fase = faseAnterior;
            SituacaoInformada = situacaoAnterior;
            Datas = datasAnteriores;
            Rotulo = rotuloAnterior;
            Observacao = observacaoAnterior;
            throw;
        }
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

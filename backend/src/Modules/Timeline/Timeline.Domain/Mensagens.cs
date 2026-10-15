namespace Timeline.Domain;

/// <summary>
/// Mensagens de regra de negócio do catálogo (arquitetura.md, seção 7.6), com o texto exato.
/// O número de cada constante é o ID da mensagem no catálogo.
/// </summary>
public static class Mensagens
{
    /// <summary>Situação alterada em Backlog ou Produção (RN-21).</summary>
    public const string Msg11 = "A situação só pode ser alterada em versões em Análise, Desenvolvimento, Testes ou Homologação.";

    /// <summary>Datas do ciclo informadas em parte (RN-09, CA-25).</summary>
    public const string Msg13 = "As datas de andamento, de homologação e de entrega devem ser informadas em conjunto.";

    /// <summary>Só uma das datas da prorrogação informada (RN-24).</summary>
    public const string Msg14 = "O início e o fim da prorrogação devem ser informados em conjunto.";

    /// <summary>Prorrogação sem as datas do ciclo (RN-09).</summary>
    public const string Msg15 = "Informe as datas de andamento, de homologação e de entrega antes de registrar a prorrogação.";

    /// <summary>Início do andamento depois do fim do andamento (RN-10).</summary>
    public const string Msg16 = "O início do andamento não pode ser posterior ao fim do andamento.";

    /// <summary>Fim do andamento depois do início da homologação (RN-10, CA-05).</summary>
    public const string Msg17 = "O fim do andamento não pode ser posterior ao início da homologação.";

    /// <summary>Início da homologação depois do fim da homologação (RN-10).</summary>
    public const string Msg18 = "O início da homologação não pode ser posterior ao fim da homologação.";

    /// <summary>Fim da homologação depois do início da prorrogação (RN-10).</summary>
    public const string Msg19 = "O fim da homologação não pode ser posterior ao início da prorrogação.";

    /// <summary>Início da prorrogação depois do fim da prorrogação (RN-10).</summary>
    public const string Msg20 = "O início da prorrogação não pode ser posterior ao fim da prorrogação.";

    /// <summary>Fim da prorrogação depois da data de entrega (RN-10, CA-32).</summary>
    public const string Msg21 = "O fim da prorrogação não pode ser posterior à data de entrega.";

    /// <summary>Sem prorrogação: fim da homologação depois da data de entrega (RN-10).</summary>
    public const string Msg22 = "O fim da homologação não pode ser posterior à data de entrega.";

    /// <summary>Nova prorrogação fora da Homologação (RN-24, CA-36, CA-42).</summary>
    public const string Msg23 = "A prorrogação só pode ser registrada para versões em Homologação.";

    /// <summary>Remoção das datas fora do Backlog (RN-07, CA-20).</summary>
    public const string Msg24 = "Para remover as datas, mude antes a fase da versão para Backlog.";

    /// <summary>Data de entrega futura em Produção (RN-07).</summary>
    public const string Msg25 = "Em Produção, a data de entrega não pode ser posterior à data atual.";

    /// <summary>Fase com valor desconhecido (RN-05). Validada na Application, com erro 400.</summary>
    public const string Msg26 = "Fase inválida.";

    /// <summary>Retorno a partir de Homologação (RN-08, CA-06).</summary>
    public const string Msg27 = "Uma versão em Homologação não retorna a fases anteriores. Para estender o prazo, altere o fim da homologação ou registre uma prorrogação.";

    /// <summary>Saída do Backlog sem datas (RN-07, CA-13).</summary>
    public const string Msg28 = "Informe as datas da versão antes de iniciá-la.";

    /// <summary>Produção com data de entrega futura (RN-07, CA-39).</summary>
    public const string Msg29 = "Para passar para Produção, a data de entrega deve ser igual ou anterior à data atual.";

    /// <summary>Mudança de fase a partir de Produção (RN-08).</summary>
    public const string Msg30 = "Uma versão em Produção não muda mais de fase.";
}

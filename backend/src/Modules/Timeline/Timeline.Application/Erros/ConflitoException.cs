namespace Timeline.Application.Erros;

/// <summary>Duplicidade, inclusive a detectada pelo banco (RN-01, RN-02). Vira erro 409.</summary>
public sealed class ConflitoException(string mensagem) : ErroDeAplicacaoException([mensagem]);

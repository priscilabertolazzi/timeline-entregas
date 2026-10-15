namespace Timeline.Application.Erros;

/// <summary>Recurso inexistente. Vira erro 404.</summary>
public sealed class NaoEncontradoException(string mensagem) : ErroDeAplicacaoException([mensagem]);

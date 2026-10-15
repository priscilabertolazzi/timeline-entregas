namespace Timeline.Application.Erros;

/// <summary>Entrada com formato inválido: campo obrigatório ausente ou tamanho excedido. Vira erro 400.</summary>
public sealed class EntradaInvalidaException(string mensagem) : ErroDeAplicacaoException([mensagem]);

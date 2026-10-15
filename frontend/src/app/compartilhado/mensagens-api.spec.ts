import { HttpErrorResponse } from '@angular/common/http';
import { MSG_ERRO_INESPERADO, mensagensDoErro } from './mensagens-api';

describe('mensagensDoErro', () => {
  it('CA-33: devolve as mensagens do Problem Details exatamente como vêm', () => {
    const erro = new HttpErrorResponse({
      status: 409,
      error: { title: 'Já existe uma aplicação com esse nome.', mensagens: ['Já existe uma aplicação com esse nome.'] },
    });

    expect(mensagensDoErro(erro)).toEqual(['Já existe uma aplicação com esse nome.']);
  });

  it('devolve todas as mensagens quando a API traz mais de uma (ADR-26)', () => {
    const erro = new HttpErrorResponse({
      status: 422,
      error: { mensagens: ['O início do andamento não pode ser posterior ao fim do andamento.', 'O fim do andamento não pode ser posterior ao início da homologação.'] },
    });

    expect(mensagensDoErro(erro)).toHaveLength(2);
  });

  it('sem mensagem na resposta (ex.: sem conexão), usa a MSG-41 do catálogo', () => {
    const semConexao = new HttpErrorResponse({ status: 0, error: new ProgressEvent('error') });
    const semCorpo = new HttpErrorResponse({ status: 404, error: null });

    expect(mensagensDoErro(semConexao)).toEqual([MSG_ERRO_INESPERADO]);
    expect(mensagensDoErro(semCorpo)).toEqual(['Ocorreu um erro inesperado. Tente novamente.']);
  });

  it('erro que não é da API também usa a MSG-41', () => {
    expect(mensagensDoErro(new Error('qualquer coisa'))).toEqual([MSG_ERRO_INESPERADO]);
  });
});

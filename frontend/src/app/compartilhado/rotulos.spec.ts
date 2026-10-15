import { FASES, rotuloFase, rotuloSituacao } from './rotulos';

describe('rótulos de exibição', () => {
  it('mostra as fases com os nomes do glossário', () => {
    expect(rotuloFase('Analise')).toBe('Análise');
    expect(rotuloFase('Homologacao')).toBe('Homologação');
    expect(rotuloFase('Producao')).toBe('Produção');
    expect(FASES.map((f) => f.rotulo)).toEqual(['Backlog', 'Análise', 'Desenvolvimento', 'Testes', 'Homologação', 'Produção']);
  });

  it('mostra as situações com os nomes do glossário', () => {
    expect(rotuloSituacao('NaoIniciado')).toBe('Não iniciado');
    expect(rotuloSituacao('OnTrack')).toBe('On track');
    expect(rotuloSituacao('Risco')).toBe('Risco');
    expect(rotuloSituacao('Entregue')).toBe('Entregue');
  });

  it('valor desconhecido aparece como veio', () => {
    expect(rotuloFase('Outra')).toBe('Outra');
    expect(rotuloSituacao('Outra')).toBe('Outra');
  });
});

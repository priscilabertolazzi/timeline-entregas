import { FASES, corDaFase, corDaSituacao, rotuloFase, rotuloSituacao } from './rotulos';

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

  it('cada fase tem a cor combinada com a autora', () => {
    expect(corDaFase('Backlog')).toBe('cinza');
    expect(['Analise', 'Desenvolvimento', 'Testes'].map(corDaFase)).toEqual(['azul', 'azul', 'azul']);
    expect(corDaFase('Homologacao')).toBe('roxo');
    expect(corDaFase('Producao')).toBe('verde-escuro');
  });

  it('cada situação tem a cor combinada com a autora', () => {
    expect(corDaSituacao('NaoIniciado')).toBe('cinza');
    expect(corDaSituacao('OnTrack')).toBe('verde-claro');
    expect(corDaSituacao('Entregue')).toBe('verde-escuro');
    expect(corDaSituacao('Risco')).toBe('risco');
  });

  it('valor desconhecido aparece como veio', () => {
    expect(rotuloFase('Outra')).toBe('Outra');
    expect(rotuloSituacao('Outra')).toBe('Outra');
  });
});

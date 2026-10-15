/** Fase como a API a envia e recebe, com o texto exibido nas telas (glossário). */
export interface OpcaoFase {
  valor: string;
  rotulo: string;
}

/** As seis fases, na ordem do ciclo (RN-05). */
export const FASES: readonly OpcaoFase[] = [
  { valor: 'Backlog', rotulo: 'Backlog' },
  { valor: 'Analise', rotulo: 'Análise' },
  { valor: 'Desenvolvimento', rotulo: 'Desenvolvimento' },
  { valor: 'Testes', rotulo: 'Testes' },
  { valor: 'Homologacao', rotulo: 'Homologação' },
  { valor: 'Producao', rotulo: 'Produção' },
];

const ROTULOS_SITUACAO: Record<string, string> = {
  NaoIniciado: 'Não iniciado',
  OnTrack: 'On track',
  Risco: 'Risco',
  Entregue: 'Entregue',
};

/** Texto exibido para uma fase da API (ex.: `Homologacao` aparece como "Homologação"). */
export function rotuloFase(valor: string): string {
  return FASES.find((fase) => fase.valor === valor)?.rotulo ?? valor;
}

/** Texto exibido para uma situação da API (ex.: `OnTrack` aparece como "On track"). */
export function rotuloSituacao(valor: string): string {
  return ROTULOS_SITUACAO[valor] ?? valor;
}

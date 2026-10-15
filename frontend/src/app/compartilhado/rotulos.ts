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

/** Cor da etiqueta, pelas cores do tema (`tema.scss`). */
export type CorEtiqueta = 'cinza' | 'azul' | 'roxo' | 'verde-claro' | 'verde-escuro' | 'risco';

/**
 * Cor da etiqueta de uma fase, só para exibição (combinada com a autora): Backlog cinza; Análise,
 * Desenvolvimento e Testes azul; Homologação roxo; Produção verde-escuro.
 */
export function corDaFase(valor: string): CorEtiqueta {
  switch (valor) {
    case 'Analise':
    case 'Desenvolvimento':
    case 'Testes':
      return 'azul';
    case 'Homologacao':
      return 'roxo';
    case 'Producao':
      return 'verde-escuro';
    default:
      return 'cinza';
  }
}

/**
 * Cor da etiqueta de uma situação, só para exibição (combinada com a autora): Não iniciado cinza;
 * On track verde-claro; Entregue verde-escuro; Risco em tom de alerta.
 */
export function corDaSituacao(valor: string): CorEtiqueta {
  switch (valor) {
    case 'OnTrack':
      return 'verde-claro';
    case 'Entregue':
      return 'verde-escuro';
    case 'Risco':
      return 'risco';
    default:
      return 'cinza';
  }
}

/** Texto exibido para uma fase da API (ex.: `Homologacao` aparece como "Homologação"). */
export function rotuloFase(valor: string): string {
  return FASES.find((fase) => fase.valor === valor)?.rotulo ?? valor;
}

/** Texto exibido para uma situação da API (ex.: `OnTrack` aparece como "On track"). */
export function rotuloSituacao(valor: string): string {
  return ROTULOS_SITUACAO[valor] ?? valor;
}

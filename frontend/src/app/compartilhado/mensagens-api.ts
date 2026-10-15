import { HttpErrorResponse } from '@angular/common/http';

/** MSG-41 do catálogo (arquitetura.md, seção 7.6): usada quando a resposta de erro não traz mensagem. */
export const MSG_ERRO_INESPERADO = 'Ocorreu um erro inesperado. Tente novamente.';

/**
 * Lê as mensagens de um erro da API, exatamente como vêm no campo `mensagens` do Problem Details
 * (ADR-35). Sem mensagem na resposta (por exemplo, sem conexão), devolve a MSG-41: as telas não
 * criam textos de erro próprios (frontend/CLAUDE.md).
 */
export function mensagensDoErro(erro: unknown): string[] {
  if (erro instanceof HttpErrorResponse) {
    const lista: unknown = erro.error?.mensagens;
    if (Array.isArray(lista) && lista.length > 0 && lista.every((m) => typeof m === 'string')) {
      return lista;
    }
  }
  return [MSG_ERRO_INESPERADO];
}

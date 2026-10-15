import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

/** Resumo de uma versão dentro da lista de aplicações (arquitetura.md, seção 7.2). */
export interface VersaoResumo {
  id: number;
  rotulo: string;
  fase: string;
  situacao: string;
}

/** Aplicação com o resumo das suas versões. */
export interface Aplicacao {
  id: number;
  nome: string;
  versoes: VersaoResumo[];
}

/** Acesso à API de aplicações, sempre pelo caminho relativo `/api` (ADR-21). */
@Injectable({ providedIn: 'root' })
export class AplicacaoService {
  private readonly http = inject(HttpClient);

  /** Lista as aplicações, cada uma com o resumo das suas versões. */
  listar(): Observable<Aplicacao[]> {
    return this.http.get<Aplicacao[]>('/api/aplicacoes');
  }
}

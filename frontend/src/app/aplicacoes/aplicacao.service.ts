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

/** Aplicação como devolvida ao cadastrar ou renomear. */
export interface AplicacaoCadastrada {
  id: number;
  nome: string;
}

/** Acesso à API de aplicações, sempre pelo caminho relativo `/api` (ADR-21). */
@Injectable({ providedIn: 'root' })
export class AplicacaoService {
  private readonly http = inject(HttpClient);

  /** Lista as aplicações, cada uma com o resumo das suas versões. */
  listar(): Observable<Aplicacao[]> {
    return this.http.get<Aplicacao[]>('/api/aplicacoes');
  }

  /** Cadastra uma aplicação só com o nome (RF-01, RN-01). */
  cadastrar(nome: string): Observable<AplicacaoCadastrada> {
    return this.http.post<AplicacaoCadastrada>('/api/aplicacoes', { nome });
  }

  /** Renomeia uma aplicação (RF-01, RN-01). */
  renomear(id: number, nome: string): Observable<AplicacaoCadastrada> {
    return this.http.put<AplicacaoCadastrada>(`/api/aplicacoes/${id}`, { nome });
  }

  /** Exclui a aplicação e todas as suas versões (RF-01, RN-04). A confirmação é pedida pela tela. */
  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`/api/aplicacoes/${id}`);
  }
}

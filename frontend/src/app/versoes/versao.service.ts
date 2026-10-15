import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

/** As sete datas da versão, em AAAA-MM-DD ou nulas (RN-09). */
export interface DatasVersao {
  inicioAndamento: string | null;
  fimAndamento: string | null;
  inicioHomologacao: string | null;
  fimHomologacao: string | null;
  inicioProrrogacao: string | null;
  fimProrrogacao: string | null;
  dataEntrega: string | null;
}

/** Versão como devolvida pela API, com todas as datas. */
export interface Versao {
  id: number;
  aplicacaoId: number;
  rotulo: string;
  /** Texto sem acento, como `Homologacao`; o texto de exibição vem de `rotuloFase`. */
  fase: string;
  /** `NaoIniciado`, `OnTrack`, `Risco` ou `Entregue` (RN-21). */
  situacao: string;
  observacao: string | null;
  datas: DatasVersao;
}

/** Corpo do salvamento único da tela da versão. Data vazia é data removida (ADR-39). */
export interface VersaoParaGravar {
  rotulo: string;
  /** `OnTrack` ou `Risco`; vazia mantém a situação atual. */
  situacao: string;
  observacao: string;
  fase: string;
  datas: Record<keyof DatasVersao, string>;
}

/** Versão como devolvida ao cadastrar. */
export type VersaoCadastrada = Versao;

/** Acesso à API de versões, sempre pelo caminho relativo `/api` (ADR-21). */
@Injectable({ providedIn: 'root' })
export class VersaoService {
  private readonly http = inject(HttpClient);

  /** Detalha uma versão, com todas as datas (RF-03). */
  detalhar(id: number): Observable<Versao> {
    return this.http.get<Versao>(`/api/versoes/${id}`);
  }

  /** Cadastra uma versão só com o rótulo; ela nasce em Backlog, sem datas (RF-03, RN-02). */
  cadastrar(aplicacaoId: number, rotulo: string): Observable<VersaoCadastrada> {
    return this.http.post<VersaoCadastrada>(`/api/aplicacoes/${aplicacaoId}/versoes`, { rotulo });
  }

  /**
   * Grava de uma vez rótulo, situação, observação, fase e as sete datas, tudo ou nada
   * (RF-03 a RF-05, RF-09, RF-10, RF-14, ADR-39).
   */
  gravar(id: number, versao: VersaoParaGravar): Observable<Versao> {
    return this.http.put<Versao>(`/api/versoes/${id}/completo`, versao);
  }

  /** Exclui a versão (RF-03, RN-04). A confirmação é pedida pela tela. */
  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`/api/versoes/${id}`);
  }
}

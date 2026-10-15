import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AplicacaoService } from './aplicacao.service';

describe('AplicacaoService (cadastro, edição e exclusão)', () => {
  let servico: AplicacaoService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    servico = TestBed.inject(AplicacaoService);
    http = TestBed.inject(HttpTestingController);
  });

  it('RF-01: cadastra com POST em /api/aplicacoes, enviando só o nome', () => {
    servico.cadastrar('Alfa').subscribe();

    const requisicao = http.expectOne('/api/aplicacoes');
    expect(requisicao.request.method).toBe('POST');
    expect(requisicao.request.body).toEqual({ nome: 'Alfa' });
    requisicao.flush({ id: 1, nome: 'Alfa' });
  });

  it('RF-01: renomeia com PUT em /api/aplicacoes/{id}', () => {
    servico.renomear(3, 'Gama').subscribe();

    const requisicao = http.expectOne('/api/aplicacoes/3');
    expect(requisicao.request.method).toBe('PUT');
    expect(requisicao.request.body).toEqual({ nome: 'Gama' });
    requisicao.flush({ id: 3, nome: 'Gama' });
  });

  it('RN-04: exclui com DELETE em /api/aplicacoes/{id}', () => {
    servico.excluir(2).subscribe();

    const requisicao = http.expectOne('/api/aplicacoes/2');
    expect(requisicao.request.method).toBe('DELETE');
    requisicao.flush(null, { status: 204, statusText: 'No Content' });
  });
});

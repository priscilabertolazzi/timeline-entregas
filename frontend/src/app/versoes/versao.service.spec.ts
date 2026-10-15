import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { VersaoParaGravar, VersaoService } from './versao.service';

describe('VersaoService', () => {
  let servico: VersaoService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    servico = TestBed.inject(VersaoService);
    http = TestBed.inject(HttpTestingController);
  });

  it('RF-03: detalha com GET em /api/versoes/{id}', () => {
    servico.detalhar(5).subscribe();

    const requisicao = http.expectOne('/api/versoes/5');
    expect(requisicao.request.method).toBe('GET');
    requisicao.flush({});
  });

  it('RF-03: cadastra com POST em /api/aplicacoes/{id}/versoes, enviando só o rótulo', () => {
    servico.cadastrar(1, 'v3').subscribe();

    const requisicao = http.expectOne('/api/aplicacoes/1/versoes');
    expect(requisicao.request.method).toBe('POST');
    expect(requisicao.request.body).toEqual({ rotulo: 'v3' });
    requisicao.flush({});
  });

  it('ADR-39: grava tudo numa só chamada PUT em /api/versoes/{id}/completo', () => {
    const corpo: VersaoParaGravar = {
      rotulo: 'v1',
      situacao: 'Risco',
      observacao: 'Acesso sendo liberado em lotes.',
      fase: 'Homologacao',
      datas: {
        inicioAndamento: '2026-09-01',
        fimAndamento: '2026-09-25',
        inicioHomologacao: '2026-10-01',
        fimHomologacao: '2026-10-13',
        inicioProrrogacao: '2026-10-13',
        fimProrrogacao: '2026-10-25',
        dataEntrega: '2026-10-28',
      },
    };

    servico.gravar(5, corpo).subscribe();

    const requisicao = http.expectOne('/api/versoes/5/completo');
    expect(requisicao.request.method).toBe('PUT');
    expect(requisicao.request.body).toEqual(corpo);
    requisicao.flush({});
    http.verify();
  });

  it('RN-04: exclui com DELETE em /api/versoes/{id}', () => {
    servico.excluir(7).subscribe();

    const requisicao = http.expectOne('/api/versoes/7');
    expect(requisicao.request.method).toBe('DELETE');
    requisicao.flush(null, { status: 204, statusText: 'No Content' });
  });
});

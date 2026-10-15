import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AplicacaoService } from './aplicacao.service';

describe('AplicacaoService', () => {
  it('lista as aplicações pelo caminho relativo /api/aplicacoes', () => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    const servico = TestBed.inject(AplicacaoService);
    const http = TestBed.inject(HttpTestingController);

    let recebidas: unknown;
    servico.listar().subscribe((r) => (recebidas = r));
    const requisicao = http.expectOne('/api/aplicacoes');
    expect(requisicao.request.method).toBe('GET');
    requisicao.flush([{ id: 1, nome: 'Alfa', versoes: [] }]);

    expect(recebidas).toEqual([{ id: 1, nome: 'Alfa', versoes: [] }]);
  });
});

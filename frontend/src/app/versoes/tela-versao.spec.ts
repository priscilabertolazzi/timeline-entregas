import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, TestRequest, provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { TelaVersao } from './tela-versao';
import { Versao } from './versao.service';

const SEM_DATAS = {
  inicioAndamento: null,
  fimAndamento: null,
  inicioHomologacao: null,
  fimHomologacao: null,
  inicioProrrogacao: null,
  fimProrrogacao: null,
  dataEntrega: null,
};

/** v1 da Alfa em Homologação, como no CA-32. */
const V1_EM_HOMOLOGACAO: Versao = {
  id: 3,
  aplicacaoId: 1,
  rotulo: 'v1',
  fase: 'Homologacao',
  situacao: 'OnTrack',
  observacao: 'Homologação em andamento.',
  datas: { ...SEM_DATAS, inicioAndamento: '2026-09-01', fimAndamento: '2026-09-25', inicioHomologacao: '2026-10-01', fimHomologacao: '2026-10-13', dataEntrega: '2026-10-20' },
};

/** v3 da Alfa em Backlog, sem datas, como no CA-25. */
const V3_EM_BACKLOG: Versao = { id: 5, aplicacaoId: 1, rotulo: 'v3', fase: 'Backlog', situacao: 'NaoIniciado', observacao: null, datas: SEM_DATAS };

describe('TelaVersao', () => {
  let fixture: ComponentFixture<TelaVersao>;
  let http: HttpTestingController;

  const tela = () => fixture.nativeElement as HTMLElement;
  const clicarSalvar = async () => {
    (Array.from(tela().querySelectorAll('button')).find((b) => b.textContent?.trim() === 'Salvar') as HTMLButtonElement).click();
    await fixture.whenStable();
  };

  async function abrir(versao: Versao): Promise<void> {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([]),
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap({ id: String(versao.id) }) } } },
      ],
    });
    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(TelaVersao);
    http.expectOne(`/api/versoes/${versao.id}`).flush(versao);
    http.expectOne('/api/aplicacoes').flush([{ id: 1, nome: 'Alfa', versoes: [] }]);
    await fixture.whenStable();
  }

  function gravacao(id: number): TestRequest {
    return http.expectOne({ method: 'PUT', url: `/api/versoes/${id}/completo` });
  }

  it('mostra a aplicação, a versão e os campos com os nomes do glossário', async () => {
    await abrir(V1_EM_HOMOLOGACAO);
    const texto = tela().textContent ?? '';

    expect(texto).toContain('Alfa · v1');
    for (const nome of ['Início do andamento', 'Fim do andamento', 'Início da homologação', 'Fim da homologação', 'Início da prorrogação', 'Fim da prorrogação', 'Data de entrega', 'Observação', 'Fase', 'Situação']) {
      expect(texto).toContain(nome);
    }
    expect(fixture.componentInstance.formulario.controls.dataEntrega.value?.getDate()).toBe(20);
  });

  it('há um só botão Salvar na tela', async () => {
    await abrir(V1_EM_HOMOLOGACAO);

    const salvar = Array.from(tela().querySelectorAll('button')).filter((b) => b.textContent?.trim().startsWith('Salvar'));
    expect(salvar).toHaveLength(1);
  });

  it('CA-32: salva tudo numa só chamada, com as sete datas, e mostra o aviso de salvo', async () => {
    await abrir(V1_EM_HOMOLOGACAO);
    const f = fixture.componentInstance.formulario.controls;
    f.inicioProrrogacao.setValue(new Date(2026, 9, 13));
    f.fimProrrogacao.setValue(new Date(2026, 9, 25));
    f.dataEntrega.setValue(new Date(2026, 9, 28));
    f.situacao.setValue('Risco');

    await clicarSalvar();

    const requisicao = gravacao(3);
    expect(requisicao.request.body).toEqual({
      rotulo: 'v1',
      situacao: 'Risco',
      observacao: 'Homologação em andamento.',
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
    });
    requisicao.flush({ ...V1_EM_HOMOLOGACAO, situacao: 'Risco' });
    await fixture.whenStable();
    expect(document.body.textContent).toContain('Alterações salvas.');
    http.verify();
  });

  it('CA-32: prorrogação até 25/10 sem mudar a entrega mostra a MSG-21 como veio da API e não mostra o aviso de salvo', async () => {
    await abrir(V1_EM_HOMOLOGACAO);
    const f = fixture.componentInstance.formulario.controls;
    f.inicioProrrogacao.setValue(new Date(2026, 9, 13));
    f.fimProrrogacao.setValue(new Date(2026, 9, 25));

    await clicarSalvar();
    gravacao(3).flush(
      { title: 'O fim da prorrogação não pode ser posterior à data de entrega.', mensagens: ['O fim da prorrogação não pode ser posterior à data de entrega.'] },
      { status: 422, statusText: 'Unprocessable Entity' },
    );
    await fixture.whenStable();

    expect(tela().querySelector('[role="alert"]')?.textContent).toContain('O fim da prorrogação não pode ser posterior à data de entrega.');
    expect(document.body.textContent).not.toContain('Alterações salvas.');
  });

  it('CA-25: só a data de entrega em Backlog envia as outras datas vazias e mostra a MSG-13', async () => {
    await abrir(V3_EM_BACKLOG);
    fixture.componentInstance.formulario.controls.dataEntrega.setValue(new Date(2026, 10, 30));

    await clicarSalvar();
    const requisicao = gravacao(5);
    expect(requisicao.request.body.datas).toEqual({ ...{ inicioAndamento: '', fimAndamento: '', inicioHomologacao: '', fimHomologacao: '', inicioProrrogacao: '', fimProrrogacao: '' }, dataEntrega: '2026-11-30' });
    requisicao.flush(
      { title: 'x', mensagens: ['As datas de andamento, de homologação e de entrega devem ser informadas em conjunto.'] },
      { status: 422, statusText: 'Unprocessable Entity' },
    );
    await fixture.whenStable();

    expect(tela().querySelector('[role="alert"]')?.textContent).toContain('As datas de andamento, de homologação e de entrega devem ser informadas em conjunto.');
  });

  it('RN-09: limpar uma data a envia vazia, como data removida', async () => {
    await abrir(V1_EM_HOMOLOGACAO);
    const botao = tela().querySelector('[aria-label="Limpar Data de entrega"]') as HTMLButtonElement;
    botao.click();

    await clicarSalvar();

    const requisicao = gravacao(3);
    expect(requisicao.request.body.datas.dataEntrega).toBe('');
    requisicao.flush(V1_EM_HOMOLOGACAO);
  });

  it('RN-21: em Backlog a situação é automática e não é enviada', async () => {
    await abrir(V3_EM_BACKLOG);

    expect(fixture.componentInstance.formulario.controls.situacao.disabled).toBe(true);
    expect(tela().textContent).not.toContain('Risco');
    await clicarSalvar();
    const requisicao = gravacao(5);
    expect(requisicao.request.body.situacao).toBe('');
    requisicao.flush(V3_EM_BACKLOG);
  });

  it('RN-21: ao escolher uma fase em andamento, a situação passa a poder ser escolhida', async () => {
    await abrir(V3_EM_BACKLOG);

    fixture.componentInstance.formulario.controls.fase.setValue('Desenvolvimento');
    await fixture.whenStable();

    expect(fixture.componentInstance.formulario.controls.situacao.enabled).toBe(true);
  });

  it('RN-04: excluir a versão pede confirmação e só chama a API depois do "Excluir"', async () => {
    await abrir(V1_EM_HOMOLOGACAO);

    (Array.from(tela().querySelectorAll('button')).find((b) => b.textContent?.trim() === 'Excluir versão') as HTMLButtonElement).click();
    await fixture.whenStable();
    expect(document.querySelector('mat-dialog-container')?.textContent).toContain('Excluir a versão v1 da aplicação Alfa?');
    http.expectNone('/api/versoes/3');

    (Array.from(document.querySelectorAll('mat-dialog-container button')).find((b) => b.textContent?.trim() === 'Excluir') as HTMLButtonElement).click();
    // A janelinha só avisa que fechou depois da animação de saída; espera a chamada aparecer.
    const exclusao = await vi.waitFor(() => http.expectOne({ method: 'DELETE', url: '/api/versoes/3' }));
    exclusao.flush(null, { status: 204, statusText: 'No Content' });
  });

  it('versão inexistente mostra a MSG-08 como veio da API', async () => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([]),
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap({ id: '99' }) } } },
      ],
    });
    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(TelaVersao);

    http.expectOne('/api/versoes/99').flush({ title: 'Versão não encontrada.', mensagens: ['Versão não encontrada.'] }, { status: 404, statusText: 'Not Found' });
    await fixture.whenStable();

    expect(tela().querySelector('[role="alert"]')?.textContent).toContain('Versão não encontrada.');
  });
});

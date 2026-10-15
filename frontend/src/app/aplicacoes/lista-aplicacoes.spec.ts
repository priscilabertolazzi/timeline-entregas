import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ListaAplicacoes } from './lista-aplicacoes';

const APLICACOES = [
  {
    id: 1,
    nome: 'Alfa',
    versoes: [
      { id: 10, rotulo: 'v1', fase: 'Homologacao', situacao: 'OnTrack' },
      { id: 11, rotulo: 'v2', fase: 'Backlog', situacao: 'NaoIniciado' },
    ],
  },
  { id: 4, nome: 'Delta', versoes: [] },
];

describe('ListaAplicacoes', () => {
  let fixture: ComponentFixture<ListaAplicacoes>;
  let http: HttpTestingController;

  const tela = () => fixture.nativeElement as HTMLElement;
  const janelinha = () => document.querySelector('mat-dialog-container') as HTMLElement | null;
  const botaoDaJanelinha = (texto: string) =>
    Array.from(document.querySelectorAll('mat-dialog-container button')).find((b) => b.textContent?.trim() === texto) as HTMLButtonElement;

  beforeEach(async () => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])] });
    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(ListaAplicacoes);
    http.expectOne('/api/aplicacoes').flush(APLICACOES);
    await fixture.whenStable();
  });

  it('mostra cada aplicação com suas versões, fase e situação com os nomes do glossário', () => {
    const texto = tela().textContent ?? '';

    expect(texto).toContain('Alfa');
    expect(texto).toContain('Homologação');
    expect(texto).toContain('On track');
    expect(texto).toContain('Não iniciado');
    expect(texto).toContain('Nenhuma versão cadastrada');
  });

  it('RN-04: ao clicar em Excluir, pede confirmação e não chama a API antes do "Excluir" da janelinha', async () => {
    (tela().querySelector('[aria-label="Excluir Alfa"]') as HTMLButtonElement).click();
    await fixture.whenStable();

    expect(janelinha()?.textContent).toContain('Excluir a aplicação Alfa?');
    http.expectNone('/api/aplicacoes/1');
  });

  it('RN-04: cancelar a confirmação não exclui nada', async () => {
    (tela().querySelector('[aria-label="Excluir Alfa"]') as HTMLButtonElement).click();
    await fixture.whenStable();

    botaoDaJanelinha('Cancelar').click();
    await fixture.whenStable();

    http.expectNone('/api/aplicacoes/1');
  });

  it('RN-04: confirmar exclui pela API e recarrega a lista', async () => {
    (tela().querySelector('[aria-label="Excluir Alfa"]') as HTMLButtonElement).click();
    await fixture.whenStable();

    botaoDaJanelinha('Excluir').click();
    // A janelinha só avisa que fechou depois da animação de saída; espera a chamada aparecer.
    const exclusao = await vi.waitFor(() => http.expectOne({ method: 'DELETE', url: '/api/aplicacoes/1' }));
    exclusao.flush(null, { status: 204, statusText: 'No Content' });
    await fixture.whenStable();

    http.expectOne({ method: 'GET', url: '/api/aplicacoes' }).flush([APLICACOES[1]]);
    await fixture.whenStable();
    expect(tela().textContent).not.toContain('Alfa');
  });

  it('CA-33: nome duplicado mostra a mensagem da API na janelinha, que continua aberta', async () => {
    (Array.from(tela().querySelectorAll('button')).find((b) => b.textContent?.trim() === 'Nova aplicação') as HTMLButtonElement).click();
    await fixture.whenStable();
    const campo = janelinha()?.querySelector('input') as HTMLInputElement;
    campo.value = 'alfa';
    campo.dispatchEvent(new Event('input'));
    await fixture.whenStable();

    botaoDaJanelinha('Salvar').click();
    await fixture.whenStable();
    const requisicao = http.expectOne({ method: 'POST', url: '/api/aplicacoes' });
    expect(requisicao.request.body).toEqual({ nome: 'alfa' });
    requisicao.flush(
      { title: 'Já existe uma aplicação com esse nome.', mensagens: ['Já existe uma aplicação com esse nome.'] },
      { status: 409, statusText: 'Conflict' },
    );
    await fixture.whenStable();

    expect(janelinha()?.textContent).toContain('Já existe uma aplicação com esse nome.');
  });

  it('RF-01: editar mostra o nome atual e renomeia pela API', async () => {
    (tela().querySelector('[aria-label="Editar Delta"]') as HTMLButtonElement).click();
    await fixture.whenStable();
    const campo = janelinha()?.querySelector('input') as HTMLInputElement;
    expect(campo.value).toBe('Delta');
    campo.value = 'Gama';
    campo.dispatchEvent(new Event('input'));
    await fixture.whenStable();

    botaoDaJanelinha('Salvar').click();
    await fixture.whenStable();

    const requisicao = http.expectOne({ method: 'PUT', url: '/api/aplicacoes/4' });
    expect(requisicao.request.body).toEqual({ nome: 'Gama' });
    requisicao.flush({ id: 4, nome: 'Gama' });
  });
});

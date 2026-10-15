import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { App } from './app';
import { VERSAO_SISTEMA } from './versao-sistema';

describe('versão do sistema', () => {
  it('aparece na barra de título, no formato vX.Y.Z', async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();

    expect(VERSAO_SISTEMA).toMatch(/^v\d+\.\d+\.\d+$/);
    expect(fixture.nativeElement.querySelector('mat-toolbar')?.textContent).toContain(VERSAO_SISTEMA);
  });
});

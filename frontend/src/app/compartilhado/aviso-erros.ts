import { Component, input } from '@angular/core';

/** Caixa vermelha com as mensagens da API, exatamente como vieram (frontend/CLAUDE.md). */
@Component({
  selector: 'app-aviso-erros',
  template: `
    @if (mensagens().length > 0) {
      <div class="aviso-erros" role="alert">
        <ul>
          @for (mensagem of mensagens(); track $index) {
            <li>{{ mensagem }}</li>
          }
        </ul>
      </div>
    }
  `,
  styles: `
    .aviso-erros {
      background: var(--mat-sys-error-container);
      color: var(--mat-sys-on-error-container);
      border-radius: 8px;
      padding: 8px 16px;
      margin: 8px 0;
    }
    ul {
      margin: 0;
      padding-left: 16px;
    }
  `,
})
export class AvisoErros {
  /** Mensagens a mostrar; lista vazia não mostra nada. */
  readonly mensagens = input.required<string[]>();
}

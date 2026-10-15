import { Component, input } from '@angular/core';
import { CorEtiqueta } from './rotulos';

/** Etiqueta colorida de fase ou situação. As cores vêm do tema (`tema.scss`), num único lugar. */
@Component({
  selector: 'app-etiqueta',
  template: `<span class="etiqueta" [class]="cor()">{{ texto() }}</span>`,
  styles: `
    .etiqueta {
      display: inline-block;
      padding: 2px 12px;
      border-radius: 12px;
      font: var(--mat-sys-label-large);
      color: #1f1f1f;
    }
    .cinza {
      background: var(--cor-nao-iniciado-claro);
    }
    .azul {
      background: var(--cor-em-andamento-claro);
    }
    .roxo {
      background: var(--cor-em-homologacao-claro);
    }
    .verde-claro {
      background: var(--cor-em-producao-claro);
    }
    .verde-escuro {
      background: var(--cor-em-producao-forte);
      color: #ffffff;
    }
    .risco {
      background: var(--cor-risco);
    }
  `,
})
export class Etiqueta {
  /** Texto exibido. */
  readonly texto = input.required<string>();
  /** Cor da etiqueta. */
  readonly cor = input.required<CorEtiqueta>();
}

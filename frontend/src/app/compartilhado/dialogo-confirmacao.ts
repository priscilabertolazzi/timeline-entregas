import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

/** Texto da janelinha de confirmação (RN-04: confirmação antes de excluir). */
export interface DadosDialogoConfirmacao {
  titulo: string;
  texto: string;
  /** Texto do botão que confirma (ex.: "Excluir"). */
  botao: string;
}

/** Janelinha de confirmação. Fecha com `true` ao confirmar e com `false` ao cancelar. */
@Component({
  selector: 'app-dialogo-confirmacao',
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ dados.titulo }}</h2>
    <mat-dialog-content>{{ dados.texto }}</mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button [mat-dialog-close]="false">Cancelar</button>
      <button mat-flat-button [mat-dialog-close]="true">{{ dados.botao }}</button>
    </mat-dialog-actions>
  `,
})
export class DialogoConfirmacao {
  protected readonly dados = inject<DadosDialogoConfirmacao>(MAT_DIALOG_DATA);
}

import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Observable } from 'rxjs';
import { AvisoErros } from './aviso-erros';
import { mensagensDoErro } from './mensagens-api';

/** O que a janelinha precisa saber para pedir e salvar um texto curto (nome ou rótulo). */
export interface DadosDialogoNome {
  titulo: string;
  /** Nome do campo, como no glossário (ex.: "Nome da aplicação"). */
  rotulo: string;
  valor: string;
  /** Chama a API; o resultado é devolvido a quem abriu a janelinha. */
  salvar: (valor: string) => Observable<unknown>;
}

/**
 * Janelinha com um campo de texto, usada para cadastrar e editar aplicações e para cadastrar
 * versões. Se a API recusar, as mensagens aparecem aqui e a janelinha continua aberta.
 */
@Component({
  selector: 'app-dialogo-nome',
  imports: [MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, FormsModule, AvisoErros],
  template: `
    <h2 mat-dialog-title>{{ dados.titulo }}</h2>
    <form (ngSubmit)="salvar()">
      <mat-dialog-content>
        <mat-form-field class="campo">
          <mat-label>{{ dados.rotulo }}</mat-label>
          <input matInput name="valor" autocomplete="off" [ngModel]="valor()" (ngModelChange)="valor.set($event)" />
        </mat-form-field>
        <app-aviso-erros [mensagens]="mensagens()" />
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" mat-dialog-close>Cancelar</button>
        <button mat-flat-button type="submit" [disabled]="salvando()">Salvar</button>
      </mat-dialog-actions>
    </form>
  `,
  styles: `
    .campo {
      width: 100%;
      min-width: 320px;
    }
  `,
})
export class DialogoNome {
  protected readonly dados = inject<DadosDialogoNome>(MAT_DIALOG_DATA);
  private readonly janela = inject<MatDialogRef<DialogoNome, unknown>>(MatDialogRef);

  protected readonly valor = signal(this.dados.valor);
  protected readonly mensagens = signal<string[]>([]);
  protected readonly salvando = signal(false);

  /** Salva pela API e fecha a janelinha; em caso de erro, mostra as mensagens e mantém a janelinha. */
  protected salvar(): void {
    this.salvando.set(true);
    this.mensagens.set([]);
    this.dados.salvar(this.valor()).subscribe({
      next: (resposta) => this.janela.close(resposta ?? true),
      error: (erro: unknown) => {
        this.mensagens.set(mensagensDoErro(erro));
        this.salvando.set(false);
      },
    });
  }
}

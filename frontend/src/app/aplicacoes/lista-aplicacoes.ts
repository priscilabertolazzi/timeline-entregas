import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatListModule } from '@angular/material/list';
import { AplicacaoService } from './aplicacao.service';

/** Lista de aplicações. Nesta tarefa mostra só os nomes; cadastro, edição e exclusão vêm na T-10. */
@Component({
  selector: 'app-lista-aplicacoes',
  imports: [MatListModule],
  templateUrl: './lista-aplicacoes.html',
})
export class ListaAplicacoes {
  protected readonly aplicacoes = toSignal(inject(AplicacaoService).listar());
}

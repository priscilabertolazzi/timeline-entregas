import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';
import { VERSAO_SISTEMA } from './versao-sistema';

/** Estrutura da página: barra de título, com a versão do sistema, e a tela atual. */
@Component({
  imports: [RouterOutlet, MatToolbarModule],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  protected readonly versaoSistema = VERSAO_SISTEMA;
}

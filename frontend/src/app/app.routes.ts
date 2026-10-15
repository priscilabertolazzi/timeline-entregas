import { Routes } from '@angular/router';
import { ListaAplicacoes } from './aplicacoes/lista-aplicacoes';
import { TelaVersao } from './versoes/tela-versao';

export const routes: Routes = [
  { path: '', component: ListaAplicacoes },
  { path: 'versoes/:id', component: TelaVersao },
  { path: '**', redirectTo: '' },
];

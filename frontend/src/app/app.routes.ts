import { Routes } from '@angular/router';
import { ListaAplicacoes } from './aplicacoes/lista-aplicacoes';

export const routes: Routes = [
  { path: '', component: ListaAplicacoes },
  { path: '**', redirectTo: '' },
];

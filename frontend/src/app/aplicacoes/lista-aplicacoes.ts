import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Router, RouterLink } from '@angular/router';
import { AvisoErros } from '../compartilhado/aviso-erros';
import { DadosDialogoConfirmacao, DialogoConfirmacao } from '../compartilhado/dialogo-confirmacao';
import { DadosDialogoNome, DialogoNome } from '../compartilhado/dialogo-nome';
import { mensagensDoErro } from '../compartilhado/mensagens-api';
import { entregaParaExibir } from '../compartilhado/datas';
import { Etiqueta } from '../compartilhado/etiqueta';
import { corDaFase, corDaSituacao, rotuloFase, rotuloSituacao } from '../compartilhado/rotulos';
import { VersaoCadastrada, VersaoService } from '../versoes/versao.service';
import { Aplicacao, AplicacaoService } from './aplicacao.service';

/**
 * Lista de aplicações: cadastro, edição e exclusão de aplicações, com confirmação antes de excluir
 * (RF-01, RN-04), e cadastro de versões, que leva à tela da versão (RF-03).
 */
@Component({
  selector: 'app-lista-aplicacoes',
  imports: [MatButtonModule, MatCardModule, MatProgressBarModule, RouterLink, AvisoErros, Etiqueta],
  templateUrl: './lista-aplicacoes.html',
  styleUrl: './lista-aplicacoes.scss',
})
export class ListaAplicacoes {
  private readonly aplicacaoService = inject(AplicacaoService);
  private readonly versaoService = inject(VersaoService);
  private readonly dialog = inject(MatDialog);
  private readonly router = inject(Router);

  protected readonly aplicacoes = signal<Aplicacao[] | undefined>(undefined);
  protected readonly erros = signal<string[]>([]);
  protected readonly entregaParaExibir = entregaParaExibir;
  protected readonly corDaFase = corDaFase;
  protected readonly corDaSituacao = corDaSituacao;
  protected readonly rotuloFase = rotuloFase;
  protected readonly rotuloSituacao = rotuloSituacao;

  constructor() {
    this.carregar();
  }

  /** Abre a janelinha de nova aplicação (RF-01). */
  protected novaAplicacao(): void {
    this.pedirNome({
      titulo: 'Nova aplicação',
      rotulo: 'Nome da aplicação',
      valor: '',
      salvar: (nome) => this.aplicacaoService.cadastrar(nome),
    });
  }

  /** Abre a janelinha de edição do nome da aplicação (RF-01). */
  protected editar(aplicacao: Aplicacao): void {
    this.pedirNome({
      titulo: 'Editar aplicação',
      rotulo: 'Nome da aplicação',
      valor: aplicacao.nome,
      salvar: (nome) => this.aplicacaoService.renomear(aplicacao.id, nome),
    });
  }

  /** Pede confirmação e, só depois do "Excluir", exclui a aplicação e suas versões (RN-04). */
  protected excluir(aplicacao: Aplicacao): void {
    const dados: DadosDialogoConfirmacao = {
      titulo: 'Excluir aplicação',
      texto: `Excluir a aplicação ${aplicacao.nome}? Todas as suas versões também serão excluídas e isso não pode ser desfeito.`,
      botao: 'Excluir',
    };
    this.dialog
      .open(DialogoConfirmacao, { data: dados })
      .afterClosed()
      .subscribe((confirmou) => {
        if (confirmou) {
          this.aplicacaoService.excluir(aplicacao.id).subscribe({
            next: () => this.carregar(),
            error: (erro: unknown) => this.erros.set(mensagensDoErro(erro)),
          });
        }
      });
  }

  /** Abre a janelinha de nova versão (só o rótulo, RN-02) e, ao salvar, abre a tela da versão. */
  protected novaVersao(aplicacao: Aplicacao): void {
    this.dialog
      .open(DialogoNome, {
        data: {
          titulo: `Nova versão de ${aplicacao.nome}`,
          rotulo: 'Rótulo da versão',
          valor: '',
          salvar: (rotulo: string) => this.versaoService.cadastrar(aplicacao.id, rotulo),
        } satisfies DadosDialogoNome,
      })
      .afterClosed()
      .subscribe((versao: VersaoCadastrada | undefined) => {
        if (versao) {
          void this.router.navigate(['/versoes', versao.id]);
        }
      });
  }

  private pedirNome(dados: DadosDialogoNome): void {
    this.dialog
      .open(DialogoNome, { data: dados })
      .afterClosed()
      .subscribe((salvou) => {
        if (salvou) {
          this.carregar();
        }
      });
  }

  private carregar(): void {
    this.erros.set([]);
    this.aplicacaoService.listar().subscribe({
      next: (lista) => this.aplicacoes.set(lista),
      error: (erro: unknown) => this.erros.set(mensagensDoErro(erro)),
    });
  }
}

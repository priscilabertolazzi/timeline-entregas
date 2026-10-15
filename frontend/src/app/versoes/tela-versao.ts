import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AplicacaoService } from '../aplicacoes/aplicacao.service';
import { AvisoErros } from '../compartilhado/aviso-erros';
import { DadosDialogoConfirmacao, DialogoConfirmacao } from '../compartilhado/dialogo-confirmacao';
import { dataParaTexto, textoParaData } from '../compartilhado/datas';
import { mensagensDoErro } from '../compartilhado/mensagens-api';
import { Etiqueta } from '../compartilhado/etiqueta';
import { FASES, corDaFase, corDaSituacao, rotuloFase, rotuloSituacao } from '../compartilhado/rotulos';
import { Versao, VersaoParaGravar, VersaoService } from './versao.service';

/** Aviso mostrado depois de salvar. */
const AVISO_SALVO = 'Alterações salvas.';

/** Nome dos sete campos de data do formulário (RN-09). */
type CampoData =
  | 'inicioAndamento'
  | 'fimAndamento'
  | 'inicioHomologacao'
  | 'fimHomologacao'
  | 'inicioProrrogacao'
  | 'fimProrrogacao'
  | 'dataEntrega';

/** As datas em grupos, na ordem cronológica da RN-10, com os nomes do glossário. */
const GRUPOS_DE_DATAS: readonly { titulo: string; campos: readonly { nome: CampoData; rotulo: string }[] }[] = [
  {
    titulo: 'Andamento',
    campos: [
      { nome: 'inicioAndamento', rotulo: 'Início do andamento' },
      { nome: 'fimAndamento', rotulo: 'Fim do andamento' },
    ],
  },
  {
    titulo: 'Homologação',
    campos: [
      { nome: 'inicioHomologacao', rotulo: 'Início da homologação' },
      { nome: 'fimHomologacao', rotulo: 'Fim da homologação' },
    ],
  },
  {
    titulo: 'Prorrogação da homologação (opcional)',
    campos: [
      { nome: 'inicioProrrogacao', rotulo: 'Início da prorrogação' },
      { nome: 'fimProrrogacao', rotulo: 'Fim da prorrogação' },
    ],
  },
  { titulo: 'Entrega', campos: [{ nome: 'dataEntrega', rotulo: 'Data de entrega' }] },
];

/**
 * Tela da versão: rótulo, fase, situação, observação e as sete datas num único formulário, com um só
 * botão Salvar que grava tudo ou nada (RF-03 a RF-05, RF-09, RF-10, RF-14, ADR-39). As regras são
 * da API; a tela mostra as mensagens exatamente como vêm.
 */
@Component({
  selector: 'app-tela-versao',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatSelectModule,
    RouterLink,
    AvisoErros,
    Etiqueta,
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './tela-versao.html',
  styleUrl: './tela-versao.scss',
})
export class TelaVersao {
  private readonly versaoService = inject(VersaoService);
  private readonly aplicacaoService = inject(AplicacaoService);
  private readonly dialog = inject(MatDialog);
  private readonly aviso = inject(MatSnackBar);
  private readonly router = inject(Router);
  private readonly id = Number(inject(ActivatedRoute).snapshot.paramMap.get('id'));

  protected readonly gruposDeDatas = GRUPOS_DE_DATAS;
  protected readonly corDaFase = corDaFase;
  protected readonly corDaSituacao = corDaSituacao;
  protected readonly rotuloFase = rotuloFase;
  protected readonly rotuloSituacao = rotuloSituacao;
  protected readonly versao = signal<Versao | undefined>(undefined);
  protected readonly nomeAplicacao = signal('');
  protected readonly erros = signal<string[]>([]);
  protected readonly salvando = signal(false);
  /** Texto da situação quando ela é automática (Backlog e Produção), para o campo só de leitura. */
  protected readonly situacaoAutomatica = signal('');

  /** Campos do formulário; as datas são `Date` do calendário, ou nulas quando vazias. */
  readonly formulario = new FormGroup({
    rotulo: new FormControl('', { nonNullable: true }),
    fase: new FormControl('Backlog', { nonNullable: true }),
    situacao: new FormControl('OnTrack', { nonNullable: true }),
    observacao: new FormControl('', { nonNullable: true }),
    inicioAndamento: new FormControl<Date | null>(null),
    fimAndamento: new FormControl<Date | null>(null),
    inicioHomologacao: new FormControl<Date | null>(null),
    fimHomologacao: new FormControl<Date | null>(null),
    inicioProrrogacao: new FormControl<Date | null>(null),
    fimProrrogacao: new FormControl<Date | null>(null),
    dataEntrega: new FormControl<Date | null>(null),
  });

  /**
   * Fases oferecidas no campo Fase. Só conveniência da tela (a API continua validando, MSG-27 e MSG-30):
   * em Homologação a versão não volta a fases anteriores (RN-08), então só se oferecem Homologação e Produção.
   */
  readonly opcoesDeFase = computed(() =>
    this.versao()?.fase === 'Homologacao' ? FASES.filter((f) => f.valor === 'Homologacao' || f.valor === 'Producao') : FASES,
  );

  /** Muda a cada alteração do formulário; faz o botão "×" das datas aparecer e sumir na hora. */
  private readonly alteracoes = toSignal(this.formulario.valueChanges);

  constructor() {
    this.formulario.controls.fase.valueChanges.subscribe((fase) => this.ajustarSituacao(fase));
    this.carregar();
  }

  /** Salva tudo de uma vez pela API; se qualquer regra falhar, nada é salvo e as mensagens aparecem. */
  protected salvar(): void {
    if (this.salvando()) {
      return;
    }
    this.salvando.set(true);
    this.erros.set([]);
    this.versaoService.gravar(this.id, this.montarCorpo()).subscribe({
      next: () => {
        // Salvou: volta para a lista de aplicações, com o aviso de salvo.
        this.salvando.set(false);
        this.aviso.open(AVISO_SALVO, undefined, { duration: 4000 });
        void this.router.navigate(['/']);
      },
      error: (erro: unknown) => {
        this.erros.set(mensagensDoErro(erro));
        this.salvando.set(false);
      },
    });
  }

  /** Pede confirmação e, só depois do "Excluir", exclui a versão e volta à lista (RN-04). */
  protected excluir(): void {
    const versao = this.versao();
    if (!versao) {
      return;
    }
    const dados: DadosDialogoConfirmacao = {
      titulo: 'Excluir versão',
      texto: `Excluir a versão ${versao.rotulo} da aplicação ${this.nomeAplicacao()}? Isso não pode ser desfeito.`,
      botao: 'Excluir',
    };
    this.dialog
      .open(DialogoConfirmacao, { data: dados })
      .afterClosed()
      .subscribe((confirmou) => {
        if (confirmou) {
          this.versaoService.excluir(this.id).subscribe({
            next: () => void this.router.navigate(['/']),
            error: (erro: unknown) => this.erros.set(mensagensDoErro(erro)),
          });
        }
      });
  }

  /** Diz se o campo de data está preenchido, para mostrar o botão que o esvazia. */
  protected temData(campo: CampoData): boolean {
    this.alteracoes();
    return this.formulario.controls[campo].value !== null;
  }

  /** Esvazia uma data; data vazia é data removida no salvamento (RN-09). */
  protected limpar(campo: CampoData): void {
    this.formulario.controls[campo].setValue(null);
  }

  private carregar(): void {
    this.versaoService.detalhar(this.id).subscribe({
      next: (versao) => {
        this.preencher(versao);
        this.aplicacaoService.listar().subscribe({
          next: (lista) => this.nomeAplicacao.set(lista.find((a) => a.id === versao.aplicacaoId)?.nome ?? ''),
          // O nome só enfeita o título; sem ele a tela continua funcionando.
          error: () => undefined,
        });
      },
      error: (erro: unknown) => this.erros.set(mensagensDoErro(erro)),
    });
  }

  private preencher(versao: Versao): void {
    this.versao.set(versao);
    const d = versao.datas;
    this.formulario.setValue({
      rotulo: versao.rotulo,
      fase: versao.fase,
      situacao: versao.situacao === 'Risco' ? 'Risco' : 'OnTrack',
      observacao: versao.observacao ?? '',
      inicioAndamento: textoParaData(d.inicioAndamento),
      fimAndamento: textoParaData(d.fimAndamento),
      inicioHomologacao: textoParaData(d.inicioHomologacao),
      fimHomologacao: textoParaData(d.fimHomologacao),
      inicioProrrogacao: textoParaData(d.inicioProrrogacao),
      fimProrrogacao: textoParaData(d.fimProrrogacao),
      dataEntrega: textoParaData(d.dataEntrega),
    });
    // RN-08: uma versão em Produção não muda mais de fase; o campo fica só de leitura (conveniência da tela).
    if (versao.fase === 'Producao') {
      this.formulario.controls.fase.disable({ emitEvent: false });
    } else {
      this.formulario.controls.fase.enable({ emitEvent: false });
    }
    this.ajustarSituacao(versao.fase);
  }

  /**
   * Em Backlog e em Produção a situação é automática (RN-21): o campo fica só de leitura. É só uma
   * conveniência da tela; quem decide é a API, que recusa a mudança com a MSG-11.
   */
  private ajustarSituacao(fase: string): void {
    const controle = this.formulario.controls.situacao;
    if (fase === 'Backlog' || fase === 'Producao') {
      controle.disable({ emitEvent: false });
      // Só para exibição: mostra já a situação que a API vai aplicar (RN-21), sem esperar o salvamento.
      this.situacaoAutomatica.set(rotuloSituacao(fase === 'Backlog' ? 'NaoIniciado' : 'Entregue'));
    } else {
      controle.enable({ emitEvent: false });
    }
  }

  private montarCorpo(): VersaoParaGravar {
    const v = this.formulario.getRawValue();
    return {
      rotulo: v.rotulo,
      situacao: this.formulario.controls.situacao.enabled ? v.situacao : '',
      observacao: v.observacao,
      fase: v.fase,
      datas: {
        inicioAndamento: dataParaTexto(v.inicioAndamento),
        fimAndamento: dataParaTexto(v.fimAndamento),
        inicioHomologacao: dataParaTexto(v.inicioHomologacao),
        fimHomologacao: dataParaTexto(v.fimHomologacao),
        inicioProrrogacao: dataParaTexto(v.inicioProrrogacao),
        fimProrrogacao: dataParaTexto(v.fimProrrogacao),
        dataEntrega: dataParaTexto(v.dataEntrega),
      },
    };
  }
}

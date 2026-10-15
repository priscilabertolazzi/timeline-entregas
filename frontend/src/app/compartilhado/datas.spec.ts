import { dataParaTexto, textoParaData } from './datas';

describe('datas', () => {
  it('converte a data escolhida em AAAA-MM-DD, sem trocar o dia', () => {
    // 01/10/2026 às 00:30 e 23:30, no horário local: o dia nunca muda (toISOString mudaria no fuso de Brasília).
    expect(dataParaTexto(new Date(2026, 9, 1, 0, 30))).toBe('2026-10-01');
    expect(dataParaTexto(new Date(2026, 9, 1, 23, 30))).toBe('2026-10-01');
  });

  it('completa mês e dia com zero à esquerda', () => {
    expect(dataParaTexto(new Date(2026, 0, 5))).toBe('2026-01-05');
  });

  it('sem data, devolve texto vazio, que a API entende como data removida', () => {
    expect(dataParaTexto(null)).toBe('');
    expect(dataParaTexto(undefined)).toBe('');
  });

  it('converte AAAA-MM-DD da API em data local do mesmo dia', () => {
    const data = textoParaData('2026-10-28');

    expect(data?.getFullYear()).toBe(2026);
    expect(data?.getMonth()).toBe(9);
    expect(data?.getDate()).toBe(28);
  });

  it('texto ausente ou fora do formato vira nulo', () => {
    expect(textoParaData(null)).toBeNull();
    expect(textoParaData('28/10/2026')).toBeNull();
  });
});

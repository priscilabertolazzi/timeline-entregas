/**
 * Converte a data escolhida no calendário no texto AAAA-MM-DD da API (ADR-27). Usa ano, mês e dia
 * locais, sem passar por UTC: `toISOString` trocaria o dia no fuso de Brasília. Sem data, devolve
 * texto vazio, que a API entende como data removida.
 */
export function dataParaTexto(data: Date | null | undefined): string {
  if (!data) {
    return '';
  }
  const mes = String(data.getMonth() + 1).padStart(2, '0');
  const dia = String(data.getDate()).padStart(2, '0');
  return `${data.getFullYear()}-${mes}-${dia}`;
}

/** Converte o texto AAAA-MM-DD da API numa data local, à meia-noite, para o calendário. */
export function textoParaData(texto: string | null | undefined): Date | null {
  const partes = /^(\d{4})-(\d{2})-(\d{2})$/.exec(texto ?? '');
  if (!partes) {
    return null;
  }
  return new Date(Number(partes[1]), Number(partes[2]) - 1, Number(partes[3]));
}

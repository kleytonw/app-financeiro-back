using System;

namespace ERP_API.Models.BI2
{
    public class RelatorioMovimentacaoPeriodoModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int? Dia { get; set; }              // nulo para granularidade Mensal
        public DateTime PeriodoRef { get; set; }   // dia (D) ou 1º dia do mês (M)
        public decimal ValorTotal { get; set; }    // Σ ValorTotal (padrão VENDA)
    }
}

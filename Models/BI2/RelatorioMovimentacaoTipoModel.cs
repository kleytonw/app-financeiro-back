namespace ERP_API.Models.BI2
{
    public class RelatorioMovimentacaoTipoModel
    {
        public string TipoMovimentacao { get; set; } = string.Empty;
        public int QtdeMovimentacoes { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Percentual { get; set; }
    }
}

namespace ERP_API.Models.DashboardTransacao
{
    public class DashboardTransacoesMeioPagamentoModel
    {
        public int IdUnidade { get; set; }

        public string MeioPagamento { get; set; }
        public string NomeOperadora { get; set; }
        public decimal? TotalLiquido { get; set; }
        public decimal? TotalBruto { get; set; }

        public decimal? TotalDespesas { get; set; }

        public int QuantidadeTransacoes { get; set; }

    }
}

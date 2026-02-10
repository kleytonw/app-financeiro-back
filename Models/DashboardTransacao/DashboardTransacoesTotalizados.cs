namespace ERP_API.Models.DashboardTransacao
{
    public class DashboardTransacoesTotalizados
    {
        public int? QuantidadeTransacoes { get; set;  }

        public decimal? TotalDespesas { get; set; }
        public decimal? TotalLiquido { get; set; }
        public decimal? TotalBruto { get; set; } 

        public string NomeOperadora { get; set; }
    }
}

namespace ERP_API.Models.BI2
{
    public class RelatorioDevolucaoModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal TotalVendas { get; set; }
        public decimal TotalDevolucoes { get; set; }
        public decimal PercentualDevolucao { get; set; }
    }
}

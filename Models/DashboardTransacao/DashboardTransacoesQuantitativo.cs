namespace ERP_API.Models.DashboardTransacao
{
    public class DashboardTransacoesQuantitativo
    {
        public int QtdeTransacoes { get; set; }

        public decimal TotalDespesa { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal TotalLiquido { get; set; }
    }
}

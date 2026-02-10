namespace ERP_API.Models.BI2
{
    public class RelatorioMargemBrutaModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal TotalVendas { get; set; }
        public decimal TotalCmv { get; set; }
        public decimal MargemBrutaPercentual { get; set; }
    }
}

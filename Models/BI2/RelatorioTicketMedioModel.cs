namespace ERP_API.Models.BI2
{
    public class RelatorioTicketMedioModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int QtdeTransacoes { get; set; }
        public decimal TotalVendas { get; set; }
        public decimal TicketMedio { get; set; }
    }
}

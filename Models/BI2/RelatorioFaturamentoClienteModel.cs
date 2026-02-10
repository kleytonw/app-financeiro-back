namespace ERP_API.Models.BI2
{
    public class RelatorioFaturamentoClienteModel
    {
        public string Cliente { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public int QtdeVendida { get; set; }
        public decimal Percentual { get; set; }
    }
}

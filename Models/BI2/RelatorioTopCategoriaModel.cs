namespace ERP_API.Models.BI2
{
    public class RelatorioTopCategoriaModel
    {
        public string Categoria { get; set; } = string.Empty;
        public int QtdeVendida { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Percentual { get; set; }
    }
}

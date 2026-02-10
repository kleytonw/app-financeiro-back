namespace ERP_API.Models.BI2
{
    public class RelatorioTopProdutoModel
    {
        public string Produto { get; set; } = string.Empty;
        public int QtdeVendida { get; set; }
        public decimal ValorTotal { get; set; }
    }
}

namespace ERP_API.Models.BI2
{
    public class RelatorioVolumeItensModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string TipoMovimentacao { get; set; } = string.Empty;
        public int QtdeTotal { get; set; }
    }
}

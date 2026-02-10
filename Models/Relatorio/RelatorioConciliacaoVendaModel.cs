namespace ERP_API.Models.Relatorio
{
    public class RelatorioConciliacaoVendaModel
    {
        public string Empresa { get; set; }
        public string DescricaoProduto { get; set; }    
        public int? QuantidadeParcela { get; set; }
        public string NomeOperadora { get; set; }

        public string Bandeira { get; set;  }
        public string MeioPagamento { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? TotalTaxa { get; set; }
        public decimal? TotalLiquido { get; set; }
    }
}
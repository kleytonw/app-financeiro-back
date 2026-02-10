namespace ERP.Models
{
    public class UnidadeParametroResponse
    {
        public int IdUnidadeParametro{ get; set; }
        public int? IdUnidade {  get; set; }
        public int? IdOperadora { get; set; }
        public int? IdEmpresa { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string Situacao { get; set; }
    }

    public class UnidadeParametroRequest
    {
        public int IdUnidadeParametro { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdOperadora { get; set; }
        public int? IdEmpresa { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string Situacao { get; set; }
    }
}

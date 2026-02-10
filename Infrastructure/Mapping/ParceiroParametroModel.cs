namespace ERP.Models
{
    public class ParceiroParametroResponse
    {
        public int IdParceiroParametro { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public int? IdParceiroSistema { get; set; }
        public string Situacao { get; set; }
    }

    public class ParceiroParametroRequest
    {
        public int IdParceiroParametro { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public int? IdParceiroSistema { get; set; }
        public string Situacao { get; set; }
    }
}

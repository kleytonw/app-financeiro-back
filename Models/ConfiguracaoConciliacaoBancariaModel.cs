namespace ERP_API.Models
{
    public class ConfiguracaoConciliacaoBancariaRequest
    {
        public int IdConfiguracaoConciliacaoBancaria { get; set; }
        public string Adquirente { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
        public string Situacao { get; set; }
    }

    public class ConfiguracaoConciliacaoBancariaResponse
    {
        public int IdConfiguracaoConciliacaoBancaria { get; set; }
        public string Adquirente { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
        public string Situacao { get; set; }
    }
}

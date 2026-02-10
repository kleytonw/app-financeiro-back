namespace ERP_API.Models
{
    public class ConfiguracaoConciliacaoRequest
    {
        public int IdConfiguracaoConciliacao { get; set; }
        public string Adquirente { get; set; }
        public string TipoTransacao { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }

    }

    public class ConfiguracaoConciliacaoResponse
    {
        public int IdConfiguracaoConciliacao { get; set; }
        public string Adquirente { get; set; }
        public string NomeAdquirente { get; set; }
        public string TipoTransacao { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }

    public class PesquisarConfiguracaoConcilicaoRequest
    {
        public string Chave { get; set; }
        public string Valor { get; set; }
    }
}

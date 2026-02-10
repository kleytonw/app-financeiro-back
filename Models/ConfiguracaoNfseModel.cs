namespace ERP_API.Models
{
    public class ConfiguracaoNfseRequest
    {
        public int IdConfiguracaoNfse { get; set; }
        public int NumeroRPS { get; set; }
        public string Situacao { get; set; }
    }

    public class ConfiguracaoNfseResponse
    {
        public int IdConfiguracaoNfse { get; set; }
        public int NumeroRPS { get; set; }
        public string Situacao { get; set; }
    }
}

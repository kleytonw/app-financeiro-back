namespace ERP.Models
{
    public class LocalizacaoResponse
    {
        public int IdLocalizacao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class LocalizacaoRequest
    {
        public int IdLocalizacao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}


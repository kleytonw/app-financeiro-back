namespace ERP.Models
{
    public class TributacaoResponse
    {
        public int IdTributacao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class TributacaoRequest
    {
        public int IdTributacao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}

namespace ERP.Models
{
    public class ProvedorResponse
    {
        public int IdProvedor { get; set; }
        public string Nome { get; set; }

        public string Situacao { get; set; }
    }

    public class ProvedorRequest
    {
        public int IdProvedor { get; set; }
        public string Nome { get; set; }

        public string Situacao { get; set; }
    }
}

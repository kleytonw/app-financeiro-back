namespace ERP.Models
{
    public class RegiaoResponse
    {
        public int IdRegiao { get; set; }
        public int? IdPessoa { get; set; }   
        public string NomeRegiao { get; set; }
        public string Situacao { get; set; }
    }

    public class RegiaoRequest
    {
        public int IdRegiao { get; set; }
        public int? IdPessoa { get; set; }
        public string NomeRegiao { get; set; }
        public string Situacao { get; set; }
    }
}


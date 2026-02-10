namespace ERP.Models
{
    public class RegiaoConsultorResponse
    {
        public int IdRegiaoConsultor { get; set; }
        public int IdRegiao { get; set; }
        public int IdConsultor { get; set; }
        public string Situacao { get; set; }
    }

    public class RegiaoConsultorRequest
    {
        public int IdRegiaoConsultor { get; set; }
        public int IdRegiao { get; set; }
        public int IdConsultor { get; set; }
        public string Situacao { get; set; }
    }
}
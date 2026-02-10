namespace ERP_API.Models
{
    public class ClasseTarifaRequest
    {
        public int IdClasseTarifa { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class ClasseTarifaResponse
    {
        public int IdClasseTarifa { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}

namespace ERP_API.Models
{
    public class PlanoContaRequest
    {
        public int IdPlanoConta { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Classificacao { get; set; }
        public string Tipo { get; set; }
        public string Situacao { get; set; }
    }

    public class PlanoContaResponse
    {
        public int IdPlanoConta { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Classificacao { get; set; }
        public string Tipo { get; set; }
        public string Situacao { get; set; }
    }
}

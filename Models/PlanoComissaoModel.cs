namespace ERP_API.Models
{
    public class PlanoComissaoRequest
    {
        public int IdPlanoComissao { get; set; }
        public int Nivel { get; set; }
        public decimal Percentual { get; set; }
        public string Situacao { get; set; }
    }

    public class PlanoComissaoResponse
    {
        public int IdPlanoComissao { get; set; }
        public int Nivel { get; set; }
        public decimal Percentual { get; set; }
        public string Situacao { get; set; }
    }
}

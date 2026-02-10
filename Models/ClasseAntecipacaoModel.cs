namespace ERP_API.Models
{
    public class ClasseAntecipacaoRequest
    {
        public int IdClasseAntecipacao { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }

    public class ClasseAntecipacaoResponse
    {
        public int IdClasseAntecipacao { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}
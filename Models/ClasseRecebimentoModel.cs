namespace ERP_API.Models
{
    public class ClasseRecebimentoRequest
    {
        public int IdClasseRecebimento { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }

    public class ClasseRecebimentoResponse
    {
        public int IdClasseRecebimento { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}

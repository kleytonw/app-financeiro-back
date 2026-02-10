namespace ERP_API.Models
{
    public class ClasseRecebimentoItemRequest
    {
        public int IdClasseRecebimentoItem { get; set; }
        public int IdClasseRecebimento { get; set; }
        public int IdBandeira { get; set; }
        public int IdMeioPagamento { get; set; }
        public int NumeroDias { get; set; }
        public string Situacao { get; set; }
    }

    public class ClasseRecebimentoItemResponse
    {
        public int IdClasseRecebimentoItem { get; set; }
        public int IdClasseRecebimento { get; set; }
        public int IdBandeira { get; set; }
        public int IdMeioPagamento { get; set; }
        public int NumeroDias { get; set; }
        public string Situacao { get; set; }
    }
}

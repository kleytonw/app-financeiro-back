namespace ERP_API.Models
{
    public class ClasseAntecipacaoItemRequest
    {
        public int IdClasseAntecipacaoItem { get; set; }
        public int IdClasseAntecipacao { get; set; }
        public int IdBandeira { get; set; }
        public int IdMeioPagamento { get; set; }
        public int NumeroDias { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Percentual { get; set; }
        public string Situacao { get; set; }
    }
    public class ClasseAntecipacaoItemResponse
    {
        public int IdClasseAntecipacaoItem { get; set; }
        public int IdClasseAntecipacao { get; set; }
        public int IdBandeira { get; set; }
        public int IdMeioPagamento { get; set; }
        public int NumeroDias { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Percentual { get; set; }
        public string Situacao { get; set; }
    }
}

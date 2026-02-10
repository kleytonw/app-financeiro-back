namespace ERP_API.Models
{
    public class ClasseTarifaItemRequest
    {
        public int IdClasseTarifaItem { get; set; }
        public int IdClasseTarifa { get; set; }
        public int IdMeioPagamento { get; set; }
        public int? IdBandeira { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? Valor { get; set; }
        public string Tipo { get; set; }
        public int? ParcelaInicio { get; set; }
        public int? ParcelaFim { get; set; }
        public string Situacao { get; set; }
    }

    public class ClasseTarifaItemResponse
    {
        public int IdClasseTarifaItem { get; set; }
        public int IdClasseTarifa { get; set; }
        public int IdMeioPagamento { get; set; }
        public int? IdBandeira { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? Valor { get; set; }
        public string Tipo { get; set; }
        public int? ParcelaInicio { get; set; }
        public int? ParcelaFim { get; set; }
        public string Situacao { get; set; }
    }
}

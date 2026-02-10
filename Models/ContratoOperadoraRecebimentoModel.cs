namespace ERP_API.Models
{
    public class ContratoOperadoraRecebimentoRequest
    {
        public int IdContratoOperadoraRecebimento { get; set; }
        public int? IdContratoOperadora { get; set; }
        public int? IdBandeira { get; set; }
        public int? IdMeioPagamento { get; set; }
        public int? NumeroDias { get; set; }
        public string Situacao { get; set; }

    }

    public class ContratoOperadoraRecebimentoResponse
    {
        public int IdContratoOperadoraRecebimento { get; set; }
        public int? IdContratoOperadora { get; set; }
        public string NomeContratoOperadora { get; set; }
        public int? IdBandeira { get; set; }
        public int? IdMeioPagamento { get; set; }
        public string NomeBandeira { get; set; }
        public int? NumeroDias { get; set; }
        public string Situacao { get; set; }
    }
}

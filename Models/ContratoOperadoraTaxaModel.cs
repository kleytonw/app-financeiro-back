using System;

namespace ERP.Models
{
    public class ContratoOperadoraTaxaResponse
    {
        public int? IdContratoOperadoraTaxa { get; set; }
        public int? IdContratoOperadora { get; set; }
        public int? IdMeioPagamento { get; set; }
        public int? IdBandeira {  get; set; }
        public decimal Taxa { get; set; }
        public decimal? Valor { get; set; }
        public int? ParcelaInicio { get; set; }
        public int? ParcelaFim { get; set; }
        public string Tipo { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdUnidade { get; set; }
        public string Situacao { get; set; }
    }

    public class ContratoOperadoraTaxaRequest
    {
        public int? IdContratoOperadoraTaxa { get; set; }
        public int? IdContratoOperadora { get; set; }
        public int? IdMeioPagamento { get; set; }
        public int? idOperadora { get; set; }
        public int? IdBandeira { get; set; }
        public decimal Taxa { get; set; }
        public decimal Valor { get; set; }
        public int? ParcelaInicio { get; set; }
        public int? ParcelaFim { get; set; }
        public string Tipo { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUnidade { get; set; }
        public string Situacao { get; set; }
    }
}

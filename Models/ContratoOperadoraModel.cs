using System;

namespace ERP.Models
{
    public class ContratoOperadoraResponse
    {
        public int IdContratoOperadora { get; set; }
        public int? IdOperadora { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdContaRecebimento { get; set; }
        public int? IdContaGravame { get; set; }
        public string Situacao { get; set; }
    }

    public class ContratoOperadoraRequest
    {
        public int IdContratoOperadora { get; set; }
        public int? IdOperadora { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdContaRecebimento { get; set; }
        public int? IdContaGravame { get; set; }
        public string Situacao { get; set; }
    }
}
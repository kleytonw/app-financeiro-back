using System;

namespace ERP_API.Models
{
    public class IntegrarTransacaoModelRequest
    {
        public int? IdUnidade { get; set; }
        public int? IdOperadora { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }

    public class IntegrarTransacaoModelResponse
    {
        public int? IdUnidade { get; set; }
        public int? IdOperadora { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}

using System;

namespace ERP_API.Service.Parceiros
{
    public class WebHookUniqueRequest
    {
        public string Evento { get; set; }
        public DateTime Data { get; set; }
        public ObjRequest ObjRequest { get; set; }

    }

    public class ObjRequest
    {
        public int IdTransacao { get; set; }
        public string Sacado { get; set; }
        public string CpfCnpj { get; set; }
        public decimal ValorVencimento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
        public string StatusTransacao { get; set; }
    }
    public class WebHookUniqueResponse
    {
        public string Evento { get; set; }
        public DateTime Data { get; set; }
        public ObjRequest ObjRequest { get; set; }
    }
}

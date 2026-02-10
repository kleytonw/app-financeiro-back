using System;

namespace ERP.Models
{
    public class MeioPagamentoResponse
    {
        public int IdMeioPagamento { get; set; }
        public string NomeMeioPagamento { get; set; }
        public string Situacao { get; set; }
    }

    public class MeioPagamentoRequest
    {
        public int IdMeioPagamento { get; set; }
        public string NomeMeioPagamento { get; set; }
        public string Situacao { get; set; }
    }
}


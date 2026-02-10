using System;

namespace ERP.Models
{
    public class BancoResponse
    {
        public int IdBanco { get; set; }
        public string NomeBanco { get; set; }
        public string CodigoBancoTecnoSpeed { get; set; }
        public string Situacao { get; set; }
    }

    public class BancoRequest
    {
        public int IdBanco { get; set; }
        public string NomeBanco { get; set; }
        public string CodigoBancoTecnoSpeed { get; set; }
        public string Situacao { get; set; }
    }
}


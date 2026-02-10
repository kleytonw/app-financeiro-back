using System;

namespace ERP.Models
{
    public class BandeiraResponse
    {
        public int IdBandeira { get; set; }
        public string NomeBandeira { get; set; }
        public string CodigoBandeiraCartao { get; set; }
        public string CodigoBandeiraCartaoRede { get; set; }
        public string Situacao { get; set; }
    }

    public class BandeiraRequest
    {
        public int IdBandeira { get; set; }
        public string NomeBandeira { get; set; }
        public string CodigoBandeiraCartao { get; set; }
        public string CodigoBandeiraCartaoRede { get; set; }
        public string Situacao { get; set; }
    }
}


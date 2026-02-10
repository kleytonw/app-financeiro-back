using System;

namespace ERP.Models
{
    public class OperadoraResponse
    {
        public int IdOperadora { get; set; }
        public string NomeOperadora { get; set; }
        public string Situacao { get; set; }
    }

    public class OperadoraRequest
    {
        public int IdOperadora { get; set; }
        public string NomeOperadora { get; set; }
        public string Situacao { get; set; }
    }
}


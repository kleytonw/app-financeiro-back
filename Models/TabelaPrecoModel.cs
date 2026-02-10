using System;

namespace ERP.Models
{
    public class TabelaPrecoResponse
    {
        public int IdTabelaPreco { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string Situacao { get; set; }
    }

    public class TabelaPrecoRequest
    {
        public int IdTabelaPreco { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string Situacao { get; set; }
    }
}

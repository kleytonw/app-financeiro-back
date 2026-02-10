using System;

namespace ERP.Models
{
    public class MovimentacaoEstoqueResponse
    {
        public int IdMovimentacaoEstoque { get; set; }
        public int IdProduto { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal? Quantidade { get; set; }
        public string Situacao { get; set; }
    }

    public class MovimentacaoEstoqueRequest
    {
        public int IdMovimentacaoEstoque { get; set; }
        public int IdProduto { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal? Quantidade { get; set; }
        public string Situacao { get; set; }
    }
}

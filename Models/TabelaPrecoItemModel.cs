using System;

namespace ERP.Models
{
    public class TabelaPrecoItemResponse
    {
        public int IdTabelaPrecoItem { get; set; }
        public int IdTabelaPreco { get; set; }
        public int IdProduto { get; set; }
        public decimal ValorVenda { get; set; }
        public string Situacao { get; set; }
    }

    public class TabelaPrecoItemRequest
    {
        public int IdTabelaPrecoItem { get; set; }
        public int IdTabelaPreco { get; set; }
        public int IdProduto { get; set; }
        public decimal ValorVenda { get; set; }
        public string Situacao { get; set; }
    }
}

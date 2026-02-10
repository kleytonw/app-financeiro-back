using System;

namespace ERP.Models
{
    public class LacamentoItemResponse
    {
        public int IdLancamentoItem { get; set; }
        public int? IdProduto { get; set; }
        public int? IdCarrinho { get; set; }
        public string NomeProduto { get; set; }
        public decimal? Preco { get; set; }
        public int? Quantidade { get; set; }
        public decimal? Subtotal { get; set; }
        public string Situacao { get; set; }
    }

    public class LancamentoItemRequest
    {
        public int IdLancamentoItem { get; set; }
        public int? IdProduto { get; set; }
        public int? IdCarrinho { get; set; }
        public string NomeProduto { get; set; }
        public decimal? Preco { get; set; }
        public int? Quantidade { get; set; }
        public decimal? Subtotal { get; set; }
        public string Situacao { get; set; }
    }
}
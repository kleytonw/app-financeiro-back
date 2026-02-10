using System;
using System.Collections.Generic;
using ERP_API.Domain.Entidades;
using Google.Protobuf.WellKnownTypes;

namespace ERP.Models
{
    public class CarrinhoResponse
    {
        public int? IdCarrinho { get; set; }
        public int? IdLancamentoItem { get; set; }
        public DateTime DataFinal { get; set; }
        public int? IdCliente { get; set; }
        public int? IdVendedor { get; set; }
        public List<LancamentoItemDTO> CarrinhoItens { get; set; }
        public string Situacao { get; set; }

        public CarrinhoResponse()
        {
            this.CarrinhoItens = new List<LancamentoItemDTO>();
        }

    }

    public class CarrinhoRequest
    {
        public int? IdCarrinho { get; set; }
        public DateTime DataFinal { get; set; }
        public int? IdCliente { get; set; }
        public int? IdVendedor { get; set; }
        public List<LancamentoItemDTO> CarrinhoItens { get; set; }
        public string Situacao { get; set; }

        public CarrinhoRequest()
        {
            this.CarrinhoItens = new List<LancamentoItemDTO>();
        }
    }
}

using ERP_API.Domain.Entidades;
using System;

namespace ERP_API.Models
{
    public class PagamentoRequest
    {
        public int IdPagamento { get; set; }
        public int IdEmpresa { get; set; }
        public int IdOperadora { get; set; }
        public int IdUnidade { get; set; }
        public DateTime? DataPagamento { get; set; }
        public int? IdBanco { get; set; }
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public int? IdBandeira { get; set; }
        public string CodigoBandeira { get; set; }
        public string NomeBandeira { get; set; }
        public string RazaoSocial { get; set; }
        public decimal? ValorPagamento { get; set; }
        public string StatusPagamento { get; set; }
        public string TipoPagamento { get; set; }
        public bool StatusConciliado { get; set; }
    }

    public class PagamentoResponse
    {
        public int IdPagamento { get; set; }
        public int IdEmpresa { get; set; }
        public int IdOperadora { get; set; }
        public int IdUnidade { get; set; }
        public DateTime? DataPagamento { get; set; }
        public int? IdBanco { get; set; }
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public int? IdBandeira { get; set; }
        public string CodigoBandeira { get; set; }
        public string NomeBandeira { get; set; }
        public string RazaoSocial { get; set; }
        public decimal? ValorPagamento { get; set; }
        public bool StatusConciliado { get; set; }
        public string StatusPagamento { get; set; }
        public string TipoPagamento { get; set; }
    }
}

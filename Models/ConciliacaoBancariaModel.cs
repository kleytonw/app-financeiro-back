using ERP_API.Domain.Entidades;
using System;
using System.Collections.Generic;

namespace ERP_API.Models
{
    public class ConciliacaoBancariaRequest
    {
        public int IdConciliacaoBancaria { get; set; }
        public int IdCliente { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal Valor { get; set; }
        public string Adquirente { get; set; }
        public string Status { get; set; }
        public string Usuario { get; set; }
    }

    public class ConciliacaoBancariaResponse
    {
        public int IdConciliacaoBancaria { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal? ValorConciliacao { get; set; }
        public decimal Valor { get; set; }
        public string Adquirente { get; set; }
        public bool? ConciliadoManual { get; set; }
        public string Status { get; set; }
        public string Situacao { get; set; }
    }

    public class PesquisarConciliacaoBancariaRequest
    {
        public string IdentificadorConciliadora { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Email { get; set; }
    }

    public class DetalharConciliacaoBancariaRequest
    {
        public int IdCliente { get; set; }
        public DateTime DataPagamento { get; set; }
        public int Adquirente { get; set; }
        public string NomeAdquirente { get; set; }

    }

    public class DadosConciliacaoModel
    {
        public int TotalPendente { get; set; }
        public int TotalConciliado { get; set; }
        public int TotalNaoConciliado { get; set; }
        public List<ConciliacaoBancaria> Transacoes { get; set; } = new List<ConciliacaoBancaria>();
        public int QuantidadeConciliacoes { get; set; }
        public decimal ValorTotalConciliacoes { get; set; }
        public int QuantidadeVendasERP { get; set; }
        public decimal ValorTotalVendasERP { get; set; }
    }
}

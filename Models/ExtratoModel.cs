using OFXSharp;
using System;

namespace ERP.Models
{
    public class ExtratoResponse
    {
        public int IdExtrato { get; set; }
        public int? IdClienteContaBancaria { get; set; }
        public int IdCliente { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string UniqueId { get; set; }
        public OFXTransactionType? Tipo { get; set; }
        public DateTime DataLancamento {  get; set; }
        public string Pagador { get; set; }
        public string CpfCnpjPagador { get; set; }
        public string Categoria { get; set; }
        public string Banco { get; set; }
        public string MetodoPagamento { get; set; }
        public string Situacao { get; set; }
    }

    public class ExtratoRequest
    {
        public int IdExtrato { get; set; }
        public int? IdClienteContaBancaria { get; set; }
        public int IdCliente { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string UniqueId { get; set; }
        public OFXTransactionType? Tipo { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Pagador { get; set; }
        public string CpfCnpjPagador { get; set; }
        public string Categoria { get; set; }
        public string Banco { get; set; }
        public string MetodoPagamento { get; set; }
        public string Situacao { get; set; }
    }
}


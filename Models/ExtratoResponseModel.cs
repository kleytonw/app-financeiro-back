using ERP_API.Domain.Entidades;
using OFXSharp;
using System;

namespace ERP_API.Models
{
    public class ExtratoOpenFinanceResponseModel
    {
        public int IdExtrato { get; private set; }
        public int IdClienteContaBancaria { get; set; }
        public string Nome { get; set; }
        public int IdCliente { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; }

        public string Conta { get; set; }

        public string Tipo { get; set; }

        public string TipoOperacao { get; set; }

        public string Categoria { get; set; }

        public string Moeada { get; set; }

        public decimal? Saldo { get; set; }
        public string IdentificadorOpenFinance { get; set; }

        public string? CpfCnpjPagador { get; set; }
         
        public string? Pagador { get; set; }
        public string Banco { get; set; }
        public string MetodoPagamento { get; set; }
    }
}

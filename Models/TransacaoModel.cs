using System;

namespace ERP_API.Models
{
    public class TransacaoRequest
    {
        public int IdTransacao { get; set; }
        public int IdCategoria { get; set; }
        public int IdCartao { get; set; }
        public int IdDependente { get; set; }
        public int NumeroParcelas { get; set; }
        public int ParcelaAtual { get; set; }
        public DateTime DataCompra { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }

    public class TransacaoResponse
    {
        public int IdTransacao { get; set; }
        public int IdCategoria { get; set; }
        public int IdCartao { get; set; }
        public int IdDependente { get; set; }
        public int NumeroParcelas { get; set; }
        public int ParcelaAtual { get; set; }
        public DateTime DataCompra { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}

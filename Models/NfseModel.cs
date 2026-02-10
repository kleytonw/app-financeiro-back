using System;

namespace ERP_API.Models
{
    public class NfseRequest
    {
        public int IdNfse { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string ChaveAcesso { get; set; }
        public string NumeroRPS { get; set; }
        public string Serie { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public DateTime? DataHoraCancelamento { get; set; }
        public string StatusNotaFiscal { get; set; }
        public int IdServicoNfse { get; set; }
        public decimal Valor { get; set; }
        public string CodigoServico { get; set; }
        public string CodigoNBS { get; set; }
        public string Situacao { get; set; }
        public string ObservacoesNotaFiscal { get; set; }
    }
    public class NfseResponse
    {
        public int IdNfse { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string ChaveAcesso { get; set; }
        public string NumeroRPS { get; set; }
        public string Serie { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public DateTime? DataHoraCancelamento { get; set; }
        public string StatusNotaFiscal { get; set; }
        public int IdServicoNfse { get; set; }
        public decimal Valor { get; set; }
        public string CodigoServico { get; set; }
        public string CodigoNBS { get; set; }
        public string Situacao { get; set; }
        public string ObservacoesNotaFiscal { get; set; }
    }

    public class NfsePesquisarRequest
    {
        public int IdCliente { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

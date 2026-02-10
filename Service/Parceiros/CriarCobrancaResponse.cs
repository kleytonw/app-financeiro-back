using System.Collections.Generic;
using System;

namespace ERP_API.Service.Parceiros
{
    public class CriarCobrancaResponse
    {
        public CobrancaData Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class CobrancaData
    {
        public int IdTransacao { get; set; }
        public string BeneficiarioFinal { get; set; }
        public string CpfCnpJBeneficiarioFinal { get; set; }
        public string EnderecoFinal { get; set; }
        public string TelefoneBeneficiarioFinal { get; set; }
        public string EmailBeneficiarioFinal { get; set; }
        public string StatusTransacao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string NomeMeioPagamento { get; set; }
        public int IdMeioPagamento { get; set; }
        public string Identificador { get; set; }
        public string NumeroPedido { get; set; }
        public string NomeCliente { get; set; }
        public int IdCliente { get; set; }
        public string NomeSacado { get; set; }
        public string CpfCnpjSacado { get; set; }
        public string EnderecoSacado { get; set; }
        public string NumeroSacado { get; set; }
        public string CepSacado { get; set; }
        public string BairroSacado { get; set; }
        public string CidadeSacado { get; set; }
        public string EmailSacado { get; set; }
        public string TelefoneSacado { get; set; }
        public int IdContaCorrente { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string CodigoBarras { get; set; }
        public string LinhaDigitavel { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataProcessamento { get; set; }
        public string BoletoMensagem1 { get; set; }
        public string BoletoMensagem2 { get; set; }
        public string BoletoMensagem3 { get; set; }
        public string BoletoMensagem4 { get; set; }
        public string QrCodePix { get; set; }
        public DateTime? DataValidadePix { get; set; }
        public string NossoNumero { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorVencimento { get; set; }
        public List<SplitValorResponse> SplitsValores { get; set; }
        public string Especie { get; set; }
        public string Aceite { get; set; }
        public string Carteira { get; set; }
        public string NumeroDocumento { get; set; }
    }

    public class SplitValorResponse
    {
        public bool CobraTarifa { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Valor { get; set; }
    }
}

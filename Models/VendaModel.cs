using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;

namespace ERP.Models
{
    public class VendaResponse
    {
        public int IdVenda { get; set; }
        public DateTime? DataVenda { get; set; }
        public DateTime? DataPrevPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string Cliente { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? ValorDespesa { get; set; }
        public decimal? ValorLiquido { get; set; }
        public decimal? ValorPagamento { get; set; }
        public decimal? Taxa { get; set; }
        public string MeioPagamento { get; set; }
        public int? IdBandeira { get; set; }
        public string NomeBandeira { get; set; }
        public int? IdOperadora { get; set; }
        public int? IdContaRecebimento { get; set; }
        public int? IdContaGravame { get; set; }
        public string NomeOperadora { get; set; }
        public string Gravame { get; set; }
        public string StatusConciliacao { get; set; }
        public string StatusVenda { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdEmpresa { get; set; }
        public string Identificador { get; set; }
        public string Produto { get; set; }
        public string ProdutoCliente { get; set; }
        public string Modalidade { get; set; }
        public string Autorizacao { get; set; }
        public string Parcela { get; set; }
        public string Terminal { get; set; }

        public string Situacao { get; set; }
    }

    public class VendaRequest
    {
        public int IdVenda { get; set; }
        public DateTime? DataVenda { get; set; }
        public DateTime? DataPrevPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string Cliente { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? ValorDespesa { get; set; }
        public decimal? ValorLiquido { get; set; }
        public decimal? ValorPagamento { get; set; }
        public decimal? Taxa { get; set; }
        public string MeioPagamento { get; set; }
        public int? IdBandeira { get; set; }
        public string NomeBandeira { get; set; }
        public int? IdOperadora { get; set; }
        public int? IdContaRecebimento { get; set; }
        public int? IdContaGravame { get; set; }
        public string NomeOperadora { get; set; }
        public string Gravame { get; set; }
        public string StatusConciliacao { get; set; }
        public string StatusVenda { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdEmpresa { get; set; }
        public string Identificador { get; set; }
        public string Produto { get; set; }
        public string ProdutoCliente {  get; set; }
        public string Modalidade { get; set; }
        public string Autorizacao { get; set; }
        public string Parcela { get; set; }
        public string Terminal { get; set; }

        public string Situacao { get; set; }
    }

    public class PesquisarVendaRequest
    {
        public List<TransacaoListResponseModelV> Transacoes { get; set; }

        public decimal? TotalPago { get; set; }
        public decimal? TotalEmAberto { get; set; }
        public decimal? TotalCancelado { get; set; }
        public decimal? TotalEmLiquidacao { get; set; }
        public string TipoPeriodo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public decimal ValorPagoConciliacao { get; set; }
        public decimal ValorTarifaConciliacao { get; set; }
        public string Observacao { get; set; }
        public string Status { get; set; }
        public string Identificador { get; set; }
        public int? IdBandeira { get; set; }
        public int? IdOperadora { get; set; }
        public int IdUnidade { get; set; }
        public int QtedTransacoes { get; set; }
        public int QtdeConciliada { get; set; }
        public int QtdeInconsistente { get; set; }
        public int QtdeNaoConciliada { get; set; }

        
    }

    public class TransacaoListResponseModelV
    {
        /// <summary>
        /// Identificador da transação
        /// </summary>
        public string TransacaoId { get; set; }
        /// <summary>
        /// Pix, Boleto, CartaoCredito
        /// </summary>
        public string TipoTransacao { get; set; }
        /// <summary>
        /// Identificador do pedido (número de controle)
        /// </summary>
        public string NumeroPedido { get; set; }
        /// <summary>
        /// Nome ou RazaoSocial do Sacado
        /// </summary>
        public string Sacado { get; set; }
        /// <summary>
        /// CPF ou CNPJ do Sacado
        /// </summary>
        public string CPFCNPJ { get; set; }
        /// <summary>
        /// Data Hora da Criação da Transação
        /// </summary>
        public DateTime DataCriacao { get; set; }
        /// <summary>
        /// Data de Vencimento
        /// </summary>
        public DateTime? DataVencimento { get; set; }
        /// <summary>
        /// Valor Original da Transação
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Valor Desconto
        /// </summary>
        public decimal? ValorDesconto { get; set; }

        /// <summary>
        /// Valor Pago
        /// </summary>
        public decimal? ValorPago { get; set; }

        /// <summary>
        /// Nosso Número
        /// </summary>
        public string? NossoNumero { get; set; }


        /// <summary>
        /// Taxa aplicada Juros 
        /// </summary>
        public decimal? TaxaJuros { get; set; }

        /// <summary>
        /// Taxa aplicada Multa 
        /// </summary>
        public decimal? TaxaMulta { get; set; }


        /// <summary>
        /// Status da Transação EmAberto, Pago, Cancelado, Liquidado
        /// </summary>
        public string Status { get; set; }

        public bool? GerouMovimentoContaCorrente { get; set; }

        /// <summary>
        /// Identificador da forma de pagamento da transação
        /// </summary>
        public string TipoPagamento { get; set; }

        /// <summary>
        /// Data e hora de liquidação
        /// </summary>
        public DateTime? DataHoraLiquidacao { get; set; }


        /// <summary>
        /// Identificador da transação na Instituição Financeira
        /// </summary>
        public string? Identificador { get; set; }

        /// <summary>
        /// Data e Hora do Pagamento
        /// </summary>
        public DateTime? DataHoraPagamento { get; set; }

        /// <summary>
        /// Data e Hora do Cancelamento
        /// </summary>
        public DateTime? DataHoraCancelamento { get; set; }

        /// <summary>
        /// Código de barras referênte ao boleto
        /// </summary>
        public string? CodigoBarras { get; set; }

        /// <summary>
        /// Agência 0001
        /// </summary>
        public string Agencia { get; set; }

        /// <summary>
        /// Conta
        /// </summary>
        public string Conta { get; set; }

        /// <summary>
        /// Cliente
        /// </summary>
        public string Cliente { get; set; }

        /// <summary>
        /// Descrição descrição conciliação
        /// </summary>
        public string? ObservacaoConciliacao { get; set; }

        /// <summary>
        /// Status realizado para conciliação
        /// </summary>
        public string StatusConciliacao { get; set; }

        /// <summary>
        /// Descrição erro conciliação
        /// </summary>
        public string? DescricaoErroConciliacao { get; set; }

        public DateTime? DataHoraConciliacao { get; set; }
    }
}


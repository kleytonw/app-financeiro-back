using System;
using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ConsultaTransacaoPagBankResponseModel
    {
        public List<DetalheTransacao> Detalhes { get; set; }
        public Paginacao Pagination { get; set; }
    }

    public class DetalheTransacao
    {
        public string MovimentoApiCodigo { get; set; }
        public string TipoRegistro { get; set; }
        public string Estabelecimento { get; set; }
        public DateTime DataInicialTransacao { get; set; }
        public string HoraInicialTransacao { get; set; }
        public DateTime DataVendaAjuste { get; set; }
        public string HoraVendaAjuste { get; set; }
        public string TipoEvento { get; set; }
        public string TipoTransacao { get; set; }
        public string CodigoTransacao { get; set; }
        public string CodigoVenda { get; set; }
        public decimal ValorTotalTransacao { get; set; }
        public decimal ValorParcela { get; set; }
        public string PagamentoPrazo { get; set; }
        public string Plano { get; set; }
        public string Parcela { get; set; }
        public string QuantidadeParcelas { get; set; }
        public DateTime DataPrevistaPagamento { get; set; }
        public decimal ValorOriginalTransacao { get; set; }
        public decimal TaxaIntermediacao { get; set; }
        public decimal TarifaIntermediacao { get; set; }
        public decimal ValorLiquidoTransacao { get; set; }
        public string StatusPagamento { get; set; }
        public string MeioPagamento { get; set; }
        public string InstituicaoFinanceira { get; set; }
        public string CanalEntrada { get; set; }
        public string Leitor { get; set; }
        public string MeioCaptura { get; set; }
        public string Nsu { get; set; }
        public string CartaoBin { get; set; }
        public string CartaoHolder { get; set; }
        public string CodigoAutorizacao { get; set; }
        public string Tid { get; set; }
        public string CodigoUr { get; set; }
        public string ArranjoUr { get; set; }
    }

    public class Paginacao
    {
        public int Elements { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int TotalElements { get; set; }
    }
}

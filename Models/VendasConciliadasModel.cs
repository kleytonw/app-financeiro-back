using System;

namespace ERP_API.Models
{
    public class VendasConciliadasRequest
    {
        public int IdVendaConciliada { get; set; }
        public string IdentificadorConciliadora { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Versao { get; set; }
        public int? Lote { get; set; }
        public string NomeSistema { get; set; }
        public int Produto { get; set; }
        public string DescricaoTipoProduto { get; set; }
        public string CodigoAutorizacao { get; set; }
        public string IdentificadorPagamento { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal ValorVendaParcela { get; set; }
        public decimal ValorLiquidoParcela { get; set; }
        public decimal TotalVenda { get; set; }
        public decimal? Taxa { get; set; }
        public int Parcela { get; set; }
        public int TotalParcelas { get; set; }
        public decimal? ValorBrutoMoeda { get; set; }
        public decimal? ValorLiquidoMoeda { get; set; }
        public decimal? CotacaoMoeda { get; set; }
        public string Moeda { get; set; }
        public string NSU { get; set; }
        public string TID { get; set; }
        public string Terminal { get; set; }
        public string MeioCaptura { get; set; }
        public int Operadora { get; set; }
        public string Modalidade { get; set; }
        public string Status { get; set; }
        public decimal? ValorBrutoConciliadora {  get; set; }
    }

    public class VendasConciliadasResponse
    {
        public int IdVendasConciliadas { get; set; }
        public string IdentificadorConciliadora { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Versao { get; set; }
        public int? Lote { get; set; }
        public string NomeSistema { get; set; }
        public int Produto { get; set; }
        public string DescricaoTipoProduto { get; set; }
        public string CodigoAutorizacao { get; set; }
        public string IdentificadorPagamento { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal ValorVendaParcela { get; set; }
        public decimal ValorLiquidoParcela { get; set; }
        public decimal TotalVenda { get; set; }
        public decimal? Taxa { get; set; }
        public int Parcela { get; set; }
        public int TotalParcelas { get; set; }
        public decimal? ValorBrutoMoeda { get; set; }
        public decimal? ValorLiquidoMoeda { get; set; }
        public decimal? CotacaoMoeda { get; set; }
        public string Moeda { get; set; }
        public string NSU { get; set; }
        public string TID { get; set; }
        public string Terminal { get; set; }
        public string MeioCaptura { get; set; }
        public int Operadora { get; set; }
        public string Modalidade { get; set; }
        public string Status { get; set; }
        public decimal? ValorBrutoConciliadora { get; set; }
    }

    public class PesquisarVendaConciliadaRequest
    {
        public string IdentificadorConciliadora { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string NSU { get; set; }
        public string MeioCaptura { get; set; }
        public int? IdCliente { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 1000;
    }


    public class VendasConciliadasRequestModelJson
    { 
        public int IdCliente { get; set; }
        public string IdentificadorConciliadora { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Versao { get; set; }
        public int? Lote { get; set; }
        public string NomeSistema { get; set; }
        public int Produto { get; set; }
        public string DescricaoTipoProduto { get; set; }
        public string CodigoAutorizacao { get; set; }
        public string IdentificadorPagamento { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal ValorVendaParcela { get; set; }
        public decimal ValorLiquidoParcela { get; set; }
        public decimal TotalVenda { get; set; }
        public decimal? Taxa { get; set; }
        public int Parcela { get; set; }
        public int TotalParcelas { get; set; }
        public decimal? ValorBrutoMoeda { get; set; }
        public decimal? ValorLiquidoMoeda { get; set; }
        public decimal? CotacaoMoeda { get; set; }
        public string Moeda { get; set; }
        public string NSU { get; set; }
        public string TID { get; set; }
        public string Terminal { get; set; }
        public string MeioCaptura { get; set; }
        public int Operadora { get; set; }
        public string Modalidade { get; set; }
        public string Status { get; set; }
    }
}

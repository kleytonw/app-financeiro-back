using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ERP_API.Service.Parceiros
{
    public class XmlErro
    {
        public int Linha { get; set; }
        public string Erro { get; set; }
        public bool Critico { get; set; }
    }

    public class ErroXmlResponse
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public List<XmlErro> XmlErros { get; set; }
    }

    public class AdquirentesConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<AdquirenteConciliadora> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class AdquirenteConciliadora
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
    }

    public class VendasCanceladasConciladoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<VendasCanceladaResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount{ get; set; }


    }

    public class VendasCanceladaResponse
    {
        [JsonPropertyName("Id")]
        public long? Id { get; set; }

        [JsonPropertyName("RefoId")]
        public int? RefoId { get; set; }
        public string Empresa { get; set; }
        public int? AdqId { get; set; }
        public string Adquirente { get; set; }
        public DateTime? DataVenda { get; set; }
        public DateTime? DataPagto { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int? CodigoProduto { get; set; }
        public string Produto { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public string Terminal { get; set; }
        public string ResumoVenda { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? OutrasDespesas { get; set; }
        public decimal? ValorLiquido { get; set; }
        public string Estabelecimento { get; set; }
        public string Cartao { get; set; }
        public int? NumParcela { get; set; }
        public int? TotalParcelas { get; set; }
        public int? Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string Motivo { get; set; }
    }

    public class ConciliacaoSistemaConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }
        [JsonPropertyName("value")]
        public List<ConciliacaoSistemaResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }
    public class ConciliacaoSistemaResponse
    {
        public long? Id { get; set; }
        public int? RefoId { get; set; }
        public string Empresa { get; set; }
        public string Cnpj { get; set; }
        public int? CodigoSistemaCliente { get; set; }
        public string SistemaCliente { get; set; }
        public int? Lote { get; set; }
        public int? AdqId { get; set; }
        public string Adquirente { get; set; }
        public int? CodigoProduto { get; set; }
        public string Produto { get; set; }
        public string ProdutoCliente { get; set; }
        public DateTime? DataVenda { get; set; }
        public DateTime? DataPrevistaPagto { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public string Terminal { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? ValorLiquido { get; set; }
        public int? NroParcela { get; set; }
        public int? TotalParcelas { get; set; }
        public string IdPagamento { get; set; }
        public int? CodigoMeioCaptura { get; set; }
        public string MeioCaptura { get; set; }
        public int? Status { get; set; }
        public string Divergencia { get; set; }
        public DateTime? DataPagto { get; set; }
        public decimal? ValorPago { get; set; }
        public bool? Antecipado { get; set; }
    }

    public class PagamentosConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<PagamentosResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class PagamentosResponse
    {
        public long? Id { get; set; }
        public string IdPagamento { get; set; }
        public int? RefoId { get; set; }
        public string Empresa { get; set; }
        public string Estabelecimento { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime? DataPrevistaPagamento { get; set; }     
        public DateTime DataVenda { get; set; }
        public int? AdqId { get; set; }
        public string Adquirente { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public int? Parcela { get; set; }
        public int? TotalParcelas { get; set; }
        public int? CodigoProduto { get; set; }
        public string Produto { get; set; }
        public string ResumoVenda { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? OutrasDespesas { get; set; }             
        public decimal? ValorLiquido { get; set; }
        public bool IdtAntecipacao { get; set; }
        public int? Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string NomeLoja { get; set; }
        public string Terminal { get; set; }
        public int? IdTipoTransacao { get; set; }
        public string TipoTransacao { get; set; }
        public int? IdStatus { get; set; }                        
        public string Status { get; set; }
        public string Divergencias { get; set; }
        public decimal? ValorLiquidoVenda { get; set; }
        public string Observacao { get; set; }
        public string MotivoAjuste { get; set; }
        public bool? ContaAdquirente { get; set; }                
        public decimal? TaxaAntecipacao { get; set; }             
        public decimal? TaxaAntecipacaoMensal { get; set; }      
        public decimal? ValorTaxaAntecipacao { get; set; }       
        public decimal? ValorTaxa { get; set; }                   
        public int? IdModalidade { get; set; }
        public string Modalidade { get; set; }
        public bool TemConciliacaoBancaria { get; set; }
        public string Cartao { get; set; }
    }
    public class PrevisaoPagamentoConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<PrevisaoPagamentoResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class PrevisaoPagamentoResponse
    {
        public long Id { get; set; }
        public int RefoId { get; set; }
        public string Empresa { get; set; }
        public string Estabelecimento { get; set; }
        public string Cnpj { get; set; }
        public int AdqId { get; set; }
        public string Adquirente { get; set; }
        public int CodigoProduto { get; set; }
        public string Produto { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime DataPagto { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal Taxa { get; set; }
        public decimal ValorLiquido { get; set; }
        public decimal ValorTaxa { get; set; }
        public decimal? OutrasDespesas { get; set; }
        public decimal ValorAReceber { get; set; }
        public string DomicilioBancario { get; set; }
        public string Terminal { get; set; }
        public string HoraTransacao { get; set; }
        public string NomeTurno { get; set; }
        public string Obs { get; set; }
        public string NumCartao { get; set; }
        public string MotivoAjuste { get; set; }
        public string NomeLoja { get; set; }
        public int Parcela { get; set; }
        public int TotalParcelas { get; set; }
    }

    public class StatusProcessamentoConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }
        [JsonPropertyName("value")]
        public List<StatusProcessamentoResponse> Value { get; set; }
    }

    public class StatusProcessamentoConciladoraRequest
    {
        public int IdEmpresa { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TipoArquivo { get; set; }
        public String ApiKey { get; set; }
        public string Status { get; set; }
    }

    public class StatusProcessamentoResponse
    {
        public int IdEmpresa { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TipoArquivo { get; set; }
        public string Status { get; set; }
        public Processos Processos { get; set; }
    }

    public class Processos
    {
        public ArquivoStatus ArquivoCliente { get; set; }
        public ArquivoStatus ArquivoAdquirente { get; set; }
        public ArquivoStatus ConciliacaoAdquirente { get; set; }
        public ArquivoStatus ConciliacaoCliente { get; set; }
    }

    public class ArquivoStatus
    {
        public List<StatusDetalhes> Status { get; set; }
    }

    public class StatusDetalhes
    {
        public StatusQuantidade Concluido { get; set; }
        public StatusQuantidade Processando { get; set; }
        public StatusQuantidade Pendente { get; set; }
        public StatusQuantidade Erro { get; set; }
    }

    public class StatusQuantidade
    {
        public int Quantidade { get; set; }
    }

    public class VendasConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }
        [JsonPropertyName("value")]
        public List<VendasResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class  VendasResponse
    {
        public long Id { get; set; }
        public int RefoId { get; set; }
        public string Empresa { get; set; }
        public string Estabelecimento { get; set; }
        public string Cnpj { get; set; }
        public int AdqId { get; set; }
        public string Adquirente { get; set; }
        public int? CodigoProduto { get; set; }
        public string Produto { get; set; }
        public int? CodigoModalidade { get; set; }
        public string Modalidade { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataPrevistaPagto { get; set; }
        public string ResumoVenda { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public string Cartao { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal PercentualTaxa { get; set; }
        public decimal ValorTaxa { get; set; }
        public decimal? OutrasDespesas { get; set; }
        public decimal ValorLiquido { get; set; }
        public string Terminal { get; set; }
        public int Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string NomeLoja { get; set; }
        public string HoraTransacao { get; set; }
        public int Parcela { get; set; }
        public int TotalParcelas { get; set; }
        public string Observacao { get; set; }
        public int Status { get; set; }
        public bool Rejeitado { get; set; }
        public bool Cancelado { get; set; }
        public bool Pago { get; set; }
        public DateTime? Cancelamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorPago { get; set; }
        public string IdPagamento { get; set; }
        public string Pedido { get; set; }
    }
    public class VendasConciliadasConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<VendasConciliadasResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class VendasConciliadasResponse
    {
        public long Id { get; set; }
        public int RefoId { get; set; }
        public DateTime DataEmissao { get; set; }
        public string IdPagamento { get; set; }
        public string Empresa { get; set; }
        public string Cnpj { get; set; }

        public string Estabelecimento { get; set; }
        public int AdqId { get; set; }
        public string Adquirente { get; set; }
        public int CodigoProduto { get; set; }
        public string Produto { get; set; }
        public int CodigoModalidade { get; set; }
        public string Modalidade { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataPrevistaPagto { get; set; }
        public string ResumoVenda { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public string Cartao { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal PercentualTaxa { get; set; }
        public decimal ValorTaxa { get; set; }
        public decimal? OutrasDespesas { get; set; }
        public decimal ValorLiquido { get; set; }
        public string Terminal { get; set; }
        public int Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string HoraTransacao { get; set; }
        public int Parcela { get; set; }
        public int TotalParcelas { get; set; }
        public string Observacao { get; set; }
        public int Status { get; set; }
        public bool Rejeitado { get; set; }
        public bool Cancelado { get; set; }
        public bool Pago { get; set; }
        public DateTime? Cancelamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorPago { get; set; }
        public string Pedido { get; set; }
    }

    public class VendasNaoEnviadasConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<VendasNaoEnviadasResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class VendasNaoEnviadasResponse
    {
        public long? Id { get; set; }
        public int? RefoId { get; set; }
        public string Empresa { get; set; }
        public string Estabelecimento { get; set; }
        public string Cnpj { get; set; }
        public int? AdqId { get; set; }
        public string Adquirente { get; set; }
        public int? CodigoProduto { get; set; }
        public string Produto { get; set; }
        public int? CodigoModalidade { get; set; }
        public string Modalidade { get; set; }
        public DateTime? DataVenda { get; set; }
        public DateTime? DataPrevistaPagto { get; set; }
        public string ResumoVenda { get; set; }
        public string Autorizacao { get; set; }
        public string Nsu { get; set; }
        public string Tid { get; set; }
        public string Cartao { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? PercentualTaxa { get; set; }
        public decimal? ValorTaxa { get; set; }
        public decimal? OutrasDespesas { get; set; }
        public decimal? ValorLiquido { get; set; }
        public string Terminal { get; set; }
        public int? Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string HoraTransacao { get; set; }
        public int? Parcela { get; set; }
        public int? TotalParcelas { get; set; }
        public string Observacao { get; set; }
        public int? Status { get; set; }
        public bool? Rejeitado { get; set; }
        public bool? Cancelado { get; set; }
        public bool? Pago { get; set; }
        public DateTime? Cancelamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorPago { get; set; }
        public string Pedido { get; set; }
    }

    public class MeioCapturaConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }
        [JsonPropertyName("value")]
        public List<MeioCapturaResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class MeioCapturaResponse
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
    }

    public class ModalidadeConciliadoraResponse
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<ModalidadeResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }

    public class ModalidadeResponse
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
    }

    public class ProdutoConciliadoraResponse 
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        public List<ProdutoResponse> Value { get; set; }

        [JsonPropertyName("@odata.count")]
        public int ODataCount { get; set; }
    }
    public class ProdutoResponse
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Modalidade { get; set; }
    }
    public class CabecalhoXml
        {
            public string Empresa { get; set; }
            public string DataInicial { get; set; }
            public string DataFinal { get; set; }
            public string Versao { get; set; } = "3";
            public string Lote { get; set; }
            public string NomeSistema { get; set; }
        }

    public class RegistroVenda
    {
        public int Produto { get; set; }
        public string DescricaoTipoProduto { get; set; }
        public string CodigoAutorizacao { get; set; }
        public string IdentificadorPagamento { get; set; }
        public string DataVenda { get; set; }
        public string DataVencimento { get; set; }
        public string ValorVendaParcela { get; set; }
        public string ValorLiquidoParcela { get; set; }
        public string TotalVenda { get; set; }
        public string Taxa { get; set; }
        public string Parcela { get; set; }
        public string TotalDeParcelas { get; set; }
        public string ValorBrutoMoeda { get; set; }
        public string ValorLiquidoMoeda { get; set; }
        public string CotacaoMoeda { get; set; }
        public string Moeda { get; set; }
        public string NSU { get; set; }
        public string TID { get; set; }
        public string Terminal { get; set; }
        public string MeioCaptura { get; set; }
        public string Operadora { get; set; }
        public string Modalidade { get; set; }
    }
}

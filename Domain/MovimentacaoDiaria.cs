using ERP.Models;
using ERP_API.Domain.Entidades;
using System;

namespace ERP_API.Domain
{
    public class MovimentacaoDiaria : BaseModel
    {

        public int IdMovimentacaoDiaria { get; set; }

        public int IdCliente { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }

        public DateTime DataMovimentacao { get; set; }

        public string FornecedorCliente { get; set; }

        public string CpfCnpjFornecedorCliente { get; set; }

        public string NotaFiscal { get; set; } = string.Empty;

        public string Produto { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string NCM { get; set; } = string.Empty;

        public string CFOP { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public string UnidadeMedida { get; set; }

        public decimal? ValorUnitario { get; set; }

        public decimal? ValorDesconto { get; set; }

        public decimal ValorTotal { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;


        public decimal? CMV_Aquisicao { get; set; }

        public decimal? CMV_Contabil { get; set; }

        public decimal? CMV_Tributos { get; set; }

        public decimal? CMV_Total { get; set; } 

        public bool Promocao { get; set;  } 
 
        public decimal? Margem { get; set; }

        public string Observacao { get; set; }

        public MovimentacaoDiaria() { }

        public MovimentacaoDiaria(string tipoMovimentacao,
                                  int idCliente,
                                  DateTime dataMovimentacao,
                                  string fornecedorCliente,
                                  string cpfCnpjFornecedorCliente,
                                  string notaFiscal,
                                  string produto,
                                  string categoria,
                                  string sku,
                                  string ncm,
                                  string cfop,
                                  int quantidade,
                                  string unidadeMedida,
                                  decimal? valorUnitario,
                                  decimal? valorDesconto,
                                  decimal? valorTotal,
                                  string codigoBarras,
                                  decimal? cmvAquisicao,
                                  decimal? cmvContabil,
                                  decimal? cmvTributos,
                                  decimal? cmvTotal,
                                  bool promocao,
                                  decimal? margem,
                                  string observacao,
                                  string usuarioInclusao)
        {
            this.TipoMovimentacao = (TipoMovimentacao)Enum.Parse(typeof(TipoMovimentacao), tipoMovimentacao);
            this.IdCliente = idCliente;
            this.DataMovimentacao = dataMovimentacao;
            this.FornecedorCliente = fornecedorCliente;
            this.CpfCnpjFornecedorCliente = cpfCnpjFornecedorCliente;
            this.NotaFiscal = notaFiscal;
            this.Produto = produto;
            this.Categoria = categoria;
            this.SKU = sku;
            this.NCM = ncm;
            this.CFOP = cfop;
            this.Quantidade = quantidade;
            this.UnidadeMedida = unidadeMedida;
            this.IdCliente = idCliente;
            this.DataMovimentacao = dataMovimentacao;
            this.FornecedorCliente = fornecedorCliente;
            this.CpfCnpjFornecedorCliente = cpfCnpjFornecedorCliente;
            this.NotaFiscal = notaFiscal;
            this.Produto = produto;
            this.Categoria = categoria;
            this.SKU = sku;
            this.NCM = ncm;
            this.CFOP = cfop;
            this.Quantidade = quantidade;
            this.UnidadeMedida = unidadeMedida;
            this.ValorUnitario = valorUnitario;
            this.ValorDesconto = valorDesconto;
            this.ValorTotal = valorTotal ?? 0;
            this.CodigoBarras = codigoBarras;
            this.CMV_Aquisicao = cmvAquisicao;
            this.CMV_Contabil = cmvContabil;
            this.CMV_Tributos = cmvTributos;
            this.CMV_Total = cmvTotal;
            this.Promocao = promocao;
            this.Margem = margem;
            this.Observacao = observacao;
            this.SetUsuarioInclusao(usuarioInclusao);
        }

         
    }

    public enum TipoMovimentacao
    {
        VENDA = 1,
        COMPRA = 2,
        DEVOLUCAO_VENDA = 3,
        DEVOLUCAO_COMPRA = 4,
        TRANSFERENCIA = 5,
        BONIFICACAO = 6,
        REMESSA = 7,
        RETORNO_REMESSA = 8,
        IMPORTACAO = 9,
        EXPORTACAO = 10,
        AJUSTE = 11
    }
}
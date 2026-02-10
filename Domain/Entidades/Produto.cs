using ERP.Models;
using ERP_API.Domain.Entidades;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Pkcs;

namespace ERP.Domain.Entidades
{
    public class Produto : BaseModel
    {
        public int IdProduto { get; private set; }
        public string NomeProduto { get; private set; }
        public int IdGrupoProduto { get; private set; } 
        public GrupoProduto GrupoProduto { get; private set; }
        public string PermitirCompra {  get; private set; }
        public int? IdUnidadeMedidaCompra { get; private set; }
        public UnidadeMedida? UnidadeMedidaCompra { get; private set; }
        public int? IdFornecedor { get; private set; }
        public Fornecedor Fornecedor { get; private set; }
        public decimal? PrecoDeCompra { get; private set; }
        public string PermitirVenda { get; private set; }
        public int? IdUnidadeMedidaVenda { get; private set; }
        public UnidadeMedida? UnidadeMedidaVenda { get; private set; }
        public decimal? PrecoDeVenda { get; private set; }
        public string ControleDeEstoque { get; private set; }
        public decimal? QuantidadeDeEstoque { get; private set; }
        public decimal? EstoqueMinimo { get; private set; }
        public UnidadeMedida UnidadeMedidaArmazenamento { get; private set; }
        public int? IdUnidadeMedidaArmazenamento { get; private set; }
        public string LinkFoto { get; private set; }
        public string Descricao {  get; private set; }
        public string CodigoProduto { get; private set; }
        public decimal? ValorVenda { get; private set; }
        public decimal? ValorCusto { get; private set; }

        public string Ean { get; private set; }
        

        public Produto() { }

        public Produto(string nome, string codigoProduto, GrupoProduto grupoProduto, Fornecedor fornecedor, string permitirCompra, UnidadeMedida unidadeMedidaCompra, decimal? precoCompra,
             string permitirVenda, UnidadeMedida unidadeMedidaVenda, decimal? precoVenda, string controleDeEstoque, decimal? quantidadeDeEstoque, decimal? estoqueMinimo,
             UnidadeMedida unidadeMedidaArmazenamento, string linkFoto, string descricao, string ean, decimal? valorVenda, 
             decimal? valorCusto, string usuarioInclusao)
        {
            NomeProduto = nome;
            CodigoProduto = codigoProduto;
            GrupoProduto = grupoProduto;
            Fornecedor = fornecedor;
            PermitirCompra = permitirCompra;
            UnidadeMedidaCompra = unidadeMedidaCompra;
            PrecoDeCompra = precoCompra;
            PermitirVenda = permitirVenda;
            UnidadeMedidaVenda = unidadeMedidaVenda;
            PrecoDeVenda = precoVenda;
            ControleDeEstoque = controleDeEstoque;
            QuantidadeDeEstoque = quantidadeDeEstoque;
            EstoqueMinimo = estoqueMinimo;
            UnidadeMedidaArmazenamento = unidadeMedidaArmazenamento;
            LinkFoto = linkFoto;
            Descricao = descricao;
            Ean = ean;
            ValorVenda = valorVenda;
            ValorCusto = valorCusto;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string codigoProduto, GrupoProduto grupoProduto, Fornecedor fornecedor, string permitirCompra, UnidadeMedida unidadeMedidaCompra, decimal? precoCompra,
             string permitirVenda, UnidadeMedida unidadeMedidaVenda, decimal? precoVenda, string controleDeEstoque, decimal? quantidadeDeEstoque, decimal? estoqueMinimo,
             UnidadeMedida unidadeMedidaArmazenamento, string linkFoto, string descricao, string ean, decimal? valorVenda, decimal? valorCusto, string usuarioAlteracao)
        {
            NomeProduto = nome;
            CodigoProduto = codigoProduto;
            GrupoProduto = grupoProduto;
            Fornecedor = fornecedor;
            PermitirCompra = permitirCompra;
            UnidadeMedidaCompra = unidadeMedidaCompra;
            PrecoDeCompra = precoCompra;
            PermitirVenda = permitirVenda;
            UnidadeMedidaVenda = unidadeMedidaVenda;
            PrecoDeVenda = precoVenda;
            ControleDeEstoque = controleDeEstoque;
            QuantidadeDeEstoque = quantidadeDeEstoque;
            EstoqueMinimo = estoqueMinimo;
            UnidadeMedidaArmazenamento = unidadeMedidaArmazenamento;
            LinkFoto = linkFoto;
            Descricao = descricao;
            Ean = ean;
            ValorVenda = valorVenda;
            ValorCusto = valorCusto;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(NomeProduto))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(PermitirCompra))
                throw new Exception("Permitir Compra é obrigatório");
            if (string.IsNullOrEmpty(PermitirVenda))
                throw new Exception("Permitir Venda é obrigatório");
            if (string.IsNullOrEmpty(ControleDeEstoque))
                throw new Exception("Controle de Estoque é obrigatório");
        }
    }
}

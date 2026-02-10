namespace ERP.Models
{
    public class ProdutoResponse
    {
        public int IdProduto { get; set; }
        public int IdGrupoProduto { get; set; }
        public int? IdSetor {  get; set; }
        public string NomeProduto { get; set; }
        public string CodigoProduto { get; set; }
        public string GrupoProduto { get; set; }
        public int? IdFornecedor { get; set; }
        public string PermitirCompra {  get; set; }
        public int? IdUnidadeMedidaCompra { get; set; }
        public decimal? PrecoDeCompra { get; set; }
        public string PermitirVenda { get; set; }
        public int? IdUnidadeMedidaVenda { get; set; }
        public decimal? PrecoDeVenda { get; set; }
        public string ControleDeEstoque { get; set; }
        public decimal? QuantidadeDeEstoque { get; set; }
        public decimal? EstoqueMinimo { get; set; }
        public int? IdUnidadeMedidaArmazenamento { get; set; }
        public string? LinkFoto { get; set; }
        public string? Descricao { get; set; }
        public string Ean { get; set; }
        public decimal? ValorVenda { get; set; }
        public decimal? ValorCusto { get; set; }
        public string Situacao { get; set; }
        
    }

    public class ProdutoRequest
    {
        public int IdProduto { get; set; }
        public int IdGrupoProduto { get; set; }
        public int? IdSetor { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoProduto { get; set; }
        public string GrupoProduto { get; set; }
        public int? IdFornecedor { get; set; }
        public string PermitirCompra { get; set; }
        public int? IdUnidadeMedidaCompra { get; set; }
        public decimal? PrecoDeCompra { get; set; }
        public string PermitirVenda { get; set; }
        public int? IdUnidadeMedidaVenda { get; set; }
        public decimal? PrecoDeVenda { get; set; }
        public string ControleDeEstoque { get; set; }
        public decimal? QuantidadeDeEstoque { get; set; }
        public decimal? EstoqueMinimo { get; set; }
        public int? IdUnidadeMedidaArmazenamento { get; set; }
        public string? LinkFoto { get; set; }
        public string? Descricao { get; set; }
        public string Ean { get; set; }
        public decimal? ValorVenda { get; set; }
        public decimal? ValorCusto { get; set; }
        public string Situacao { get; set; }
    }
}

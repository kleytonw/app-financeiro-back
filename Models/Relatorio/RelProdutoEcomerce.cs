using System;

namespace ERP.Models.Relatorio
{
    public class RelProdutoEcomerce
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public string GrupoNome { get; set; }
        public string Situacao { get; set; }
        public string Peso { get; set; }
    }

    public class RelTodosProdutosEcomerce
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public string NomeEmpresa { get; set; }
        public string Situacao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
    }

    public class FiltroProdutoEcomerce
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}

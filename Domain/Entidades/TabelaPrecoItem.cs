using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class TabelaPrecoItem : BaseModel
    {

        public int IdTabelaPrecoItem { get; private set; }
        public int IdTabelaPreco { get; private set; }
        public TabelaPreco TabelaPreco { get; private set; }
        public int IdProduto { get; private set; }
        public Produto Produto { get; private set; }
        public decimal ValorVenda { get; private set; }


        public TabelaPrecoItem() { }

        public TabelaPrecoItem(TabelaPreco tabelaPreco, Produto produto, decimal valorVenda, string usuarioInclusao)
        {
            TabelaPreco = tabelaPreco;
            Produto = produto;
            ValorVenda = valorVenda;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(TabelaPreco tabelaPreco, Produto produto, decimal valorVenda, string usuarioAlteracao)
        {
            TabelaPreco = tabelaPreco;
            Produto = produto;
            ValorVenda = valorVenda;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            
        }

        public void Valida()
        {
            if (TabelaPreco == null)
                throw new Exception("Tabela de Preço é obrigatório");
            if (Produto == null)
                throw new Exception("Produto é obrigatório");
            if (ValorVenda == 0)
                throw new Exception("Valor de Venda é obrigatório");
        }
    }
}



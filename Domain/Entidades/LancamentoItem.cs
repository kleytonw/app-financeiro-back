using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class LancamentoItem : BaseModel
    {
        public int IdLancamentoItem { get; private set; }
        public int IdProduto { get; private set; }
        public Produto Produto { get; private set; }
        public string NomeProduto { get; private set; }
        public int IdCarrinho { get; private set; }
        public Carrinho Carrinho { get; private set; }
        public decimal Preco { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Subtotal { get; private set; }

        public LancamentoItem() { }

        public LancamentoItem(Produto produto, decimal preco, int quantidade, string usuarioInclusao)
        {
            Produto = produto;
            NomeProduto = produto.NomeProduto;
            Preco = preco;
            Quantidade = quantidade;
            Subtotal = preco*quantidade;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        } 

        public void Valida()
        {
            if (Produto == null)
                throw new Exception("Produto é obrigatório");
            if (Quantidade == 0)
                throw new Exception("Quantidade é obrigatório");
            if (Preco == 0)
                throw new Exception("O preço é obrigatório");
        }
    }
}


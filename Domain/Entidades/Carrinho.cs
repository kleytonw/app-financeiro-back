using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ERP_API.Domain.Entidades
{
    public class Carrinho : BaseModel
    {
        public int IdCarrinho { get; private set; }
        public DateTime DataFinal { get; private set; }
        public int? IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public int? IdVendedor { get; private set; }
        public Vendedor Vendedor { get; private set; }
        public List<LancamentoItem> Itens { get; private set; }

        public decimal Total { get; private set;  }
            
        public Carrinho() { }

        public Carrinho(DateTime dataFinal, Cliente cliente, Vendedor vendedor, string usuarioInclusao)
        {
            DataFinal = dataFinal;
            Cliente = cliente;
            Vendedor = vendedor;
            SetUsuarioInclusao(usuarioInclusao);
            this.Itens = new List<LancamentoItem>();
            Valida();
        }

        public void Alterar(DateTime dataFinal, Cliente cliente, Vendedor vendedor,  string usuarioAlteracao)
        {
            DataFinal = dataFinal;
            Cliente = cliente;
            Vendedor = vendedor;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void CalcularTotal()
        {
            this.Total = this.Itens.Sum(x => x.Subtotal);
        }

        public void Valida()
        {
            if (Cliente == null)
            throw new Exception("Cliente é obrigatório");
            if (Vendedor == null)
                throw new Exception("Vendedor é obrigatório");

        }
    }
}

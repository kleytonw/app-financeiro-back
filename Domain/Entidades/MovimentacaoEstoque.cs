using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class MovimentacaoEstoque : BaseModel
    {

        public int IdMovimentacaoEstoque { get; private set; }
        public Produto Produto { get; private set; }
        public int IdProduto {  get; private set; }
        public DateTime Data {  get; private set; }
        public string Tipo  { get; private set; }   
        public decimal?Quantidade { get; private set; }

        public MovimentacaoEstoque() { }

        public MovimentacaoEstoque(Produto produto, DateTime data, string tipo, decimal? quantidade, string usuarioInclusao)
        {
            Produto = produto;
            Data = data;
            Tipo = tipo;
            Quantidade = quantidade;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Produto produto, DateTime data, string tipo, decimal? quantidade, string usuarioAlteracao)
        {
            Produto = produto;
            Data = data;
            Tipo = tipo;
            Quantidade = quantidade;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            //Valida();
        }

        public void Valida()
        {
            if (Produto == null)
                throw new Exception("O Produto é obrigatório!");
            if (Data == default(DateTime))
                throw new Exception("A Data é obrigatória!");
            if (string.IsNullOrEmpty(Tipo))
                throw new Exception("Nome é obrigatório");
            if (!Quantidade.HasValue || Quantidade <= 0)
                throw new Exception("A Quantidade deve ser maior que zero!");
            
        }
    }
}


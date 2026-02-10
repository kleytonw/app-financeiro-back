using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class SetorProduto : BaseModel
    {
        public int IdSetorProduto { get; private set; }
        public Setor Setor {  get; private set; }
        public int?  IdSetor { get; private set; }
        public Produto Produto { get; private set; }
        public int? IdProduto { get; private set; }

        public SetorProduto() { }

        public SetorProduto(Setor setor, Produto produto, string usuarioInclusao)
        {
            
            Setor = setor;
            Produto = produto;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(Setor setor, Produto produto, string usuarioAlteracao)
        {
           
            Setor = setor;
            Produto = produto;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);

        }

        public void Valida()
        {
            if (IdSetor == null)
                throw new Exception("IdSetor é obrigatório");
            if (IdProduto == null)
                throw new Exception("IdProduto é obrigatório");
        }
    }
}

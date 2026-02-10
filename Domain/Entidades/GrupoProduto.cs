using ERP.Models;
using System;

namespace ERP.Domain.Entidades
{
    public class GrupoProduto : BaseModel
    {
        public int IdGrupoProduto { get; private set; }
        public string Nome { get; private set; }
        public GrupoProduto() { }

        public GrupoProduto(string nome, string usuarioInclusao)
        {
            Nome = nome;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string usuarioAlteracao)
        {
            Nome = nome;
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
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
        }
    }
}

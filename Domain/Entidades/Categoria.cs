using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Categoria : BaseModel
    {
        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        protected Categoria() { }
        public Categoria(string nome, string usuarioInclusao)
        {
            this.Nome = nome;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(string nome, string usuarioAlteracao)
        {
            this.Nome = nome;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("O nome é obrigatório");
        }
    }
}

using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClasseTarifa: BaseModel
    {
        public int IdClasseTarifa { get; set; }
        public string Nome { get; set; }

        public ClasseTarifa() { }

        public ClasseTarifa(string nome,
                            string usuarioInclusao)
        {
            Nome = nome;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome,
                            string usuarioAlteracao)
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
                throw new Exception("A Nome é obrigatório");
        }
    }
}

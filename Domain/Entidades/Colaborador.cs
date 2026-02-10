using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Colaborador : BaseModel
    {
        public int IdPessoa { get; private set; }

        public Pessoa Pessoa { get; private set; }

        public Colaborador() { }

        public Colaborador(Pessoa pessoa, string usuarioInclusao)
        {
            Pessoa = pessoa;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Pessoa pessoa, string usuarioAlteracao)
        {
            Pessoa = pessoa;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Pessoa == null)
                throw new Exception("A pessoa do colaborador é obrigatória.");
        }
    }
}

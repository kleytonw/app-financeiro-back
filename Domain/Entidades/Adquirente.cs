using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Adquirente : BaseModel
    {
        public int IdPessoa { get; private set; }
        public Pessoa Pessoa { get; private set; }

        public Adquirente() { }

        public Adquirente(Pessoa pessoa, string usuarioInclusao)
        {
            Pessoa = pessoa;
            SetUsuarioInclusao(usuarioInclusao);
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
                throw new Exception("Pessoa é obrigatório");
        }
    }
}
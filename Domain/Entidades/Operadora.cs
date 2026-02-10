using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Operadora : BaseModel
    {

        public int IdOperadora { get; private set; }
        public string NomeOperadora { get; private set; }


        public Operadora() { }

        public Operadora(string nomeOperadora, string usuarioInclusao)
        {
            NomeOperadora = nomeOperadora;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nomeOperadora, string usuarioAlteracao)
        {
            NomeOperadora = nomeOperadora;
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
            if (string.IsNullOrEmpty(NomeOperadora))
                throw new Exception("O nome é obrigatório");
        }
    }
}




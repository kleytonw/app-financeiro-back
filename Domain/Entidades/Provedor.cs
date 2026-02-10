using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class Provedor : BaseModel
    {
        public int IdProvedor { get; private set; }

        public string NomeProvedor { get; private set; }

        public Provedor() { }

        public Provedor(string nome, string usuarioInclusao)
        {
            NomeProvedor = nome;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string usuarioAlteracao)
        {
            NomeProvedor = nome;
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
            if (string.IsNullOrEmpty(NomeProvedor))
                throw new Exception("Nome é obrigatorio");
        }


    }
}

using ERP.Models;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class RamoAtividade : BaseModel
    {
        public int IdRamoAtividade { get; private set; }

        public string Nome { get; private set; }

        public RamoAtividade() { }

        public RamoAtividade(string nome, string usuarioInclusao)
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
                throw new Exception("Nome é obrigatorio");
        }


    }
}
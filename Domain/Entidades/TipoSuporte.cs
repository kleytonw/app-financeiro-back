using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class TipoSuporte : BaseModel
    {
        public int IdTipoSuporte { get; private set; }

        public string NomeTipoSuporte { get; private set; }

        public TipoSuporte() { }

        public TipoSuporte(string nome, string usuarioInclusao)
        {
            NomeTipoSuporte = nome;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string usuarioAlteracao)
        {
            NomeTipoSuporte = nome;
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
            if (string.IsNullOrEmpty(NomeTipoSuporte))
                throw new Exception("Nome é obrigatorio");
        }


    }
}


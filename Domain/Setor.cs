using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class Setor : BaseModel
    {
        public int IdSetor { get; private set; }
        public int? IdSetorPai { get; private set; }
        public string NumeroOrdem { get; private set; }
    
        public string Nome { get; private set; }

        public Setor() { }

        public Setor(string nome, int? idSetorPai, string numeroOrdem, string usuarioInclusao)
        {
            Nome = nome;
            IdSetorPai = idSetorPai;
            NumeroOrdem = numeroOrdem; 
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, int? idSetorPai, string numeroOrdem, string usuarioAlteracao)
        {
            Nome = nome;
            IdSetorPai = idSetorPai;
            NumeroOrdem = numeroOrdem;
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


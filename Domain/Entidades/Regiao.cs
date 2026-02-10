using ERP.Models;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class Regiao: BaseModel
    {
        public int IdRegiao { get; private set; }

        public string NomeRegiao { get; private set; }

        public Regiao() { }

        public Regiao(string nomeRegiao, string usuarioInclusao)
        {
            NomeRegiao = nomeRegiao;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nomeRegiao, string usuarioAlteracao)
        {
            NomeRegiao = nomeRegiao;
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
            if (string.IsNullOrEmpty(NomeRegiao))
                throw new Exception("Nome é obrigatorio");
        }

        
    }
}

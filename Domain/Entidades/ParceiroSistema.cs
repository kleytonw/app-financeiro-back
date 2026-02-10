using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ParceiroSistema : BaseModel
    {
        public int IdParceiroSistema { get; private set; }
        public string NomeParceiroSistema { get; private set; }
        public string Observacao { get; private set; }

        public ParceiroSistema() { }

        public ParceiroSistema(string nomeParceiroSistema, string observacao, string usuarioInclusao)
        {
            NomeParceiroSistema = nomeParceiroSistema;
            Observacao = observacao;
            SetUsuarioInclusao(usuarioInclusao);
            
        }

        public void Alterar(string nomeParceiroSistema, string observacao, string usuarioAlteracao)
        {
            NomeParceiroSistema = nomeParceiroSistema;
            Observacao = observacao;
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
            if (string.IsNullOrEmpty(NomeParceiroSistema))
                throw new Exception("Nome é obrigatório");
        }
    }
}

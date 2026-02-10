using ERP.Models;
using System;
using System.Security.Policy;

namespace ERP_API.Domain.Entidades
{
    public class ConfiguracaoConciliacaoBancaria : BaseModel
    {
        public int IdConfiguracaoConciliacaoBancaria { get; private set; }
        public string Adquirente { get; private set; }
        public string De { get; private set; }
        public string Para { get; private set; }


        public ConfiguracaoConciliacaoBancaria( string adquirente, string de, string para, string usuarioInlcusao)
        {
            Adquirente = adquirente;
            De = de;
            Para = para;
            SetUsuarioInclusao(usuarioInlcusao);
        }

        public void Alteracao(string adquirente, string de, string para, string usuarioAlteracao)
        {
            Adquirente = adquirente;
            De = de;
            Para = para;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida() 
        {
            if (string.IsNullOrEmpty(Adquirente))
                throw new Exception("Adquirente é obrigatório");
            if (string.IsNullOrEmpty(De))
                throw new Exception("De é obrigatório");
            if (string.IsNullOrEmpty(Para))
                throw new Exception("Para é obrigatório");
        }

    }
}

using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ConfiguracaoConciliacao : BaseModel
    {
        public int IdConfiguracaoConciliacao { get; private set; }
        public string Adquirente { get; private set; }
        public string TipoTransacao { get; private set; }
        public string Descricao { get; private set; }

        public ConfiguracaoConciliacao() { }

        public ConfiguracaoConciliacao(string adquirente, string tipoTransacao, string descricao, string usuarioInclusao)
        {
            Adquirente = adquirente;
            TipoTransacao = tipoTransacao;
            Descricao = descricao;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string adquirente, string tipoTransacao, string descricao, string usuarioAlteracao)
        {
            Adquirente = adquirente;
            TipoTransacao = tipoTransacao;
            Descricao = descricao;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Adquirente == null)
                throw new Exception("Adquirente é obrigatório");
            if (string.IsNullOrWhiteSpace(TipoTransacao))
                throw new Exception("Tipo de Transação é obrigatório");
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new Exception("Descrição é obrigatória");
        }
    }
}

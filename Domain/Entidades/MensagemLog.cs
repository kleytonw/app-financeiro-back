using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class MensagemLog : BaseModel
    {
        public int IdMensagemLog { get; private set; }
        public Mensagem Mensagem { get; private set; }
        public int IdMensagem { get; private set; }
        public string Descricao { get; private set; }
        public string LogMensagemErro { get; private set; }
    


        public MensagemLog() { }

        public MensagemLog(Mensagem mensagem, string descricao, string logMesagemErro, string usuarioInclusao)
        {
            Mensagem = mensagem;
            Descricao = descricao;
            LogMensagemErro = logMesagemErro;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Mensagem mensagem, string descricao, string logMesagemErro, string usuarioAlteracao)
        {
            Mensagem = mensagem;
            Descricao = descricao;
            LogMensagemErro = logMesagemErro;
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
            if (string.IsNullOrEmpty(Descricao))
                throw new Exception("Texto é obrigatorio");
            if (string.IsNullOrEmpty(LogMensagemErro))
                throw new Exception("Texto é obrigatorio");
            if (Mensagem == null)
                throw new Exception("Mensagem é obrigatoria");
        }


    }
}

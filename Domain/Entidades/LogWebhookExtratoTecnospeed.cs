using ERP.Models;
using System;
using System.Runtime.InteropServices.JavaScript;

namespace ERP_API.Domain.Entidades
{
    public class LogWebhookExtratoTecnospeed : BaseModel
    {
        public int IdLogWebhookExtratoRede { get; set; }
        public DateTime? Data {  get; private set; }
        public string Happen { get; private set; }
        public string Balance { get; private set; }
        public string UniqueId { get; private set; }
        public string CreatedAt { get; private set; }
        public string AccountHash { get; private set; }

        public LogWebhookExtratoTecnospeed() { }

        public LogWebhookExtratoTecnospeed(DateTime? data, string happen, string balance, string uniqueId, string createdAt, string accountHash, string usuarioInlcusao)
        {
            Data = data;
            Happen = happen;
            Balance = balance;
            UniqueId = uniqueId;
            CreatedAt = createdAt;
            AccountHash = accountHash;
            SetUsuarioInclusao(usuarioInlcusao);
            Valida();
        }

        public void Alterar(DateTime? data, string happen, string balance, string uniqueId, string createdAt, string accountHash, string usuarioAlteracao)
        {
            Data = data;
            Happen = happen;
            Balance = balance;
            UniqueId = uniqueId;
            CreatedAt = createdAt;
            AccountHash = accountHash;
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
            if (!Data.HasValue && Data == DateTime.MinValue)
                throw new Exception("O campo Data é obrigatório!");
            if (string.IsNullOrEmpty(Happen))
                throw new Exception("O campo Happen é obrigatório!");
            if (string.IsNullOrEmpty(Balance))
                throw new Exception("O campo Balance é obrigatório!");
            if (string.IsNullOrEmpty(UniqueId))
                throw new Exception("O campo UniqueId é obrigatório!");
            if (string.IsNullOrEmpty(CreatedAt))
                throw new Exception("O campo CreatedAt é obrigatório!");
            if (string.IsNullOrEmpty(AccountHash))
                throw new Exception("O campo AccountHash é obrigatório!");
        }

    }
}

using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ControleCartaVanHistorico : BaseModel
    {
        public int IdControleCartaVanHistorico { get; set; }
        public int IdControleCartaVan {  get; set; }
        public ControleCartaVan ControleCartaVan { get; private set; }
        public int IdEtapa {  get; set; }
        public Etapa Etapa { get; set; }
        public DateTime Data {  get; set; }
        public string Guid { get; set; }
        public bool EnviarEmail { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }

        public ControleCartaVanHistorico() { }

        public ControleCartaVanHistorico(ControleCartaVan controleCartaVan, Etapa etapa, DateTime data, string descricao, string guid, bool enviarEmail, string email, string assunto, string usuarioInclusao)
        {
            ControleCartaVan = controleCartaVan;
            Etapa = etapa;
            Data = data;
            Descricao = descricao;
            Guid = guid;
            EnviarEmail = enviarEmail;
            Email = email;
            Assunto = assunto;

            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(ControleCartaVan controleCartaVan, Etapa etapa, DateTime data, string descricao, string guid, bool enviarEmail, string email, string assunto, string usuarioAlteracao)
        {
            ControleCartaVan = controleCartaVan;
            Etapa = etapa;
            Data = data;
            Descricao = descricao;
            Guid = guid;
            EnviarEmail = enviarEmail;
            Email = email;
            Assunto = assunto;

            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (ControleCartaVan == null)
                throw new Exception("O controle da carta van é obrigatório!");
            if (Etapa == null)
                throw new Exception("A etapa é obrigatória!");
        }

    }
}

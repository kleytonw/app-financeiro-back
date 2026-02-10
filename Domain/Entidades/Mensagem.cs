using ERP.Domain.Entidades;
using ERP.Models;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class Mensagem : BaseModel
    {
        public int IdMensagem { get; private set; }
        public DateTime Data {  get; private set; }
        public string Texto { get; private set; }
        public TipoMensagem TipoMensagem { get; private set; }
        public int? IdTipoMensagem { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public Provedor Provedor { get; private set; }
        public int? IdProvedor { get; private set; }


        public Mensagem() { }

        public Mensagem(DateTime data, string texto, TipoMensagem tipoMensagem, string telefone, string email,  Provedor provedor, string usuarioInclusao)
        {
            Data = data;
            Texto = texto;
            TipoMensagem = tipoMensagem;
            Telefone = telefone;
            Email = email;
            Provedor = provedor;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(DateTime data, string texto, TipoMensagem tipoMensagem, string telefone, string email, Provedor provedor, string usuarioAlteracao)
        {
            Data = data;
            Texto = texto;
            TipoMensagem = tipoMensagem;
            Telefone = telefone;
            Email = email;
            Provedor = provedor;
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
            if (string.IsNullOrEmpty(Texto))
                throw new Exception("Texto é obrigatorio");
            if (string.IsNullOrEmpty(Telefone))
                throw new Exception("Telefone é obrigatorio");
            if (TipoMensagem == null)
                throw new Exception("O tipo de mensagem é obrigatorio");
        }


    }
}
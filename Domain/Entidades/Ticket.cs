using ERP.Domain.Entidades;
using ERP.Models;
using ERP.Models.SecurityToken;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Data.Entity.Infrastructure;

namespace ERP_API.Domain.Entidades
{
    public class Ticket : BaseModel
    {
        public int IdTicket { get; private set; }
        public int? IdPessoa { get; private set; }
        public Cliente Cliente { get; private set; }
        public int? IdTipoSuporte { get; private set; }
        public TipoSuporte TipoSuporte { get; private set; }
        public string Titulo { get; private set; }
        public string Mensagem {  get; private set; }
        public string Status { get; private set; }
        public DateTime? DataAbertura { get; private set; } 
        public DateTime? DataConclusao { get; private set; }
        public DateTime? DataAndamento { get; private set; }
        public string UsuarioAtendimento { get; private set; }
        public string UsuarioConclusao { get; private set; }

        public Ticket() { }

        public Ticket(Cliente cliente, TipoSuporte tipoSuporte, string titulo, string mensagem, string usuarioInclusao)
        {
            Cliente = cliente;
            TipoSuporte = tipoSuporte;
            Mensagem = mensagem;
            Status = "Aberto";
            Titulo = titulo;
            DataAbertura = DateTime.Now;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, TipoSuporte tipoSuporte, string titulo, string mensagem, string usuarioAlteracao)
        {
            Cliente = cliente;
            TipoSuporte = tipoSuporte;
            Mensagem = mensagem;
            Status = "Aberto";
            Titulo = titulo;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void EmAtendimento(string usuarioAtendimento)
        { 
            this.Status = "Em Atendimento";
            this.DataAndamento = DateTime.Now;
            this.UsuarioAtendimento = usuarioAtendimento;
        }

        public void ConclusaoAntendimento(string usuarioConclusao)
        {
            this.Status = "Concluído";
            this.DataConclusao = DateTime.Now;
            this.UsuarioConclusao = usuarioConclusao;
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Mensagem))
                throw new Exception("A mensagem é obrigatorio");
            if (string.IsNullOrEmpty(Titulo))
                throw new Exception("O título é obrigatorio");
            if (TipoSuporte == null)
                throw new Exception("É obrigatorio informar o tipo de suporte");
            if (Cliente == null)
                throw new Exception("É obrigatorio informar a empresa");
        }


    }
}


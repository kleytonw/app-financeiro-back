using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class TicketMensagem : BaseModel
    {
        public int IdTicketMensagem { get; private set; }
        public Ticket Ticket { get; private set; }
        public int IdTicket { get; private set; }
        public string Mensagem { get; private set; }
        public string Arquivo { get; private set; }
        public string Usuario { get; private set; }
        public DateTime? Data { get; private set; }
        public TicketMensagem() { }
        public TicketMensagem(Ticket ticket, string mensagem, string Arquivo, string usuarioInclusao)
        {
            Ticket = ticket;
            Mensagem = mensagem;
            this.Arquivo = Arquivo;
            Data = DateTime.Now;
            Usuario = usuarioInclusao;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }
        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }
        public void Valida()
        {
            if (Ticket == null)
                throw new Exception("O ticket é obrigatório.");
            if (string.IsNullOrWhiteSpace(Mensagem))
                throw new Exception("A mensagem é obrigatória.");
        }
    }
}

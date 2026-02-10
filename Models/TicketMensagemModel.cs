using System;

namespace ERP_API.Models
{
    public class TicketMensagemRequest
    {
        public int IdTicketMensagem { get; set; }
        public int IdTicket { get; set; }
        public string Mensagem { get; set; }
        public string Arquivo { get; set; }
        public DateTime? Data { get; set; }
        public string Usuario { get; set; }
        public string Situacao { get; set; }
    }

    public class TicketMensagemResponse
    {
        public int IdTicketMensagem { get; set; }
        public int IdTicket { get; set; }
        public string Mensagem { get; set; }
        public string Arquivo { get; set; }
        public DateTime? Data { get; set; }
        public string Usuario { get; set; }
        public string TipoUsuario { get; set; }
        public string Situacao { get; set; }
    }
}

using System;

namespace ERP.Models
{
    public class TicketResponse
    {
        public int IdTicket { get; set; }
        public int? IdPessoa { get; set; }
        public string NomePessoa { get; set; }
        public int? IdTipoSuporte { get; set; }
        public string NomeTipoSuporte { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Status { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        public DateTime? DataAndamento { get; set; }
        public string UsuarioAtendimento { get; set; }
        public string UsuarioConclusao { get; set; }
        public string Situacao { get; set; }
    }

    public class TicketRequest
    {
        public int IdTicket { get; set; }
        public int? IdPessoa { get; set; }
        public int? IdTipoSuporte { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Status { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        public DateTime? DataAndamento { get; set; }
        public string Situacao { get; set; }
    }

    public class TicketFiltroRequest
    {
        public int? IdPessoa { get; set; }
        public int? IdTipoSuporte { get; set; }
        public string TipoPeriodo { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
    }
}

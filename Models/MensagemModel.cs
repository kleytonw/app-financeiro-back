using System;

namespace ERP.Models
{
    public class MensagemResponse
    {
        public int IdMensagem { get; set; }
        public DateTime Data { get; set; }
        public string Texto { get; set; }
        public int? IdTipoMensagem { get; set; }
        public string NomeTipoMensagem { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string NomeProvedor { get; set; }
        public int? IdProvedor { get; set; }
        public string Situacao { get; set; }
    }

    public class MensagemRequest
    {
        public int IdMensagem { get; set; }
        public DateTime Data { get; set; }
        public string Texto { get; set; }
        public int? IdTipoMensagem { get; set; }
        public string NomeTipoMensagem { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int? IdProvedor { get; set; }
        public string NomeProvedor { get; set; }
        public string Situacao { get; set; }
    }
}

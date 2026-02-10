using System;

namespace ERP.Models
{
    public class ContatoSiteResponse
    {
        public int IdContatoSite { get; set; }
        public string NomeContato { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int? IdEmpresa { get; set; }
        public string Mensagem { get; set; }
        public DateTime Data { get; set; }
        public string Situacao { get; set; }
    }

    public class ContatoSiteRequest
    {
        public int IdContatoSite { get; set; }
        public string NomeContato { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int? IdEmpresa { get; set; }
        public string Mensagem { get; set; }
        public DateTime Data { get; set; }
        public string Situacao { get; set; }
    }
}

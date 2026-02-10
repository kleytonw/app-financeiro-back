using System;

namespace ERP_API.Models
{
    public class ControleCartaVanHistoricoRequest
    {
        public int IdControleCartaVanHistorico {  get; set; }
        public int IdControleCartaVan {  get; set; }
        public int IdEtapa { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Guid { get; set; }
        public bool EnviarEmail { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public string Situacao { get; set; }
    }

    public class ControleCartaVanHistoricoResponse
    {
        public int IdControleCartaVanHistorico { get; set; }
        public int IdControleCartaVan { get; set; }
        public int IdEtapa { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Guid { get; set; }
        public bool EnviarEmail { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public string Situacao { get; set; }
    }

}

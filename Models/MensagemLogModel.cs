using System;

namespace ERP.Models
{
    public class MensagemLogResponse
    {
        public int IdMensagemLog { get; set; }
        public int? IdMensagem { get; set; }
        public string Texto { get; set; }
        public string LogMensagemErro { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }

    public class MensagemLogRequest
    {
        public int IdMensagemLog { get; set; }
        public int? IdMensagem { get; set; }
        public string LogMensagemErro { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}


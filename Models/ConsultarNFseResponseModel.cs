using System;

namespace ERP_API.Models
{
    public class ConsultarNFseResponseModel
    {
        public bool sucesso { get; set; }

        public int codigo { get; set; }

        public string mensagem { get; set; }

        public string status { get; set; }

        public string numero { get; set; }

        public string rps_numero { get; set; }

        public string rps_serie { get; set; }
        public string chave { get; set; }

        public string  codigo_verificacao { get; set; }

        public DateTime? data_hora_evento { get; set; }

        // Base64 raw strings (XML e PDF codificados em Base64) 
        public string xml { get; set; }

        public string pdf { get; set; }

        // URL para visualização (nem sempre presente) 
        public string link_pdf { get; set; }
    }
}

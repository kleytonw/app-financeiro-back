using System;

namespace ERP_Application.Models.Parceiros.IntegraNotas
{
    public class CancelarNFseResponseModel
    {
        public bool sucesso { get; set; }
        public int codigo { get; set; }
        public string mensagem { get; set; } = string.Empty;

        public DateTime? data_hora_evento { get; set; }
        public string pdf { get; set; } = string.Empty;
    }
}

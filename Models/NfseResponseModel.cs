
namespace ERP_API.Models
{
    public class NfseResponseModel
    {
        public bool Sucesso { get; set; }

        public int Codigo { get; set; }

        public string Mensagem { get; set; }

        public string Chave { get; set; }

        public StatusNotaFiscal StatusNotaFiscal { get; set; }

        public string Erros { get; set; }
    }

    public enum StatusNotaFiscal
    {
        Autorizado,
        Pendente,
        Cancelado,
        Rejeitado,
        EmProcessamento,
        Erro
    }


    public class NfseResponseAutorizadoModel
    {
        public bool Sucesso { get; set; }
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public string Status { get; set; }
        public string Numero { get; set; }
        public string Rps_Numero { get; set; }
        public string Rps_Serie { get; set; }
        public string Chave { get; set; }
        public string Codigo_Verificacao { get; set; }
        public string Data_Hora_Evento { get; set; }  // pode mapear para DateTime se preferir
        public string Xml { get; set; }  // base64
        public string Pdf { get; set; }  // base64
        public string Link_Pdf { get; set; }
    }
}

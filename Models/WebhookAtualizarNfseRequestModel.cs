using System.Text.Json.Serialization;

namespace ERP_Application.Models.Parceiros.IntegraNotas
{
    public class WebhookAtualizarNfseRequestModel
    {
        [JsonPropertyName("origem")]
        public string? Origem { get; set; }

        [JsonPropertyName("cnpj_cpf")]
        public string? CnpjCpf { get; set; }

        [JsonPropertyName("signature")]
        public string? Signature { get; set; }

        [JsonPropertyName("sucesso")]
        public bool Sucesso { get; set; }

        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("mensagem")]
        public string? Mensagem { get; set; }

        [JsonPropertyName("chave")]
        public string? Chave { get; set; }

        /// <summary>
        /// Conteúdo XML em Base64.
        /// </summary>
        [JsonPropertyName("xml")]
        public string? XmlBase64 { get; set; }

        /// <summary>
        /// PDF em Base64 (binário).
        /// </summary>
        [JsonPropertyName("pdf")]
        public string? PdfBase64 { get; set; }


    }
}

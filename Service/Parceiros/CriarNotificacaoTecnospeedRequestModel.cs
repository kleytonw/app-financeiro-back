using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ERP_API.Service.Parceiros
{
    public class CriarNotificacaoTecnospeedRequestModel
    {
        [JsonPropertyName("type")]
        [Required]
        public string Type { get; set; } = "webhook"; // Valor fixo

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("cc")]
        public string? Cc { get; set; }

        [JsonPropertyName("headers")]
        public Dictionary<string, string>? Headers { get; set; }

        [JsonPropertyName("url")]
        [Required]
        public string Url { get; set; }

        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        [JsonPropertyName("happen")]
        public List<string> Happen { get; set; } = new();
    }
}

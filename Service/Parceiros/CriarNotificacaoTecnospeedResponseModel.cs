using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP_API.Service.Parceiros
{

public class CriarNotificacaoTecnospeedResponseModel
    {
        [JsonPropertyName("uniqueId")]
        public string UniqueId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("cc")]
        public string? Cc { get; set; }

        [JsonPropertyName("headers")]
        public Dictionary<string, string>? Headers { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        [JsonPropertyName("happen")]
        public List<string> Happen { get; set; } = new();
    }
}

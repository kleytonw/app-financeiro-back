using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP_API.Service.Parceiros
{
    public class DeletarContaRequestModel
    {

        [JsonPropertyName("accountHash")]
        public List<string> AccountHash { get; set; }

        public DeletarContaRequestModel()
        {
            AccountHash = new List<string>();
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ERP_API.Service.Parceiros
{
    public class DeletarContaResponseModel
    {
        [JsonPropertyName("accounts")]
        public List<ContaInfo> Accounts { get; set; }
    }

    public class ContaInfo
    {
        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("accountHash")]
        public string AccountHash { get; set; }

        [JsonPropertyName("agency")]
        public string Agency { get; set; }

        [JsonPropertyName("agencyDigit")]
        public string AgencyDigit { get; set; }

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("accountNumberDigit")]
        public string AccountNumberDigit { get; set; }

        [JsonPropertyName("convenioAgency")]
        public string ConvenioAgency { get; set; }

        [JsonPropertyName("convenioNumber")]
        public string ConvenioNumber { get; set; }

        [JsonPropertyName("remessaSequential")]
        public int RemessaSequential { get; set; }
    }

}

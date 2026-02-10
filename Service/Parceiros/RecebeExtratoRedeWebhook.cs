using System;

namespace ERP_API.Service.Parceiros
{
    public class RecebeExtratoRedeWebhook
    {
        public int IdLogWebhookExtratoRede { get; set; }
        public DateTime Data {  get; set; }
        public string Happen {  get; set; }
        public string Balance { get; set; }
        public string UniqueId { get; set; }
        public string CreatedAt { get; set; }
        public string AccountHash { get; set; }
        public string Situacao { get; set; }
    }
}

using System;

namespace ERP_API.Models
{

    public class LogWebhookPluggyRequestModel
    {
        public int IdWebHookPluggy { get; set; }
        public DateTime Data { get; set; }

        public string ObjJson { get; set; }
    }
    public class LogWebhookPluggyResponseModel
    {
        public int IdWebHookPluggy { get; set; }
        public DateTime Data { get; set; }

        public string ObjJson { get; set; }
    }

    public class LogWebhookPluggyPesquisarModel
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

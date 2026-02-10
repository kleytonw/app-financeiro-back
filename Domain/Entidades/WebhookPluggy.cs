using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class WebhookPluggy : BaseModel
    {
        public int IdWebHookPluggy { get; set; }
        public DateTime Data { get; set; }
        public string ObjJson { get; set; }
        

        public WebhookPluggy() { }

        public WebhookPluggy(DateTime data, string objJson)
        {
            Data = data;
            ObjJson = objJson;
            SetUsuarioInclusao("admin");
        }
    }
}

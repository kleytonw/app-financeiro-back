using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class DeletarNotificacaoTecnospeedResponseModel
    {
        public List<NotificationDeletar> Notification { get; set; }
    }

    public class NotificationDeletar
    {
        public string UniqueId { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Cc { get; set; }
        public object Headers { get; set; } 
        public string Url { get; set; }
        public string Mobile { get; set; }
        public List<string> Happen { get; set; }
    }
}

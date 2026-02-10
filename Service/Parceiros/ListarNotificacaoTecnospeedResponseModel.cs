using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ListarNotificacaoTecnospeedResponseModel
    {
        public int IdUnidade {  get; set; }
        public List<Notification> Notification { get; set; }
    }

    public class Notification
    {
        public string UniqueId { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Cc { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Url { get; set; }
        public string Mobile { get; set; }
        public List<string> Happen { get; set; }
    }

}

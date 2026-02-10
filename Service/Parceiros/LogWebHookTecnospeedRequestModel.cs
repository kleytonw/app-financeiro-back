using Org.BouncyCastle.Bcpg;

namespace ERP_API.Service.Parceiros
{
    public class LogWebHookTecnospeedRequestModel
    {
        public string Happen {  get; set; }
        public string Balance { get; set; }
        public string UniqueId { get; set; }
        public string CreatedAt { get; set; }
        public string AccountHash { get; set; }
    }
}

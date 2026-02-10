namespace ERP_API.Service.Parceiros
{
    public class LoginStoneResquestModel
    {
        public string ClientApplicationKey { get; set; }
        public string ClientEncryptionString { get; set; }
        public string SecretKey { get; set; }
        public string ReferenceDate { get; set; }
        public string StoneCode { get; set; }
    }
}

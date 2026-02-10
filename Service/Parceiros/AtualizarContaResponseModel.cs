namespace ERP_API.Service.Parceiros
{
    public class AtualizarContaResponseModel
    {
        public string BankCode { get; set; }
        public string AccountHash { get; set; }
        public string Agency { get; set; }
        public string AgencyDigit { get; set; }
        public string AccountNumber { get; set; }
        public string AccountNumberDigit { get; set; }
        public string ConvenioAgency { get; set; }
        public string ConvenioNumber { get; set; }
        public decimal RemessaSequential { get; set; }
    }
}

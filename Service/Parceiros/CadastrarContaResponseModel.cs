using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class CadastrarContaListResponseModel
    {
        public List<CadastrarContaResponseModel> accounts { get; set; }
    }
    public class CadastrarContaResponseModel
    {
        public string BankCode { get; set; }
        public string AccountHash { get; set; }
        public string Agency { get; set; }
        public string AgencyDigit { get; set; }
        public string AccountNumber { get; set; }
        public string AccountNumberDigit { get; set; }
        public string ConventionAgency { get; set; }
        public string ConventionNumber { get; set; }
        public int RemessaSequential { get; set; }
        public bool AccountsPayment { get; set; }
    }
}

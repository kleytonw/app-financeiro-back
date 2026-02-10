using System.Collections.Generic;
using System;

namespace ERP_API.Service.Parceiros
{
    public class BilhetagemTecnospeedResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string CnpjSh { get; set; }
        public int TotalPayments { get; set; }
        public int TotalVan { get; set; }
        public List<Payer> Payers { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }

    public class Payer
    {
        public int QtdePayments { get; set; }
        public int QtdeVan { get; set; }
        public string CpfCnpj { get; set; }
        public List<Payment> Payments { get; set; }
    }

    public class Payment
    {
        public string UniqueId { get; set; }
        public string AccountHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

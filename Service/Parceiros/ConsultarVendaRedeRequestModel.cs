namespace ERP_API.Service.Parceiros
{
    public class ConsultarVendaRedeRequestModel
    {
        public string Authorization { get; set; } 
        public string ParentCompanyNumber { get; set; } 
        public string ParentMerchantId { get; set; }
        public string Subsidiaries { get; set; } 
        public string StartDate { get; set; } 
        public string EndDate { get; set; } 
        public string StatusType { get; set; }
        public string ModalityProducts { get; set; }
        public string Url { get; set; }
        public int? Brands { get; set; } 
        public string Modalities { get; set; } 
        public string Status { get; set; } 
        public int? Size { get; set; } 
        public string PageKey { get; set; } 
    }
}

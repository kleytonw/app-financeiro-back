using System;

namespace ERP.Models
{
    public class NewsletterResponse
    {
        public int IdNewsletter { get; set; }
        public int? IdEmpresa { get; set; }
        public string Email { get; set; }
        public DateTime? Data { get; set; }
        public string Situacao { get; set; }
    }

    public class NewsletterRequest
    {
        public int IdNewsletter { get; set; }
        public int? IdEmpresa { get; set; }
        public string Email { get; set; }
        public DateTime? Data { get; set; }
        public string Situacao { get; set; }
    }
}


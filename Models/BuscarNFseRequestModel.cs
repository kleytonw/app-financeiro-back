using System;

namespace ERP_API.Models
{
    public class BuscarNfseRequestModel
    {
        public string NumeroInicial { get; set; }
        public string NumeroFinal { get; set; }
        public string Serie { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? CancelInicial { get; set; }
        public DateTime? CancelFinal { get; set; }
    }
}

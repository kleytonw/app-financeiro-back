using System;

namespace ERP_API.Service.Parceiros
{
    public class FiltroCobrancaRequest
    {
        public string status { get; set; }
        public string tipoPeriodo { get; set; }
        public string identificador { get; set; }
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
    }
}

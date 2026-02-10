using System;

namespace ERP_API.Models.BI2.Filtros
{
    public class FiltroBIModel
    {
        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public int IdCliente { get; set; }
    }

    public class FiltroBIMovModel
    {
        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public int IdCliente { get; set; }

        public string Granularidade { get; set; }
    }
}

using System;

namespace ERP_API.Models.DashboardTransacao
{
    public class FiltroDashboardTransacaoRequestModel
    {
        public int IdUnidade { get; set; }

        public int IdBandeira { get; set; }

        public int IdOperadora { get; set;  }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }
    }
}

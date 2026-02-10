using System;

namespace ERP_API.Models.BI
{
    public class BIRequestFaturamentoRegiaoModel
    {
        public int IdEmpresa { get; set; }

        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}

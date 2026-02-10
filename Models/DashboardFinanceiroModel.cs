using System;

namespace ERP_API.Models
{
    public class DashboardFinanceiroPlanoContaResponse
    {
        public string Descricao { get; set; }
        public decimal Total { get; set; }

    }

    public class DashboardFinanceiroCustoFixoVariavelResponse
    {
        public string Tipo { get; set; }
        public decimal Total { get; set; }
    }

    public class DashboardFinanceiroPesquisaRequest
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class DashboardFinanceiroContasPagarReceberResponse
    {
        public string MesAno { get; set; }  // Mudei de DateTime para string
        public decimal ContasPagar { get; set; }
        public decimal ContasReceber { get; set; }
    }
}

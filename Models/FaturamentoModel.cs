namespace ERP_API.Models
{
    public class FaturamentoRequest
    {
        public int IdFaturamento { get; set; }
        public int IdCliente { get; set; }
        public int IdFinanceiro { get; set; }
        public int NumeroVendas { get; set; }
        public decimal TotalVendas { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public decimal ValorMensalidade { get; set; }
        public string Situacao { get; set; }
    }

    public class FaturamentoResponse
    {
        public int IdFaturamento { get; set; }
        public int IdCliente { get; set; }
        public int IdFinanceiro { get; set; }
        public int NumeroVendas { get; set; }
        public decimal TotalVendas { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public decimal ValorMensalidade { get; set; }
        public string Situacao { get; set; }
    }

    public class PesquisarFaturamentoRequestModel
    {
        public int? IdCliente { get; set; }
        public string Nome { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
    }

    public class SalvarFaturamento
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
    }
}

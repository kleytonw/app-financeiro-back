namespace ERP.Models
{
    public class PlanoResponse
    {
        public int IdPlano { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAdesao { get; set; }
        public decimal ValorRepasse { get; set; }
        public string Descricao { get; set; }
        public int? QuantidadeVendasInicial { get; set; }
        public int? QuantidadeVendasFinal { get; set; }
        public string Situacao { get; set; }
    }

    public class PlanoRequest
    {
        public int IdPlano { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAdesao { get; set; }
        public decimal ValorRepasse { get; set; }
        public string Descricao { get; set; }
        public int? QuantidadeVendasInicial { get; set; }
        public int? QuantidadeVendasFinal { get; set; }
        public string? Situacao { get; set; }
    }
}
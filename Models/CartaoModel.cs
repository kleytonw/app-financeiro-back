namespace ERP_API.Models
{
    public class CartaoRequest
    {
        public int IdCartao { get; set; }
        public string Nome { get; set; }
        public string Bandeira { get; set; }
        public string UltimosDigitos { get; set; }
        public int DiaFechamento { get; set; }
        public int DiaVencimento { get; set; }
        public decimal LimiteTotal { get; set; }
        public string Situacao { get; set; }
    }

    public class CartaoResponse
    {
        public int IdCartao { get; set; }
        public string Nome { get; set; }
        public string Bandeira { get; set; }
        public string UltimosDigitos { get; set; }
        public int DiaFechamento { get; set; }
        public int DiaVencimento { get; set; }
        public decimal LimiteTotal { get; set; }
        public string Situacao { get; set; }
    }
}

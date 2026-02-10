namespace ERP.Models
{
    public class ServicoResponse
    {
        public int IdServico { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }

    public class ServicoRequest
    {
        public int IdServico { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}


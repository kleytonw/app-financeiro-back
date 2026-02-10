namespace ERP_API.Models
{
    public class ClienteParametrosRequest
    {
        public int IdClienteParametros { get; set; }
        public int IdCliente { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string Situacao { get; set; }
    }

    public class ClienteParametrosResponse
    {
        public int IdClienteParametros { get; set; }
        public int IdCliente { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string NomeCliente { get; set; }
        public string Situacao { get; set; }
    }
}

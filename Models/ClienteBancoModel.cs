namespace ERP_API.Models
{
    public class ClienteBancoRequest
    {
        public int IdClienteBanco { get; set; }
        public int IdBanco { get; set; }
        public int IdCliente { get; set; }
        public string Situacao { get; set; }
    }
    public class ClienteBancoResponse
    {
        public int IdClienteBanco { get; set; }
        public int IdBanco { get; set; }
        public string NomeBanco { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Situacao { get; set; }
    }
}

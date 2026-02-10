namespace ERP_API.Models
{
    public class ClienteAdquirenteRequest
    {
        public int IdClienteAdquirente { get; set; }
        public int IdOperadora { get; set; }
        public int IdCliente { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Arquivo { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }

    }

    public class ClienteAdquirenteResponse
    {
        public int IdClienteAdquirente { get; set; }
        public int IdOperadora { get; set; }
        public string NomeOperadora { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Situacao { get; set; }
    }
}

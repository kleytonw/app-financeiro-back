namespace ERP_API.Models
{
    public class ERPClienteRequest
    {
        public int IdClienteERP { get; set; }
        public int IdCliente { get; set; }
        public int IdERPs { get; set; }
        public string Situacao { get; set; }
    }

    public class ERPClienteResponse
    {
        public int IdClienteERP { get; set; }
        public int IdCliente { get; set; }
        public int IdERPs { get; set; }
        public string NomeCliente { get; set; }
        public string NomeERP { get; set; }
        public string Situacao { get; set; }
    }
}

namespace ERP_API.Models
{
    public class UsuarioClienteRequest
    {
        public int IdUsuarioCliente {  get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string Situacao {  get; set; }

    }

    public class UsuarioClienteResponse
    {
        public int IdUsuarioCliente { get; set; }
        public int IdCliente { set; get; }
        public string NomeCliente { get; set; }
        public int IdUsuario { set; get; }
        public string NomeUsuario { get; set; }
        public string Situacao { get; set; }

    }
}

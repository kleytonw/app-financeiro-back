using System;

namespace ERP_API.Models
{
    public class ClienteContatoRequest
    {
        public int IdClienteContato { get; set; }
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
    }
    public class ClienteContatoResponse
    {
        public int IdClienteContato { get; set; }
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
    }
}

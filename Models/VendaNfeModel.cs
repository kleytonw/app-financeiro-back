using System;

namespace ERP_API.Models
{
    public class VendaNfeRequest
    {
        public int IdVendaNfe { get; set; }
        public int IdCliente { get; set; }
        public string Senha { get; set; }
        public DateTime DataVenda { get; set; }
        public int Modelo { get; set; }
        public string Arquivo { get; set; }
        public string Situacao { get; set; }
    }

    public class VendaNfeResponse
    {
        public int IdVendaNfe { get; set; }
        public int IdCliente { get; set; }
        public string Senha { get; set; }
        public DateTime DataVenda { get; set; }
        public int Modelo { get; set; }
        public string Arquivo { get; set; }
        public string Situacao { get; set; }
        public string Mensagem { get; set; }
    }

    public class PesquisarVendaNfeRequest
    {
        public string NomeCliente { get; set; }
        public string OpcaoBusca { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
    }
}

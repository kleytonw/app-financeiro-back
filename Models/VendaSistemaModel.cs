using Microsoft.AspNetCore.Http;
using System;

namespace ERP_API.Models
{
    public class VendaSistemaRequest
    {
        public int IdCliente { get; set; }
        public int IdERPs { get; set; }
        public string Senha { get; set; }
        public IFormFile Arquivo { get; set; }
    }

    public class PesquisaVendaSistemaRequest
    {
        public int IdCliente { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }

    public class VendaSistemaResponse
    {
        public int IdVendaSistema { get; set; }
        public DateTime Data { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public int IdERPs { get; set; }
        public string NomeERPs { get; set; }
        public string Arquivo { get; set; }
        public string NomeArquivo {  get; set; }
        public string Situacao { get; set; }
    }
}

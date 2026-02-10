
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace ERP_API.Models
{
    public class ClienteDocumentoRequest
    {
        public int IdClienteDocumento { get; set; }
        public int IdCliente { get; set; }
        public int IdTipoDocumento { get; set; }
        public IFormFile Arquivo { get; set; }
        public string Situacao { get; set; }
    }

    public class ClienteDocumentoResponse
    {
        public int IdClienteDocumento { get; set; }
        public int IdCliente { get; set; }
        public int IdTipoDocumento { get; set; }
        public IFormFile Arquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string Situacao { get; set; }
    }
}

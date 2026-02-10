using Microsoft.AspNetCore.Http;

namespace ERP_API.Models
{
    public class ImportarExtratoModel
    {
        public IFormFile OfxFile { get; set; }
        public int IdCliente { get; set; }
        public int IdClienteContaBancaria { get; set; }
    }
}

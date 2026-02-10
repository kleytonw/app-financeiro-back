using System.Collections.Generic;

namespace ERP_API.Models
{
    public class BuscarNFseResponseModel
    {

        public bool Sucesso { get; set; }
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public int Pagina { get; set; }

        public int TotalPaginas { get; set; }

        public List<string> Chaves { get; set; }
    }
}

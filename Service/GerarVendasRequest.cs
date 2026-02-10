using ERP_API.Service.Parceiros;
using System.Collections.Generic;

namespace ERP_API.Service
{
    public class GerarVendasRequest
    {
        public string Senha { get; set; }
        public string IdEmpresa { get; set; }
        public CabecalhoXml Cabecalho { get; set; }
        public List<RegistroVenda> Registros { get; set; }
    }
}

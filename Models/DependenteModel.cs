using ERP.Models;
using System;

namespace ERP_API.Models
{
    public class DependenteResponse
    {
        public int IdDependente { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }

    }

    public class DependenteRequest
    {
        public int IdDependente { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }

    }
}

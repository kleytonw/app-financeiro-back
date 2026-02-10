using System;

namespace ERP_API.Models
{
    public class PesquisarConsultaPagamento
    {
        public int? IdOperadora { get; set; }
        public int? IdUnidade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}

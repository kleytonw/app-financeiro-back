using System;

namespace ERP_API.Models
{
    public class PesquisarExtrato
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public int IdCliente { get; set; }
        public int IdClienteContaBancaria { get; set; }

        public bool OpenFinance { get; set; }
    }
}

using System;

namespace ERP.Models.Relatorio
{
    public class RelClienteEcomerce
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
    }

    public class FiltroClienteEcomerce
    {
        public int IdEmpresa { get; set;}
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}

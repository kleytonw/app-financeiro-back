using System;

namespace ERP.Models.Relatorio
{
    public class FiltroExcelPedidoEcomerce
    {
        public int IdEmpresa { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string? Situacao { get; set; }
        public string? Tipo { get; set; }
    }
}

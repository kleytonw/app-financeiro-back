using System;

namespace ERP_API.Service.Parceiros
{
    public class ConsultaPagBankRequestModel
    {
        public string Token { get; set; }
        public string User { get; set; }
        public DateTime DataConsulta { get; set; }
        /// <summary>
        /// Número da página
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Quantidade de registros por página
        /// </summary>
        public int PageSize { get; set; }
    }
}

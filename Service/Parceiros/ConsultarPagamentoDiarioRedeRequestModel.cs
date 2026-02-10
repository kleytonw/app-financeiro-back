using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ERP_API.Service.Parceiros
{
    public class ConsultarPagamentoDiarioRedeRequestModel
    {
        /// <summary>
        /// Token de acesso ao serviço (Authorization Header)
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// Data de início da consulta no formato yyyy-MM-dd
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Data de fim da consulta no formato yyyy-MM-dd
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        ///  Url de chamada da requisição 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Número do Ponto de Venda (PV)
        /// </summary>
        public int ParentCompanyNumber { get; set; }

        /// <summary>
        /// Código do Status do pagamento para filtro (Opcional)
        /// </summary>
        public int? StatusCodes { get; set; }

        /// <summary>
        /// Status do pagamento (Opcional)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Tipo do pagamento (Opcional)
        /// </summary>
        public string Types { get; set; }

        /// <summary>
        /// Lista de códigos bancários que serão filtrados (Opcional)
        /// </summary>
        public string BankAccounts { get; set; }

        /// <summary>
        /// Código da bandeira de pagamento (Opcional)
        /// </summary>
        public int? Brands { get; set; }

        /// <summary>
        /// Lista de IDs de pagamentos que serão filtrados (Opcional)
        /// </summary>
        public string PaymentIds { get; set; }

        /// <summary>
        /// Quantidade de pagamentos a serem retornados (Opcional)
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Chave para paginação (Opcional)
        /// </summary>
        public string PageKey { get; set; }
    }

}

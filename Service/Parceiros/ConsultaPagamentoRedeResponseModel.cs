using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ConsultaPagamentoRedeResponseModel
    {
        /// <summary>
        /// Lista de pagamentos retornados na consulta.
        /// </summary>
        public ContentResponse Content { get; set; }

        /// <summary>
        /// Informações sobre a paginação dos resultados.
        /// </summary>
        public CursorResponse Cursor { get; set; }
    }

    public class ContentResponse
    {
        /// <summary>
        /// Lista de pagamentos.
        /// </summary>
        public List<PaymentResponse> Payments { get; set; }
    }

    public class PaymentResponse
    {
        /// <summary>
        /// Identificador único do pagamento.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Data do pagamento (formato yyyy-MM-dd).
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// Código do banco onde foi efetuado o pagamento.
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// Código da agência bancária.
        /// </summary>
        public int BankBranchCode { get; set; }

        /// <summary>
        /// Número da conta bancária.
        /// </summary>
        public int AccountNumber { get; set; }

        /// <summary>
        /// Código da bandeira de pagamento.
        /// </summary>
        public int BrandCode { get; set; }

        /// <summary>
        /// Número da empresa responsável pelo pagamento.
        /// </summary>
        public string CompanyNumber { get; set; }

        /// <summary>
        /// Número do documento da empresa.
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Nome da empresa.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Nome fantasia da empresa.
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// Valor líquido do pagamento.
        /// </summary>
        public decimal? NetAmount { get; set; }

        /// <summary>
        /// Status do pagamento.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Código do status do pagamento.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Tipo de pagamento (ex: CREDIT).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Código do tipo de pagamento.
        /// </summary>
        public string TypeCode { get; set; }
    }

    public class CursorResponse
    {
        /// <summary>
        /// Indica se há mais páginas disponíveis.
        /// </summary>
        public bool HasNextKey { get; set; }

        /// <summary>
        /// Chave da próxima página para paginação.
        /// </summary>
        public string NextKey { get; set; }
    }

}

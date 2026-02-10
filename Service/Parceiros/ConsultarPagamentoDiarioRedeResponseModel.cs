using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ConsultarPagamentoDiarioRedeResponseModel
    {
        /// <summary>
        /// Conteúdo da resposta, contendo pagamentos diários.
        /// </summary>
        public ContentResponseDaily Content { get; set; }
    }

    public class ContentResponseDaily
    {
        /// <summary>
        /// Lista de pagamentos diários.
        /// </summary>
        public List<PaymentsDailyResponse> PaymentsDaily { get; set; }
    }

    public class PaymentsDailyResponse
    {
        /// <summary>
        /// Data do pagamento.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Quantidade de pagamentos no dia.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Valor líquido total do pagamento.
        /// </summary>
        public decimal NetAmount { get; set; }

        /// <summary>
        /// Resumo de pagamentos pagos.
        /// </summary>
        public PaymentSummary Paid { get; set; }

        /// <summary>
        /// Resumo de pagamentos pendentes.
        /// </summary>
        public PaymentSummary Pending { get; set; }

        /// <summary>
        /// Resumo de pagamentos suspensos.
        /// </summary>
        public PaymentSummary Suspended { get; set; }

        /// <summary>
        /// Resumo de pagamentos rejeitados.
        /// </summary>
        public PaymentSummary Rejected { get; set; }

        /// <summary>
        /// Lista de pagamentos detalhados.
        /// </summary>
        public List<PaymentResponseDaily> Payments { get; set; }
    }

    public class PaymentSummary
    {
        /// <summary>
        /// Quantidade de pagamentos no status específico.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Valor líquido dos pagamentos no status específico.
        /// </summary>
        public decimal NetAmount { get; set; }
    }

    public class PaymentResponseDaily
    {
        /// <summary>
        /// Identificador único do pagamento.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Data do pagamento.
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
        /// Código da bandeira do pagamento.
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
        public decimal NetAmount { get; set; }

        /// <summary>
        /// Status do pagamento.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Código do status do pagamento.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Tipo de pagamento.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Código do tipo de pagamento.
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// Data de bloqueio do pagamento.
        /// </summary>
        public string BlockDate { get; set; }

        /// <summary>
        /// Lista de cobranças associadas ao pagamento.
        /// </summary>
        public List<ChargeResponse> Charges { get; set; }

        /// <summary>
        /// Indica se o valor foi atualizado.
        /// </summary>
        public bool HasUpdatedValue { get; set; }

        /// <summary>
        /// Identificador do pedido de crédito original.
        /// </summary>
        public string CreditOrderParent { get; set; }

        /// <summary>
        /// Origem do pedido de crédito.
        /// </summary>
        public string CreditOrderOrigin { get; set; }

        /// <summary>
        /// Nome do cessionário.
        /// </summary>
        public string TransfereeName { get; set; }

        /// <summary>
        /// Documento do cessionário.
        /// </summary>
        public string TransfereeDocument { get; set; }

        /// <summary>
        /// Tipo de pagamento.
        /// </summary>
        public string TypePayment { get; set; }

        /// <summary>
        /// Tipo de negociação.
        /// </summary>
        public string TypeNegotiation { get; set; }

        /// <summary>
        /// Número de depósitos relacionados ao pagamento.
        /// </summary>
        public int DepositCount { get; set; }

        /// <summary>
        /// Número do contrato interno.
        /// </summary>
        public string InternalContractNumber { get; set; }

        /// <summary>
        /// Código do cliente cessionário.
        /// </summary>
        public string TransfereeCustomerCode { get; set; }

        /// <summary>
        /// Lista de negociações associadas ao pagamento.
        /// </summary>
        public List<NegotiationResponse> Negotiations { get; set; }
    }

    public class ChargeResponse
    {
        /// <summary>
        /// Data e hora da ocorrência da cobrança.
        /// </summary>
        public string TimestampOc { get; set; }

        /// <summary>
        /// Valor da cobrança.
        /// </summary>
        public decimal Amount { get; set; }
    }

    public class NegotiationResponse
    {
        /// <summary>
        /// Identificador único do pagamento na negociação.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Data do pagamento na negociação.
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// Código do banco na negociação.
        /// </

    }
}

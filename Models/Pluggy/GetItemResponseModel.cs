using System.Collections.Generic;
using System;

namespace ERP_API.Models.Pluggy
{
    public class GetItemResponseModel
    {
        public string Id { get; set; }
        public ConnectorModel Connector { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; }
        public string ExecutionStatus { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string WebhookUrl { get; set; }
        public PluggyError Error { get; set; }
        public string ClientUserId { get; set; }
        public int ConsecutiveFailedLoginAttempts { get; set; }
        public string StatusDetail { get; set; }

        public ItemParameterModel Parameter { get; set; }

        public string UserAction { get; set; }
        public DateTime? NextAutoSyncAt { get; set; }
        public DateTime? AutoSyncDisabledAt { get; set; }
        public DateTime? ConsentExpiresAt { get; set; }
        public List<string> Products { get; set; }
        public string OauthRedirectUri { get; set; }
    }

    public class PluggyError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }


    public class ConnectorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PrimaryColor { get; set; }
        public string InstitutionUrl { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public List<ConnectorCredentialModel> Credentials { get; set; }
        public string ImageUrl { get; set; }
        public bool HasMFA { get; set; }
        public bool Oauth { get; set; }
        public ConnectorHealthModel Health { get; set; }
        public List<string> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSandbox { get; set; }
        public bool IsOpenFinance { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool SupportsPaymentInitiation { get; set; }
        public bool SupportsScheduledPayments { get; set; }
        public bool SupportsSmartTransfers { get; set; }
        public bool SupportsBoletoManagement { get; set; }
    }

    public class ConnectorCredentialModel
    {
        public string Validation { get; set; }
        public string ValidationMessage { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Placeholder { get; set; }
        public bool Optional { get; set; }
    }

    public class ConnectorHealthModel
    {
        public string Status { get; set; }
        public string Stage { get; set; }
    }

    public class ItemParameterModel
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Instructions { get; set; }
        public string Data { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class ListaContasResponse
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<ContaResponse> Results { get; set; } = new();
    }

    public class ContaResponse
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Tipo da conta (ex: BANK)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Subtipo (ex: CHECKING_ACCOUNT)
        /// </summary>
        public string Subtype { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string CurrencyCode { get; set; }

        public Guid ItemId { get; set; }

        public string Number { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? MarketingName { get; set; }

        public string TaxNumber { get; set; }

        public string? Owner { get; set; }

        public DadosBancariosResponse BankData { get; set; }

        public DadosCreditoResponse? CreditData { get; set; }
    }

    public class DadosBancariosResponse
    {
        public string TransferNumber { get; set; }

        public decimal ClosingBalance { get; set; }

        public decimal AutomaticallyInvestedBalance { get; set; }

        public decimal OverdraftContractedLimit { get; set; }

        public decimal OverdraftUsedLimit { get; set; }

        public decimal UnarrangedOverdraftAmount { get; set; }
    }

    public class DadosCreditoResponse
    {
        public decimal? CreditLimit { get; set; }
        public decimal? UsedCredit { get; set; }
        public decimal? AvailableCredit { get; set; }
    }
}

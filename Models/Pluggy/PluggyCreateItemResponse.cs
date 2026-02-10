using System.Collections.Generic;
using System;

public class PluggyConnectResponse
{
    public Guid Id { get; set; }

    public int? ConnectorId { get; set; }
    public PluggyConnector Connector { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; } = null!;
    public string ExecutionStatus { get; set; } = null!;

    public DateTime? LastUpdatedAt { get; set; }

    public string? WebhookUrl { get; set; }
    public PluggyItemError? Error { get; set; }

    public string? ClientUserId { get; set; }
    public int ConsecutiveFailedLoginAttempts { get; set; }

    public string? StatusDetail { get; set; }

    public PluggyItemParameter? Parameter { get; set; }
    public PluggyItemUserAction? UserAction { get; set; }

    public DateTime? NextAutoSyncAt { get; set; }
    public DateTime? AutoSyncDisabledAt { get; set; }
    public DateTime? ConsentExpiresAt { get; set; }

    // JSON array
    public List<string> Products { get; set; } = new();

    public string? OauthRedirectUri { get; set; }
}

public class PluggyConnector
{
    public int Id { get; set; } // connector.id

    public string Name { get; set; } = null!;
    public string? PrimaryColor { get; set; }
    public string? InstitutionUrl { get; set; }
    public string? Country { get; set; }
    public string? Type { get; set; }

    // JSON array
    public List<PluggyCredential> Credentials { get; set; } = new();

    public string? ImageUrl { get; set; }
    public bool HasMfa { get; set; }
    public bool Oauth { get; set; }

    public PluggyConnectorHealth? Health { get; set; }

    // JSON array
    public List<string> Products { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public bool IsSandbox { get; set; }
    public bool IsOpenFinance { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool SupportsPaymentInitiation { get; set; }
    public bool SupportsScheduledPayments { get; set; }
    public bool SupportsSmartTransfers { get; set; }
    public bool SupportsBoletoManagement { get; set; }
}

public class PluggyCredential
{
    public string? Validation { get; set; }
    public string? ValidationMessage { get; set; }
    public string? Label { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Placeholder { get; set; }
    public bool Optional { get; set; }
}

public class PluggyConnectorHealth
{
    public string? Status { get; set; }
    public string? Stage { get; set; }
}

public class PluggyItemParameter
{
    public string? Label { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Instructions { get; set; }
    public string? Data { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

// Se hoje vem null, deixa pronto pra quando vier objeto
public class PluggyItemError
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public string? ProviderMessage { get; set; }
}

public class PluggyItemUserAction
{
    public string? Type { get; set; }
    public string? Message { get; set; }
}
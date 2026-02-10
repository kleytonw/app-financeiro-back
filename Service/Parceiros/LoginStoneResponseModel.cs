using System.Globalization;
using System;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Reflection;

namespace ERP_API.Service.Parceiros
{
    public class LoginStoneResponseModel
    {
        [XmlElement("Header")]
        public HeaderInfo Header { get; set; }

        [XmlElement("FinancialTransactions")]
        public FinancialTransactionsInfo FinancialTransactions { get; set; }

        [XmlElement("FinancialEvents")]
        public FinancialEventsInfo FinancialEvents { get; set; }

        [XmlElement("FinancialTransactionsAccounts")]
        public FinancialTransactionsAccountsInfo FinancialTransactionsAccounts { get; set; }

        [XmlElement("FinancialEventAccounts")]
        public FinancialEventAccountsInfo FinancialEventAccounts { get; set; }

        [XmlElement("Payments")]
        public PaymentsInfo Payments { get; set; }

        [XmlElement("Trailer")]
        public TrailerInfo Trailer { get; set; }
    }

    public class HeaderInfo
    {
        [XmlElement("GenerationDateTime")]
        public string GenerationDateTime { get; set; }

        [XmlIgnore]
        public DateTime GenerationDateTimeValue
        {
            get
            {
                return DateTime.ParseExact(
                    GenerationDateTime,
                    "yyyyMMddHHmmss",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("StoneCode")]
        public long StoneCode { get; set; }

        [XmlElement("LayoutVersion")]
        public int LayoutVersion { get; set; }

        [XmlElement("FileId")]
        public long FileId { get; set; }

        [XmlElement("ReferenceDate")]
        public string ReferenceDate { get; set; }
    }

    public class TrailerInfo
    {
        [XmlElement("CapturedTransactionsQuantity")]
        public int CapturedTransactionsQuantity { get; set; }

        [XmlElement("CanceledTransactionsQuantity")]
        public int CanceledTransactionsQuantity { get; set; }

        [XmlElement("PaidInstallmentsQuantity")]
        public int PaidInstallmentsQuantity { get; set; }

        [XmlElement("ChargedCancellationsQuantity")]
        public int ChargedCancellationsQuantity { get; set; }

        [XmlElement("ChargebacksQuantity")]
        public int ChargebacksQuantity { get; set; }

        [XmlElement("ChargebacksRefundQuantity")]
        public int ChargebacksRefundQuantity { get; set; }

        [XmlElement("ChargedChargebacksQuantity")]
        public int ChargedChargebacksQuantity { get; set; }

        [XmlElement("PaidChargebacksRefundQuantity")]
        public int PaidChargebacksRefundQuantity { get; set; }

        [XmlElement("PaidEventsQuantity")]
        public int PaidEventsQuantity { get; set; }

        [XmlElement("ChargedEventsQuantity")]
        public int ChargedEventsQuantity { get; set; }
    }

    public class FinancialTransactionsInfo 
    {
        [XmlElement("Transaction")]
        public List<TransactionInfo> Transactions { get; set; }

        [XmlElement("Events")]
        public List<EventInfo> Events { get; set; }

        [XmlElement("POI")]
        public List<POIInfo> POIs { get; set; }

        [XmlElement("FeeType")]
        public List<FeeTypeInfo> FeeTypes { get; set; }

        [XmlElement("Cancellations")]
        public List<CancellationInfo> Cancellations { get; set; }

        [XmlElement("Billing")]
        public List<BillingInfo> Billings { get; set; }

        [XmlElement("Installments")]
        public List<InstallmentInfo> Installments { get; set; }

        [XmlElement("Chargeback")]
        public List<ChargebackInfo> Chargebacks { get; set; }

        [XmlElement("ChargebackRefund")]
        public List<ChargebackRefundInfo> ChargebackRefunds { get; set; }
    }

    public class TransactionInfo 
    {
        [XmlElement("Events")]
        public string Events { get; set; }

        [XmlElement("AcquirerTransactionKey")]
        public string AcquirerTransactionKey { get; set; }

        [XmlElement("InitiatorTransactionKey")]
        public string InitiatorTransactionKey { get; set; }

        [XmlElement("AuthorizationDateTime")]
        public string AuthorizationDateTime { get; set; }

        [XmlIgnore]
        public DateTime AuthorizationDateTimeValue
        {
            get
            {
                return DateTime.ParseExact(
                    AuthorizationDateTime,
                    "yyyyMMddHHmmss",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("CaptureLocalDateTime")]
        public string CaptureLocalDateTime { get; set; }

        [XmlIgnore]
        public DateTime CaptureLocalDateTimeValue
        {
            get
            {
                return DateTime.ParseExact(
                    CaptureLocalDateTime,
                    "yyyyMMddHHmmss",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("International")]
        public bool International { get; set; }

        [XmlElement("AccountType")]
        public string AccountType { get; set; }

        [XmlElement("InstallmentType")]
        public string InstallmentType { get; set; }

        [XmlElement("NumberOfInstallments")]
        public int NumberOfInstallments { get; set; }

        [XmlElement("AuthorizedAmount")]
        public decimal AuthorizedAmount { get; set; }

        [XmlElement("CapturedAmount")]
        public decimal CapturedAmount { get; set; }

        [XmlElement("CanceledAmount")]
        public decimal CanceledAmount { get; set; }

        [XmlElement("AuthorizationCurrencyCode")]
        public int AuthorizationCurrencyCode { get; set; }

        [XmlElement("IssuerAuthorizationCode")]
        public string IssuerAuthorizationCode { get; set; }

        [XmlElement("BrandId")]
        public int BrandId { get; set; }

        [XmlElement("CardNumber")]
        public string CardNumber { get; set; }

        [XmlElement("EntryMode")]
        public int EntryMode { get; set; }

        [XmlElement("Poi")]
        public POIInfo Poi { get; set; }

        [XmlElement("Cancellations")]
        public CancellationInfo Cancellations { get; set; }

        [XmlElement("Installments")]
        public InstallmentInfo Installments { get; set; }
    }
    public class EventInfo
    {
        [XmlElement("CancellationCharges")]
        public int CancellationCharges { get; set; }

        [XmlElement("Cancellation")]
        public int Cancellation { get; set; }

        [XmlElement("Captures")]
        public int Captures { get; set; }

        [XmlElement("ChargebackRefunds")]
        public int ChargebackRefunds { get; set; }

        [XmlElement("Chargebacks")]
        public int Chargebacks { get; set; }

        [XmlElement("Payments")]
        public int Payments { get; set; }
    }
    public class POIInfo
    {
        [XmlElement("PosType")]
        public int PosType { get; set; }

        [XmlElement("SerialNumber")]
        public string SerialNumber { get; set; }
    }
    public class FeeTypeInfo { }
    public class CancellationInfo 
    {
        [XmlElement("PaymentId")]
        public long PaymentId { get; set; }

        [XmlElement("OperationKey")]
        public string OperationKey { get; set; }

        [XmlElement("InstallmentNumber")]
        public int InstallmentNumber { get; set; }

        [XmlElement("CancellationDateTime")]
        public string CancellationDateTime { get; set; }

        [XmlIgnore]
        public DateTime CancellationDateTimeValue
        {
            get
            {
                return DateTime.ParseExact(
                    CancellationDateTime,
                    "yyyyMMddHHmmss",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("ReturnedAmount")]
        public decimal ReturnedAmount { get; set; }

        [XmlElement("Billing")]
        public BillingInfo Billing { get; set; }

    }
    public class BillingInfo 
    {
        [XmlElement("ChargedAmount")]
        public decimal ChargedAmount { get; set; }

        [XmlElement("PrevisionChargeDate")]
        public string PrevisionChargeDate { get; set; }

        [XmlIgnore]
        public DateTime PrevisionChargeDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    PrevisionChargeDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        public string ChargeDate { get; set; }

        public DateTime ChargeDDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    ChargeDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }
    }
    public class InstallmentInfo 
    {
        [XmlElement("InstallmentNumber")]
        public int InstallmentNumber { get; set; }

        [XmlElement("GrossAmount")]
        public decimal GrossAmount { get; set; }

        [XmlElement("NetAmount")]
        public decimal NetAmount { get; set; }

        [XmlElement("PrevisionPaymentDate")]
        public string PrevisionPaymentDate { get; set; }

        [XmlIgnore]
        public DateTime PrevisionPaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    PrevisionPaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("SaleFee")]
        public decimal SaleFee { get; set; }

        [XmlElement("MdrAmount")]
        public decimal MdrAmount { get; set; }

        [XmlElement("OriginalPaymentDate")]
        public string OriginalPaymentDate { get; set; }

        [XmlIgnore]
        public DateTime OriginalPaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    OriginalPaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("PaymentDate")]
        public string PaymentDate { get; set; }

        [XmlIgnore]
        public DateTime PaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    PaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("AdvanceRateAmount")]
        public decimal AdvanceRateAmount { get; set; }
        [XmlElement("AdvancedReceivableOriginalPaymentDate")]
        public string AdvancedReceivableOriginalPaymentDate { get; set; }

        [XmlIgnore]
        public DateTime AdvancedReceivableOriginalPaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    AdvancedReceivableOriginalPaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("PaymentId")]
        public long PaymentId { get; set; }

        [XmlElement("SuspendedByChargeback")]
        public bool SuspendedByChargeback { get; set; }

        [XmlElement("Chargeback")]
        public ChargebackInfo Chargeback { get; set; }

        [XmlElement("ChargebackRefund")]
        public ChargebackRefundInfo ChargebackRefund { get; set; }

    }
    public class ChargebackInfo
    {
        [XmlElement("PaymentId")]
        public long PaymentId { get; set; }

        [XmlElement("Id")]
        public long Id { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlIgnore]
        public DateTime DateValue
        {
            get
            {
                return DateTime.ParseExact(
                    Date,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("ChargeDate")]
        public string ChargeDate { get; set; }

        [XmlIgnore]
        public DateTime ChargeDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    ChargeDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("ReasonCode")]
        public int ReasonCode { get; set; }
    }
    public class ChargebackRefundInfo
    {
        [XmlElement("PaymentId")]
        public long PaymentId { get; set; }

        [XmlElement("Id")]
        public long Id { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlIgnore]
        public DateTime DateValue
        {
            get
            {
                return DateTime.ParseExact(
                    Date,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("PaymentDate")]
        public string PaymentDate { get; set; }

        [XmlIgnore]
        public DateTime PaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    PaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("ReasonCode")]
        public int? ReasonCode { get; set; }

    }
    public class FinancialEventsInfo 
    {
        [XmlElement("Event")]
        public List<FinancialEventInfo> Events { get; set; }
    }

    public class FinancialEventInfo
    {
        [XmlElement("PaymentId")]
        public long PaymentId { get; set; }

        [XmlElement("EventId")]
        public string EventId { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Type")]
        public int Type { get; set; }

        [XmlElement("PrevisionPaymentDate")]
        public string PrevisionPaymentDate { get; set; }

        [XmlIgnore]
        public DateTime PrevisionPaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    PrevisionPaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("LinkedMerchant")]
        public long LinkedMerchant { get; set; }
    }

    public class FinancialEventsAccountsInfo
    {
        [XmlElement("Events")]
        public List<FinancialEventAccountInfo> Events { get; set; }
    }

    public class FinancialEventAccountInfo
    {
        [XmlElement("EventId")]
        public string EventId { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Type")]
        public int Type { get; set; }

        [XmlElement("PaymentDate")]
        public string PaymentDate { get; set; }

        [XmlIgnore]
        public DateTime PaymentDateValue
        {
            get
            {
                return DateTime.ParseExact(
                    PaymentDate,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            }
        }

        [XmlElement("LinkedMerchant")]
        public long LinkedMerchant { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }
    }
    public class FinancialTransactionsAccountsInfo
    {
        [XmlElement("Transaction")]
        public List<TransactionInfo> Transactions { get; set; }

        [XmlElement("FeeType")]
        public List<FeeTypeInfo> FeeTypes { get; set; }

        [XmlElement("Cancellations")]
        public List<CancellationInfo> Cancellations { get; set; }

        [XmlElement("Billing")]
        public List<BillingInfo> Billings { get; set; }

        [XmlElement("Installments")]
        public List<InstallmentInfo> Installments { get; set; }
    }
    public class FinancialEventAccountsInfo { }
    public class PaymentsInfo
    {
        [XmlElement("Payment")]
        public List<PaymentInfo> Payments { get; set; }

        [XmlElement("FavoredBankAccount")]
        public List<FavoredBankAccountInfo> FavoredBankAccounts { get; set; }
    }

    public class PaymentInfo
    {
        [XmlElement("Id")]
        public long Id { get; set; }

        [XmlElement("WalletType")]
        public int WalletType { get; set; }

        [XmlElement("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [XmlElement("TotalFinancialAccountsAmount")]
        public decimal TotalFinancialAccountsAmount { get; set; }

        [XmlElement("LastNegativeAmount")]
        public decimal LastNegativeAmount { get; set; }

        [XmlElement("FavoredBankAccount")]
        public FavoredBankAccountInfo FavoredBankAccount { get; set; }
    }

    public class FavoredBankAccountInfo
    {
        [XmlElement("BankCode")]
        public int BankCode { get; set; }

        [XmlElement("BankBranch")]
        public string BankBranch { get; set; }

        [XmlElement("BankAccountNumber")]
        public long BankAccountNumber { get; set; }
    }
}

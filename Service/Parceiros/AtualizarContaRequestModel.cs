using System.ComponentModel.DataAnnotations;

public class AtualizarContaRequestModel
{
    [Required]
    [StringLength(3, MinimumLength = 1)]
    public string BankCode { get; set; }

    [Required]
    [StringLength(10)]
    public string Agency { get; set; }

    [StringLength(2)]
    public string AgencyDigit { get; set; }

    [Required]
    [StringLength(12)]
    public string AccountNumber { get; set; }

    [StringLength(2)]
    public string AccountNumberDigit { get; set; }

    [StringLength(2)]
    public string AccountDac { get; set; }

    public int AccountType { get; set; }

    [Range(0, 9999999999)]
    public decimal? ConvenioAgency { get; set; }

    [StringLength(20)]
    public string ConvenioNumber { get; set; }

    public decimal? RemessaSequential { get; set; }

    public bool RecipientNotification { get; set; }
}

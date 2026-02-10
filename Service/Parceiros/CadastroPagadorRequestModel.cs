using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CadastroPagadorRequestModel
{
    /// <summary>
    /// Nome da empresa
    /// </summary>
    [Required]
    [StringLength(250)]
    public string name { get; set; }

    [StringLength(250)]
    public string email { get; set; }

    [Required]
    [StringLength(18)]
    public string cpfCnpj { get; set; }

    public List<AccountModelRequest> accounts { get; set; }
    /// <summary>
    /// Define se o pagador tera serviço DDA
    /// </summary>
    public bool? ddaActivated { get; set; }

    [StringLength(250)]
    public string street { get; set; }

    /// <summary>
    /// Bairro
    /// </summary>
    [Required]
    [StringLength(250)]
    public string neighborhood { get; set; }
    /// <summary>
    /// Numero de endereço
    /// </summary>
    [Required]
    [StringLength(10)]
    public string addressNumber { get; set; }
    /// <summary>
    /// Complemento do endereço
    /// </summary>
    [StringLength(250)]
    public string addressComplement { get; set; }

    [Required]
    [StringLength(250)]
    public string city { get; set; }

    [Required]
    [StringLength(2)]
    public string state { get; set; }

    [Required]
    [StringLength(10)]
    public string zipcode { get; set; }
}

public class AccountModelRequest
{
    /// <summary>
    /// Código do Banco
    /// </summary>
    [Required]
    [StringLength(3)]
    public string bankCode { get; set; }
    /// <summary>
    /// Agência
    /// </summary>
    [Required]
    [StringLength(10)]
    public string agency { get; set; }
    /// <summary>
    /// Digito da agência
    /// </summary>
    [StringLength(2)]
    public string agencyDigit { get; set; }
    /// <summary>
    /// Numero da conta
    /// </summary>
    [Required]
    [StringLength(12)]
    public string accountNumber { get; set; }
    /// <summary>
    /// Digito da Conta
    /// </summary>
    [StringLength(2)]
    public string accountNumberDigit { get; set; }
    /// <summary>
    /// DAC da agência ou conta
    /// </summary>
    [StringLength(2)]
    public string accountDac { get; set; }

    /// <summary>
    /// Número do convênio
    /// </summary>

    [StringLength(20)]
    public string convenioAgency { get; set; }
    /// <summary>
    /// Número do convênio
    /// </summary>

    [StringLength(20)]
    public string convenioNumber { get; set; }
    /// <summary>
    /// Numero sequencial da remessa
    /// </summary>

    [Range(0, 9999999999)]
    public long? remessaSequential { get; set; }

    public int accountType { get; set; }
}


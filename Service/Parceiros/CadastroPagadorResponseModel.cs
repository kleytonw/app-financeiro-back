using System.Collections.Generic;

public class CadastroPagadorResponseModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string CpfCnpj { get; set; }
    public List<AccountModelResponse> Accounts { get; set; }
    public string Street { get; set; }
    public string Neighborhood { get; set; }
    public string AddressNumber { get; set; }
    public string AddressComplement { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Token { get; set; }
}

public class AccountModelResponse
{
    public string bankCode { get; set; }
    public string accountHash { get; set; }
    public string agency { get; set; }
    public string agencyDigit { get; set; }
    public string accountNumber { get; set; }
    public string accountNumberDigit { get; set; }
    public string accountDac { get; set; }
    public string convenioNumber { get; set; }
    public int remessaSequential { get; set; }
}
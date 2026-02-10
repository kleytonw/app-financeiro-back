using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class BuscarPagadorResponseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
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
}

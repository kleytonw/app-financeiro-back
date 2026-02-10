namespace ERP_API.Models.Pluggy
{
    public class CreateItemPessoalPluggyRequestModel
    {
        public int connectorId { get; set; }
        public PluggyParametersPessoal parameters { get; set; }
        public string clientUserId { get; set; }
    }

    public class PluggyParametersPessoal
    {
        public string cpf { get; set; }
    }


    public class CreateItemEmpresarialPluggyRequestModel
    {
        
        public int connectorId { get; set; }
        public PluggyParametersEmpresarial parameters { get; set; }
        public string clientUserId { get; set; }
    }

    public class PluggyParametersEmpresarial
    {
        public string cpf { get; set; }
        public string cnpj { get; set; }
    }
}
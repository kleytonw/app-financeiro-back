namespace ERP_Application.Models.Parceiros.IntegraNotas
{
    public class CriarEmitenteIntegraNotasRequestModel
    {
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Cnae { get; set; }
        public string Crt { get; set; }
        public string InscricaoMunicipal { get; set; }

        public string LoginPrefeitura { get; set; }
        public string SenhaPrefeitura { get; set; }
        public string ClientIdPrefeitura { get; set; }
        public string ClientSecretPrefeitura { get; set; }

        public string Telefone { get; set; }
        public string Email { get; set; }

        public EnderecoModel Endereco { get; set; }

        public string LogoBase64 { get; set; }
    }

    public class EnderecoModel
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string CodigoIbge { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
    }
}

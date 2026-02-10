namespace ERP.Models
{
    public class UnidadeResponse
    {
        public int IdUnidade { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        public string RazaoSocial { get; set; }
        public string CpfCnpj { get; set; }
        public int IdGrupoEmpresa { get; set; }
        public int? IdRegiao { get; set; }
        public int IdRamoAtividade { get; set; }
        public string NomeRegiao { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Referencia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string TokenTecnoSpeed { get; set; }
        public string UniqueId { get; set; }
        public string Situacao { get; set; }
    }

    public class UnidadeRequest
    {
        public int IdUnidade { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        public string RazaoSocial { get; set; }
        public string CpfCnpj { get; set; }
        public int? IdGrupoEmpresa { get; set; }
        public int? IdRegiao { get; set; }
        public int? IdRamoAtividade { get; set; }
        public string NomeRegiao { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Referencia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string TokenTecnoSpeed { get; set; }
        public string UniqueId { get; set; }
        public string Situacao { get; set; }
    }
}

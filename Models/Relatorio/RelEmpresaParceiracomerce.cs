using System;

namespace ERP.Models.Relatorio
{
    public class FiltroEmpresasParceiras
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }

    public class RelEmpresasParceiras
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string RzSocial { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string TelefoneCelularEmpresa { get; set; }
        public string InscEstadual { get; set; }
        public string NomeFantasia { get; set; }
        public string NomeSocios { get; set; }
        public string NomeGerencia { get; set; }
        public string EmailGerencia { get; set; }
        public string Situacao { get; set; }

        public string Cep { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
    }
}

using ERP.Models;
using System;

namespace ERP_API.Models
{
    public class VendedorResponse
    {
        public int IdPessoa { get; set; }
        public int? IdRegiao { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public string CpfCnpj { get; set; }
        public string Cep { get; set; }
        public string Sexo { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Mae { get; set; }
        public string Pai { get; set; }
        public string TipoPessoa { get; set; }

        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Situacao { get; set; }

    }

    public class VendedorRequest
    {
        public int IdPessoa { get; set; }
        public int? IdRegiao { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public string CpfCnpj { get; set; }
        public string Cep { get; set; }
        public string Sexo { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Mae { get; set; }
        public string Pai { get; set; }
        public string TipoPessoa { get; set; }

        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Situacao { get; set; }

    }
}

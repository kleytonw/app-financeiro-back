using ERP.Domain;
using ERP.Domain.Entidades;
using ERP_API.Domain.Entidades;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;

namespace ERP.Models
{
    public class Pessoa : BaseModel
    {

        public int IdPessoa { get; set; }
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

        public Pessoa () { }

        public Pessoa(string nome, string sexo, DateTime? dataNascimento, string mae, string pai, string tipoPessoa, string razaoSocial, string cpfCnpj, string telefone1, string telefone2, string email, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string referencia, string inscricaoEstadual, string inscricaoMunicipal, string usuarioInclusao)
        {
            Nome = nome;
            Sexo = sexo;
            DataNascimento = dataNascimento;
            Mae = mae;
            Pai = pai;
            TipoPessoa = tipoPessoa;
            RazaoSocial = razaoSocial;
            CpfCnpj = cpfCnpj;
            Telefone1 = telefone1;
            Telefone2 = telefone2;
            Email = email;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Referencia = referencia;
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string sexo, DateTime? dataNascimento, string mae, string pai, string tipoPessoa, string razaoSocial, string cpfCnpj, string telefone1, string telefone2, string email, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string referencia, string inscricaoEstadual, string inscricaoMunicipal, string usuarioAlteracao)
        {
            Nome = nome;
            Sexo = sexo;
            DataNascimento = dataNascimento;
            Mae = mae;
            Pai = pai;
            TipoPessoa = tipoPessoa;
            RazaoSocial = razaoSocial;
            CpfCnpj = cpfCnpj;
            Telefone1 = telefone1;
            Telefone2 = telefone2;
            Email = email;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Referencia = referencia;
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(TipoPessoa))
                throw new Exception("Tipo Pessoa é obrigatório");
            if (string.IsNullOrEmpty(CpfCnpj))
                throw new Exception("CPF ou CNPJ é obrigatório");
            if (string.IsNullOrEmpty(Email))
                throw new Exception("Email é obrigatório");
            if (string.IsNullOrEmpty(Cep))
                throw new Exception("CEP é obrigatório");
            if (string.IsNullOrEmpty(Logradouro))
                throw new Exception("Logradarou é obrigatório");
            if (string.IsNullOrEmpty(Numero))
                throw new Exception("Numero é obrigatório");
            if (string.IsNullOrEmpty(Bairro))
                throw new Exception("Bairro é obrigatório");
            if (string.IsNullOrEmpty(Cidade))
                throw new Exception("Cidade é obrigatório");
            if (string.IsNullOrEmpty(Estado))
                throw new Exception("Estado é obrigatório");
        }
    }
}

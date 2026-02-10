using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Proposta : BaseModel
    {
        public int IdProposta { get; private set; }
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
        public int IdPlano { get; private set; }
        public Plano Plano { get; private set; }
        public int IdVendedor { get; private set; }
        public Vendedor Vendedor { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataTermino { get; private set; }
        public string StatusProposta { get; private set; }

        public Proposta() { }

        public Proposta(string nome, string sexo, DateTime? dataNascimento, string mae, string pai, string tipoPessoa, string razaoSocial, string cpfCnpj, string telefone1, string telefone2, string email, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string referencia, string inscricaoEstadual, string inscricaoMunicipal, Plano plano, Vendedor vendedor, DateTime dataInicio, DateTime dataTermino, string statusProposta, string usuarioInclusao)
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
            Plano = plano;
            Vendedor = vendedor;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            StatusProposta = statusProposta;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, string sexo, DateTime? dataNascimento, string mae, string pai, string tipoPessoa, string razaoSocial, string cpfCnpj, string telefone1, string telefone2, string email, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string referencia, string inscricaoEstadual, string inscricaoMunicipal, Plano plano, Vendedor vendedor, DateTime dataInicio, DateTime dataTermino, string statusProposta, string usuarioAlteracao)
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
            Plano = plano;
            Vendedor = vendedor;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            StatusProposta = statusProposta;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
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
            if (Plano == null)
                throw new Exception("Plano é obrigatório");
            if (Vendedor == null)
                throw new Exception("Vendedor é obrigatório");
            if (DataInicio == default)
                throw new Exception("Data de início é obrigatória");
            if (DataTermino == default)
                throw new Exception("Data de término é obrigatória");
            if (string.IsNullOrEmpty(StatusProposta))
                throw new Exception("Status da proposta é obrigatório");

        }
    }
}

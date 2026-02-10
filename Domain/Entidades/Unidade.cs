using ERP.Models;
using ERP_API.Domain.Entidades;
using System;

namespace ERP.Domain.Entidades
{
    public class Unidade : BaseModel
    {
        public int IdUnidade { get; private set; }
        public int IdEmpresa { get; private set; }
        public Empresa Empresa { get; private set; }
        public string? TipoPessoa { get; private set; }
        public string? Nome { get; private set; }
        public string? RazaoSocial { get; private set; }
        public string? CpfCnpj { get; private set; }
        public int IdRegiao { get; private set; }
        public Regiao Regiao { get; private set; }
        public int IdGrupoEmpresa { get; private set; }
        public GrupoEmpresa GrupoEmpresa { get; private set; }
        public int IdRamoAtividade { get; private set; }
        public RamoAtividade RamoAtividade { get; private set; }
        public string? Telefone1 { get; private set; }
        public string? Telefone2 { get; private set; }
        public string? Email { get; private set; }
        public string? Cep { get; private set; }
        public string? Logradouro { get; private set; }
        public string? Numero { get; private set; }
        public string? Complemento { get; private set; }
        public string? Bairro { get; private set; }
        public string? Cidade { get; private set; }
        public string? Estado { get; private set; }
        public string? Referencia { get; private set; }
        public string? InscricaoEstadual { get; private set; }
        public string? InscricaoMunicipal { get; private set; }
        public string TokenTecnoSpeed { get; private set; }
        public string UniqueId { get; private set; }

        public Unidade() { }

        public Unidade(Empresa empresa, string nome, string tipoPessoa, string razaoSocial, string cpfCnpj, GrupoEmpresa grupoEmpresa, Regiao regiao, RamoAtividade ramoAtividade, string telefone1, string telefone2, string email, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string referencia, string inscricaoEstadual, string inscricaoMunicipal, string usuarioInclusao)
        {
            Empresa = empresa;
            Nome = nome;
            TipoPessoa = tipoPessoa;
            RazaoSocial = razaoSocial;
            CpfCnpj = cpfCnpj;
            GrupoEmpresa = grupoEmpresa;
            RamoAtividade = ramoAtividade;
            Regiao = regiao;
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

        public void Alterar(Empresa empresa, string nome, string tipoPessoa, string razaoSocial, string cpfCnpj, GrupoEmpresa grupoEmpresa, Regiao regiao, RamoAtividade ramoAtividade, string telefone1, string telefone2, string email, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string referencia, string inscricaoEstadual, string inscricaoMunicipal, string usuarioAlteracao)
        {
            Empresa = empresa;
            Nome = nome;
            TipoPessoa = tipoPessoa;
            RazaoSocial = razaoSocial;
            CpfCnpj = cpfCnpj;
            GrupoEmpresa = grupoEmpresa;
            Regiao = regiao;
            RamoAtividade = ramoAtividade;
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

        public void SetDadosTecnoSpeed(string token)
        {
            TokenTecnoSpeed = token;
        }
        public void SetUniqueId(string uniqueId)
        {
            UniqueId = uniqueId;

        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Empresa == null)
                throw new Exception("Empresa é obrigatória");
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(TipoPessoa))
                throw new Exception("Tipo Pessoa é obrigatório");
            if (string.IsNullOrEmpty(CpfCnpj))
                throw new Exception("CPF ou CNPJ é obrigatório");
            if (string.IsNullOrEmpty(Telefone1))
                throw new Exception("Telefone Celular é obrigatório");
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

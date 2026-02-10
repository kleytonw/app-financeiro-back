using System;

namespace ERP.Models
{
    public class ContratoResponse
    {
        public int IdContrato { get; set; }
        public int? IdCliente { get; set; }
        public string ClienteRazaoSocial { get; set; }
        public string ClienteCpfCnpj { get; set; }
        public string ClienteLogradouro { get; set; }
        public string ClienteNumero { get; set; }
        public string ClienteBairro { get; set; }
        public string ClienteCidade { get; set; }
        public string ClienteEstado { get; set; }
        public int? IdVendedor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public decimal ValorMensalidade { get; set; }
        public int? NumeroParcelas { get; set; }
        public string LinkContrato { get; set; }
        public string Descricao { get; set; }
        public int IdPlano { get; set; }
        public decimal ValorAdesao { get; set; }
        public DateTime? DataAdesao { get; set; }
        public DateTime DataPrimeiraMensalidade { get; set; }
        public bool? ContratoAdesao { get; set; }
        public decimal ValorTotal { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelCpf { get; set; }
        public string ResponsavelCargo { get; set; }
        public string ResponsavelEmail { get; set; }
        public string ResponsavelTelefone { get; set; }
        public string ResponsavelCelular { get; set; }
        public DateTime DataAssinatura { get; set; }
        public string Situacao { get; set; }
    }

    public class ContratoRequest
    {
        public int IdContrato { get; set; }
        public DateTime DataPrimeiraMensalidade { get; set; }
        public int? IdCliente { get; set; }
        public int? IdVendedor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public decimal ValorMensalidade { get; set; }
        public string LinkContrato { get; set; }
        public string Descricao { get; set; }
        public int NumeroParcelas { get; set; }
        public int IdPlano { get; set; }
        public decimal ValorAdesao { get; set; }
        public DateTime? DataAdesao { get; set; }
        public bool? ContratoAdesao { get; set; }
        public decimal ValorTotal { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelCpf { get; set; }
        public string ResponsavelCargo { get; set; }
        public string ResponsavelEmail { get; set; }
        public string ResponsavelTelefone { get; set; }
        public string ResponsavelCelular { get; set; }
        public DateTime DataAssinatura { get; set; }
        public int IdPlanoConta { get; set; }
        public string Situacao { get; set; }
    }

    public class CriarContaRequestModel
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
        public string SenhaConciliadora { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string IdentificadorConciliadora { get; set; }
        public string ApiKeyConciliadora { get; set; }
        public string NomeResponsavel { get; set; }
        public string CelularResponsavel { get; set; }
        public string EmailResponsavel { get; set; }
        public string NomeContratante { get; set; }
        public string CelularContratante { get; set; }
        public string EmailContratante { get; set; }
        public string Senha { get; set; }
        public string Situacao { get; set; }
        public int? IdColaborador { get; set; }
        public int? IdAfiliado { get; set; }
        public int IdPlano { get; set; }
    }
}
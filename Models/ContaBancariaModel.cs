using System;

namespace ERP.Models
{
    public class ContaBancariaResponse
    {
        public int IdContaBancaria { get; set; }
        public int? IdBanco { get; set; }
        public string NomeBanco {  get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string CodigoSistema { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdOperadora { get; set; }
        public string HashDaConta {  get; set; }
        public decimal? Saldo { get; set; }
        public string Situacao { get; set; }
    }

    public class ContaBancariaRequest
    {
        public int IdContaBancaria { get; set; }
        public int? IdBanco { get; set; }
        public string NomeBanco { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string CodigoSistema { get; set; }
        public int? IdUnidade { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdOperadora { get; set; }
        public string HashDaConta { get; set; }
        public decimal? Saldo { get; set; }
        public string Situacao { get; set; }
    }
}


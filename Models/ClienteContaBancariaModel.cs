using System;

namespace ERP_API.Models
{
    public class ClienteContaBancariaRequest
    {
        public int IdClienteContaBancaria {  get; set; }
        public int IdCliente { get; set; }
        public int IdBanco { get; set; }
        public string Agencia { get; set; }
        public string Conta {  get; set; }
        public string DigitoConta {  get; set; }
        public string DigitoAgencia { get; set; }
        public string Descricao { get; set; }
        public decimal ?Saldo { get; set; }
        public DateTime? DataDoSaldo { get; set; }
        public string Situacao { get; set; }

        public string CPF { get; set; }
        public string CNPJ { get; set; }

        public string AccountIdOpenFinance { get; set; }
        public string ItemIdOpenFinance { get; set; }



    }

    public class ClienteContaBancariaResponse
    {
        public int IdClienteContaBancaria { get; set; }
        public int IdCliente { get; set; }
        public int IdBanco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string DigitoAgencia { get; set; }
        public string Descricao { get; set; }
        public decimal? Saldo { get; set; }
        public DateTime? DataDoSaldo { get; set; }
        public string Situacao { get; set; }
        public string AccountIdOpenFinance { get; set; }
        public string ItemIdOpenFinance { get; set; }


    }
}

using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClienteContaBancaria : BaseModel
    {
        public int IdClienteContaBancaria { get;  set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdBanco { get; private set; }
        public Banco? Banco { get; private set; }
        public string Agencia { get; private set; }
        public string Conta {  get; private set; }
        public string DigitoConta {  get; private set; }
        public string DigitoAgencia { get; private set; }
      
        public DateTime? DataDoSaldo { get; private set; }

        public string Descricao { get; set; }
        public string? AccountIdOpenFinance { get; set; }
        public string? ItemIdOpenFinance { get; set; }

        public string? UrlAtivacaoConta { get; set; }



        public string? Tipo { get; set; }
        public string? SubTipo { get; set; }
        public string? Nome { get; set; }
        public string? Numero { get; set; }
        public string? TransferNumber { get; set; }

        public decimal? Saldo { get;  set; }


        public string? IdentificadorConta { get; set; } // identificadador da conta para retorno da api open finance
        public ClienteContaBancaria() { }

        public ClienteContaBancaria(Cliente cliente, Banco? banco, string agencia, string conta, string digitoConta, string digitoAgencia, string descricao, string usuarioInclusao, string accountIdOpenFinance, string itemIdOpenFinance)
        {
            this.Cliente = cliente;
            this.Banco = banco;
            this.Agencia = agencia;
            this.Conta = conta;
            this.DigitoConta = digitoConta;
            this.DigitoAgencia = digitoAgencia;
            this.Descricao = descricao;

            SetUsuarioInclusao(usuarioInclusao);
            AccountIdOpenFinance = accountIdOpenFinance;
            ItemIdOpenFinance = itemIdOpenFinance;
            IdentificadorConta = Guid.NewGuid().ToString();
        }

        public void Alterar(Cliente cliente, Banco? banco, string agencia, string conta, string digitoConta, string digitoAgencia, string descricao, string usuarioAlteracao, string accountIdOpenFinance, string itemIdOpenFinance)
        {
            this.Cliente = cliente;
            this.Banco = banco;
            this.Agencia = agencia;
            this.Conta = conta;
            this.DigitoConta = digitoConta;
            this.DigitoAgencia= digitoAgencia;
            Descricao = descricao;

            SetUsuarioAlteracao(usuarioAlteracao);
            AccountIdOpenFinance = accountIdOpenFinance;
            ItemIdOpenFinance = itemIdOpenFinance;
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void SetSaldo(decimal saldo, DateTime? dataDoSaldo, string usuarioAlteracao)
        {
            this.Saldo = saldo;
            this.DataDoSaldo = dataDoSaldo;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Valida()
        {
            if (this.Cliente == null)
                throw new Exception("O Cliente é obrigatório!");
            if(this.Banco == null)
                throw new Exception("O Banco é obrigatório!");
            if (string.IsNullOrEmpty(this.Agencia))
                throw new Exception("A Agencia é obrigatória!");
            if (string.IsNullOrEmpty(this.Conta))
                throw new Exception("A Conta é obrigatória");

        }
    }
}

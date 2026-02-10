using ERP.Domain.Entidades;
using ERP.Models;
using Org.BouncyCastle.Security;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ContaBancaria : BaseModel
    {
        public int IdContaBancaria { get; private set; }
        public Banco Banco { get; private set; }
        public int? IdBanco { get; private set; }
        public string Agencia { get; private set; }
        public string DigitoAgencia { get; private set; }
        public string Conta { get; private set; }
        public string DigitoConta { get; private set; }
        public string CodigoSistema {  get; private set; }
        public decimal? Saldo { get; private set; }
        public Unidade Unidade { get; private set; }
        public int? IdUnidade { get; private set; }
        public Empresa Empresa { get; private set; }
        public int? IdEmpresa { get; private set; }
        public Operadora Operadora { get; private set; }
        public int? IdOperadora { get; private set; }
        public string? HashDaConta {  get; private set; }
        public DateTime? DataDoSaldo { get; private set; }
        public ContaBancaria() { }

        public ContaBancaria(Banco banco, string agencia, string digitoAgencia, string conta, string digitoConta, string codigoSistema, Unidade unidade, Empresa empresa, Operadora operadora, string usuarioInclusao)
        {
            Banco = banco;
            Agencia = agencia;
            DigitoAgencia = digitoAgencia;
            Conta = conta;
            DigitoConta = digitoConta;
            CodigoSistema = codigoSistema;
            Unidade = unidade;
            Empresa = empresa;
            Operadora = operadora;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Banco banco, string agencia, string digitoAgencia, string conta, string digitoConta, string codigoSistema, Unidade unidade,Empresa empresa, Operadora operadora, string usuarioAlteracao)
        {
            Banco = banco;
            Agencia = agencia;
            DigitoAgencia = digitoAgencia;
            Conta = conta;
            DigitoConta = digitoConta;
            CodigoSistema = codigoSistema;
            Unidade = unidade;
            Empresa = empresa;
            Operadora = operadora;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void SetHashConta(string hashDaConta)
        {
            HashDaConta = hashDaConta;
        }

        public void SetSaldo(decimal? saldo)
        {
            Saldo = saldo;
        }

        public void SetDataDoSaldo(DateTime? dataDoSaldo)
        {
            DataDoSaldo = dataDoSaldo;
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Banco == null)
                throw new Exception("O banco é obrigatório");
            if (string.IsNullOrEmpty(Agencia))
                throw new Exception("A agência é obrigatória");
            if (string.IsNullOrEmpty(Conta))
                throw new Exception("A conta é obrigatória");
            if (string.IsNullOrEmpty(DigitoConta))
                throw new Exception("O digito da conta é obrigatório");
            if (Unidade == null)
                throw new Exception("A unidade é obrigatória!");
            if (Empresa == null)
                throw new Exception("A Empresa é obrigatória!");
            if (Operadora == null)
                throw new Exception("A operadora é obrigatória!");
        }
    }
}



using ERP.Domain.Entidades;
using ERP.Models;
using OFXSharp;
using System;
using System.Diagnostics.Metrics;


namespace ERP_API.Domain.Entidades
{
    public class Extrato : BaseModel
    {

        public int IdExtrato { get; private set; }
        public ClienteContaBancaria ClienteContaBancaria { get; private set; }
        public int IdClienteContaBancaria { get; set; }
        public Cliente Cliente { get; private set; }
        public int IdCliente { get; set; }
        public string Descricao { get; private set; }
        public decimal Valor {  get; private set; }
        public string UniqueId { get; private set; }
        public OFXTransactionType? Tipo { get; private set; }
        public DateTime DataLancamento { get; private set; }
        public string Pagador { get; private set; }
        public string CpfCnpjPagador { get; private set; }
        public string Categoria { get; private set; }
        public string Banco { get; private set; }
        public string MetodoPagamento { get; private set; }

        public Extrato() { }

        public Extrato(ClienteContaBancaria clienteContaBancaria, Cliente cliente, string descricao, decimal valor, OFXTransactionType? tipo, DateTime dataLancamento, string pagador, string cpfCnpj, string categoria, string banco, string metodoPagamento, string usuarioInclusao)
        {
            ClienteContaBancaria = clienteContaBancaria;
            Cliente = cliente;
            Descricao = descricao;
            Valor = valor;  
            Tipo = tipo;
            DataLancamento = dataLancamento;
            Pagador = pagador;
            CpfCnpjPagador = cpfCnpj;
            Categoria = categoria;
            Banco = banco;
            MetodoPagamento = metodoPagamento;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(ClienteContaBancaria clienteContaBancaria, Cliente cliente, string descricao, decimal valor, OFXTransactionType? tipo, DateTime dataLancamento, string pagador, string cpfCnpj, string categoria, string banco, string metodoPagamento, string usuarioAlteracao)
        {
            ClienteContaBancaria = clienteContaBancaria;
            Cliente = cliente;
            Descricao = descricao;
            Valor = valor;
            Tipo = tipo;
            DataLancamento = dataLancamento;
            Pagador = pagador;
            CpfCnpjPagador = cpfCnpj;
            Categoria = categoria;
            Banco = banco;
            MetodoPagamento = metodoPagamento;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void AtualizarValorExtratoStone(decimal valor, string usuarioAlteracao)
        {
            Valor = valor;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void SetUniqueId(string uniqueId)
        {
            UniqueId = uniqueId;
        }


        public void Valida()
        {
            if (ClienteContaBancaria == null)
                throw new Exception("A conta bancaria do cliente é obrigatória!");
            if (Cliente == null)
                throw new Exception("O cliente é obrigatória!");
            if (string.IsNullOrEmpty(Descricao))
                throw new Exception("A descrição é obrigatória!");
            if (Tipo == null)
                throw new Exception("O tipo é obrigatório");
            if (DataLancamento == DateTime.MinValue)
                throw new Exception("Insira uma data valida!");
        }
    }
}




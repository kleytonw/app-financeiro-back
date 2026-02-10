using ERP.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Faturamento : BaseModel
    {
        public int IdFaturamento { get; private set; }
        public int IdCliente { get; private set; }
        public Cliente Cliente { get; private set; }
        public int IdFinanceiro { get; private set; }
        public Financeiro Financeiro { get; private set; }
        public int NumeroVendas {  get; private set; }
        public decimal TotalVendas { get; private set; }
        public int Mes { get; private set; }
        public int Ano { get; private set; }
        public decimal ValorMensalidade {  get; private set; }

        public Faturamento() { }

        // Construtor sem Financeiro (para criação inicial)
        public Faturamento(Cliente cliente, int numeroVendas, decimal totalVendas, int mes, int ano, decimal valorMensalidade, string usuarioInclusao)
        {
            Cliente = cliente;
            NumeroVendas = numeroVendas;
            TotalVendas = totalVendas;
            Mes = mes;
            Ano = ano;
            ValorMensalidade = valorMensalidade;
            SetUsuarioInclusao(usuarioInclusao);
            ValidaSemFinanceiro();
        }

        // Construtor com Financeiro (para manter compatibilidade)
        public Faturamento(Cliente cliente, Financeiro financeiro, int numeroVendas, decimal totalVendas, int mes, int ano, decimal valorMensalidade, string usuarioInclusao)
        {
            Cliente = cliente;
            Financeiro = financeiro;
            NumeroVendas = numeroVendas;
            TotalVendas = totalVendas;
            Mes = mes;
            Ano = ano;
            ValorMensalidade = valorMensalidade;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Cliente cliente, Financeiro financeiro, int numeroVendas, decimal totalVendas, int mes, int ano, decimal valorMensalidade, string usuarioAlteracao)
        {
            Cliente = cliente;
            Financeiro = financeiro;
            NumeroVendas = numeroVendas;
            TotalVendas = totalVendas;
            Mes = mes;
            Ano = ano;
            ValorMensalidade = valorMensalidade;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void AssociarFinanceiro(Financeiro financeiro, string usuarioAlteracao)
        {
            if (financeiro == null)
                throw new Exception("Financeiro não pode ser nulo.");

            Financeiro = financeiro;
            IdFinanceiro = financeiro.IdFinanceiro;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Valida()
        {
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório para o faturamento.");
            if (Financeiro == null)
                throw new Exception("O Financeiro é obrigatório para o faturamento");
            if (NumeroVendas <= 0)
                throw new Exception("Número de vendas deve ser maior que zero.");
            if (TotalVendas <= 0)
                throw new Exception("Total de vendas deve ser maior que zero.");
            if (Mes < 1 || Mes > 12)
                throw new Exception("Mês inválido.");
            if (Ano > DateTime.Now.Year || Ano == 0)
                throw new Exception("Ano inválido.");
            if (ValorMensalidade <= 0)
                throw new Exception("Valor da mensalidade deve ser maior que zero.");
        }

        private void ValidaSemFinanceiro()
        {
            if (Cliente == null)
                throw new Exception("Cliente é obrigatório para o faturamento.");
            if (NumeroVendas <= 0)
                throw new Exception("Número de vendas deve ser maior que zero.");
            if (TotalVendas <= 0)
                throw new Exception("Total de vendas deve ser maior que zero.");
            if (Mes < 1 || Mes > 12)
                throw new Exception("Mês inválido.");
            if (Ano > DateTime.Now.Year || Ano == 0)
                throw new Exception("Ano inválido.");
            if (ValorMensalidade <= 0)
                throw new Exception("Valor da mensalidade deve ser maior que zero.");
        }


    }
}

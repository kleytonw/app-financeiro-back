using System.Collections.Generic;
using System;

namespace ERP_API.Models
{
    public class FinanceiroModel
    {

    }

    public class FiltroFinanceiroModel
    {
        public string Tipo { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Situacao { get; set; }
        public string TipoPeriodo { get; set; } // Data Vencimento ou Data Acerto
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public int IdCentroCusto { get; set; }
        public string CpfCnpj { get; set; }
    }

    public class SalvarFinanceiroModel
    {
        public int IdPessoa { get; set; }
        public decimal Total { get; set; }
        public int IdPlanoConta { get; set; }
        public int IdCentroCusto { get; set; }
        public int IdConta { get; set; }
        public string Observacao { get; set; }
        public int IdUnidadeAtendimento { get; set; }
        public string Tipo { get; set; } // Contas a Pagar ou Contas a Receber 
        public ICollection<SalvarFinanceiroParcelaModel> Parcelas { get; set; }

        public SalvarFinanceiroModel()
        {
            this.Parcelas = new List<SalvarFinanceiroParcelaModel>();
        }
    }

    public class BaixarParcelModel
    {
        public int IdFinanceiroParcela { get; set; }
        public int IdFinanceiro { get; set; }

        public decimal ValorVencimento { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public decimal ValorAcerto { get; set; }
        public int IdConta { get; set; }
        public int IdPlanoConta { get; set; }
        public int IdMeioPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataAcerto { get; set; }
        public int IdCentroCusto { get; set; }

        public string NumeroNf { get; set; }

        public string Observacao { get; set; }
    }
    public class SalvarFinanceiroParcelaModel
    {
        public int NumeroParcela { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
    }

    public class FinanceiroParcelaModel
    {
        public decimal Valor { get; set; }
        public string Observacao { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public int Numero { get; set; }
        public int? IdPlanoConta { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string TipoConta { get; set; }
        public int? IdMeioPagamento { get; set; }
        public int IdConta { get; set; }
        public string Conta { get; set; }
        public int IdFinanceiroParcela { get; set; }
        public int? IdentificadorBoletoUnique { get; set; }
        public DateTime DataVencimentoPrimeiraParcela { get; set; }
        public DateTime? DataPagamento { get; set; }
        public bool IncluirParcelasPagas { get; set; }
        public int QuantidadeParcelas { get; set; }
        public int IdParcela { get; set; }
        public decimal ValorAdesao { get; set; }
        public decimal? ValorAcerto { get; set; }
        public decimal? TotalAcerto { get; set; }
        public DateTime DataVencimentoAdesao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataAcerto { get; set; }
        public DateTime DataTermino { get; set; }
        public DateTime DataVencimento { get; set; }
        public int NumeroParcela { get; set; }
        public string NomeCliente { get; set; }
        public string PlanoConta { get; set; }
        public string Situacao { get; set; }
        public int IdFinanceiro { get; set; }

        public string Inclusao { get; set; }
        public string Exclusao { get; set; }
        public string Alteracao { get; set; }
    }

    public class GerarParcelaFinanceiroModel
    {
        public DateTime DataVencimentoPrimeiraParcela { get; set; }
        public int QuantidadeParcelas { get; set; }
        public decimal Valor { get; set; }
    }

    public class ItemParcelaFinanceiroModel
    {
        public DateTime DataVencimento { get; set; }
        public int NumeroParcela { get; set; }
        public decimal Valor { get; set; }
    }
    public class ListaFinanceiroModel
    {
        public decimal? TotalAberto { get; set; }
        public decimal? TotalBaixado { get; set; }

        public ICollection<ListaItemFinanceiroModel> Itens { get; set; }
    }
    public class ListaItemFinanceiroModel
    {
        public int IdConta { get; set; }
        public int IdPlanoConta { get; set; }
        public int IdFinanceiroParcela { get; set; }
        public int IdFinanceiro { get; set; }
        public int IdCentroCusto { get; set; }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string PlanoConta { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataAcerto { get; set; }
        public decimal ValorVencimento { get; set; }
        public decimal? ValorAcerto { get; set; }
        public int? IdentificadorBoletoUnique { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string Descricao { get; set; }
        public string NumeroNf { get; set; }
        public string Situacao { get; set; }
    }


    // Visão Ano 
    public class DashboardFinanceiroAnoModel
    {
        public decimal TotalAReceberAtrada { get; set; }
        public decimal TotalAPagarAtrasada { get; set; }

        public ICollection<TotalizadoMensalModel> TotalizadoMensalModel { get; set; }
        public ICollection<TotalizadoPlanoContaModel> TotalizadoPlanoContaModel { get; set; }
    }

    public class TotalizadoMensalModel
    {
        public string Mes { get; set; }
        public decimal? TotalAPago { get; set; }
        public decimal? TotalAPagar { get; set; }
        public decimal? TotalARecebido { get; set; }
        public decimal? TotalAReceber { get; set; }
    }

    public class TotalizadoPlanoContaModel
    {
        public string Nome { get; set; }
        public decimal? Total { get; set; }
        public string Tipo { get; set; }
    }

    public class DashboardAtendimentoModel
    {
        public string Tipo { get; set; }
        public string Mes { get; set; }
        public string Situacao { get; set; }
        public decimal TotalVencimento { get; set; }
        public decimal TotalAcerto { get; set; }
        public decimal TotalAberto { get; set; }
    }
}

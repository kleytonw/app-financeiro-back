using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class FinanceiroParcela : BaseModel
    {
        public int IdFinanceiroParcela { get; set; }
        public int Numero { get; set; }
        public int IdFinanceiro { get; set; }
        public int? IdMeioPagamento { get; set; }
        public virtual MeioPagamento MeioPagamento { get; set; }
        public virtual Financeiro Financeiro { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorVencimento { get; set; }
        public DateTime? DataAcerto { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? ValorAcerto { get; set; }
        public int? IdentificadorBoletoUnique { get; set; }
        public DateTime? DataBaixa { get; set; }
        public string UsuarioBaixa { get; set; }
        public string NumeroNf { get; set; }
        public DateTime? DataEnvioLembrete { get; set; }
        public DateTime? DataEnvioLembreteVencimento { get; set; }
        public string Observacao { get; set; }
        public int? IdPlanoConta { get; set; }
        public PlanoConta PlanoConta { get; set; }
        //  public string NossoNumero { get; set; }

        /*  public int? IdRemessa { get; set; }
         public Remessa Remessa { get; set; } */

        protected FinanceiroParcela() { }

        /// <summary>
        /// inclui parcela em aberto
        /// </summary>
        public FinanceiroParcela(
            int numero,
            DateTime dataVencimento,
            decimal valor,
            string observacao,
            PlanoConta planoConta,
            string usuarioInclusao)
        {
            this.DataVencimento = dataVencimento;
            this.ValorVencimento = valor;
            this.Observacao = observacao;
            this.Numero = numero;
            this.PlanoConta = planoConta;   
            SetUsuarioInclusao(usuarioInclusao);
            this.Situacao = "Aberto";
        }

        public void SetIdentificadorBoletoUnique(int identificadorBoletoUnique)
        {
            this.IdentificadorBoletoUnique = identificadorBoletoUnique;
        }

        public void SetDataLemrebrete(DateTime dataEnvioLembrete)
        {
            this.DataEnvioLembrete = dataEnvioLembrete;
        }

        public void SetDataLembreteVencimento(DateTime dataEnvioLembreteVencimento)
        {
            this.DataEnvioLembreteVencimento = dataEnvioLembreteVencimento;
        }

        public void BaixarConta(
            DateTime dataVencimento,
            DateTime dataAcerto,
            string numeroNf,
            string observacao,
            decimal? valorDesconto,
            decimal? valorAcrescimo,
            decimal valorAcerto,
            decimal valorVencimento,
            MeioPagamento meioPagamento,
            string usuarioBaixa
            )
        {
            this.DataVencimento = dataVencimento;
            this.DataAcerto = dataAcerto;
            this.Observacao = observacao;
            this.ValorDesconto = valorDesconto;
            this.ValorAcrescimo = valorAcrescimo;
            this.ValorAcerto = valorAcerto;
            this.ValorVencimento = valorVencimento;
            this.NumeroNf = numeroNf;

            this.UsuarioBaixa = usuarioBaixa;
            this.Situacao = "Baixado";
            this.DataBaixa = DateTime.Now;
            this.MeioPagamento = meioPagamento;


            if (this.DataAcerto == null)
                throw new Exception("Data acerto inválida ");

            if (ValorDesconto > ValorVencimento)
                throw new Exception("Valor desconto maior que o valor total ");

            if (MeioPagamento == null)
                throw new Exception("Forma de Pagamento inválida ");




        }
    }
}

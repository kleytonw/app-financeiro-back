using Microsoft.AspNetCore.Identity;
using ERP.Domain.Entidades;
using ERP.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace ERP_API.Domain.Entidades
{
    public class Financeiro : BaseModel
    {
        public int IdFinanceiro { get; private set; }
        public int IdPessoa { get; private set; }
        public string Nome { get; private set; }
        public string CpfCnpj { get; private set; }
        public virtual Pessoa Pessoa { get; private set; }
        public string Tipo { get; set; }

        public decimal? TotalVencimento { get; private set; }
        public decimal? TotalDesconto { get; private set; }
        public decimal? TotalAcrescimo { get; private set; }
        public decimal? TotalAcerto { get; private set; }

        public virtual ICollection<FinanceiroParcela> Parcelas { get; private set; }

        public Financeiro() { }

        public Financeiro(Pessoa pessoa, string tipo, string usuarioInclusao)
        {
            this.Pessoa = pessoa;
            this.Nome = pessoa.Nome;
            this.CpfCnpj = pessoa.CpfCnpj;
            this.Tipo = tipo;

            Validar();

            this.DataInclusao = DateTime.Now;
            this.UsuarioInclusao = usuarioInclusao;
            this.Situacao = "Aberto";


        }

        public void AddParcela(FinanceiroParcela parcela)
        {
            if (this.Parcelas == null)
                this.Parcelas = new Collection<FinanceiroParcela>();
            else if (this.Parcelas.Contains(parcela))
                return;
            //TODO: auditoria usuário inclusão
            this.Parcelas.Add(parcela);

            this.SetTotal(this.Parcelas);
        }

        public void SetTotal(ICollection<FinanceiroParcela> parcelas)
        {
            if (parcelas == null)
            {
                throw new Exception("Nenhuma parcela encontrada");
            }
            this.TotalVencimento = parcelas.Sum(x => x.ValorVencimento);
            this.TotalAcerto = parcelas.Sum(x => x.ValorAcerto);
        }

        public void ExcluirParcela(FinanceiroParcela parcela, string usuario)
        {
            if (parcela == null)
                throw new Exception("Nenhuma parcela encontrada");
            else
                parcela.Situacao = "Excluido";
            parcela.SetUsuarioExclusao(usuario);
            RecalcularFinanceiro();
        }

        public void BaixarConta(FinanceiroParcela parcela, string usuariobaixa)
        {
            if (parcela == null)
                throw new Exception("Nenhuma parcela encontrada");
            else

                if (parcela.DataAcerto == null)
            {
                throw new Exception("Campo Data Acerto é Obrigatório ");
            }
            else if (parcela.ValorAcerto == null)
            {
                throw new Exception("Campo Total Acerto é Obrigatório ");
            }
            else if (parcela.MeioPagamento == null)
            {
                throw new Exception("Campo Forma Pagamento é Obrigatório ");
            }

            parcela.Situacao = "Baixado";
            parcela.UsuarioBaixa = usuariobaixa;
            parcela.DataBaixa = DateTime.Now;

            RecalcularFinanceiro();
        }

        public void AlterarConta(FinanceiroParcela parcela, string usuarioAlteracao)
        {
            if (parcela == null)
                throw new Exception("Nenhuma parcela encontrada");
            else
                parcela.UsuarioAlteracao = usuarioAlteracao;
            parcela.DataAlteracao = DateTime.Now;
            RecalcularFinanceiro();
        }

        public void RecalcularFinanceiro()
        {
            this.TotalVencimento = this.Parcelas.Where(x => x.Situacao != "Excluido").Sum(x => x.ValorVencimento);
            this.TotalAcerto = this.Parcelas.Where(x => x.Situacao == "Baixado").Sum(x => x.ValorAcerto);
            this.TotalDesconto = this.Parcelas.Where(x => x.Situacao == "Baixado").Sum(x => x.ValorDesconto);
            this.TotalAcrescimo = this.Parcelas.Where(x => x.Situacao == "Baixado").Sum(x => x.ValorAcrescimo);
        }

        public void Validar()
        {
            if (this.Pessoa == null)
                throw new Exception(" Pessoa obrigatório "); // adicionar notificação 

            if (string.IsNullOrEmpty(this.Nome))
                throw new Exception("Nome obrigatório ");

            if (string.IsNullOrEmpty(this.CpfCnpj))
                throw new Exception("CpfCnpj obrigatório ");

            if (!(this.Tipo == "Contas a Pagar" || this.Tipo == "Contas a Receber"))
                throw new Exception("Tipo inválido ");

        }
    }
}

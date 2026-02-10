using ERP.Models;
using System;
namespace ERP_API.Domain.Entidades
{
    public class ContratoOperadoraAntecipacao : BaseModel
    {
        public int IdContratoOperadoraAntecipacao { get; private set; }
        public int? IdContratoOperadora { get; private set; }
        public ContratoOperadora ContratoOperadora { get; private set; }
        public int? IdBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public int? IdMeioPagamento { get; private set; }
        public MeioPagamento MeioPagamento { get; private set; }
        public int? NumeroDias { get; private set; }
        public decimal? Valor {  get; private set; }
        public decimal? Percentual { get; private set; }
        public ContratoOperadoraAntecipacao() { }
        public ContratoOperadoraAntecipacao(ContratoOperadora contratoOperadora, Bandeira bandeira, MeioPagamento meioPagamento, int? numeroDias, decimal? valor, decimal? percentual, string usuarioInclusao)
        {
            ContratoOperadora = contratoOperadora;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
            Valor = valor;
            Percentual = percentual;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }
        public void Alterar(ContratoOperadora contratoOperadora, Bandeira bandeira, MeioPagamento meioPagamento, int? numeroDias, decimal? valor, decimal? percentual, string usuarioAlteracao)
        {
            ContratoOperadora = contratoOperadora;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
            Valor = valor;
            Percentual = percentual;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }
        public void Excluir(string usuarioAlteracao)
        {
            SetUsuarioAlteracao(usuarioAlteracao);
        }
        public void Valida()
        {
            if (ContratoOperadora == null)
                throw new Exception("O contrato operadora é obrigatório");
            if (Bandeira == null)
                throw new Exception("A bandeira é obrigatória");
            if (MeioPagamento == null)
                throw new Exception("O meio de pagamento é obrigatório");
            if (NumeroDias == null)
                throw new Exception("O número de dias é obrigatório");
        }
    }
}

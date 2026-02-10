using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ContratoOperadoraRecebimento : BaseModel
    {
        public int IdContratoOperadoraRecebimento { get; private set; }
        public int? IdContratoOperadora { get; private set; }
        public ContratoOperadora ContratoOperadora { get; private set; }
        public int? IdBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public int? IdMeioPagamento { get; private set; }
        public MeioPagamento MeioPagamento { get; private set; }
        public int? NumeroDias { get; private set; }

        public ContratoOperadoraRecebimento() { }

        public ContratoOperadoraRecebimento(ContratoOperadora contratoOperadora, Bandeira bandeira, MeioPagamento meioPagamento, int? numeroDias, string usuarioInclusao)
        {
            ContratoOperadora = contratoOperadora;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(ContratoOperadora contratoOperadora, Bandeira bandeira, MeioPagamento meioPagamento, int? numeroDias, string usuarioAlteracao)
        {
            ContratoOperadora = contratoOperadora;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
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

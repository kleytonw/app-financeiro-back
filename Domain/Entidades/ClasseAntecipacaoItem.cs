using ERP.Models;
using System;
namespace ERP_API.Domain.Entidades
{
    public class ClasseAntecipacaoItem : BaseModel
    {
        public int IdClasseAntecipacaoItem { get; private set; }
        public int IdClasseAntecipacao { get; private set; }
        public ClasseAntecipacao ClasseAntecipacao { get; private set; }
        public int IdMeioPagamento { get; private set; }
        public MeioPagamento MeioPagamento { get; private set; }
        public int IdBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public int NumeroDias { get; private set; }
        public decimal? Valor { get; private set; }
        public decimal? Percentual { get; private set; }
        public ClasseAntecipacaoItem() { }
        public ClasseAntecipacaoItem(ClasseAntecipacao classeAntecipacao,
                                     Bandeira bandeira,
                                     MeioPagamento meioPagamento,
                                     int numeroDias,
                                     decimal? valor,
                                     decimal? percentual,
                                     string usuarioInclusao)
        {
            ClasseAntecipacao = classeAntecipacao;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
            Valor = valor;
            Percentual = percentual;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }
        public void Alterar(ClasseAntecipacao classeAntecipacao,
                            Bandeira bandeira,
                            MeioPagamento meioPagamento,
                            int numeroDias,
                            decimal? valor,
                            decimal? percentual,
                            string usuarioAlteracao)
        {
            ClasseAntecipacao = classeAntecipacao;
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
            Valida();
        }
        public void Valida()
        {
            if (ClasseAntecipacao == null)
                throw new Exception("Classe de antecipacao é obrigatório");
            if (Bandeira == null)
                throw new Exception("A bandeira é obrigatória");
            if (MeioPagamento == null)
                throw new Exception("O meio de pagamento é obrigatório");
            if (NumeroDias <= 0)
                throw new Exception("Número de dias é obrigatório");
        }
    }
}
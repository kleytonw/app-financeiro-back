using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClasseRecebimentoItem : BaseModel
    {
        public int IdClasseRecebimentoItem { get; private set; }
        public int IdClasseRecebimento { get; private set; }
        public ClasseRecebimento ClasseRecebimento { get; private set; }
        public int IdMeioPagamento { get; private set; }
        public MeioPagamento MeioPagamento { get; private set; }
        public int IdBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public int NumeroDias { get; private set; }

        public ClasseRecebimentoItem() { }

        public ClasseRecebimentoItem(ClasseRecebimento classeRecebimento,
                                     Bandeira bandeira,
                                     MeioPagamento meioPagamento,
                                     int numeroDias, 
                                     string usuarioInclusao)
        {
            ClasseRecebimento = classeRecebimento;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(ClasseRecebimento classeRecebimento,
                            Bandeira bandeira,
                            MeioPagamento meioPagamento,
                            int numeroDias,
                            string usuarioAlteracao)
        {
            ClasseRecebimento = classeRecebimento;
            Bandeira = bandeira;
            MeioPagamento = meioPagamento;
            NumeroDias = numeroDias;
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
            if (ClasseRecebimento == null)
                throw new Exception("Classe de recebimento é obrigatório");
            if (Bandeira == null) 
                throw new Exception("A bandeira é obrigatória");
            if (MeioPagamento == null)
                throw new Exception("O meio de pagamento é obrigatório");
            if (NumeroDias <= 0)
                throw new Exception("Número de dias é obrigatório");
        }
    }
}

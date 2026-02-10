using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class ClasseTarifaItem : BaseModel
    {
        public int IdClasseTarifaItem {  get; set; }
        public int IdClasseTarifa { get; set; }
        public ClasseTarifa ClasseTarifa { get; set; }  
        public int IdMeioPagamento { get; set; }
        public MeioPagamento MeioPagamento { get; set; }
        public int? IdBandeira { get; set; }
        public Bandeira Bandeira { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? Valor { get; set; }
        public string Tipo { get; set; }
        public int? ParcelaInicio { get; set; }
        public int? ParcelaFim { get; set; }

        public ClasseTarifaItem() { }

        public ClasseTarifaItem(ClasseTarifa classeTarifa,
                                 MeioPagamento meioPagamento,
                                 Bandeira bandeira,
                                 decimal? taxa,
                                 decimal? valor,
                                 string tipo,
                                 int? parcelaInicio,
                                 int? parcelaFim,
                                 string usuarioInclusao)
        {
            ClasseTarifa = classeTarifa;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            Taxa = taxa;
            Valor = valor;
            Tipo = tipo;
            ParcelaInicio = parcelaInicio;
            ParcelaFim = parcelaFim;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(ClasseTarifa classeTarifa,
                                 MeioPagamento meioPagamento,
                                 Bandeira bandeira,
                                 decimal? taxa,
                                 decimal? valor,
                                 string tipo,
                                 int? parcelaInicio,
                                 int? parcelaFim,
                                 string usuarioAlteracao)
        {
            IdClasseTarifa = classeTarifa.IdClasseTarifa;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            Taxa = taxa;
            Valor = valor;
            Tipo = tipo;
            ParcelaInicio = parcelaInicio;
            ParcelaFim = parcelaFim;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (ClasseTarifa ==  null )
                throw new Exception("A classe tarifa é obrigatória");
            if (MeioPagamento == null)
                throw new Exception("O meio de pagamento é obrigatório");
        }
    }
}

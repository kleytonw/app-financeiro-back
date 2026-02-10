using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class ContratoOperadoraTaxa : BaseModel
    {

        public int IdContratoOperadoraTaxa { get; private set; }
        public int? IdContratoOperadora { get; private set; }
        public ContratoOperadora ContratoOperadora { get; private set; }
        public int? IdMeioPagamento { get; private set; }
        public MeioPagamento MeioPagamento { get; private set; }
        public int? IdBandeira { get; private set; }
        public Bandeira Bandeira { get; private set; }
        public decimal Taxa { get; private set; }
        public decimal? Valor { get; private set; }
        public int? ParcelaInicio { get; private set; }
        public int? ParcelaFim { get; private set; }
        public string Tipo { get; private set; }
        public int? IdEmpresa { get; private set; }
        public Empresa Empresa { get; private set; }
        public Unidade Unidade { get; private set; }
        public int? IdUnidade { get; private set; }


        public ContratoOperadoraTaxa() { }

        public ContratoOperadoraTaxa(ContratoOperadora contratoOperadora, MeioPagamento meioPagamento, Bandeira bandeira, decimal taxa, decimal valor, int? parcelaInicio, int? parcelaFim, string tipo, Empresa empresa, Unidade unidade, string usuarioInclusao)
        {
            ContratoOperadora = contratoOperadora;
            MeioPagamento = meioPagamento;
            Bandeira = bandeira;
            Taxa = taxa;
            Valor = valor;
            ParcelaInicio = parcelaInicio;
            ParcelaFim = parcelaFim;
            Tipo = tipo;
            Empresa = empresa;
            Unidade = unidade;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(ContratoOperadora contratoOperadora, MeioPagamento meioPagamento, Bandeira bandeira, decimal taxa, decimal valor, int? parcelaInicio, int? parcelaFim, string tipo, Empresa empresa, Unidade unidade, string usuarioAlteracao)
        {
            ContratoOperadora = contratoOperadora;
            MeioPagamento = meioPagamento;
            Taxa = taxa;
            Valor = valor;
            ParcelaInicio = parcelaInicio;
            ParcelaFim = parcelaFim;
            Tipo = tipo;
            Empresa = empresa;
            Unidade = unidade;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (ContratoOperadora == null)
                throw new Exception("O contrato da operadora é obrigatório");
            if (MeioPagamento == null)
                throw new Exception("O meio de pagamento é obrigatório");
            if (Empresa == null)
                throw new Exception("A empresa é obrigatória");
            if (Unidade == null)
                throw new Exception("A unidade é obrigatória");
        }
    }
}




using ERP.Models;
using ERP.Domain.Entidades;


using System.Security.Policy;
using System;

namespace ERP_API.Domain.Entidades
{
    public class BIFaturamentoPeriodoReais : BaseModel
    {
        public int IdFaturamentoPeriodoReais { get; set; }
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public int IdUnidade { get; set; }
        public Unidade Unidade { get; set; }
        public string Descricao { get; set; }
        public string Ano { get; set; }
        public decimal? Janeiro { get; set; }
        public decimal? Fevereiro { get; set; }
        public decimal? Marco { get; set; }
        public decimal? Abril { get; set; }
        public decimal? Maio { get; set; }
        public decimal? Junho { get; set; }
        public decimal? Julho { get; set; }
        public decimal? Agosto { get; set; }
        public decimal? Setembro { get; set; }
        public decimal? Outubro { get; set; }
        public decimal? Novembro { get; set; }
        public decimal? Dezembro { get; set; }

        public BIFaturamentoPeriodoReais() { }

        public BIFaturamentoPeriodoReais(Empresa empresa,
                                         Unidade unidade,
                                         string descricao,
                                         string ano,
                                         decimal? janeiro,
                                         decimal? fevereiro,
                                         decimal? marco,
                                         decimal? abril,
                                         decimal? maio,
                                         decimal? junho,
                                         decimal? julho,
                                         decimal? agosto,
                                         decimal? setembro,
                                         decimal? outubro,
                                         decimal? novembro,
                                         decimal? dezembro,
                                         string usuarioInclusao)
        {
            Empresa = empresa;
            Unidade = unidade;
            Descricao = descricao;
            Ano = ano;
            Janeiro = janeiro;
            Fevereiro = fevereiro;
            Marco = marco;
            Abril = abril;
            Maio = maio;
            Junho = junho;
            Julho = julho;
            Agosto = agosto;
            Setembro = setembro;
            Outubro = outubro;
            Novembro = novembro;
            Dezembro = dezembro;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();

        }

        public void Alterar(Empresa empresa,
                            Unidade unidade,
                            string descricao,
                            string ano,
                            decimal? janeiro,
                            decimal? fevereiro,
                            decimal? marco,
                            decimal? abril,
                            decimal? maio,
                            decimal? junho,
                            decimal? julho,
                            decimal? agosto,
                            decimal? setembro,
                            decimal? outubro,
                            decimal? novembro,
                            decimal? dezembro,
                            string usuarioAlteracao)
        {
            Empresa = empresa;
            Unidade = unidade;
            Descricao = descricao;
            Ano = ano;
            Janeiro = janeiro;
            Fevereiro = fevereiro;
            Marco = marco;
            Abril = abril;
            Maio = maio;
            Junho = junho;
            Julho = julho;
            Agosto = agosto;
            Setembro = setembro;
            Outubro = outubro;
            Novembro = novembro;
            Dezembro = dezembro;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();

        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }
        public void Valida()
        {
            if (Empresa == null)
                throw new Exception("Empresa não informada");
            if (Unidade == null)
                throw new Exception("Unidade não informada");
        }
    }

}

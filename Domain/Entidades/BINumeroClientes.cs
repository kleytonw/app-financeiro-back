using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class BINumeroClientes : BaseModel
    {
        public int IdNumeroClientes { get; set; }
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public int IdUnidade { get; set; }
        public Unidade Unidade { get; set; }
        public string Descricao { get; set; }
        public string Ano { get; set; }
        public int? Janeiro { get; set; }
        public int? Fevereiro { get; set; }
        public int? Marco { get; set; }
        public int? Abril { get; set; }
        public int? Maio { get; set; }
        public int? Junho { get; set; }
        public int? Julho { get; set; }
        public int? Agosto { get; set; }
        public int? Setembro { get; set; }
        public int? Outubro { get; set; }
        public int? Novembro { get; set; }
        public int? Dezembro { get; set; }

        public BINumeroClientes() { }

        public BINumeroClientes(Empresa empresa,
                              Unidade unidade,
                              string descricao,
                              string ano,
                              int? janeiro,
                              int? fevereiro,
                              int? marco,
                              int? abril,
                              int? maio,
                              int? junho,
                              int? julho,
                              int? agosto,
                              int? setembro,
                              int? outubro,
                              int? novembro,
                              int? dezembro,
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
        }

        public void Alterar(Empresa empresa,
                          Unidade unidade,
                          string descricao,
                          string ano,
                          int? janeiro,
                          int? fevereiro,
                          int? marco,
                          int? abril,
                          int? maio,
                          int? junho,
                          int? julho,
                          int? agosto,
                          int? setembro,
                          int? outubro,
                          int? novembro,
                          int? dezembro,
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

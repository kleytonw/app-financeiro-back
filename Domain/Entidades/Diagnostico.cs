using ERP.Domain.Entidades;
using ERP.Models;
using Org.BouncyCastle.Security;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Diagnostico : BaseModel
    {
        public int IdDiagnostico { get; private set; }
        public DateTime? Data { get; private set; }
        public int? QtdeTransacoes { get; private set; }
        public int? QtdeVendas { get; private set; }
        public int? QtdeTransacoesConciliadas { get; private set; }
        public int? QtdeTransacoesInconsistentes { get; private set; }
        public int? QtdeTransacoesNaoEncontradas { get; private set; }
        public int? QtdeVendasConciliadas { get; private set; }
        public int? QtdeVendasInconsistentes { get; private set; }
        public int? QtdeVendasNaoEncontradas { get; private set; }

        public int? QtdePagamentosNaoEncontrados {  get; private set; }
        public int? QtdePagamentosEncontrados { get; private set; }
        public string StatusDiagnostico {  get; private set; }
        public Empresa Empresa { get; private set; }
        public int? IdEmpresa { get; private set; }
        public int? IdUnidade { get; private set; }
        public Unidade Unidade { get; private set; }

        public Diagnostico() { }

        public Diagnostico(DateTime data, int? qtdeTransacoes, int? qtdeVendas, int? qtdeTransacoesConciliadas, int? qtdeTransacoesInconsistentes, int? qtdeTransacoesNaoEncontradas, int? qtdeVendasConciliadas, int? qtdeVendasInconsistentes, int? qtdeVendasNaoEncontradas, Empresa empresa, Unidade unidade, string usuarioInclusao)
        {
            Data = data;
            QtdeTransacoes = qtdeTransacoes;
            QtdeVendas = qtdeVendas;
            QtdeTransacoesConciliadas = qtdeTransacoesConciliadas;
            QtdeTransacoesInconsistentes = qtdeTransacoesInconsistentes;
            QtdeTransacoesNaoEncontradas = qtdeTransacoesNaoEncontradas;
            QtdeVendasConciliadas = qtdeVendasConciliadas;
            QtdeVendasInconsistentes = qtdeVendasInconsistentes;
            QtdeVendasNaoEncontradas = qtdeVendasNaoEncontradas;
            Empresa = empresa;
            Unidade = unidade;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(DateTime data, int? qtdeTransacoes, int? qtdeVendas, int? qtdeTransacoesConciliadas, int? qtdeTransacoesInconsistentes, int? qtdeTransacoesNaoEncontradas, int? qtdeVendasConciliadas, int? qtdeVendasInconsistentes, int? qtdeVendasNaoEncontradas, Empresa empresa, Unidade unidade, string usuarioAlteracao)
        {
            Data = data;
            QtdeTransacoes = qtdeTransacoes;
            QtdeVendas = qtdeVendas;
            QtdeTransacoesConciliadas = qtdeTransacoesConciliadas;
            QtdeTransacoesInconsistentes = qtdeTransacoesInconsistentes;
            QtdeTransacoesNaoEncontradas = qtdeTransacoesNaoEncontradas;
            QtdeVendasConciliadas = qtdeVendasConciliadas;
            QtdeVendasInconsistentes = qtdeVendasInconsistentes;
            QtdeVendasNaoEncontradas = qtdeVendasNaoEncontradas;
            Empresa = empresa;
            Unidade = unidade;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void AlterarStatus(string statusDiagnostico, string usuarioAlteracao)
        {
            StatusDiagnostico = statusDiagnostico;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void SetPagamentosEncontradosENaoEncontrados(int? qtdePagamentosEncontrados, int? qtdePagamentosNaoEncontrados, string usuarioAlteracao)
        {
            QtdePagamentosEncontrados = qtdePagamentosEncontrados;
            QtdePagamentosNaoEncontrados = qtdePagamentosNaoEncontrados;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Data == null)
                throw new Exception("A Data é obrigatoria");
            if (Empresa == null)
                throw new Exception("Empresa é obrigatória");
            if (Unidade == null)
                throw new Exception("A unidade é obrigatória");
        }
    }
}




using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class UnidadeParametro : BaseModel
    {

        public int IdUnidadeParametro { get; private set; }
        public int? IdUnidade { get; private set; }
        public Unidade Unidade { get; private set; }
        public int? IdOperadora { get; private set; }
        public Operadora Operadora { get; private set; }
        public int? IdEmpresa { get; private set; }
        public Empresa Empresa { get; private set; }
        public string Chave { get; private set; }
        public string Valor { get; private set; }

        public UnidadeParametro() { }

        public UnidadeParametro(Unidade unidade, Operadora operadora, Empresa empresa, string chave, string valor, string usuarioInclusao)
        {
            Unidade = unidade;
            Operadora = operadora;
            Empresa = empresa;
            Chave = chave;
            Valor = valor;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Unidade unidade, Operadora operadora, Empresa empresa, string chave, string valor, string usuarioAlteracao)
        {
            Unidade = unidade;
            Operadora = operadora;
            Empresa = empresa;
            Chave = chave;
            Valor = valor;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Unidade == null)
                throw new Exception("A unidade é obrigatória");
            if (Operadora == null)
                throw new Exception("A operadora é obrigatória");
            if (Empresa == null)
                throw new Exception("A empresa é obrigatória");
            if (string.IsNullOrEmpty(Chave))
                throw new Exception("Chave é obrigatório");
            if (string.IsNullOrEmpty(Valor))
                throw new Exception("Valor é obrigatório");
        }
    }
}




using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class PlanoConta : BaseModel
    {
        public int IdPlanoConta { get; private set; }
        public string Codigo { get; private set; }
        public string Descricao { get; private set; }
        public string Classificacao { get; private set; }
        public string Tipo { get; private set; }

        public PlanoConta() { }

        public PlanoConta(string codigo, string descricao, string classificacao, string tipo, string usuarioInclusao)
        {
            Codigo = codigo;
            Descricao = descricao;
            Classificacao = classificacao;
            Tipo = tipo;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string codigo, string decisao, string classificacao, string tipo, string usuarioAlteracao)
        {
            Codigo = codigo;
            Descricao = decisao;
            Classificacao = classificacao;
            Tipo = tipo;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if(string.IsNullOrEmpty(Descricao))
                throw new Exception("A descrição do plano de conta é obrigatória.");

            if(string.IsNullOrEmpty(Classificacao))
                throw new Exception("A classificação do plano de conta é obrigatória.");

            if (string.IsNullOrEmpty(Tipo))
                throw new Exception("O tipo do plano de conta é obrigatório.");

        }
    }
}

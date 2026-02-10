using ERP.Domain.Entidades;
using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class ContratoOperadora : BaseModel
    {

        public int IdContratoOperadora { get; private set; }
        public Operadora Operadora { get; private set; }
        public int? IdOperadora { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataTermino { get; private set; }
        public Empresa Empresa { get; private set; }
        public int? IdEmpresa { get; private set; }
        public Unidade Unidade { get; private set; }
        public int? IdUnidade { get; private set; }
        public ContaBancaria ContaRecebimento { get; private set; }
        public int? IdContaRecebimento { get; private set; }
        public ContaBancaria ContaGravame { get; private set; }
        public int? IdContaGravame { get; private set; }



        public ContratoOperadora() { }

        public ContratoOperadora(Operadora operadora, DateTime dataInicio, DateTime dataTermino, Empresa empresa, Unidade unidade, ContaBancaria contaRecebimento, ContaBancaria contaGravame, string usuarioInclusao)
        {
            Operadora = operadora;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            Empresa = empresa;
            Unidade = unidade;
            ContaRecebimento = contaRecebimento;
            ContaGravame = contaGravame;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Operadora operadora, DateTime dataInicio, DateTime dataTermino, Empresa empresa, Unidade unidade, ContaBancaria contaRecebimento, ContaBancaria contaGravame, string usuarioAlteracao)
        {
            Operadora = operadora;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            Empresa = empresa;
            Unidade = unidade;
            ContaRecebimento = contaRecebimento;
            ContaGravame = contaGravame;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            //if (IdEmpresa == null)
            //throw new Exception("Empresa é obrigatório");
            if (Operadora == null)
                throw new Exception("A operadora é obrigatório");
            if (ContaRecebimento == null)
                throw new Exception("A conta de Recebimento é obrigatótia!");
            if (DataInicio == default(DateTime))
                throw new Exception("A Data incorreta!");
            if (DataTermino == default(DateTime))
                throw new Exception("A Data incorreta!");
            if (Empresa == null)
                throw new Exception("A empresa é obrigatória");
            if (Unidade == null)
                throw new Exception("A unidade é obrigatória");
        }
    }
}




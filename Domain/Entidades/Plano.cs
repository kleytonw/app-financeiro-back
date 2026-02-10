using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Plano : BaseModel
    {
    
        public int IdPlano { get; private set; }
        public string Nome { get; private set; }
        public decimal Valor { get; private set; }
        public decimal ValorAdesao { get; private set; }
        public decimal ValorRepasse { get; private set; }
        public string Descricao { get; private set; }
        public int? QuantidadeVendasInicial { get; private set; }
        public int? QuantidadeVendasFinal { get; private set; }

        public Plano() { }

        public Plano(string nome, decimal valor, decimal valorAdesao, decimal valorRepasse, string descricao, int? quantidadeVendasInicial, int? quantidadeVendasFinal, string usuarioInclusao)
        {
            Nome = nome;
            Valor = valor;
            ValorAdesao = valorAdesao;
            ValorRepasse = valorRepasse;
            Descricao = descricao;
            QuantidadeVendasInicial = quantidadeVendasInicial;
            QuantidadeVendasFinal = quantidadeVendasFinal;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, decimal valor, decimal valorAdesao, decimal valorRepasse, string descricao, int? quantidadeVendasInicial, int? quantidadeVendasFinal, string usuarioAlteracao)
        {
            Nome = nome;
            Valor = valor;
            ValorAdesao = valorAdesao;
            ValorRepasse = valorRepasse;
            Descricao = descricao;
            QuantidadeVendasInicial = quantidadeVendasInicial;
            QuantidadeVendasFinal = quantidadeVendasFinal;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            Valida();
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if(Valor == 0)
                throw new Exception("Valor é obrigatório");
            if (ValorAdesao == 0)
                throw new Exception("Valor de adesão é obrigatório");
            if (ValorRepasse == 0)
                throw new Exception("Valor de repasse é obrigatório");
            if (string.IsNullOrEmpty(Descricao))
                throw new Exception("Descrição é obrigatório");
            if (QuantidadeVendasFinal <= 0)
                throw new Exception("Faturamento final deve ser maior que zero.");
        }
    }
}

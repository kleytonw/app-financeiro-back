

using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Transacao : BaseModel
    {
        public int IdTransacao { get; private set; }
        public Categoria Categoria { get; private set; }
        public int IdCategoria { get; private set; }
        public Cartao Cartao { get; private set; }
        public int IdCartao { get; private set; }
        public Dependente Dependente { get; private set; }
        public int IdDependente { get; private set; }
        public int NumeroParcelas { get; private set; }
        public int ParcelaAtual { get; private set; }
        public DateTime DataCompra { get; private set; }
        public decimal Valor { get; private set; }
        public string Descricao { get; private set; }
        public Transacao() { }

        public Transacao(Categoria categoria, Cartao cartao, Dependente dependente, int numeroParcelas, int parcelaAtual, DateTime dataCompra, decimal valor, string descricao, string usuarioInclusao)
        {
            Categoria = categoria;
            IdCategoria = categoria.IdCategoria;
            Cartao = cartao;
            IdCartao = cartao.IdCartao;
            Dependente = dependente;
            IdDependente = dependente.IdDependente;
            NumeroParcelas = numeroParcelas;
            ParcelaAtual = parcelaAtual;
            DataCompra = dataCompra;
            Valor = valor;
            Descricao = descricao;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(Categoria categoria, Cartao cartao, Dependente dependente, int numeroParcelas, int parcelaAtual, DateTime dataCompra, decimal valor, string descricao, string usuarioAlteracao)
        {
            Categoria = categoria;
            IdCategoria = categoria.IdCategoria;
            Cartao = cartao;
            IdCartao = cartao.IdCartao;
            Dependente = dependente;
            IdDependente = dependente.IdDependente;
            NumeroParcelas = numeroParcelas;
            ParcelaAtual = parcelaAtual;
            DataCompra = dataCompra;
            Valor = valor;
            Descricao = descricao;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }

        public void Valida()
        {
            if (Categoria == null)
                throw new Exception("Categoria é obrigatória");
            if (Cartao == null)
                throw new Exception("Cartão é obrigatório");
            if (Dependente == null)
                throw new Exception("Dependente é obrigatório");
            if (NumeroParcelas < 1)
                throw new Exception("Número de parcelas deve ser maior que zero");
            if (ParcelaAtual < 1 || ParcelaAtual > NumeroParcelas)
                throw new Exception("Parcela atual deve ser entre 1 e o número de parcelas");
            if (DataCompra == default)
                throw new Exception("Data de compra é obrigatória");
            if (Valor <= 0)
                throw new Exception("Valor deve ser maior que zero");
        }


    }
}

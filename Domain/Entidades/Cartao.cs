using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class Cartao : BaseModel
    {
        public int IdCartao { get; private set; }
        public string Nome { get; private set; }
        public string Bandeira { get; private set; }
        public string UltimosDigitos { get; private set; }
        public int DiaFechamento { get; set; }
        public int DiaVencimento { get; set; }
        public decimal LimiteTotal { get; set; }



        public Cartao() { }

        public Cartao(string nome, string bandeira, string ultimosDigitos, int diaFechamento, int diaVencimento, decimal limiteTotal, string usuarioInclusao)
        {
            Nome = nome;
            Bandeira = bandeira;
            UltimosDigitos = ultimosDigitos;
            DiaFechamento = diaFechamento;
            DiaVencimento = diaVencimento;
            LimiteTotal = limiteTotal;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(string nome, string bandeira, string ultimosDigitos, int diaFechamento, int diaVencimento, decimal limiteTotal, string usuarioAlteracao)
        {
            Nome = nome;
            Bandeira = bandeira;
            UltimosDigitos = ultimosDigitos;
            DiaFechamento = diaFechamento;
            DiaVencimento = diaVencimento;
            LimiteTotal = limiteTotal;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
            
        }

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("Nome é obrigatório");
            if (string.IsNullOrEmpty(Bandeira))
                throw new Exception("Bandeira é obrigatório");
            if (string.IsNullOrEmpty(UltimosDigitos))
                throw new Exception("Últimos dígitos é obrigatório");
            if (DiaFechamento < 1 || DiaFechamento > 31)
                throw new Exception("Dia de fechamento inválido");
            if (DiaVencimento < 1 || DiaVencimento > 31)
                throw new Exception("Dia de vencimento inválido");
            if (LimiteTotal <= 0)
                throw new Exception("Limite total deve ser maior que zero");
        }
    }
}
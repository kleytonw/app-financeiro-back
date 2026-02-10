using ERP.Domain.Entidades;
using ERP.Models;
using Microsoft.AspNetCore.Builder;
using System;


namespace ERP_API.Domain.Entidades
{
    public class TabelaPreco : BaseModel
    {

        public int IdTabelaPreco { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataTermino { get; private set; }
     


        public TabelaPreco () { }

        public TabelaPreco(string nome, DateTime dataInicio, DateTime dataTermino, string usuarioInclusao)
        {
            Nome = nome;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, DateTime dataInicio, DateTime dataTermino, string usuarioAlteracao)
        {
            Nome = nome;
            DataInicio = dataInicio;
            DataTermino = dataTermino;
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
            if (DataTermino.Date <= DataInicio.Date)
                throw new Exception("A data de termino precisa ser maior que data de ínicio");
        }
    }
}



using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Servico : BaseModel
    {

        public int IdServico { get; private set; }
        public string Nome { get; private set; }
        public decimal Valor { get; private set; }
        public string Descricao { get; private set; }


        public Servico() { }

        public Servico(string nome, decimal valor, string descricao, string usuarioInclusao)
        {
            Nome = nome;
            Valor = valor;
            Descricao = descricao;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, decimal valor, string descricao, string usuarioAlteracao)
        {
            Nome = nome;
            Valor = valor;
            Descricao = descricao;
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
            if (Valor == 0)
                throw new Exception("Valor é obrigatório");
        }
    }
}

using ERP.Models;
using System;


namespace ERP_API.Domain.Entidades
{
    public class Curso : BaseModel
    {

        public int IdCurso { get; private set; }
        public string NomeCurso { get; private set; }
        public decimal Valor { get; private set; }

        public Curso() { }

        public Curso(string nome, decimal valor, string usuarioInclusao)
        {
            NomeCurso = nome;
            Valor = valor;
            SetUsuarioInclusao(usuarioInclusao);
            Valida();
        }

        public void Alterar(string nome, decimal valor, string usuarioAlteracao)
        {
            NomeCurso = nome;
            Valor = valor;
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
            if (string.IsNullOrEmpty(NomeCurso))
                throw new Exception("Nome é obrigatório");
        }
    }
}

using ERP.Domain.Entidades;
using ERP.Models;
using System;

namespace ERP_API.Domain.Entidades
{
    public class DisciplinaCurso : BaseModel
    {
        public int IdDisciplinaCurso { get; private set; }
        public Disciplina Disciplina { get; private set; }
        public int? IdDisciplina { get; private set; }
        public Curso Curso { get; private set; }
        public int? IdCurso { get; private set; }

        public DisciplinaCurso() { }

        public DisciplinaCurso(Disciplina disciplina, Curso curso, string usuarioInclusao)
        {

            Disciplina = disciplina;
            Curso = curso;
            SetUsuarioInclusao(usuarioInclusao);
        }

        public void Alterar(Disciplina disciplina, Curso curso, string usuarioAlteracao)
        {

            Disciplina = disciplina;
            Curso = curso;
            SetUsuarioAlteracao(usuarioAlteracao);
            Valida();
        }

        public void Excluir(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);

        }

        public void Valida()
        {
            if (IdDisciplina == null)
                throw new Exception("IdDisciplina é obrigatório");
            if (IdCurso == null)
                throw new Exception("IdCurso é obrigatório");
        }
    }
}

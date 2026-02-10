namespace ERP.Models
{
    public class DisciplinaCursoResponse
    {
        public int IdDisciplinaCurso { get; set; }
        public int IdDisciplina { get; set; }
        public int IdCurso { get; set; }
        public string Situacao { get; set; }
    }

    public class DisciplinaCursoRequest
    {
        public int IdDisciplinaCurso { get; set; }
        public int IdDisciplina { get; set; }
        public int IdCurso { get; set; }
        public string Situacao { get; set; }
    }
}

namespace ERP.Models
{
    public class DisciplinaResponse
    {
        public int IdDisciplina { get; set; }
        public int IdCurso {  get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class DisciplinaRequest
    {
        public int IdDisciplina { get; set; }
        public int IdCurso { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}


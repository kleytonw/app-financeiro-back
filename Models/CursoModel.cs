namespace ERP.Models
{
    public class CursoResponse
    {
        public int IdCurso { get; set; }
        public int? IdDisciplina { get; set; }
        public string NomeCurso { get; set; }
        public decimal Valor { get; set; }
        public string Situacao { get; set; }
    }

    public class CursoRequest
    {
        public int IdCurso { get; set; }
        public int? IdDisciplina { get; set; }
        public string NomeCurso { get; set; }
        public decimal Valor { get; set; }
        public string Situacao { get; set; }
    }
}

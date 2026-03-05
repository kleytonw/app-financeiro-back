namespace ERP_API.Models
{
    public class CategoriaRequest
    {
        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public string Cor { get; set; }
        public string Situacao { get; set; }
    }

    public class CategoriaResponse
    {
        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public string Cor { get; set; }
        public string Situacao { get; set; }
    }
}

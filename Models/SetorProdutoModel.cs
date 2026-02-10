namespace ERP.Models
{
    public class SetorProdutoResponse
    {
        public int IdSetorProduto { get; set; }
        public int IdSetor { get; set; }
        public int IdProduto { get; set; }
        public string Situacao { get; set; }
    }

    public class SetorProdutoRequest
    {
        public int IdSetorProduto { get; set; }
        public int IdSetor { get; set; }
        public int IdProduto { get; set; }
        public string Situacao { get; set; }
    }
}

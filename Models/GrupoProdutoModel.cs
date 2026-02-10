namespace ERP.Models
{
    public class GrupoProdutoResponse
    {
        public int IdGrupoProduto { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class GrupoProdutoRequest
    {
        public int IdGrupoProduto { get; set;  }
        public string Nome { get; set; }
        public string Situacao { get; set;  }
    }
}

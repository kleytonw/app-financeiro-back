using System.Windows.Forms.PropertyGridInternal;

namespace ERP.Models
{
    public class SetorResponse
    {
        public int IdSetor { get; set; }
        public string Nome { get; set; }
        public int? IdProduto { get; set; }
        public int? IdSetorPai { get; set; }
        public string NumeroOrdem { get; set; }
        public string NomeProduto { get; set; }

        public string Situacao { get; set; }
    }

    public class SetorRequest
    {
        public int IdSetor { get; set; }
        public string Nome { get; set; }
        public int? IdProduto { get; set; }
        public int? IdSetorPai { get; set; }

        public string NumeroOrdem { get; set; }

        public string NomeProduto { get; set; }

        public string Situacao { get; set; }
    }
}
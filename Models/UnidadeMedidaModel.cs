namespace ERP.Models
{
    public class UnidadeMedidaResponse
    {
        public int IdUnidadeMedida { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class UnidadeMedidaRequest
    {
        public int IdUnidadeMedida { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}

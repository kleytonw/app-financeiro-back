namespace ERP_API.Models
{
    public class ERPsRequest
    {
        public int IdERPs { get; set; }
        public int IdFornecedor { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class ERPsResponse
    {
        public int IdERPs { get; set; }
        public int IdFornecedor { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}

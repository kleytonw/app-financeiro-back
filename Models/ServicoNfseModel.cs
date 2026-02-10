namespace ERP_API.Models
{
    public class ServicoNfseRequest
    {
        public int IdServicoNfse { get; set; }
        public string Codigo { get; set; }
        public string CodigoNBS { get; set; }
        public string Nome { get; set; }
        public decimal AliquotaISS { get; set; }
        public string DescricaoServico { get; set; }
    }

    public class  ServicoNfseResponse 
    {
        public int IdServicoNfse { get; set; }
        public string Codigo { get; set; }
        public string CodigoNBS { get; set; }
        public string Nome { get; set; }
        public decimal AliquotaISS { get; set; }
        public string DescricaoServico { get; set; }
    }
}

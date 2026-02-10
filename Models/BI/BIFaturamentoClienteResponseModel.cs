namespace ERP_API.Models.BI
{
    public class BIFaturamentoClienteResponseModel
    {
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string CpfCnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public float? Total { get; set; }
    }
}

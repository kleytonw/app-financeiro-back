namespace ERP_API.Models
{
    public class ControleCartaVanRequest
    {
        public int IdControleCartaVan { get; set; }
        public int IdCliente { get; set; }
        public int IdClienteContaBancaria { get; set; }
        public int IdEtapa {  get; set; }
        public string TicketFornecedor { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }

    }

    public class ControleCartaVanResponse
    {
        public int IdControleCartaVan { get; set; }
        public int IdCliente { get; set; }
        public int IdClienteContaBancaria { get; set; }
        public int IdEtapa { get; set; }
        public string TicketFornecedor { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}

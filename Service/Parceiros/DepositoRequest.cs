namespace ERP_API.Service.Parceiros
{
    public class DepositoRequest
    {
        public int idContaCorrente { get; set; }
        public int idCliente { get; set; }
        public decimal valor { get; set; }
    }
}

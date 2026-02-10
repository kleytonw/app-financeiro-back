namespace ERP.Models
{
    public class CaracteristicaResponse
    {
        public int IdCaracteristica { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class CaracteristicaRequest
    {
        public int IdCaracteristica { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}

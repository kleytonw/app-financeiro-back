namespace ERP_API.Models
{
    public class ParceiroSistemaRequest
    {
        public int IdParceiroSistema { get; set; }
        public string NomeParceiroSistema { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
    }
    public class ParceiroSistemaResponse
    {
        public int IdParceiroSistema { get; set; }
        public string NomeParceiroSistema { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
    }
}

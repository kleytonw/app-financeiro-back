namespace ERP_API.Models
{
    public class EtapaRequest
    {
        public int IdEtapa { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool EtapaConcluida { get; set; }
        public string Situacao { get; set; }
    }

    public class EtapaResponse
    {
        public int IdEtapa { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool EtapaConcluida { get; set; }
        public string Situacao { get; set; }
    }
}

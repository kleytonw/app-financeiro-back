namespace ERP.Models
{
    public class TipoSuporteResponse
    {
        public int IdTipoSuporte { get; set; }
        public string NomeTipoSuporte { get; set; }
        public string Situacao { get; set; }
    }

    public class TipoSuporteRequest
    {
        public int IdTipoSuporte { get; set; }
        public string NomeTipoSuporte { get; set; }
        public string Situacao { get; set; }
    }
}

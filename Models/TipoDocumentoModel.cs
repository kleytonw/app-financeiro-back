namespace ERP_API.Models
{
    public class TipoDocumentoRequest
    {
        public int IdTipoDocumento { get; set; }
        public string Nome { get; set; }
        public bool Obrigatorio { get; set; }
        public string Situacao { get; set; }
    }
    public class TipoDocumentoResponse
    {
        public int IdTipoDocumento { get; set; }
        public string Nome { get; set; }
        public bool Obrigatorio { get; set; }
        public string Situacao { get; set; }
    }
   
}

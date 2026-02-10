namespace ERP.Models
{
    public class GrupoComissaoResponse
    {
        public int IdGrupoComissao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class GrupoComissaoRequest
    {
        public int IdGrupoComissao { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }
}

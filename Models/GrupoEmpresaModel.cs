namespace ERP.Models
{
    public class GrupoEmpresaResponse
    {
        public int IdGrupoEmpresa { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class GrupoEmpresaRequest
    {
        public int IdGrupoEmpresa { get; set;  }
        public string Nome { get; set; }
        public string Situacao { get; set;  }
    }
}

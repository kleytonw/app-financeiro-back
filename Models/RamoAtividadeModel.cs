namespace ERP.Models
{
    public class RamoAtividadeResponse
    {
        public int IdRamoAtividade { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class RamoAtividadeRequest
    {
        public int IdRamoAtividade { get; set;  }
        public string Nome { get; set; }
        public string Situacao { get; set;  }
    }
}

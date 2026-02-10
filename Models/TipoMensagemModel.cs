namespace ERP.Models
{
    public class TipoMensagemResponse
    {
        public int IdTipoMensagem { get; set; }
        public string Nome { get; set; }

        public string Situacao { get; set; }
    }

    public class TipoMensagemRequest
    {
        public int IdTipoMensagem { get; set; }
        public string Nome { get; set; }

        public string Situacao { get; set; }
    }
}


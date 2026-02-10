namespace ERP_API.Service.Parceiros
{
    public class UniquePayOptions
    {
        public string Ambiente { get; set; }
        public Urls Urls { get; set; }
    }

    public class Urls
    {
        public string Producao { get; set; }
        public string Homologacao { get; set; }

        public string GetBaseUrl(string ambiente) =>
            ambiente?.ToLower() == "producao" ? Producao : Homologacao;
    }
}

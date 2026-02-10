namespace ERP_API.Models.BI
{
    public class BIFaturamentoRegiaoResponseModel
    {
        public string Regiao { get; set; }
        public float Janeiro { get; set; }
        public float Fevereiro { get; set; }
        public float Marco { get; set; }
        public float Abril { get; set; }
        public float Maio { get; set; }
        public float Junho { get; set; }
        public float Julho { get; set; }
        public float Agosto { get; set; }
        public float Setembro { get; set; }
        public float Outubro { get; set; }
        public float Novembro { get; set; }
        public float Dezembro { get; set; }
        public int Ano { get; set; }
    }

    public class BIRegiaoTotalResponseModel
    {
        public string Regiao { get; set; }
        public float Total { get; set; }
    }
}

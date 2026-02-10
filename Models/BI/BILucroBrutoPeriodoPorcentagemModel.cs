namespace ERP_API.Models.BI
{
    public class BILucroBrutoPeriodoPorcentagemRequest
    {
        public int IdLucroBrutoPeriodoPorcentagem { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUnidade { get; set; }
        public int IdCliente { get; set; }
        public string Descricao { get; set; }
        public string Ano { get; set; }
        public decimal? Janeiro { get; set; }
        public decimal? Fevereiro { get; set; }
        public decimal? Marco { get; set; }
        public decimal? Abril { get; set; }
        public decimal? Maio { get; set; }
        public decimal? Junho { get; set; }
        public decimal? Julho { get; set; }
        public decimal? Agosto { get; set; }
        public decimal? Setembro { get; set; }
        public decimal? Outubro { get; set; }
        public decimal? Novembro { get; set; }
        public decimal? Dezembro { get; set; }
    }

    public class BILucroBrutoPeriodoPorcentagemResponse
    {
        public int IdLucroBrutoPeriodoPorcentagem { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUnidade { get; set; }
        public int IdCliente { get; set; }
        public string Descricao { get; set; }
        public string Ano { get; set; }
        public decimal? Janeiro { get; set; }
        public decimal? Fevereiro { get; set; }
        public decimal? Marco { get; set; }
        public decimal? Abril { get; set; }
        public decimal? Maio { get; set; }
        public decimal? Junho { get; set; }
        public decimal? Julho { get; set; }
        public decimal? Agosto { get; set; }
        public decimal? Setembro { get; set; }
        public decimal? Outubro { get; set; }
        public decimal? Novembro { get; set; }
        public decimal? Dezembro { get; set; }
    }
}

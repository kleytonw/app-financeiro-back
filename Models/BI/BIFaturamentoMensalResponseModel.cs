using System.Collections.Generic;

namespace ERP_API.Models.BI
{ 
    public class BIFaturamentoListaMensalResponseModel 
    {
        public string Descricao { get; set; }
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

    public class BIFaturamentoMensalResponseModel
    {
        public List<BIFaturamentoListaMensalResponseModel> ListaFaturamento { get; set; }
        public BIFaturamentoMensalResponseModel() 
        {
            this.ListaFaturamento = new List<BIFaturamentoListaMensalResponseModel>();

        }
        public float TotalJaneiro { get; set; }
        public float TotalFevereiro { get; set; }
        public float TotalMarco { get; set; }
        public float TotalAbril { get; set; }
        public float TotalMaio { get; set; }
        public float TotalJunho { get; set; }
        public float TotalJulho { get; set; }
        public float TotalAgosto { get; set; }
        public float TotalSetembro { get; set; }
        public float TotalOutubro { get; set; }
        public float TotalNovembro { get; set; }
        public float TotalDezembro { get; set; }
        public float TotalGeral { get; set; }
    }
}

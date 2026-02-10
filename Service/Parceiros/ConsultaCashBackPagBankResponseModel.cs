using Newtonsoft.Json;
using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ConsultaCashBackPagBankResponseModel
    {
        public List<CashBackDetalheModel> Detalhes { get; set; }

        public PaginacaoCashBackModel Pagination { get; set; }
    }

    public class CashBackDetalheModel
    {
        public string TipoRegistro { get; set; }

        public string Estabelecimento { get; set; }

        public string DataCashout { get; set; }

        public string CodigoUr { get; set; }

        public string CodigoCashout { get; set; }

        public string ArranjoPagamento { get; set; }

        public decimal ValorCashout { get; set; }

        public string TipoCashout { get; set; }
    }

    public class PaginacaoCashBackModel
    {
        [JsonProperty("elements")]
        public int Elements { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }
    }

}

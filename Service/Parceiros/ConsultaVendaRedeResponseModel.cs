using System.Collections.Generic;

namespace ERP_API.Service.Parceiros
{
    public class ConsultaVendaRedeResponseModel
    {
        public class ConsultaVendaResponseModel
        {
            public ContentModel Content { get; set; }
            public CursorModel Cursor { get; set; }
        }

        public class ContentModel
        {
            public List<TransactionModel> Transactions { get; set; }
        }

        public class TransactionModel
        {
            public MerchantModel Merchant { get; set; }
            public int BrandCode { get; set; }
            public int AuthorizationCode { get; set; }
            public ModalityModel Modality { get; set; }
            /// <summary>
            /// Quantidade de Parcela
            /// </summary>
            public int InstallmentQuantity { get; set; }
            public long Nsu { get; set; }
            /// <summary>
            /// Numero da Venda 
            /// </summary>
            public long SaleSummaryNumber { get; set; }
            /// <summary>
            /// DataMovimentacao
            /// </summary>
            public string MovementDate { get; set; }
            /// <summary>
            /// DataVenda
            /// </summary>
            public string SaleDate { get; set; }
            public string SaleHour { get; set; }
            /// <summary>
            /// StatusTransacao
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// Estorno
            /// </summary>
            public string ChargebackStatus { get; set; }
            public string DeviceType { get; set; }
            /// <summary>
            /// Terminal
            /// </summary>
            public string Device { get; set; }
            /// <summary>
            /// TipoCaptura
            /// </summary>
            public string CaptureType { get; set; }
            public int CaptureTypeCode { get; set; }
            public decimal Amount { get; set; }
            public decimal MdrFee { get; set; }
            public decimal MdrAmount { get; set; }
            public bool Flex { get; set; }
            public decimal FlexFee { get; set; }
            public decimal FlexAmount { get; set; }
            /// <summary>
            /// ValorTotalTaxa
            /// </summary>
            public decimal FeeTotal { get; set; }
            /// <summary>
            /// DescontoTransacao
            /// </summary>
            public decimal DiscountAmount { get; set; }
            public decimal NetAmount { get; set; }
            /// <summary>
            /// ValorEmbarqueTransacao
            /// </summary>
            public decimal BoardingFeeAmount { get; set; }
            public List<TrackingModel> Tracking { get; set; }
            /// <summary>
            /// Tokennizado
            /// </summary>
            public bool Tokenized { get; set; }
            /// <summary>
            /// TransacaoId(Rede)
            /// </summary>
            public string Tid { get; set; }
            /// <summary>
            /// NumeroPedido
            /// </summary>
            public string OrderNumber { get; set; }
            /// <summary>
            /// NumeroCartao
            /// </summary>
            public string CardNumber { get; set; }
            public string StrAuthorizationCode { get; set; }
        }

        public class MerchantModel
        {
            public string CompanyNumber { get; set; }
            public string DocumentNumber { get; set; }
            public string CompanyName { get; set; }
            public string TradeName { get; set; }
        }

        public class ModalityModel
        {
            public string Type { get; set; }
            public int Code { get; set; }
            public string Product { get; set; }
            public int ProductCode { get; set; }
        }

        public class TrackingModel
        {
            public decimal Amount { get; set; }
            public string Date { get; set; }
            public string Status { get; set; }
        }

        public class CursorModel
        {
            public bool HasNextKey { get; set; }
            public string NextKey { get; set; }
        }

    }
}

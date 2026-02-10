using System.Collections.Generic;
using System;

namespace ERP_API.Models.NotaFiscal
{
    public class ListaNotaFiscalResponseModel
    {
        public decimal? QtdeValorPendente { get; set; }
        public int? QtdePendente { get; set; }
        public decimal? QtdeValorAutorizado { get; set; }
        public int? QtdeAutorizado { get; set; }
        public decimal? QtdeValorCancelado { get; set; }
        public int? QtdeCancelado { get; set; }

        public List<NotaFiscalResponseModel> Lista { get; set; } = new List<NotaFiscalResponseModel>();
    }

    public class PdfNFseResponseModel
    {
        public string Pdf { get; set; } = string.Empty;
    }


    public class NotaFiscalResponseModel
    {
        public int IdNotaFiscal { get; set; }
        public int IdSacado { get; set; }
        public string NomeCliente { get; set; }
        public string ChaveAcesso { get; set; }
        public int NumeroRPS { get; set; }
        public int Serie { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public DateTime? DataHoraCancelamento { get; set; }
        public StatusNotaFiscal StatusNotaFiscal { get; set; }
        public decimal TotalServico { get; set; }
        public string? Observacao { get; set; }
        public string? UrlNotaFiscal { get; set; }

        public string Situacao { get; set; }

        public string? PdfBase64 { get; set; }

        public string? Errors { get; set; }

        public List<NotaFiscalItemResponseModel> Itens { get; set; } = new List<NotaFiscalItemResponseModel>();
    }

    public class NotaFiscalItemResponseModel
    {
        public int IdNotaFiscalItem { get; set; }
        public string NomeServico { get; set; }
        public string CodigoServico { get; set; }
        public decimal ValorServico { get; set; }
        public decimal ValorISS { get; set; }
        public decimal Aliquota { get; set; }
    }

    public class NotaFiscalListaResponseModel
    {
        public List<NotaFiscalResponseModel> Lista { get; set; } = new List<NotaFiscalResponseModel>();
    }

    public enum StatusNotaFiscal
    {
        Autorizado,
        Pendente,
        Cancelado,
        Rejeitado,
        EmProcessamento,
        Erro
    }
}

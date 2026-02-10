using System.Collections.Generic;
using System;

namespace ERP_API.Models.NotaFiscal
{
    public class CriarNotaFiscalRequestModel
    {
        public int IdCliente { get; set; }

        public string NomeCliente { get; set; }

        public int IdSacadoUnique { get; set; }

        public decimal Valor { get; set; } = decimal.Zero;
    } 

    public class CriarServicoUniqueRequestModel
    {
        public int IdSacado { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string ChaveAcesso { get; set; } = string.Empty;

        /// <summary>
        /// Valor total do serviço (ex: "100")
        /// </summary>
        public decimal TotalServico { get; set; }

        public int Serie { get; set; }

        public DateTime DataHoraInclusao { get; set; }

        public string? Observacao { get; set; }

        public List<ItemServicoUniqueRequestModel> Itens { get; set; } = new();
    }

    public class ItemServicoUniqueRequestModel
    {
        public int IdNotaFiscalItem { get; set; } = 0;  
        public string NomeServico { get; set; } = string.Empty;
        public string CodigoServico { get; set; } = string.Empty;

        public decimal ValorServico { get; set; }
        public decimal ValorISS { get; set; }
        public decimal Aliquota { get; set; }
    }
}

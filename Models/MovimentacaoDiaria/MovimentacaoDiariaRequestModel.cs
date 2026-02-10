using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_API.Models.MovimentacaoDiaria
{

    public class MovimentacaoDiariaRequestModel
    {

        [Required]
        public int IdCliente { get; set; }
        public string TipoMovimentacao { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string FornecedorCliente { get; set; }
        public string NotaFiscal { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string NCM { get; set; } = string.Empty;
        public string CFOP { get; set; } = string.Empty;
        public int Quantidade { get; set; }

        public int PrecoAquisicao { get; set; }
        
        public string UnidadeMedida { get; set; }
        public decimal? ValorUnitario { get; set; } 
        public decimal? ValorDesconto { get; set; }
        public decimal ValorTotal { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;

        #region Custo Atual 

        public decimal? CMV_Aquisicao { get; set; }
        public decimal? CMV_Contabil { get; set; }
        public decimal? CMV_Tributos { get; set; }
        public decimal? CMV_Total { get; set; }


        #endregion 


        public decimal? Margem { get; set; }
        public string Observacao { get; set; }


        public string EAN { get; set; }
        public string CodigoInterno { get; set; }


        // Custo Contabil atual (menos creditos)
        public decimal? Custo_Contabil_Atual { get; set; }
        public decimal? Custo_Aquisicao_Atual { get; set; }
        public decimal? Custo_Tributos_Atual { get; set; }
        public decimal? Preco_Venda_Unitario { get; set; }

        public bool? Promocao { get; set; }

        public void CalcularMargem()
        {
            if (ValorTotal > 0 && CMV_Total.HasValue)
            {
                Margem = ((ValorTotal - CMV_Total.Value) / ValorTotal) * 100;
            }
            else
            {
                Margem = null; // sem dados suficientes
            }
        }
    }
}
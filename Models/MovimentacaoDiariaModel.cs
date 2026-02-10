using ERP_API.Domain;
using System;

namespace ERP_API.Models
{
    public class MovimentacaoDiariaRequest
    {
        public int IdCliente { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }

        public DateTime DataMovimentacao { get; set; }

        public string FornecedorCliente { get; set; }

        public string CpfCnpjFornecedorCliente { get; set; }

        public string NotaFiscal { get; set; } = string.Empty;

        public string Produto { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string NCM { get; set; } = string.Empty;

        public string CFOP { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public string UnidadeMedida { get; set; }

        public decimal? ValorUnitario { get; set; }

        public decimal? ValorDesconto { get; set; }

        public decimal ValorTotal { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;


        public decimal? CMV_Aquisicao { get; set; }

        public decimal? CMV_Contabil { get; set; }

        public decimal? CMV_Tributos { get; set; }

        public decimal? CMV_Total { get; set; }

        public bool Promocao { get; set; }

        public decimal? Margem { get; set; }

        public string Observacao { get; set; }
    }

    public class MovimentacaoDiariaResponse
    {
        public int IdMovimentacaoDiaria { get; set; }
        public int IdCliente { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }

        public DateTime DataMovimentacao { get; set; }

        public string FornecedorCliente { get; set; }

        public string CpfCnpjFornecedorCliente { get; set; }

        public string NotaFiscal { get; set; } = string.Empty;

        public string Produto { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string NCM { get; set; } = string.Empty;

        public string CFOP { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public string UnidadeMedida { get; set; }

        public decimal? ValorUnitario { get; set; }

        public decimal? ValorDesconto { get; set; }

        public decimal ValorTotal { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;


        public decimal? CMV_Aquisicao { get; set; }

        public decimal? CMV_Contabil { get; set; }

        public decimal? CMV_Tributos { get; set; }

        public decimal? CMV_Total { get; set; }

        public bool Promocao { get; set; }

        public decimal? Margem { get; set; }

        public string Observacao { get; set; }
    }

    public class PesquisarMovimentacaoDiariaRequest
    {
        public int? IdCliente { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

    }
}

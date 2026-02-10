using System;

namespace ERP_API.Models
{
    public class ConectorOpenFinanceResponseModel
    {

        public int IdConector { get; set; }

        public string Nome { get; set; } = null!;
        public string? CorPrimaria { get; set; }
        public string? InstitutionUrl { get; set; }
        public string? ImageUrl { get; set; }

        public string Pais { get; set; } = null!;
        public string Tipo { get; set; } = null!;

        public bool PossuiMFA { get; set; }
        public bool OAuth { get; set; }

        public string? StatusHealth { get; set; } = null!;
        public string? HealthStage { get; set; }

        public bool IsSandbox { get; set; }
        public bool IsOpenFinance { get; set; }

        public bool SuportaIniciacaoPagamento { get; set; }
        public bool SuportaPagamentosAgendados { get; set; }
        public bool SuportaSmartTransfer { get; set; }
        public bool SuportaBoleto { get; set; }

        /// <summary>
        /// Lista CSV: ACCOUNTS,TRANSACTIONS,...
        /// </summary>
        public string? Produtos { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}

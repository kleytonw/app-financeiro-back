using System.Collections.Generic;

namespace ERP_API.Models
{
    public class EmailRelatorioData
    {
        public string NomeCliente { get; set; }
        public string MesAno { get; set; }
        public string Subject { get; set; }
        public List<AdquirenteResumo> Adquirentes { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal TotalLiquido { get; set; }
        public decimal TotalTaxa { get; set; }
        public decimal TaxaMediaGeral { get; set; }
        public int QuantidadeTotal { get; set; }
        public decimal TicketMedioGeral { get; set; }
        public int QuantidadeVendasERP { get; set; }
        public decimal TotalVendasERP { get; set; }
    }

    public class AdquirenteResumo
    {
        public string Adquirente { get; set; }
        public decimal ValorBrutoTotal { get; set; }
        public decimal ValorLiquidoTotal { get; set; }
        public decimal ValorTaxaTotal { get; set; }
        public int QuantidadeTransacoes { get; set; }
        public decimal TaxaMediaPonderada { get; set; }
        public decimal TicketMedio { get; set; }
    }

    public class EmailConciliacaoBancariaData
    {
        public string NomeCliente { get; set; }
        public string MesAno { get; set; }
        public string Subject { get; set; }
        public List<StatusResumo> StatusResumo { get; set; }
        public decimal TotalGeral { get; set; }
        public int QuantidadeTotal { get; set; }
    }

    public class StatusResumo
    {
        public string Status { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
        public List<ConciliacaoBancariaResumo> Detalhes { get; set; }
    }

    public class ConciliacaoBancariaResumo
    {
        public string Status { get; set; }
        public string Adquirente { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
    }

    public class EmailVendasConciliadasData
    {
        public string NomeCliente { get; set; }
        public string MesAno { get; set; }
        public string Subject { get; set; }
        public List<VendasConciliadasResumo> VendasResumo { get; set; }
        public decimal TotalGeral { get; set; }
        public int QuantidadeTotal { get; set; }
    }

    public class VendasConciliadasResumo
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
    }
}

using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class VendasConciliadas_bkpMap : BaseModelMap<VendasConciliadas_bkp>
    {
        public override void Configure(EntityTypeBuilder<VendasConciliadas_bkp> builder)
        {
            builder.HasKey(c => c.IdVendasConciliadas);
            builder.ToTable("VendasConciliadas_bkp");

            builder.Property(c => c.IdentificadorConciliadora);
            builder.Property(c => c.DataInicial);
            builder.Property(c => c.DataFinal);
            builder.Property(c => c.Versao);
            builder.Property(c => c.Lote);
            builder.Property(c => c.NomeSistema);
            builder.Property(c => c.Produto);
            builder.Property(c => c.DescricaoTipoProduto);
            builder.Property(c => c.CodigoAutorizacao);
            builder.Property(c => c.IdentificadorPagamento);
            builder.Property(c => c.DataVenda);
            builder.Property(c => c.DataVencimento);
            builder.Property(c => c.ValorVendaParcela);
            builder.Property(c => c.ValorLiquidoParcela);
            builder.Property(c => c.TotalVenda);
            builder.Property(c => c.Taxa);
            builder.Property(c => c.Parcela);
            builder.Property(c => c.TotalParcelas);
            builder.Property(c => c.ValorBrutoMoeda);
            builder.Property(c => c.ValorLiquidoMoeda);
            builder.Property(c => c.CotacaoMoeda);
            builder.Property(c => c.Moeda);
            builder.Property(c => c.NSU);
            builder.Property(c => c.TID);
            builder.Property(c => c.Terminal);
            builder.Property(c => c.MeioCaptura);
            builder.Property(c => c.Operadora);
            builder.Property(c => c.Modalidade);
            builder.Property(c => c.Status);
            builder.Property(c => c.Observacao);
            builder.Property(c => c.ValorBrutoConciliadora);

            base.Configure(builder);
        }
    }
}

using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class VendasConcilidasMap : BaseModelMap<VendasConciliadas>
    {
        public override void Configure(EntityTypeBuilder<VendasConciliadas> builder)
        {
            builder.HasKey(c => c.IdVendasConciliadas);
            builder.Property(c => c.IdentificadorConciliadora).IsRequired();
            builder.Property(c => c.DataInicial).IsRequired();
            builder.Property(c => c.DataFinal).IsRequired();
            builder.Property(c => c.Versao).IsRequired();
            builder.Property(c => c.Lote);
            builder.Property(c => c.NomeSistema).IsRequired();
            builder.Property(c => c.Produto).IsRequired();
            builder.Property(c => c.DescricaoTipoProduto);
            builder.Property(c => c.CodigoAutorizacao).IsRequired();
            builder.Property(c => c.IdentificadorPagamento);
            builder.Property(c => c.DataVenda).IsRequired();
            builder.Property(c => c.DataVencimento);
            builder.Property(c => c.ValorVendaParcela).IsRequired();
            builder.Property(c => c.ValorLiquidoParcela);
            builder.Property(c => c.TotalVenda).IsRequired();
            builder.Property(c => c.Taxa);
            builder.Property(c => c.Parcela).IsRequired();
            builder.Property(c => c.TotalParcelas).IsRequired();
            builder.Property(c => c.ValorBrutoMoeda);
            builder.Property(c => c.ValorLiquidoMoeda);
            builder.Property(c => c.CotacaoMoeda);
            builder.Property(c => c.Moeda);
            builder.Property(c => c.NSU).IsRequired();
            builder.Property(c => c.TID);
            builder.Property(c => c.Terminal);
            builder.Property(c => c.MeioCaptura).IsRequired();
            builder.Property(c => c.Operadora);
            builder.Property(c => c.Modalidade);
            builder.Property(c => c.Status);
            builder.Property(c => c.ValorBrutoConciliadora);
            base.Configure(builder);
        }
    }
}

using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class FaturamentoMap : BaseModelMap<Faturamento>
    {
        public override void Configure(EntityTypeBuilder<Faturamento> builder)
        {
            builder.HasKey(f => f.IdFaturamento);
            builder.HasOne(f => f.Cliente)
                .WithMany()
                .HasForeignKey(f => f.IdCliente);
            builder.HasOne(f => f.Financeiro)
                .WithMany()
                .HasForeignKey(f => f.IdFinanceiro);
            builder.Property(f => f.NumeroVendas);
            builder.Property(f => f.TotalVendas);
            builder.Property(f => f.Mes);
            builder.Property(f => f.Ano);
            builder.Property(f => f.ValorMensalidade);
            base.Configure(builder);
        }
    }
}

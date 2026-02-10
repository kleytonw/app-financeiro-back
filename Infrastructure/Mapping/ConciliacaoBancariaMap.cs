using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ConciliacaoBancariaMap : BaseModelMap<ConciliacaoBancaria>
    {
        public override void Configure(EntityTypeBuilder<ConciliacaoBancaria> builder)
        {
            builder.HasKey(c => c.IdConciliacaoBancaria);
            builder.HasOne(c => c.Cliente)
                   .WithMany()
                   .HasForeignKey(c => c.IdCliente);
            builder.Property(c => c.DataPagamento);
            builder.Property(c => c.ValorConciliacao);
            builder.Property(c => c.Valor);
            builder.Property(c => c.ConciliadoManual);
            builder.Property(c => c.Adquirente);
            builder.Property(c => c.Status);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}

using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class VendaNfeMap : BaseModelMap<VendaNfe>
    {
        public override void Configure(EntityTypeBuilder<VendaNfe> builder)
        {
            builder.HasKey(x => x.IdVendaNfe);
            builder.HasOne(x => x.Cliente)
                   .WithMany()
                   .HasForeignKey(x => x.IdCliente);
            builder.Property(x => x.Senha);
            builder.Property(x => x.DataVenda);
            builder.Property(x => x.Modelo);
            builder.Property(x => x.Arquivo);
            base.Configure(builder);
        }
    }
}

using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ERPsMap : BaseModelMap<ERPs>
    {
        public override void Configure(EntityTypeBuilder<ERPs> builder)
        {
            builder.HasKey(c => c.IdERPs);
            builder.HasOne(c => c.Fornecedor)
                .WithMany()
                .HasForeignKey(c => c.IdFornecedor);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}

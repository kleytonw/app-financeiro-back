using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ProvedorMap : BaseModelMap<Provedor>
    {
        public override void Configure(EntityTypeBuilder<Provedor> builder)
        {
            builder.HasKey(c => c.IdProvedor);
            builder.Property(c => c.NomeProvedor);

            base.Configure(builder);
        }
    }
}

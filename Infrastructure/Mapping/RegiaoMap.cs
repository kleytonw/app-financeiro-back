using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class RegiaoMap : BaseModelMap<Regiao>
    {
        public override void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.HasKey(c => c.IdRegiao);
            builder.Property(c => c.NomeRegiao);

            base.Configure(builder);
        }
    }
}

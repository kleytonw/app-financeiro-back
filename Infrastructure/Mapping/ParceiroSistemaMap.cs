using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ParceiroSistemaMap: BaseModelMap<ParceiroSistema>
    {
        public override void Configure(EntityTypeBuilder<ParceiroSistema> builder)
        {
            builder.HasKey(c => c.IdParceiroSistema);
            builder.Property(c => c.NomeParceiroSistema);
            builder.Property(c => c.Observacao);
            base.Configure(builder);
        }
    }
}

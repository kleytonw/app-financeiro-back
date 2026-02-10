using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ERP_API.Infrastructure.Mapping
{
    public class PlanoComissaoMap : BaseModelMap<PlanoComissao>
    {
        public override void Configure(EntityTypeBuilder<PlanoComissao> builder)
        {
            builder.HasKey(c => c.IdPlanoComissao);
            builder.Property(c => c.Nivel);
            builder.Property(c => c.Percentual);
            base.Configure(builder);
        }
    }
}

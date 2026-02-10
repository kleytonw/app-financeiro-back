using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TipoSuporteMap : BaseModelMap<TipoSuporte>
    {
        public override void Configure(EntityTypeBuilder<TipoSuporte> builder)
        {
            builder.HasKey(c => c.IdTipoSuporte);
            builder.Property(c => c.NomeTipoSuporte);

            base.Configure(builder);
        }
    }
}

using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TipoDocumentoMap : BaseModelMap<TipoDocumento>
    {
        public override void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            builder.HasKey(c => c.IdTipoDocumento);
            builder.Property(c => c.Nome);
            builder.Property(c => c.Obrigatorio);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}

using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class TipoMensagemMap : BaseModelMap<TipoMensagem>
    {
        public override void Configure(EntityTypeBuilder<TipoMensagem> builder)
        {
            builder.HasKey(c => c.IdTipoMensagem);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}
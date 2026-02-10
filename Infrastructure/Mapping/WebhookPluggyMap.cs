using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class WebhookPluggyMap : BaseModelMap<WebhookPluggy>
    {
        public override void Configure(EntityTypeBuilder<WebhookPluggy> builder)
        {
            builder.HasKey(x => x.IdWebHookPluggy);
            builder.Property(x => x.Data);
            builder.Property(x => x.ObjJson);

            base.Configure(builder);
        }
    }
}

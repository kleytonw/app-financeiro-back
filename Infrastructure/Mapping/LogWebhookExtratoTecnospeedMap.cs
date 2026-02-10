using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class LogWebhookExtratoTecnospeedMap : BaseModelMap<LogWebhookExtratoTecnospeed>
    {
        public override void Configure(EntityTypeBuilder<LogWebhookExtratoTecnospeed> builder)
        {
            builder.HasKey(c => c.IdLogWebhookExtratoRede);
            builder.Property(c => c.Data);
            builder.Property(c => c.Happen);
            builder.Property(c => c.Balance);
            builder.Property(c => c.UniqueId);
            builder.Property(c => c.CreatedAt);
            builder.Property(c => c.AccountHash);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
        }
    }
}

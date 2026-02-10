using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ConfiguracaoNfseMap : BaseModelMap<ConfiguracaoNfse>
    {
        public override void  Configure(EntityTypeBuilder<ConfiguracaoNfse> builder)
        {
            builder.HasKey(x => x.IdConfiguracaoNfse);
            builder.Property(x => x.NumeroRPS);
            base.Configure(builder);
        }
    }
}

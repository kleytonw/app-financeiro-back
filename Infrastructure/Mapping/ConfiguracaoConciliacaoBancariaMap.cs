using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ConfiguracaoConciliacaoBancariaMap: BaseModelMap<ConfiguracaoConciliacaoBancaria>
    {
        public override void Configure(EntityTypeBuilder<ConfiguracaoConciliacaoBancaria> builder)
        {
            builder.HasKey(c => c.IdConfiguracaoConciliacaoBancaria);
            builder.Property(c => c.Adquirente);
            builder.Property(c => c.De);
            builder.Property(c => c.Para);
            builder.Property(c => c.Situacao);
            base.Configure(builder);
            
        }
    }
}

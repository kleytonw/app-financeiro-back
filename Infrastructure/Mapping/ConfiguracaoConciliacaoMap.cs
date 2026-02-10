using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ConfiguracaoConciliacaoMap : BaseModelMap<ConfiguracaoConciliacao>
    {
        public override void Configure(EntityTypeBuilder<ConfiguracaoConciliacao> builder)
        {
            builder.HasKey(x => x.IdConfiguracaoConciliacao);
            builder.Property(x => x.Adquirente);
            builder.Property(x => x.TipoTransacao);
            builder.Property(x => x.Descricao);
            builder.Property(x => x.Situacao);
            base.Configure(builder);
        }
    }
}

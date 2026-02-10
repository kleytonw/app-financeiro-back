using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClasseTarifaItemMap : BaseModelMap<ClasseTarifaItem>
    {
        public override void Configure(EntityTypeBuilder<ClasseTarifaItem> builder)
        {
            builder.HasKey(x => x.IdClasseTarifaItem);
            builder.HasOne(x => x.ClasseTarifa)
                .WithMany()
                .HasForeignKey(x => x.IdClasseTarifa);
            builder.HasOne(x => x.MeioPagamento)
                .WithMany()
                .HasForeignKey(x => x.IdMeioPagamento);
            builder.HasOne(x => x.Bandeira)
                .WithMany()
                .HasForeignKey(x => x.IdBandeira);
            builder.Property(x => x.Taxa);
            builder.Property(x => x.Valor);
            builder.Property(x => x.Tipo);
            builder.Property(x => x.ParcelaInicio);
            builder.Property(x => x.ParcelaFim);
            base.Configure(builder);
        }
    }
}

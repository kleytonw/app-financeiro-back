using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClasseTarifaMap : BaseModelMap<ClasseTarifa>
    {
        public override void Configure(EntityTypeBuilder<ClasseTarifa> builder)
        {
            builder.HasKey(x => x.IdClasseTarifa);
            builder.Property(x => x.Nome);
            base.Configure(builder);
        }
    }
}

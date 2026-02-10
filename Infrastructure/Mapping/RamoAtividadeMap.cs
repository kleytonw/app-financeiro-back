using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class RamoAtividadeMap : BaseModelMap<RamoAtividade>
    {
        public override void Configure(EntityTypeBuilder<RamoAtividade> builder)
        {
            builder.HasKey(c => c.IdRamoAtividade);
            builder.Property(c => c.Nome);

            base.Configure(builder);
        }
    }
}
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;

namespace ERP_API.Infrastructure.Mapping
{
    public class PlanoContaMap: BaseModelMap<PlanoConta>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PlanoConta> builder)
        {
            builder.HasKey(c => c.IdPlanoConta);
            builder.Property(c => c.Codigo);
            builder.Property(c => c.Descricao);
            builder.Property(c => c.Classificacao);
            builder.Property(c => c.Tipo);
            base.Configure(builder);
        }
    }
}

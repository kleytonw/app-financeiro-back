using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClasseRecebimentoMap: BaseModelMap<ClasseRecebimento>
    {
        public override void Configure(EntityTypeBuilder<ClasseRecebimento> builder)
        {
            builder.HasKey(x => x.IdClasseRecebimento);
            builder.Property(x => x.Descricao);
            base.Configure(builder);
        }
    }
}

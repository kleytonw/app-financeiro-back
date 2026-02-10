using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ClasseAntecipacaoMap : BaseModelMap<ClasseAntecipacao>
    {
        public override void Configure(EntityTypeBuilder<ClasseAntecipacao> builder)
        {
            builder.HasKey(x => x.IdClasseAntecipacao);
            builder.Property(x => x.Descricao);
            base.Configure(builder);
        }
    }
}


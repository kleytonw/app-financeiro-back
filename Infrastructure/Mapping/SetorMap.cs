using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class SetorMap : BaseModelMap<Setor>
    {
        public override void Configure(EntityTypeBuilder<Setor> builder)
        {
            builder.HasKey(c => c.IdSetor);
            builder.Property(c => c.Nome);
            builder.Property(c => c.IdSetorPai);
            builder.Property(c => c.NumeroOrdem);
       
            base.Configure(builder);
        }
    }
}
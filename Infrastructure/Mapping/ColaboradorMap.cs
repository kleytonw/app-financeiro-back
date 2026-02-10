using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class ColaboradorMap : BaseModelMap<Colaborador>
    {
        public override void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            builder.HasKey(c => c.IdPessoa);

            builder.HasOne(c => c.Pessoa)
                   .WithMany()
                   .HasForeignKey(c => c.IdPessoa);
            base.Configure(builder);
        }
    }
}

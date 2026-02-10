using ERP.Domain.Entidades;
using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class MensagemMap : BaseModelMap<Mensagem>
    {
        public override void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            builder.HasKey(c => c.IdMensagem);
            builder.Property(c => c.Data);
            builder.Property(c => c.Texto);
            builder.HasOne(c => c.TipoMensagem)
                .WithMany()
                .HasForeignKey(c => c.IdTipoMensagem);
            builder.Property(c => c.Telefone);
            builder.Property(c => c.Email);
            builder.HasOne(c => c.Provedor)
                .WithMany()
                .HasForeignKey(c => c.IdProvedor);

            base.Configure(builder);
        }
    }
}

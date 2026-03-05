using ERP.Infrastructure.Mapping;
using ERP_API.Domain.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_API.Infrastructure.Mapping
{
    public class UsuarioMap : BaseModelMap<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(c => c.IdUsuario);
            builder.Property(c => c.Nome).HasMaxLength(200);
            builder.Property(c => c.Email).HasMaxLength(200);
            builder.Property(c => c.Senha).HasMaxLength(500);
            builder.Property(c => c.Telefone).HasMaxLength(20);
            builder.Property(c => c.Foto).HasMaxLength(500);
            builder.Property(c => c.GoogleId).HasMaxLength(200);
            builder.Property(c => c.EmailConfirmado);

            builder.HasIndex(c => c.Email).IsUnique();
            builder.HasIndex(c => c.GoogleId);

            base.Configure(builder);
        }
    }
}
